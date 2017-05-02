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
using Google.Apis.Gmail.v1.Data;
using HtmlAgilityPack;
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
            var logCommandRecieved =
                new Action<string>(
                    command => LogMaker.Log(Logger, $"{command} command received from user with id {(string)message.From}", false));

            #region testname
            if (message.Text == Commands.TESTNAME_COMMAND)
            {
                logCommandRecieved(Commands.TESTNAME_COMMAND);
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
            #endregion
            if (message.Text == Commands.TESTMESSAGE_COMMAND)
            {
                logCommandRecieved(Commands.TESTMESSAGE_COMMAND);
                try
                {
                    await HandleTestMessageCommand(message);
                }
                catch
                {
                }
            }
            #region connect
            if (message.Text == Commands.CONNECT_COMMAND)
            {
                LogMaker.Log(Logger, $"{Commands.TESTMESSAGE_COMMAND} command received from user with id {(string)message.From}", false);
                try
                {
                    await _authorizer.SendAuthorizeLink(message);
                }
                catch (AuthorizeException ex)
                {
                    LogMaker.Log(Logger, ex);
                }
            }
            #endregion
            if (message.Text ==Commands.INBOX_COMMAND)
            {
                logCommandRecieved(Commands.INBOX_COMMAND);
                try
                {
                    await HandleGetInboxMessagesCommand(message);
                }
                catch
                {

                }
            }

        }

        private async void _updatesHandler_TelegramCallbackQueryEvent(CallbackQuery callbackQuery)
        {
            if (callbackQuery.Data == null)
                return;

            if (callbackQuery.Data == Commands.CONNECT_COMMAND)
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
            query.LabelIds = "INBOX";
            var listMessagesResponce = await query.ExecuteAsync();
            if (listMessagesResponce?.Messages == null) return;

            var message = listMessagesResponce.Messages.First();
            var mailInfoRequest = service.GmailService.Users.Messages.Get("me", message.Id);
            var mailInfoResponce = await mailInfoRequest.ExecuteAsync();
            if (mailInfoResponce == null) return;
            string snippet = "";
            string senderAddress = "";
            string subject = "";
            string date = "";
            string body = "";
            MessagePayload(mailInfoResponce, ref snippet, ref senderAddress, ref subject, ref date, ref body);
            

            Debug.WriteLine(body);

        }

        private async Task HandleGetInboxMessagesCommand(ISender sender)
        {
            var service = SearchServiceByUserId(sender.From);
            var query = service.GmailService.Users.Messages.List("me");
            query.LabelIds = "INBOX";
            var listMessagesResponce = await query.ExecuteAsync();
            if (listMessagesResponce?.Messages != null)
                await _botMessages.EmptyInboxMessage(sender.From);

            var message = listMessagesResponce.Messages.First();
            var mailInfoRequest = service.GmailService.Users.Messages.Get("me", message.Id);
            var mailInfoResponce = await mailInfoRequest.ExecuteAsync();
            if (mailInfoResponce == null) return;

            //await _botMessages.EmailAddressMessage(sender.From, userinfo.Name);
        }

        private void MessagePayload(Google.Apis.Gmail.v1.Data.Message message, ref string snippet, ref string sender, ref string subject, ref string date, ref string body)
        {
            if (message?.Payload == null)
                throw new ArgumentNullException(nameof(message.Payload));

            snippet = message.Snippet;
            var messagePartHeader = message.Payload.Headers.FirstOrDefault(h => h.Name == "From");
            if (messagePartHeader != null)
                sender = messagePartHeader.Value;
            messagePartHeader = message.Payload.Headers.FirstOrDefault(h => h.Name == "Subject");
            if (messagePartHeader != null)
                subject = messagePartHeader.Value;
            messagePartHeader = message.Payload.Headers.FirstOrDefault(h => h.Name == "Date");
            if (messagePartHeader != null)
                date = messagePartHeader.Value;
            if (message.Payload.Parts != null)
                DecodeDevidedBody(message.Payload.Parts, out body);
            else if (message.Payload.Body?.Data != null)
                body = DecodeBase64(message.Payload.Body.Data);
        }

        private static string DecodeBase64(string base64EncodedData)
        {
            base64EncodedData = base64EncodedData.Replace('-', '+');
            base64EncodedData = base64EncodedData.Replace('_', '/');
            return Base64.Decode(base64EncodedData);
        }

        private void DecodeDevidedBody(IList<MessagePart> parts, out string decodedBody)
        {
            parts.NullInspect(nameof(parts));

            decodedBody = "";
            foreach (var part in parts)
            {
                if (part.Parts != null)
                    DecodeDevidedBody(part.Parts, out decodedBody);
                else if (part.Body?.Data != null)
                    decodedBody += DecodeBase64(part.Body.Data);
            }
        }

        private void PrepareMessage(ref string message)
        {
            
        }

        private string ParseInnerText(string html)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            return htmlDoc.DocumentNode.InnerText;
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