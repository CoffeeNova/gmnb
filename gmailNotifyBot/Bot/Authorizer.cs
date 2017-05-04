using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CoffeeJelly.gmailNotifyBot.Bot.Attributes;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels;
using CoffeeJelly.gmailNotifyBot.Bot.Exceptions;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.TelegramApiWrapper.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;

namespace CoffeeJelly.gmailNotifyBot.Bot
{
    public sealed class Authorizer
    {
        private Authorizer(string token, UpdatesHandler updatesHandler, Secrets clientSecret)
        {
            updatesHandler.NullInspect(nameof(updatesHandler));
            clientSecret.NullInspect(nameof(clientSecret));

            // _updatesHandler = updatesHandler;
            // _updatesHandler.TelegramTextMessageEvent += _updatesHandler_TelegramTextMessageEvent;
            ClientSecret = clientSecret;
            _botMessages = new BotMessages(token);
            AuthorizeRequestEvent += Authorizer_AuthorizeRequestEvent;
        }

        public static Authorizer GetInstance(string token, UpdatesHandler updatesHandler, Secrets clientSecret)
        {
            if (Instance == null)
            {
                lock (_locker)
                {
                    if (Instance == null)
                        Instance = new Authorizer(token, updatesHandler, clientSecret);
                }
            }
            return Instance;
        }

        public Uri GetAuthenticationUri(string state, List<string> scopes)
        {
            var scopesStr = string.Join("+", scopes);

            string oauth =
                GoogleOAuthCodeEndpoint +
                $"client_id={ClientSecret.ClientId}" +
                $"&redirect_uri={ClientSecret.RedirectUris[0]}" +
                $"&scope={scopesStr}" +
                $"&response_type=code" +
                $"&state={state}" +
                $"&access_type=offline" +
                $"&prompt=consent";
            return new Uri(oauth);
        }

        public static void HandleAuthResponse(string code, string state, string error)
        {
            Instance?.AuthorizeRequestEvent?.Invoke(code, state, error);
        }

        public async Task RefreshAccessToken(UserModel userModel)
        {
            try
            {
                //var gmailDbContextWorker = new GmailDbContextWorker();
                //var userModel = await gmailDbContextWorker.FindUserAsync(userId);

                var parameters = new NameValueCollection
                {
                    {"refresh_token", userModel.RefreshToken},
                    {"client_id", ClientSecret.ClientId},
                    {"client_secret", ClientSecret.Secret},
                    {"redirect_uri", ClientSecret.RedirectUris[0]},
                    {"grant_type", "refresh_token"}
                };

                using (var webClient = new WebClient())
                {
                    webClient.Headers.Add(HttpRequestHeader.ContentType, @"application/x-www-form-urlencoded");
                    var byteResult = await webClient.UploadValuesTaskAsync(GoogleOAuthTokenEndpoint, "POST", parameters);
                    var strResult = webClient.Encoding.GetString(byteResult);

                    JsonConvert.PopulateObject(strResult, userModel);
                    TokenRefreshed?.Invoke(userModel);
                }
            }
            catch (WebException ex)
            {
                throw new RefreshTokenException("An error occurred while trying to refresh access token.", ex);
            }
        }

