﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels;
using CoffeeJelly.gmailNotifyBot.Bot.Exceptions;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram.Exceptions;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram.JsonParsers;
using CoffeeJelly.gmailNotifyBot.Extensions;
using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;

namespace CoffeeJelly.gmailNotifyBot.Bot
{
    public sealed class Authorizer
    {
        private Authorizer(string token, UpdatesHandler updatesHandler, ClientSecret clientSecret, List<string> scopes)
        {
            updatesHandler.NullInspect(nameof(updatesHandler));
            clientSecret.NullInspect(nameof(clientSecret));
            scopes.NullInspect(nameof(scopes));

            _updatesHandler = updatesHandler;
            _updatesHandler.TelegramTextMessageEvent += _updatesHandler_TelegramTextMessageEvent;
            ClientSecret = clientSecret;
            Scopes = scopes;
            _telegramMethods = new TelegramMethods(token);
            AuthorizeRequestEvent += Authorizer_AuthorizeRequestEvent;
        }

        public static Authorizer GetInstance(string token, UpdatesHandler updatesHandler, ClientSecret clientSecret, List<string> scopes)
        {
            if (Instance == null)
            {
                lock (_locker)
                {
                    if (Instance == null)
                        Instance = new Authorizer(token, updatesHandler, clientSecret, scopes);
                }
            }
            return Instance;
        }

        public Uri GetAuthenticationUri(string state)
        {
            var scopes = string.Join("+", Scopes);

            string oauth =
                GoogleOAuthCodeEndpoint +
                $"client_id={ClientSecret.ClientId}" +
                $"&redirect_uri={ClientSecret.RedirectUris[0]}" +
                $"&scope={scopes}" +
                $"&response_type=code" +
                $"&state={state}" +
                $"&access_type=offline";
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

        public async Task RevokeToken(UserModel userModel)
        {
            try
            {
                var parameters = new NameValueCollection { {"token", userModel.RefreshToken }};

                using (var webClient = new WebClient())
                {
                    webClient.Headers.Add(HttpRequestHeader.ContentType, @"application/x-www-form-urlencoded");
                    var byteResult = await webClient.UploadValuesTaskAsync(GoogleOAuthTokenEndpoint, "POST", parameters);
                    var strResult = webClient.Encoding.GetString(byteResult);
                    throw new NotImplementedException("define responce status code, shuold be 200 and return true.");
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



        private async void _updatesHandler_TelegramTextMessageEvent(TextMessage message)
        {
            if (message.Text == ConnectStringCommand)
            {
                try
                {
                    await SendAuthorizeLink(message);
                }
                catch (Exception ex)
                {
                    throw new AuthorizeException("An error occurred while trying to send the authentication link to the user", ex);
                }
            }
        }

        private async Task SendAuthorizeLink(TextMessage message)
        {
            var gmailDbContextWorker = new GmailDbContextWorker();

            LogMaker.Log(Logger, $"Start authorizing user with UserId={message.Chat.Id}.", false);
            var userModel = await gmailDbContextWorker.FindUserAsync(message.Chat.Id) ??
                           await gmailDbContextWorker.AddNewUserAsync(message.Chat);

            LogMaker.Log(Logger, $"The user with id:{userModel.UserId} has requested authorization", false);
            if (CheckUserAuthorization(userModel)) return;

            var state = Base64.Encode($"{userModel.UserId}");

            var pendingUserModel = await gmailDbContextWorker.FindPendingUserAsync(userModel.UserId);
            if (pendingUserModel != null)
                await gmailDbContextWorker.UpdateRecordJoinTimeAsync(pendingUserModel.Id, DateTime.Now);
            else
                await gmailDbContextWorker.QueueAsync(userModel.UserId, state);


            var uri = GetAuthenticationUri(state);
            await _telegramMethods.SendMessageAsync(message.Chat.Id.ToString(),
                $"Open this link to authorize the bot: \r\n {uri.OriginalString}");
        }

        private bool CheckUserAuthorization(UserModel userModel)
        {
            Debug.Assert(false, $"{nameof(CheckUserAuthorization)} is not impemented yet.");
            return false;
        }

        private void Authorizer_AuthorizeRequestEvent(string code, string state, string error)
        {
            long id;
            if (!RestoreState(state, out id))
                return;

            PendingUserModel pendingUserModel = null;
            var gmailDbContextWorker = new GmailDbContextWorker();

            try
            {
                pendingUserModel = gmailDbContextWorker.FindPendingUser(id);
                if (pendingUserModel == null)
                    return;
                if (DateTime.Now.Subtract(pendingUserModel.JoinTime).Minutes > MaxPendingMinutes)
                {
                    _telegramMethods.SendMessage(id.ToString(),
                        @"Time for authorization has expired. Please type again /connect command.");
                    LogMaker.Log(Logger,
                        $"Authorization attempt from user with id:{id} when authorization time has expired.", false);
                    return;
                }

                if (!string.IsNullOrEmpty(error))
                {
                    _telegramMethods.SendMessage(id.ToString(), "Authorization failed. See ya!");
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
                var userModel = gmailDbContextWorker.FindUser(id);
                ExchangeCodeForToken(code, userModel);
                gmailDbContextWorker.UpdateUserRecord(userModel);

                AuthorizationRegistredEvent?.Invoke(userModel);
                _telegramMethods.SendMessage(id.ToString(), "Authorization successfull! Now you can recieve notifications about new emails and use other functions!");
            }
            catch (Exception ex)
            {
                throw new AuthorizeException("An error occurred while attempting to authorize the user.", ex);
            }
            finally
            {
                if (pendingUserModel != null)
                    gmailDbContextWorker.RemoveFromQueue(pendingUserModel);
            }
        }

        private static bool RestoreState(string state, out long id)
        {
            id = 0;
            bool value = !string.IsNullOrEmpty(state);
            if (value)
            {
                try
                {
                    var idstr = Base64.Decode(state);
                    value = long.TryParse(idstr, out id);
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

                    //JsonConvert.DeserializeObject<JToken>(strResult);
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

        public delegate void AuthorizationRegistredEventHandler(UserModel userModel);
        public event AuthorizationRegistredEventHandler AuthorizationRegistredEvent;

        public delegate void TokenRevorkedEventHandler(UserModel userModel);
        public event TokenRevorkedEventHandler TokenRevorkedEvent;

        public delegate void TokenRefreshedEventHandler(UserModel userModel);
        public event TokenRefreshedEventHandler TokenRefreshed;


        private UpdatesHandler _updatesHandler;
        private readonly TelegramMethods _telegramMethods;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static readonly string GoogleOAuthCodeEndpoint = @"https://accounts.google.com/o/oauth2/auth?";
        private static readonly string GoogleOAuthTokenEndpoint = @"https://www.googleapis.com/oauth2/v4/token";
        private static readonly string GoogleOAuthRevokeTokenEndpoint = @"https://accounts.google.com/o/oauth2/revoke";
        private static readonly object _locker = new object();
        private const int MaxPendingMinutes = 5;

        public static Authorizer Instance { get; private set; }

        public string ConnectStringCommand { get; set; } = @"/connect";


        public ClientSecret ClientSecret { get; set; }

        public List<string> Scopes { get; set; }

        //public string Token { get; private set; }

    }
}