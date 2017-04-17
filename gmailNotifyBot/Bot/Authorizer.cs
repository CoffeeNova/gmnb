using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram.Exceptions;
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
                $"&state={state}";
            return new Uri(oauth);
        }

        public static void HandleAuthResponse(string code, string state, string error)
        {
            Instance?.AuthorizeRequestEvent(code, state, error);
        }

        private void _updatesHandler_TelegramTextMessageEvent(TextMessage message)
        {
            if (message.Text == ConnectStringCommand)
                SendAuthorizeLink(message);
        }

        private async void SendAuthorizeLink(TextMessage message)
        {
            LogMaker.Log(Logger, $"Start authorizing user with UserId={message.Chat.Id}.", false);
            var userModel = await UserContextWorker.FindUserAsync(message.Chat) ??
                            await UserContextWorker.AddNewUserAsync(message.Chat);

            LogMaker.Log(Logger, $"The user with id:{userModel.Id} has requested authorization", false);
            if (CheckUserAuthorization(userModel)) return;

            var state = Base64.Encode($"{userModel.Id}");
            await UserContextWorker.QueueAsync(userModel.Id, state);

            var uri = GetAuthenticationUri(state);
            await _telegramMethods.SendMessageAsync(message.Chat.Id.ToString(),
                $"Open this link to authorize the bot: /r/n {uri.OriginalString}");
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

        private async void Authorizer_AuthorizeRequestEvent(string code, string state, string error)
        {
            long id;
            if (!RestoreState(state, out id))
                return;

            var pendingUserModel = await UserContextWorker.FindPendingUserAsync(id);
            if (pendingUserModel == null)
                return;

            if (DateTime.Now.Subtract(pendingUserModel.JoinTime).Minutes > MaxPendingMinutes)
            {
                await _telegramMethods.SendMessageAsync(id.ToString(),
                        @"Time for authorization has expired. Please type again /connect command.");
                await UserContextWorker.RemoveFromQueueAsync(pendingUserModel);
                LogMaker.Log(Logger, $"Authorization attempt from user with id:{id} when authorization time has expired.", false);
                return;
            }

            if (!string.IsNullOrEmpty(error))
            {
                await _telegramMethods.SendMessageAsync(id.ToString(), "Authorization failed. See ya!");
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
            var responceJson = ExchangeCodeForToken(code);

        }

        private static bool RestoreState(string state, out long id)
        {
            id = 0;
            bool value = !string.IsNullOrEmpty(state);
            if (value)
            {
                var idstr = Base64.Decode(state);
                value = long.TryParse(idstr, out id);
            }

            if (!value)
                LogMaker.Log(Logger, $"Unauthorized authorization attempt", false);
            return value;
        }

        private JToken ExchangeCodeForToken(string code)
        {
            var parameters = new NameValueCollection
            {
                {"code", code},
                {"client_id", ClientSecret.ClientId},
                {"client_secret", ClientSecret.Secret},
                {"redirect_uri", ClientSecret.RedirectUris[1]},
                {"grant_type", "authorization_code"}
            };

            using (var webClient = new WebClient())
            {
                try
                {
                    var byteResult = webClient.UploadValues(GoogleOAuthTokenEndpoint, "POST", parameters);
                    var strResult = webClient.Encoding.GetString(byteResult);

                    return JsonConvert.DeserializeObject<JToken>(strResult);
                }
                catch (WebException ex)
                {
                    throw new NotImplementedException();
                }
            }
        }

        private delegate void AuthorizerEventHandler(string code, string state, string error);
        private event AuthorizerEventHandler AuthorizeRequestEvent;


        private UpdatesHandler _updatesHandler;
        private readonly TelegramMethods _telegramMethods;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static readonly string GoogleOAuthCodeEndpoint= @"https://accounts.google.com/o/oauth2/auth?";
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