using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase;
using CoffeeJelly.gmailNotifyBot.Bot.Exceptions;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.TelegramBotApiWrapper.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;
using CoffeeJelly.TelegramBotApiWrapper.Types.InlineQueryResult;
using CoffeeJelly.TelegramBotApiWrapper.Types.Messages;
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
            _dbWorker = new GmailDbContextWorker();
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
                    Debug.Assert(false, $"Message to chat about Commands.TESTNAME_COMMAND exeption");
                    // throw new AuthorizeException("An error occurred while trying to send the authentication link to the user", ex);
                }
            }
            #endregion
            else if (message.Text == Commands.TESTMESSAGE_COMMAND)
            {
                logCommandRecieved(Commands.TESTMESSAGE_COMMAND);
                try
                {
                    await HandleTestMessageCommand(message);
                }
                catch (CommandHandlerException ex)
                {
                    LogMaker.Log(Logger, ex);
                    await _botActions.WrongCredentialsMessage(message.From);
                }
                catch (Exception ex)
                {
                    LogMaker.Log(Logger, ex);
                    Debug.Assert(false, $"Message to chat about Commands.TESTMESSAGE_COMMAND exeption");
                }
            }
            #region connect
            else if (message.Text == Commands.CONNECT_COMMAND)
            {
                logCommandRecieved(message.Text);
                try
                {
                    await _authorizer.SendAuthorizeLink(message);
                }
                catch (CommandHandlerException ex)
                {
                    LogMaker.Log(Logger, ex);
                    await _botActions.WrongCredentialsMessage(message.From);
                }
                catch (AuthorizeException ex)
                {
                    LogMaker.Log(Logger, ex);
                    Debug.Assert(false, $"Message to chat about Commands.CONNECT_COMMAND exeption");

                }
            }
            #endregion
            else if (message.Text == Commands.INBOX_COMMAND)
            {
                logCommandRecieved(message.Text);
                try
                {
                    await HandleGetInboxMessagesCommand(message);
                }
                catch (CommandHandlerException ex)
                {
                    LogMaker.Log(Logger, ex);
                    await _botActions.WrongCredentialsMessage(message.From);
                }
                catch (Exception ex)
                {
                    LogMaker.Log(Logger, ex);
                    Debug.Assert(false, $"Message to chat about Commands.INBOX_COMMAND exeption");
                }
            }
            #region system delete message
            //else if (message.Text == Commands.PROCEED_EDIT_MESSAGE_COMMAND)
            //{
            //    logCommandRecieved(Commands.PROCEED_EDIT_MESSAGE_COMMAND);
            //    try
            //    {
            //        await _botActions.EditProceedMessage(message.Chat.Id.ToString(), message.MessageId.ToString());
            //    }
            //catch (CommandHandlerException ex)
            //    {
            //        LogMaker.Log(Logger, ex);
            //        await _botActions.WrongCredentialsMessage(message.From);
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
                    Debug.Assert(false, $"Message to chat about _updatesHandler_TelegramCallbackQueryEvent exeption");
                }
            }
            else if (callbackQuery.Data.StartsWith(Commands.EXPAND_COMMAND))
            {
                logCommandRecieved(callbackQuery.Data);
                try
                {
                    var callbackData = new CallbackData(callbackQuery.Data);
                    await HandleCallbackQueryExpandCommand(callbackQuery, callbackData.MessageId, callbackData.Page, callbackData.MessageKeyboardState);
                }
                catch (CommandHandlerException ex)
                {
                    LogMaker.Log(Logger, ex);
                    await _botActions.WrongCredentialsMessage(callbackQuery.From);
                }
                catch (Exception ex)
                {
                    LogMaker.Log(Logger, ex);
                    Debug.Assert(false, $"Message to chat about Commands.EXPAND_COMMAND exeption");
                }
            }
        }

        private async void _updatesHandler_TelegramInlineQueryEvent(InlineQuery inlineQuery)
        {
            if (inlineQuery?.Query == null)
                throw new ArgumentNullException(nameof(inlineQuery));

            var logCommandRecieved = new Action<string>(command => LogMaker.Log(Logger, $"{command} command received from user with id {(string)inlineQuery.From}", false));
            if (inlineQuery.Query == Commands.INBOX_INLINE_QUERY_COMMAND || inlineQuery.Query == Commands.ALL_INLINE_QUERY_COMMAND)
            {
                logCommandRecieved(inlineQuery.Query);
                try
                {
                    var labelId = "";
                    if (inlineQuery.Query == Commands.INBOX_INLINE_QUERY_COMMAND)
                        labelId = "INBOX";
                    else if (inlineQuery.Query == Commands.ALL_INLINE_QUERY_COMMAND)
                        labelId = "ALL";
                    await HandleShowMessagesInlineQueryCommand(inlineQuery, labelId);
                }
                catch (Exception ex)
                {
                    LogMaker.Log(Logger, ex);
                    Debug.Assert(false, $"Message to chat about _updatesHandler_TelegramInlineQueryEvent exeption");
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
            if (chosenInlineResult.Query == Commands.INBOX_INLINE_QUERY_COMMAND || chosenInlineResult.Query == Commands.ALL_INLINE_QUERY_COMMAND)
            {
                logCommandRecieved(chosenInlineResult.Query);
                try
                {
                    await HandleGetMesssagesChosenInlineResult(chosenInlineResult);
                }
                catch (Exception ex)
                {
                    LogMaker.Log(Logger, ex);
                    Debug.Assert(false, $"Message to chat about _updatesHandler_TelegramChosenInlineEvent exeption");
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

        private async Task HandleGetInboxMessagesCommand(Message sender)
        {
            await _botActions.GmailInlineCommandMessage(sender.From);
        }

        private async Task HandleShowMessagesInlineQueryCommand(InlineQuery sender, string labelId)
        {
            var service = SearchServiceByUserId(sender.From);
            var query = service.GmailService.Users.Messages.List("me");
            query.LabelIds = labelId;
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
            await _botActions.ShowShortMessageAnswerInlineQuery(sender.Id, formatedMessages);
        }

        private async Task HandleGetMesssagesChosenInlineResult(ChosenInlineResult sender)
        {
            var messageId = sender.ResultId;
            var formattedMessage = await FormateGmailMessage(sender, messageId);
            var isIgnored = await _dbWorker.IsPresentInIgnoreListAsync(sender.From, formattedMessage.SenderEmail);
            await _botActions.ChosenShortMessage(sender.From, formattedMessage, isIgnored);
        }

        /// <summary>
        /// Handles <see cref="CallbackQuery"/> <see cref="Commands.EXPAND_COMMAND"/>.
        /// This method returns the page <paramref name="page"/> of a message with id=<paramref name="messageId"/> to the chat with updated <see cref="InlineKeyboardMarkup"/> (page slider keyboard)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="messageId"></param>
        /// <param name="page"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        private async Task HandleCallbackQueryExpandCommand(CallbackQuery sender, string messageId, int page, MessageKeyboardState state)
        {
            if (state == MessageKeyboardState.Maximized || state == MessageKeyboardState.MaximizedActions)
                throw new ArgumentException("Must be a minimized or minimizedAction state.", nameof(state));

            var formattedMessage = await FormateGmailMessage(sender, messageId);
            var isIgnored = await _dbWorker.IsPresentInIgnoreListAsync(sender.From, formattedMessage.SenderEmail);
            var newState = state == MessageKeyboardState.Minimized
                ? MessageKeyboardState.Maximized
                : MessageKeyboardState.MaximizedActions;
            var newPage = page == 0 ? 1 : page;
            await _botActions.ShowMessage(sender.Message.Chat, sender.Message.MessageId, formattedMessage, newPage, newState, isIgnored);

        }

        private async Task<FormattedGmailMessage> FormateGmailMessage(ISender sender, string messageId)
        {
            var service = SearchServiceByUserId(sender.From);
            var query = service.GmailService.Users.Messages.Get("me", messageId);
            var messageResponce = await query.ExecuteAsync();
            return new FormattedGmailMessage(messageResponce);
        }

        private UpdatesHandler _updatesHandler;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static readonly object _locker = new object();
        private BotActions _botActions;
        private Authorizer _authorizer;
        private GmailDbContextWorker _dbWorker;

        public static CommandHandler Instance { get; private set; }


        public Secrets ClientSecret { get; set; }


    }
}