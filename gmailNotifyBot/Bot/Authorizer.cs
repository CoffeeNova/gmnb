using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram;
using CoffeeJelly.gmailNotifyBot.Extensions;
using NLog;

namespace CoffeeJelly.gmailNotifyBot.Bot
{
    public class Authorizer
    {
        public Authorizer(RequestsHandler requestHandler, string clientId, string redirectUri, List<string> scopes)
        {
            if (requestHandler == null) throw new ArgumentNullException(nameof(requestHandler));
            if (clientId == null) throw new ArgumentNullException(nameof(clientId));
            if (redirectUri == null) throw new ArgumentNullException(nameof(redirectUri));
            if (scopes == null) throw new ArgumentNullException(nameof(scopes));

            _requestHandler = requestHandler;
            _requestHandler.TelegramTextMessageEvent += _requestHandler_TelegramTextMessageEvent;
            ClientId = clientId;
            RedirectUri = redirectUri;
            Scopes = scopes;
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

        private void _requestHandler_TelegramTextMessageEvent(TextMessage message)
        {
            if (message.Text == ConnectStringCommand)
                StartAuthorizeAction(message);

        }

        private async void StartAuthorizeAction(TextMessage message)
        {
            //LogMaker.Log(Logger, $"Start authorizing user with UserId={message.From.Id}.", false);
            //var userModel = await UserContextWorker.FindUserAsync(message.From)
            //             ?? await UserContextWorker.AddNewUserAsync(message.From);

            //if (CheckUserAuthorization(userModel)) return;

            //var state = GenerateState(userModel.Id);
            //await UserContextWorker.QueueAsync(userModel.Id, state);

            //var uri = GetAuthenticationUri(state);
            //SendMessage();

        }

        private bool CheckUserAuthorization(UserModel userModel)
        {
            Debug.Assert(false, $"{nameof(CheckUserAuthorization)} is not impemented yet.");
            return false;
        }


        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private RequestsHandler _requestHandler;
        private static readonly string GoogleOAuthString = @"https://accounts.google.com/o/oauth2/auth?";
       // private static readonly List<string> Scopes = new List<string>
        //{
        //    @"https://www.googleapis.com/auth/gmail.compose",
        //    @"https://mail.google.com/",
        //    @"https://www.googleapis.com/auth/userinfo.profile"
        //};

       // private const string RedirectUri = @"http://gmailnotify.somee.com/OAuth";

        public string ConnectStringCommand { get; set; } = @"/connect";

        public string ClientId { get; set; }

        public string RedirectUri { get; set; }

        public List<string> Scopes { get; set; }

    }
}