        public async Task RevokeTokenAsync(UserModel userModel)
        {
            try
            {
                var parameters = userModel.RefreshToken != null 
                    ? new NameValueCollection { { "token", userModel.RefreshToken } } 
                    : new NameValueCollection { { "token", userModel.AccessToken } };

                using (var webClient = new WebClient())
                {
                    webClient.Headers.Add(HttpRequestHeader.ContentType, @"application/x-www-form-urlencoded");
                    var byteResult = await webClient.UploadValuesTaskAsync(GoogleOAuthTokenEndpoint, "POST", parameters);
                    var strResult = webClient.Encoding.GetString(byteResult);
                    throw new NotImplementedException("define responce status code, should be 200 and return true.");
                    if (true)
                    {
                        userModel.RefreshToken = "";
                        userModel.AccessToken = "";
                        TokenRevorkedEvent?.Invoke(userModel);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new RevokeTokenException("An error occurred while trying to refresh access token.", ex);
            }

        }

        public async Task SendAuthorizeLink(ISender message)
        {
            try
            {
                var gmailDbContextWorker = new GmailDbContextWorker();

                LogMaker.Log(Logger, $"Start authorizing user with UserId={message.From.Id}.", false);
                var userModel = await gmailDbContextWorker.FindUserAsync(message.From.Id) ??
                                await gmailDbContextWorker.AddNewUserAsync(message.From);

                LogMaker.Log(Logger, $"The user with id:{userModel.UserId} has requested authorization", false);
                if (CheckUserAuthorization(userModel)) return;

                var fullAccessState = Base64.Encode($"{userModel.UserId},{UserAccess.Full}");
                var notifyAccessState = Base64.Encode($"{userModel.UserId},{UserAccess.Notify}");

                var pendingUserModel = await gmailDbContextWorker.FindPendingUserAsync(userModel.UserId);
                if (pendingUserModel != null)
                    await gmailDbContextWorker.UpdateRecordJoinTimeAsync(pendingUserModel.Id, DateTime.UtcNow);
                else
                    await gmailDbContextWorker.QueueAsync(userModel.UserId);

                var notifyAccessUri = GetAuthenticationUri(notifyAccessState, UserAccessAttribute.GetScopesValue(UserAccess.Notify));
                var fullAccessUri = GetAuthenticationUri(fullAccessState, UserAccessAttribute.GetScopesValue(UserAccess.Full));
                await _botMessages.AuthorizeMessage(message.From.Id.ToString(), notifyAccessUri, fullAccessUri);
            }
            catch (Exception ex)
            {
                throw new AuthorizeException("An error occurred while trying to send the authentication link to the user", ex);
            }
        }

        private bool CheckUserAuthorization(UserModel userModel)
        {
            Debug.Assert(false, $"{nameof(CheckUserAuthorization)} is not impemented yet.");
            return false;
        }

#pragma warning disable 4014
        private void Authorizer_AuthorizeRequestEvent(string code, string state, string error)
        {

            long id;
            string access;
            if (!RestoreState(state, out id, out access))
                return;

            UserModel userModel = null;
            PendingUserModel pendingUserModel = null;
            var gmailDbContextWorker = new GmailDbContextWorker();

            try
            {
                pendingUserModel = gmailDbContextWorker.FindPendingUser(id);
                if (pendingUserModel == null)
                {
                    _botMessages.AuthorizationTimeExpiredMessage(id.ToString());
                    return;
                }
                if (DateTime.Now.Subtract(pendingUserModel.JoinTimeUtc).Minutes > MaxPendingMinutes)
                {
                    _botMessages.AuthorizationTimeExpiredMessage(id.ToString());
                    LogMaker.Log(Logger,
                        $"Authorization attempt from user with id:{id} when authorization time has expired.", false);
                    return;
                }

                if (!string.IsNullOrEmpty(error))
                {
                    _botMessages.AuthorizationFailedMessage(id.ToString());
                    LogMaker.Log(Logger,
                        error == "access_denied"
                            ? $"User with id:{id} user declined the authorization request."
                            : $"Authorization user with id:{id} has faulted with error={error}", false);
                    return;
                }

                if (string.IsNullOrEmpty(code))
                {
                    LogMaker.Log(Logger, $"Server returned empty authorization code for user with id:{id}.", false);
                    return;
                }
                userModel = gmailDbContextWorker.FindUser(id);
                ExchangeCodeForToken(code, userModel);
                
                gmailDbContextWorker.UpdateUserRecord(userModel);

                var userSettings = gmailDbContextWorker.FindUserSettings(id) ??
                                   gmailDbContextWorker.AddNewUserSettings(id, access);
                AuthorizationRegistredEvent?.Invoke(userModel, userSettings);
                _botMessages.AuthorizationSuccessfulMessage(id.ToString());
            }
            catch (Exception ex)
            {
                gmailDbContextWorker.RemoveUserRecord(userModel);
                throw new AuthorizeException("An error occurred while attempting to authorize the user.", ex);
            }
            finally
            {
                if (pendingUserModel != null)
                    gmailDbContextWorker.RemoveFromQueue(pendingUserModel);
            }
        }
#pragma warning enable 4014

        private static bool RestoreState(string state, out long id, out string access)
        {
            id = 0;
            access = "";
            bool value = !string.IsNullOrEmpty(state);
            if (value)
            {
                try
                {
                    var str = Base64.Decode(state);
                    var splittedStr = str.Split(',');

                    value = long.TryParse(splittedStr.First(), out id);
                    access = splittedStr.Last();
                }
                catch (FormatException ex)
                {
                    value = false;
                }
            }

            if (!value)
                LogMaker.Log(Logger, $"Unauthorized authorization attempt", false);
            return value;
        }

        private void ExchangeCodeForToken(string code, UserModel userModel)
        {
            userModel.NullInspect(nameof(userModel));

            var parameters = new NameValueCollection
            {
                {"code", code},
                {"client_id", ClientSecret.ClientId},
                {"client_secret", ClientSecret.Secret},
                {"redirect_uri", ClientSecret.RedirectUris[0]},
                {"grant_type", "authorization_code"}
            };

            try
            {
                using (var webClient = new WebClient())
                {
                    webClient.Headers.Add(HttpRequestHeader.ContentType, @"application/x-www-form-urlencoded");
                    var byteResult = webClient.UploadValues(GoogleOAuthTokenEndpoint, "POST", parameters);
                    var strResult = webClient.Encoding.GetString(byteResult);
                    JsonConvert.PopulateObject(strResult, userModel);
                }
            }
            catch (WebException ex)
            {
                throw new ExchangeException("Failure when exchanging code for token, perhaps bad request.", ex);
            }
        }

        private delegate void AuthorizerEventHandler(string code, string state, string error);
        private event AuthorizerEventHandler AuthorizeRequestEvent;

        public delegate void AuthorizationRegistredEventHandler(UserModel userModel, UserSettingsModel userSettingsModel);
        public event AuthorizationRegistredEventHandler AuthorizationRegistredEvent;

        public delegate void TokenRevorkedEventHandler(UserModel userModel);
        public event TokenRevorkedEventHandler TokenRevorkedEvent;

        public delegate void TokenRefreshedEventHandler(UserModel userModel);
        public event TokenRefreshedEventHandler TokenRefreshed;


        //       private UpdatesHandler _updatesHandler;
        //       private readonly TelegramMethods _telegramMethods;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static readonly string GoogleOAuthCodeEndpoint = @"https://accounts.google.com/o/oauth2/auth?";
        private static readonly string GoogleOAuthTokenEndpoint = @"https://www.googleapis.com/oauth2/v4/token";
        private static readonly string GoogleOAuthRevokeTokenEndpoint = @"https://accounts.google.com/o/oauth2/revoke";
        private static readonly object _locker = new object();
        private const int MaxPendingMinutes = 5;
        private readonly BotMessages _botMessages;

        public static Authorizer Instance { get; private set; }



        public Secrets ClientSecret { get; set; }

    }
}