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

            _botMessages = new BotMessages(token);

            _authorizer = Authorizer.GetInstance(token, updatesHandler, clientSecret);
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
            if (message.Text == Commands.TEST_STRING_COMMAND)
            {
                LogMaker.Log(Logger, $"{Commands.TEST_STRING_COMMAND} command received from user with id {message.Chat.Id}", false);
                try
                {
                    await HandleTestCommand(message);
                }
                catch (CommandHandlerException ex)
                {
                    LogMaker.Log(Logger, ex);
                    _botMessages.WrongCredentialsMessage(message.Chat.Id.ToString());
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, $"Exception is not impemented yet.");
                    // throw new AuthorizeException("An error occurred while trying to send the authentication link to the user", ex);
                }
            }
            if (message.Text == Commands.CONNECT_STRING_COMMAND)
            {
                try
                {
                    await _authorizer.SendAuthorizeLink(message);
                }
                catch (AuthorizeException ex)
                {
                    LogMaker.Log(Logger, ex);
                }
            }
        }

        private async Task HandleTestCommand(TextMessage message)
        {
            var gmailServiceFactory = GmailServiceFactory.GetInstanse(ClientSecret);
            //await gmailServiceFactory.RestoreServicesFromStore();

            var service = gmailServiceFactory.Services.FirstOrDefault(s => (s.HttpClientInitializer as UserCredential).UserId == message.Chat.Id.ToString());
            if (service == null)
                throw new CommandHandlerException($"Service with credentials from user with id={message.Chat.Id} is not created. User, probably, is not authorized");

            var query = service.Users.Messages.List("me");

            var mail = query.Execute();
            foreach (var item in mail.Messages)
            {
                var msg = service.Users.Messages.Get("me", item.Id).Execute();
                Debug.WriteLine("Message dump: {0}", msg.Payload.Body.Data);
            }
        }

        private UpdatesHandler _updatesHandler;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static readonly object _locker = new object();
        private BotMessages _botMessages;
        private Authorizer _authorizer;

        public static CommandHandler Instance { get; private set; }


        public Secrets ClientSecret { get; set; }


    }
}