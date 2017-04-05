using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.DataBaseModels;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram;
using NLog;

namespace CoffeeJelly.gmailNotifyBot.Bot
{
    public class Authorizer
    {
        public Authorizer(RequestsHandler requestHandler)
        {
            _requestHandler = requestHandler;
            _requestHandler.TelegramTextMessageEvent += _requestHandler_TelegramTextMessageEvent;
        }

        private void _requestHandler_TelegramTextMessageEvent(TelegramTextMessage message)
        {
            if (message.Text == ConnectStringCommand)
                StartAuthorizeAction(message);

        }

        private async void StartAuthorizeAction(TelegramTextMessage message)
        {
            LogMaker.Log(Logger, $"Start authorizing user with UserId={message.From.Id}.", false);
            var userModel = await DbWorker.FindUserAsync(message.From)
                         ?? await DbWorker.AddNewUserAsync(message.From);

            if (CheckUserAuthorization(userModel)) return;


        }

        private bool CheckUserAuthorization(UserModel userModel)
        {
            Debug.Assert(false, $"{nameof(CheckUserAuthorization)} is not impemented yet.");
            return false;
        }


        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private RequestsHandler _requestHandler;
        public string ConnectStringCommand { get; set; } = @"/connect";
    }
}