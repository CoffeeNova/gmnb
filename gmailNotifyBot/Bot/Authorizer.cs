using System;
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
                    throw new NotImplementedException();
                }
            }
        }

        private async Task SendAuthorizeLink(TextMessage message)
        {
            var userContextWorker = new UserContextWorker();

            LogMaker.Log(Logger, $"Start authorizing user with UserId={message.Chat.Id}.", false);
            var userModel = await userContextWorker.FindUserAsync(message.Chat.Id) ??
                           await userContextWorker.AddNewUserAsync(message.Chat);

            LogMaker.Log(Logger, $"The user with id:{userModel.UserId} has requested authorization", false);
            if (CheckUserAuthorization(userModel)) return;

            var state = Base64.Encode($"{userModel.UserId}");

            var pendingUserModel = await userContextWorker.FindPendingUserAsync(userModel.UserId);
            if (pendingUserModel != null)
                await userContextWorker.UpdateRecordJoinTimeAsync(pendingUserModel.Id, DateTime.Now);
            else
                await userContextWorker.QueueAsync(userModel.UserId, state);


            var uri = GetAuthenticationUri(state);
            await _telegramMethods.SendMessageAsync(message.Chat.Id.ToString(),
                $"Open this link to authorize the bot: \r\n {uri.OriginalString}");
        }

        private bool CheckUserAuthorization(UserModel userModel)
        {
            Debug.Assert(false, $"{nameof(CheckUserAuthorization)} is not impemented yet.");
            return false;
        }

        //private string GenerateState(int id)
        //{
        //    return 
        //}

        private async Task Authorizer_AuthorizeRequestEvent(string code, string state, string error)
        {
            long id;
            if (!RestoreState(state, out id))
                return;

            PendingUserModel pendingUserModel = null;
            var userContextWorker = new UserContextWorker();

            try
            {
                pendingUserModel = userContextWorker.FindPendingUser(id);
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
                var userModel = userContextWorker.FindUser(id);
                ExchangeCodeForToken(code, ref userModel);
                userContextWorker.UpdateUserRecord(userModel);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
            finally
            {
                if (pendingUserModel != null)
                    userContextWorker.RemoveFromQueue(pendingUserModel);
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

        private void ExchangeCodeForToken(string code, ref UserModel userModel)
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
                throw new NotImplementedException();
            }
        }

        private void ExchangeCodeForToken2(string code, ref UserModel userModel)
        {
            userModel.NullInspect(nameof(userModel));

            try
            {
                using (var form = new MultipartFormDataContent())
                {
                    if (code != null)
                        form.Add(new StringContent(code, Encoding.UTF8), "code");
                    if (ClientSecret.ClientId != null)
                        form.Add(new StringContent(ClientSecret.ClientId, Encoding.UTF8), "client_id");
                    if (ClientSecret.RedirectUris[1] != null)
                        form.Add(new StringContent(ClientSecret.RedirectUris[1], Encoding.UTF8), "redirect_uri");
                    form.Add(new StringContent("authorization_code", Encoding.UTF8), "authorization_code");

                    using (var httpClient = new HttpClient())
                    {
                        httpClient.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");
                        var responce = httpClient.PostAsync(GoogleOAuthTokenEndpoint, form).Result;

                        var strResult = responce.Content.ReadAsStringAsync().Result;
                        JsonConvert.PopulateObject(strResult, userModel);
                        //var json = JsonConvert.DeserializeObject<JToken>(strResult);
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                throw new TelegramMethodsException(
                    "Bad http request, wrong parameters or something. See inner exception.", ex);
            }
        }

        private delegate Task AuthorizerEventHandler(string code, string state, string error);
        private event AuthorizerEventHandler AuthorizeRequestEvent;


        private UpdatesHandler _updatesHandler;
        private readonly TelegramMethods _telegramMethods;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static readonly string GoogleOAuthCodeEndpoint = @"https://accounts.google.com/o/oauth2/auth?";
        private static readonly string GoogleOAuthTokenEndpoint = @"https://www.googleapis.com/oauth2/v4/token";
        private static readonly object _locker = new object();
        private const int MaxPendingMinutes = 5;

        public static Authorizer Instance { get; private set; }

        public string ConnectStringCommand { get; set; } = @"/connect";


        public ClientSecret ClientSecret { get; set; }

        public List<string> Scopes { get; set; }

        //public string Token { get; private set; }

    }
}