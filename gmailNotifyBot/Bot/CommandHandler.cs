using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CoffeeJelly.gmailNotifyBot.Bot.Exceptions;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.TelegramApiWrapper.Types;
using CoffeeJelly.TelegramApiWrapper.Types.General;
using CoffeeJelly.TelegramApiWrapper.Types.InlineQueryResult;
using CoffeeJelly.TelegramApiWrapper.Types.Messages;
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

            _botActions = new BotActions(token);

            _authorizer = Authorizer.GetInstance(token, updatesHandler, clientSecret);
            _updatesHandler = updatesHandler;
            ClientSecret = clientSecret;
            _updatesHandler.TelegramTextMessageEvent += _updatesHandler_TelegramTextMessageEvent;
            _updatesHandler.TelegramCallbackQueryEvent += _updatesHandler_TelegramCallbackQueryEvent;
            _updatesHandler.TelegramInlineQueryEvent += _updatesHandler_TelegramInlineQueryEvent;
            _updatesHandler.TelegramChosenInlineEvent += _updatesHandler_TelegramChosenInlineEvent;
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
            if (message?.Text == null)
                throw new ArgumentNullException(nameof(message));

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
                    await _botActions.WrongCredentialsMessage(message.From);
                }
                catch (Exception ex)
                {
                    LogMaker.Log(Logger, ex);
                    Debug.Assert(false, $"Message to chat abou exeption");
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
                catch (Exception ex)
                {
                    LogMaker.Log(Logger, ex);
                    Debug.Assert(false, $"Message to chat abou exeption");
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
                    Debug.Assert(false, $"Message to chat abou exeption");

                }
            }
            #endregion
            if (message.Text == Commands.INBOX_COMMAND)
            {
                logCommandRecieved(Commands.INBOX_COMMAND);
                try
                {
                    await HandleGetInboxMessagesCommand(message);
                }
                catch (Exception ex)
                {
                    LogMaker.Log(Logger, ex);
                    Debug.Assert(false, $"Message to chat abou exeption");
                }
            }
            #region system delete message
            //if (message.Text == Commands.PROCEED_EDIT_MESSAGE_COMMAND)
            //{
            //    logCommandRecieved(Commands.PROCEED_EDIT_MESSAGE_COMMAND);
            //    try
            //    {
            //        await _botActions.EditProceedMessage(message.Chat.Id.ToString(), message.MessageId.ToString());
            //    }
            //    catch(Exception ex)
            //    {
            //        LogMaker.Log(Logger, ex);
            //    }
            //}
            #endregion

        }

        private async void _updatesHandler_TelegramCallbackQueryEvent(CallbackQuery callbackQuery)
        {
            if (callbackQuery?.Data == null)
                throw new ArgumentNullException(nameof(callbackQuery));

            var logCommandRecieved = new Action<string>(command => LogMaker.Log(Logger, $"{command} command received from user with id {(string)callbackQuery.From}", false));
            if (callbackQuery.Data == Commands.CONNECT_COMMAND)
            {
                logCommandRecieved(Commands.CONNECT_COMMAND);
                try
                {
                    await _authorizer.SendAuthorizeLink(callbackQuery);
                }
                catch (AuthorizeException ex)
                {
                    LogMaker.Log(Logger, ex);
                    Debug.Assert(false, $"Message to chat about exeption");
                }
            }
        }

        private async void _updatesHandler_TelegramInlineQueryEvent(InlineQuery inlineQuery)
        {
            if (inlineQuery?.Query == null)
                throw new ArgumentNullException(nameof(inlineQuery));

            var logCommandRecieved = new Action<string>(command => LogMaker.Log(Logger, $"{command} command received from user with id {(string)inlineQuery.From}", false));
            if (inlineQuery.Query == Commands.INBOX_INLINE_QUERY_COMMAND)
            {
                logCommandRecieved(Commands.INBOX_INLINE_QUERY_COMMAND);
                try
                {
                    await HandleInboxInlineQueryCommand(inlineQuery);
                }
                catch (Exception ex)
                {
                    LogMaker.Log(Logger, ex);
                    Debug.Assert(false, $"Message to chat about exeption");
                }
            }
        }

        private async void _updatesHandler_TelegramChosenInlineEvent(ChosenInlineResult chosenInlineResult)
        {
            if (chosenInlineResult?.Query == null)
                throw new ArgumentNullException(nameof(chosenInlineResult), $"{nameof(chosenInlineResult.Query)} must not be a null.");
            if (chosenInlineResult.ResultId == null)
                throw new ArgumentNullException(nameof(chosenInlineResult), $"{nameof(chosenInlineResult.ResultId)} must not be a null.");

            var logCommandRecieved = new Action<string>(command => LogMaker.Log(Logger, $"{command} command received from user with id {(string)chosenInlineResult.From}", false));
            if (chosenInlineResult.Query == Commands.INBOX_INLINE_QUERY_COMMAND)
            {
                logCommandRecieved(Commands.INBOX_INLINE_QUERY_COMMAND);
                try
                {
                    await HandleInboxChosenInineResult(chosenInlineResult);
                }
                catch (Exception ex)
                {
                    LogMaker.Log(Logger, ex);
                    Debug.Assert(false, $"Message to chat about exeption");
                }
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
            await _botActions.EmailAddressMessage(sender.From, userinfo.Name);
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
            var formatedMessage = new FormattedGmailMessage(mailInfoResponce);


            Debug.WriteLine(formatedMessage.Body);

        }

        private async Task HandleGetInboxMessagesCommand(ISender sender)
        {
            await _botActions.GmailInlineCommandMessage(sender.From);
        }

        private async Task HandleInboxInlineQueryCommand(InlineQuery sender)
        {
            var service = SearchServiceByUserId(sender.From);
            var query = service.GmailService.Users.Messages.List("me");
            query.LabelIds = "INBOX";
            var listMessagesResponce = await query.ExecuteAsync();
            if (listMessagesResponce?.Messages == null || listMessagesResponce.Messages.Count == 0)
            {
                await _botActions.EmptyInboxMessage(sender.From);
                return;
            }

            var formatedMessages = new List<FormattedGmailMessage>();
            foreach (var message in listMessagesResponce.Messages.Take(5))
            {
                var mailInfoRequest = service.GmailService.Users.Messages.Get("me", message.Id);
                mailInfoRequest.Format = UsersResource.MessagesResource.GetRequest.FormatEnum.Metadata;
                var mailInfoResponce = await mailInfoRequest.ExecuteAsync();
                if (mailInfoResponce == null) continue;
                formatedMessages.Add(new FormattedGmailMessage(mailInfoResponce));
            }
            await _botActions.InboxAnswerInlineQuery(sender.Id, formatedMessages);
        }

        private async Task HandleInboxChosenInineResult(ChosenInlineResult chosenInlineResult)
        {
            var messageId = chosenInlineResult.ResultId;
            var service = SearchServiceByUserId(chosenInlineResult.From);
            var query = service.GmailService.Users.Messages.Get("me", messageId);
            var messageResponce = await query.ExecuteAsync();
            var formattedMessage = new FormattedGmailMessage(messageResponce);

            //await _botActions.InboxAnswerInlineQuery(sender.Id, formatedMessages);
        }

        private void PrepareMessage(ref string message)
        {
            
        }



        private UpdatesHandler _updatesHandler;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static readonly object _locker = new object();
        private BotActions _botActions;
        private Authorizer _authorizer;

        public static CommandHandler Instance { get; private set; }


        public Secrets ClientSecret { get; set; }


    }
}