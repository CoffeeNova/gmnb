using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.Exceptions;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram;
using Google.Apis.Auth.OAuth2;
using NLog;

namespace CoffeeJelly.gmailNotifyBot.Bot
{
    public sealed class CommandHandler
    {
        private CommandHandler(string token, UpdatesHandler updatesHandler, Secrets clientSecret)
        {
            updatesHandler.NullInspect(nameof(updatesHandler));
            clientSecret.NullInspect(nameof(clientSecret));

            _updatesHandler = updatesHandler;
            ClientSecret = clientSecret;
            _updatesHandler.TelegramTextMessageEvent += _updatesHandler_TelegramTextMessageEvent;
        }

        public static CommandHandler GetInstance(string token, UpdatesHandler updatesHandler, Secrets clientSecret)
        {
            if (Instance == null)
            {
                lock (_locker)
                {
                    if (Instance == null)
                        Instance = new CommandHandler(token, updatesHandler, clientSecret);
                }
            }
            return Instance;
        }

        private async void _updatesHandler_TelegramTextMessageEvent(TextMessage message)
        {
            if (message.Text == TestStringCommand)
            {
                try
                {
                    await HandleTestCommand(message);
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, $"Exception is not impemented yet.");
                    // throw new AuthorizeException("An error occurred while trying to send the authentication link to the user", ex);
                }
            }
        }

        private async Task HandleTestCommand(TextMessage message)
        {
            var gmailServiceFactory = GmailServiceFactory.GetInstanse(ClientSecret);
            await gmailServiceFactory.RestoreServicesFromStore();

            var service = gmailServiceFactory.Services.FirstOrDefault(s => (s.HttpClientInitializer as UserCredential).UserId == message.Chat.Id.ToString());
            if (service == null)
                return;
            var query = service.Users.Messages.List("me");

            var mail = query.Execute();
            foreach (var item in mail.Messages)
            {
                var msg = service.Users.Messages.Get("me", item.Id).Execute();
                Debug.WriteLine("Message dump: {0}", msg.Payload.Body.Data);
            }
        }

        private UpdatesHandler _updatesHandler;
        private readonly TelegramMethods _telegramMethods;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static readonly object _locker = new object();

        public static CommandHandler Instance { get; private set; }

        public string TestStringCommand { get; set; } = @"/test";
        public string SettingsStringCommand { get; set; } = @"/settings";

        public Secrets ClientSecret { get; set; }


    }
}