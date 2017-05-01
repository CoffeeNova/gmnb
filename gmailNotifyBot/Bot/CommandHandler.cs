using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.Exceptions;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
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
            _updatesHandler.TelegramCallbackQueryEvent += _updatesHandler_TelegramCallbackQueryEvent;
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
            if (message.Text == Commands.TESTNAME_STRING_COMMAND)
            {
                LogMaker.Log(Logger, $"{Commands.TESTNAME_STRING_COMMAND} command received from user with id {(string)message.From}", false);
                try
                {
                    await HandleTestNameCommand(message);
                }
                catch (CommandHandlerException ex)
                {
                    LogMaker.Log(Logger, ex);
                    _botMessages.WrongCredentialsMessage(message.From);
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, $"Exception is not impemented yet.");
                    // throw new AuthorizeException("An error occurred while trying to send the authentication link to the user", ex);
                }
            }
            if (message.Text == Commands.TESTGETMESSAGE_STRING_COMMAND)
            {
                LogMaker.Log(Logger, $"{Commands.TESTNAME_STRING_COMMAND} command received from user with id {(string)message.From}", false);
                try
                {
                    await HandleTestMessageCommand(message);
                }
                catch
                {
                }
            }

            if (message.Text == Commands.CONNECT_STRING_COMMAND)
                try
                {
                    await _authorizer.SendAuthorizeLink(message);
                }
                catch (AuthorizeException ex)
                {
                    LogMaker.Log(Logger, ex);
                }
        }

        private async void _updatesHandler_TelegramCallbackQueryEvent(CallbackQuery callbackQuery)
        {
            if (callbackQuery.Data == null)
                return;

            if (callbackQuery.Data == Commands.CONNECT_STRING_COMMAND)
                try
                {
                    await _authorizer.SendAuthorizeLink(callbackQuery);
                }
                catch (AuthorizeException ex)
                {
                    LogMaker.Log(Logger, ex);
                }
        }

        private Service SearchServiceByUserId(string userId)
        {
            var gmailServiceFactory = ServiceFactory.GetInstanse(ClientSecret);
            var service = gmailServiceFactory.ServiceCollection.FirstOrDefault(s => s.UserCredential.UserId == userId);
            if (service == null)
                throw new CommandHandlerException($"Service with credentials from user with id={userId} is not created. User, probably, is not authorized");
            return service;
        }

        private async Task HandleTestNameCommand(ISender sender)
        {
            var service = SearchServiceByUserId(sender.From);
            var query = service.Oauth2Service.Userinfo.Get();
            var userinfo = await query.ExecuteAsync();
            await _botMessages.EmailAddressMessage(sender.From, userinfo.Name);
        }

        private async Task HandleTestMessageCommand(ISender sender)
        {
            var service = SearchServiceByUserId(sender.From);
            var query = service.GmailService.Users.Messages.List("me");
            var listMessagesResponce = await query.ExecuteAsync();
            if (listMessagesResponce?.Messages == null) return;

            var message = listMessagesResponce.Messages.First();
            var mailInfoRequest = service.GmailService.Users.Messages.Get("me", message.Id);
            var mailInfoResponce = await mailInfoRequest.ExecuteAsync();
            if (mailInfoResponce == null) return;



            //var getReq =
            //    new UsersResource.MessagesResource.GetRequest(service.GmailService, "me", message.Id)
            //    {
            //        Format = UsersResource.MessagesResource.GetRequest.FormatEnum.Full
            //    };

            //var message1 = await getReq.ExecuteAsync();


            //foreach (var part in message1.Payload.Parts)
            //{
            //    byte[] data = Convert.FromBase64String(part.Body.Data);
            //    string decodedString = Encoding.UTF8.GetString(data);
            //    Debug.WriteLine(decodedString);
            //}
        }

        private void MessagePayload(Google.Apis.Gmail.v1.Data.Message message, out string snippet, out string sender, out string subject, out string body)
        {
            throw new NotImplementedException();
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