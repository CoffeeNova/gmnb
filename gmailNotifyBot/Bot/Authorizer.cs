using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram;
using CoffeeJelly.gmailNotifyBot.Extensions;
using Google.Apis.Auth.OAuth2;
using NLog;

namespace CoffeeJelly.gmailNotifyBot.Bot
{
    public sealed class Authorizer
    {
        private Authorizer(string token, UpdatesHandler updatesHandler, string clientId, string redirectUri, List<string> scopes)
        {
            updatesHandler.NullInspect(nameof(updatesHandler));
            clientId.NullInspect(nameof(clientId));
            redirectUri.NullInspect(nameof(redirectUri));
            scopes.NullInspect(nameof(scopes));

            _updatesHandler = updatesHandler;
            _updatesHandler.TelegramTextMessageEvent += _updatesHandler_TelegramTextMessageEvent;
            ClientId = clientId;
            RedirectUri = redirectUri;
            Scopes = scopes;
            _telegramMethods = new TelegramMethods(token);
            AuthorizeRequestEvent += Authorizer_AuthorizeRequestEvent;
        }

        public static Authorizer GetInstance(string token, UpdatesHandler updatesHandler, string clientId, string redirectUri, List<string> scopes)
        {
            if (Instance == null)
            {
                lock (_locker)
                {
                    if (Instance == null)
                        Instance = new Authorizer(token, updatesHandler, clientId, redirectUri, scopes);
                }
            }
            return Instance;
        }

        public Uri GetAuthenticationUri(string state)
        {
            var scopes = string.Join("+", Scopes);

            string oauth =
                GoogleOAuthString +
                $"client_id={ClientId}" +
                $"&redirect_uri={RedirectUri}" +
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

        private void ExchangeCodeForToken(string code)
        {
        }

        private delegate void AuthorizerEventHandler(string code, string state, string error);
        private event AuthorizerEventHandler AuthorizeRequestEvent;


        private UpdatesHandler _updatesHandler;
        private readonly TelegramMethods _telegramMethods;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static readonly string GoogleOAuthString = @"https://accounts.google.com/o/oauth2/auth?";
        private static readonly object _locker = new object();
        private const int MaxPendingMinutes = 5;
        // private static readonly List<string> Scopes = new List<string>
        //{
        //    @"https://www.googleapis.com/auth/gmail.compose",
        //    @"https://mail.google.com/",
        //    @"https://www.googleapis.com/auth/userinfo.profile"
        //};

        // private const string RedirectUri = @"http://gmailnotify.somee.com/OAuth";

        public static Authorizer Instance { get; private set; }

        public string ConnectStringCommand { get; set; } = @"/connect";

        public string ClientId { get; set; }

        public string RedirectUri { get; set; }

        public List<string> Scopes { get; set; }

        //public string Token { get; private set; }

    }
}