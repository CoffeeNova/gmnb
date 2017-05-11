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
using Google.Apis.Gmail.v1.Data;
using NLog;
using Message = CoffeeJelly.TelegramBotApiWrapper.Types.Messages.Message;

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
            if (!message.Text.StartsWithAny(Commands.TESTNAME_COMMAND, Commands.TESTMESSAGE_COMMAND,
                    Commands.CONNECT_COMMAND, Commands.INBOX_COMMAND, Commands.TESTTHREAD_COMMAND)) return;

            LogMaker.Log(Logger, $"{message.Text} command received from user with id {(string)message.From}", false);
            try
            {
                if (message.Text.StartsWith(Commands.TESTNAME_COMMAND))
                    await HandleTestNameCommand(message);
                else if (message.Text.StartsWith(Commands.TESTMESSAGE_COMMAND))
                {
                    var splittedtext = message.Text.Split(' ');
                    var messageID = splittedtext.Length > 1 ? splittedtext[1] : "";
                    await HandleTestMessageCommand(message, messageID);
                }
                else if (message.Text == Commands.CONNECT_COMMAND)
                    await _authorizer.SendAuthorizeLink(message);
                else if (message.Text == Commands.INBOX_COMMAND)
                    await HandleGetInboxMessagesCommand(message);
                else if (message.Text == Commands.TESTTHREAD_COMMAND)
                    await HandleTestThreadCommand(message);
            }
            catch (ServiceNotFoundException ex)
            {
                LogMaker.Log(Logger, ex);
                await _botActions.WrongCredentialsMessage(message.From);
            }
            catch (AuthorizeException ex)
            {
                LogMaker.Log(Logger, ex);
                await _botActions.AuthorizationErrorMessage(message.Chat);
            }
            catch (Exception ex)
            {
                LogMaker.Log(Logger, ex, $"An exception has been thrown in processing TextMessage with command {message.Text}");
            }
        }

        private async void _updatesHandler_TelegramCallbackQueryEvent(CallbackQuery callbackQuery)
        {
            if (callbackQuery?.Data == null)
                throw new ArgumentNullException(nameof(callbackQuery));

            if (!callbackQuery.Data.StartsWithAny(Commands.CONNECT_COMMAND, Commands.EXPAND_COMMAND,
                    Commands.HIDE_COMMAND, Commands.EXPAND_ACTIONS_COMMAND, Commands.HIDE_ACTIONS_COMMAND,
                    Commands.TO_READ_COMMAND, Commands.TO_UNREAD_COMMAND, Commands.TO_SPAM_COMMAND,
                    Commands.REMOVE_SPAM_COMMAND, Commands.NEXTPAGE_COMMAND, Commands.PREVPAGE_COMMAND)) return;

            LogMaker.Log(Logger,
                $"{callbackQuery.Data} command received from user with id {(string)callbackQuery.From}", false);
            try
            {
                var callbackData = new CallbackData(callbackQuery.Data);

                if (callbackData.Command == Commands.CONNECT_COMMAND)
                    await _authorizer.SendAuthorizeLink(callbackQuery);

                else if (callbackData.Command == Commands.EXPAND_COMMAND)
                    await HandleCallbackQueryExpandCommand(callbackQuery, callbackData);

                else if (callbackData.Command == Commands.HIDE_COMMAND)
                    await HandleCallbackQueryHideCommand(callbackQuery, callbackData);

                else if (callbackData.Command == Commands.EXPAND_ACTIONS_COMMAND)
                    await HandleCallbackQueryExpandActionsCommand(callbackQuery, callbackData);

                else if (callbackData.Command == Commands.HIDE_ACTIONS_COMMAND)
                    await HandleCallbackQueryHideActionsCommand(callbackQuery, callbackData);

                else if (callbackData.Command == Commands.TO_READ_COMMAND)
                    await HandleCallbackQueryToReadCommand(callbackQuery, callbackData);

                else if (callbackData.Command == Commands.TO_UNREAD_COMMAND)
                    await HandleCallbackQueryToUnReadCommand(callbackQuery, callbackData);

                else if (callbackData.Command == Commands.TO_SPAM_COMMAND)
                    await HandleCallbackQueryToSpamCommand(callbackQuery, callbackData);

                else if (callbackData.Command == Commands.TO_INBOX_COMMAND)
                    await HandleCallbackQueryRemoveSpamCommand(callbackQuery, callbackData);

                else if (callbackData.Command == Commands.DELETE_COMMAND)
                    await HandleCallbackQueryToTrashCommand(callbackQuery, callbackData);

                else if (callbackData.Command == Commands.ARCHIVE_COMMAND)
                    await HandleCallbackQueryArchiveCommand(callbackQuery, callbackData);

                else if (callbackData.Command == Commands.UNIGNORE_COMMAND)
                    await HandleCallbackQueryUnignoreCommand(callbackQuery, callbackData);

                else if (callbackData.Command == Commands.IGNORE_COMMAND)
                    await HandleCallbackQueryIgnoreCommand(callbackQuery, callbackData);

                else if (callbackData.Command == Commands.NEXTPAGE_COMMAND)
                    await HandleCallbackQueryNextPageCommand(callbackQuery, callbackData);

                else if (callbackData.Command == Commands.PREVPAGE_COMMAND)
                    await HandleCallbackQueryPrevPageCommand(callbackQuery, callbackData);
            }
            catch (AuthorizeException ex)
            {
                LogMaker.Log(Logger, ex);
                await _botActions.AuthorizationErrorMessage(callbackQuery.Message.Chat);
            }
            catch (ServiceNotFoundException ex)
            {
                LogMaker.Log(Logger, ex);
                await _botActions.WrongCredentialsMessage(callbackQuery.Message.Chat);
            }
            catch (Exception ex)
            {
                LogMaker.Log(Logger, ex, $"An exception has been thrown in processing CallbackQuery with command {callbackQuery.Data}");
            }
        }

        private async void _updatesHandler_TelegramInlineQueryEvent(InlineQuery inlineQuery)
        {
            if (inlineQuery?.Query == null)
                throw new ArgumentNullException(nameof(inlineQuery));

            if (!inlineQuery.Query.StartsWithAny(Commands.INBOX_INLINE_QUERY_COMMAND, Commands.ALL_INLINE_QUERY_COMMAND)) return;

            LogMaker.Log(Logger, $"{inlineQuery.Query} command received from user with id {(string)inlineQuery.From}", false);
            try
            {
                var labelId = "";
                if (inlineQuery.Query.StartsWith(Commands.INBOX_INLINE_QUERY_COMMAND))
                    labelId = "INBOX";

                var splittedQuery = inlineQuery.Query.Split(' ');
                var pageStr = splittedQuery.Length > 1 ? splittedQuery[1] : "";
                int page;
                page = Int32.TryParse(pageStr, out page) == false ? 1 : page;
                await HandleShowMessagesInlineQueryCommand(inlineQuery, labelId, page);
            }
            catch (ServiceNotFoundException ex)
            {
                LogMaker.Log(Logger, ex);
                await _botActions.WrongCredentialsMessage(inlineQuery.From);
            }
            catch (Exception ex)
            {
                LogMaker.Log(Logger, ex, $"An exception has been thrown in processing InlineQuery with command {inlineQuery.Query}");
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

        private async Task HandleTestNameCommand(ISender sender)
        {
            var service = SearchServiceByUserId(sender.From);
            var query = service.Oauth2Service.Userinfo.Get();
            var userinfo = await query.ExecuteAsync();
            await _botActions.EmailAddressMessage(sender.From, userinfo.Name);
        }

        private async Task HandleTestMessageCommand(ISender sender, string messageId)
        {
            var service = SearchServiceByUserId(sender.From);
            var query = service.GmailService.Users.Messages.List("me");
            //query.LabelIds = "INBOX";
            var listMessagesResponce = await query.ExecuteAsync();
            if (listMessagesResponce?.Messages == null) return;

            var message = listMessagesResponce.Messages.First();
            if (!string.IsNullOrEmpty(messageId))
                message = listMessagesResponce.Messages.SingleOrDefault(m => m.Id == messageId);
            if (message == null)
                return;

            var getMailRequest = service.GmailService.Users.Messages.Get("me", message.Id);
            var mailInfoResponce = await getMailRequest.ExecuteAsync();
            if (mailInfoResponce == null) return;
            var formattedMessage = new FormattedGmailMessage(mailInfoResponce);
            var isIgnored = await _dbWorker.IsPresentInIgnoreListAsync(sender.From, formattedMessage.SenderEmail);
            await _botActions.ChosenShortMessage(sender.From, formattedMessage, isIgnored);
        }

        private async Task HandleTestThreadCommand(ISender sender)
        {
            var service = SearchServiceByUserId(sender.From);
            var query = service.GmailService.Users.Threads.List("me");
            query.LabelIds = "INBOX";
            var listThreadsResponce = await query.ExecuteAsync();
            if (listThreadsResponce?.Threads == null) return;

            var thread = listThreadsResponce.Threads.First();
            var getMailRequest = service.GmailService.Users.Messages.Get("me", thread.Id);
            var mailInfoResponce = await getMailRequest.ExecuteAsync();
            if (mailInfoResponce == null) return;
            var formattedMessage = new FormattedGmailMessage(mailInfoResponce);
            var isIgnored = await _dbWorker.IsPresentInIgnoreListAsync(sender.From, formattedMessage.SenderEmail);
            await _botActions.ChosenShortMessage(sender.From, formattedMessage, isIgnored);
        }

        private async Task HandleGetInboxMessagesCommand(Message sender)
        {
            await _botActions.GmailInlineCommandMessage(sender.From);
        }

        private async Task HandleShowMessagesInlineQueryCommand(InlineQuery sender, string labelId, int page = 1)
        {
            if (page < 1)
                throw new ArgumentOutOfRangeException(nameof(page), "Must be not lower then 1");

            var service = SearchServiceByUserId(sender.From);

            var query = service.GmailService.Users.Messages.List("me");
            if (string.IsNullOrEmpty(labelId))
                query.IncludeSpamTrash = false;
            else
                query.LabelIds = labelId;

            query.MaxResults = 50;
            int offset;
            Int32.TryParse(sender.Offset, out offset);
            if (offset >= 50)
            {
                page++;
                offset = offset - 50;
            }
            ListMessagesResponse listMessagesResponce = null;
            string pageToken = null;
            while (page >= 1)
            {
                query.PageToken = pageToken;
                listMessagesResponce = await query.ExecuteAsync();
                if (string.IsNullOrEmpty(listMessagesResponce.NextPageToken))
                    break;
                pageToken = listMessagesResponce.NextPageToken;
                page--;
            }
            if (listMessagesResponce?.Messages == null || listMessagesResponce.Messages.Count == 0)
            {
                if (string.IsNullOrEmpty(labelId))
                    await _botActions.EmptyAllMessage(sender.From, page);
                else
                    await _botActions.EmptyLabelMessage(sender.From, labelId, page);
                return;
            }
            var formatedMessages = new List<FormattedGmailMessage>();
            foreach (var message in listMessagesResponce.Messages.Skip(offset).Take(5))
            {
                var getMailRequest = service.GmailService.Users.Messages.Get("me", message.Id);
                getMailRequest.Format = UsersResource.MessagesResource.GetRequest.FormatEnum.Metadata;
                var mailInfoResponce = await getMailRequest.ExecuteAsync();
                if (mailInfoResponce == null) continue;
                formatedMessages.Add(new FormattedGmailMessage(mailInfoResponce));
            }

            await _botActions.ShowShortMessageAnswerInlineQuery(sender.Id, formatedMessages, offset + 5);
        }

        private async Task HandleGetMesssagesChosenInlineResult(ChosenInlineResult sender)
        {
            var messageId = sender.ResultId;
            var formattedMessage = await GetMessage(sender.From, messageId);
            var isIgnored = await _dbWorker.IsPresentInIgnoreListAsync(sender.From, formattedMessage.SenderEmail);
            await _botActions.ChosenShortMessage(sender.From, formattedMessage, isIgnored);
        }

        /// <summary>
        /// Handles <see cref="CallbackQuery"/> <see cref="Commands.EXPAND_COMMAND"/>.
        /// This method calls <see cref="BotActions.UpdateMessage"/> method for message with <paramref name="callbackData"/>
        ///  which <see cref="CallbackData.MessageKeyboardState"/> equals <see langword="MessageKeyboardState.Maximized"/> or 
        /// <see langword="MessageKeyboardState.MaximizedActions"/> which updates it to the 1st page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="callbackData"></param>
        /// <returns></returns>
        private async Task HandleCallbackQueryExpandCommand(CallbackQuery sender, CallbackData callbackData)
        {
            if (callbackData.MessageKeyboardState.EqualsAny(MessageKeyboardState.Maximized, MessageKeyboardState.MaximizedActions))
                throw new ArgumentException("Must be a Minimized or MinimizedAction state.", nameof(callbackData.MessageKeyboardState));

            var formattedMessage = await GetMessage(sender.From, callbackData.MessageId);
            var isIgnored = await _dbWorker.IsPresentInIgnoreListAsync(sender.From, formattedMessage.SenderEmail);
            var newState = callbackData.MessageKeyboardState == MessageKeyboardState.Minimized
                ? MessageKeyboardState.Maximized
                : MessageKeyboardState.MaximizedActions;
            var newPage = 1;
            await _botActions.UpdateMessage(sender.Message.Chat, sender.Message.MessageId, formattedMessage, newPage, newState, isIgnored);
        }

        /// <summary>
        /// Handles <see cref="CallbackQuery"/> <see cref="Commands.HIDE_COMMAND"/>.
        /// This method calls <see cref="BotActions.UpdateMessage"/> method for message with <paramref name="callbackData"/>
        /// which <see cref="CallbackData.MessageKeyboardState"/> equals <see langword="MessageKeyboardState.Minimized"/> or 
        /// <see langword="MessageKeyboardState.MinimizedActions"/> which updates it to the 0 page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="callbackData"></param>
        /// <returns></returns>
        private async Task HandleCallbackQueryHideCommand(CallbackQuery sender, CallbackData callbackData)
        {
            if (callbackData.MessageKeyboardState.EqualsAny(MessageKeyboardState.Minimized, MessageKeyboardState.MinimizedActions))
                throw new ArgumentException("Must be a Maximized or MaximizedAction state.", nameof(callbackData.MessageKeyboardState));

            var formattedMessage = await GetMessage(sender.From, callbackData.MessageId);
            var isIgnored = await _dbWorker.IsPresentInIgnoreListAsync(sender.From, formattedMessage.SenderEmail);
            var newState = callbackData.MessageKeyboardState == MessageKeyboardState.Maximized
                ? MessageKeyboardState.Minimized
                : MessageKeyboardState.MinimizedActions;
            var newPage = 0;
            await _botActions.UpdateMessage(sender.Message.Chat, sender.Message.MessageId, formattedMessage, newPage, newState, isIgnored);

        }

        /// <summary>
        /// Handles <see cref="CallbackQuery"/> <see cref="Commands.EXPAND_ACTIONS_COMMAND"/>.
        /// This method calls <see cref="BotActions.UpdateMessage"/> method for message with <paramref name="callbackData"/>
        /// which <see cref="CallbackData.MessageKeyboardState"/> equals <see langword="MessageKeyboardState.MinimizedActions"/> or 
        /// <see langword="MessageKeyboardState.MaximizedActions"/> which updates it on the set <see cref="CallbackData.Page"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="callbackData"></param>
        /// <returns></returns>
        private async Task HandleCallbackQueryExpandActionsCommand(CallbackQuery sender, CallbackData callbackData)
        {
            if (callbackData.MessageKeyboardState.EqualsAny(MessageKeyboardState.MinimizedActions, MessageKeyboardState.MaximizedActions))
                throw new ArgumentException("Must be a Minimized or Maximized state.", nameof(callbackData.MessageKeyboardState));

            var formattedMessage = await ModifyMessageLabels(ModifyLabelsAction.Remove, sender.From, callbackData.MessageId, callbackData.Etag, "UNREAD");
            var isIgnored = await _dbWorker.IsPresentInIgnoreListAsync(sender.From, formattedMessage.SenderEmail);
            var newState = callbackData.MessageKeyboardState == MessageKeyboardState.Minimized
                ? MessageKeyboardState.MinimizedActions
                : MessageKeyboardState.MaximizedActions;
            await _botActions.UpdateMessage(sender.Message.Chat, sender.Message.MessageId, formattedMessage, callbackData.Page, newState, isIgnored);
        }

        /// <summary>
        /// Handles <see cref="CallbackQuery"/> <see cref="Commands.HIDE_ACTIONS_COMMAND"/>.
        /// This method calls <see cref="BotActions.UpdateMessage"/> method for message with <paramref name="callbackData"/>
        /// which <see cref="CallbackData.MessageKeyboardState"/> equals <see langword="MessageKeyboardState.Minimized"/> or 
        /// <see langword="MessageKeyboardState.Maximized"/> which updates it on the set <see cref="CallbackData.Page"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="callbackData"></param>
        /// <returns></returns>
        private async Task HandleCallbackQueryHideActionsCommand(CallbackQuery sender, CallbackData callbackData)
        {
            if (callbackData.MessageKeyboardState.EqualsAny(MessageKeyboardState.Minimized, MessageKeyboardState.Maximized))
                throw new ArgumentException("Must be a MinimizedActions or MaximizedActions state.", nameof(callbackData.MessageKeyboardState));

            var formattedMessage = await GetMessage(sender.From, callbackData.MessageId);
            var isIgnored = await _dbWorker.IsPresentInIgnoreListAsync(sender.From, formattedMessage.SenderEmail);
            var newState = callbackData.MessageKeyboardState == MessageKeyboardState.MinimizedActions
                ? MessageKeyboardState.Minimized
                : MessageKeyboardState.Maximized;
            await _botActions.UpdateMessage(sender.Message.Chat, sender.Message.MessageId, formattedMessage, callbackData.Page, newState, isIgnored);
        }

        /// <summary>
        /// Handles <see cref="CallbackQuery"/> <see cref="Commands.TO_READ_COMMAND"/>.
        /// This method removes message's "UNREAD" label and calls <see cref="BotActions.UpdateMessage"/> method for message with <paramref name="callbackData"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="callbackData"></param>
        /// <returns></returns>
        private async Task HandleCallbackQueryToReadCommand(CallbackQuery sender, CallbackData callbackData)
        {
            var formattedMessage = await ModifyMessageLabels(ModifyLabelsAction.Remove, sender.From, callbackData.MessageId, callbackData.Etag, "UNREAD");
            var isIgnored = await _dbWorker.IsPresentInIgnoreListAsync(sender.From, formattedMessage.SenderEmail);

            await _botActions.UpdateMessage(sender.Message.Chat, sender.Message.MessageId, formattedMessage, callbackData.Page, callbackData.MessageKeyboardState, isIgnored);
        }

        /// <summary>
        /// Handles <see cref="CallbackQuery"/> <see cref="Commands.TO_READ_COMMAND"/>.
        /// This method adds "UNREAD" label to message and calls <see cref="BotActions.UpdateMessage"/> method for message with <paramref name="callbackData"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="callbackData"></param>
        /// <returns></returns>
        private async Task HandleCallbackQueryToUnReadCommand(CallbackQuery sender, CallbackData callbackData)
        {
            var formattedMessage = await ModifyMessageLabels(ModifyLabelsAction.Add, sender.From, callbackData.MessageId, callbackData.Etag, "UNREAD");
            var isIgnored = await _dbWorker.IsPresentInIgnoreListAsync(sender.From, formattedMessage.SenderEmail);

            await _botActions.UpdateMessage(sender.Message.Chat, sender.Message.MessageId, formattedMessage, callbackData.Page, callbackData.MessageKeyboardState, isIgnored);
        }

        /// <summary>
        /// Handles <see cref="CallbackQuery"/> <see cref="Commands.TO_SPAM_COMMAND"/>.
        /// This method adds "SPAM" label to message and calls <see cref="BotActions.UpdateMessage"/> method for message with <paramref name="callbackData"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="callbackData"></param>
        /// <returns></returns>
        private async Task HandleCallbackQueryToSpamCommand(CallbackQuery sender, CallbackData callbackData)
        {
            var formattedMessage = await ModifyMessageLabels(ModifyLabelsAction.Add, sender.From, callbackData.MessageId, callbackData.Etag, "SPAM");
            var isIgnored = await _dbWorker.IsPresentInIgnoreListAsync(sender.From, formattedMessage.SenderEmail);

            await _botActions.UpdateMessage(sender.Message.Chat, sender.Message.MessageId, formattedMessage, callbackData.Page, callbackData.MessageKeyboardState, isIgnored);
        }

        /// <summary>
        /// Handles <see cref="CallbackQuery"/> <see cref="Commands.REMOVE_SPAM_COMMAND"/>.
        /// This method adds "INBOX" label to message and calls <see cref="BotActions.UpdateMessage"/> method for message with <paramref name="callbackData"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="callbackData"></param>
        /// <returns></returns>
        private async Task HandleCallbackQueryRemoveSpamCommand(CallbackQuery sender, CallbackData callbackData)
        {
            var formattedMessage = await ModifyMessageLabels(ModifyLabelsAction.Add, sender.From, callbackData.MessageId, callbackData.Etag, "INBOX");
            var isIgnored = await _dbWorker.IsPresentInIgnoreListAsync(sender.From, formattedMessage.SenderEmail);

            await _botActions.UpdateMessage(sender.Message.Chat, sender.Message.MessageId, formattedMessage, callbackData.Page, callbackData.MessageKeyboardState, isIgnored);
        }

        /// <summary>
        /// Handles <see cref="CallbackQuery"/> <see cref="Commands.REMOVE_SPAM_COMMAND"/>.
        /// This method adds "TRASH" label to message and calls <see cref="BotActions.UpdateMessage"/> method for message with <paramref name="callbackData"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="callbackData"></param>
        /// <returns></returns>
        private async Task HandleCallbackQueryToTrashCommand(CallbackQuery sender, CallbackData callbackData)
        {
            var formattedMessage = await ModifyMessageLabels(ModifyLabelsAction.Add, sender.From, callbackData.MessageId, callbackData.Etag, "TRASH");
            var isIgnored = await _dbWorker.IsPresentInIgnoreListAsync(sender.From, formattedMessage.SenderEmail);

            await _botActions.UpdateMessage(sender.Message.Chat, sender.Message.MessageId, formattedMessage, callbackData.Page, callbackData.MessageKeyboardState, isIgnored);
        }

        /// <summary>
        /// Handles <see cref="CallbackQuery"/> <see cref="Commands.REMOVE_SPAM_COMMAND"/>.
        /// This method removes "INBOX" label to message and calls <see cref="BotActions.UpdateMessage"/> method for message with <paramref name="callbackData"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="callbackData"></param>
        /// <returns></returns>
        private async Task HandleCallbackQueryArchiveCommand(CallbackQuery sender, CallbackData callbackData)
        {
            var formattedMessage = await ModifyMessageLabels(ModifyLabelsAction.Remove, sender.From, callbackData.MessageId, callbackData.Etag, "INBOX");
            var isIgnored = await _dbWorker.IsPresentInIgnoreListAsync(sender.From, formattedMessage.SenderEmail);

            await _botActions.UpdateMessage(sender.Message.Chat, sender.Message.MessageId, formattedMessage, callbackData.Page, callbackData.MessageKeyboardState, isIgnored);
        }

        /// <summary>
        /// Handles <see cref="CallbackQuery"/> <see cref="Commands.UNIGNORE_COMMAND"/>.
        /// Removes senders email address from db.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="callbackData"></param>
        /// <returns></returns>
        private async Task HandleCallbackQueryUnignoreCommand(CallbackQuery sender, CallbackData callbackData)
        {
            var formattedMessage = await GetMessage(sender.From, callbackData.MessageId);
            await _dbWorker.RemoveFromIgnoreListAsync(sender.From, formattedMessage.SenderEmail);
            await _botActions.UpdateMessage(sender.Message.Chat, sender.Message.MessageId, formattedMessage, callbackData.Page, callbackData.MessageKeyboardState, false);
        }

        /// <summary>
        /// Handles <see cref="CallbackQuery"/> <see cref="Commands.IGNORE_COMMAND"/>.
        /// Adds senders email address to db.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="callbackData"></param>
        /// <returns></returns>
        private async Task HandleCallbackQueryIgnoreCommand(CallbackQuery sender, CallbackData callbackData)
        {
            var formattedMessage = await GetMessage(sender.From, callbackData.MessageId);
            await _dbWorker.AddToIgnoreListAsync(sender.From, formattedMessage.SenderEmail);
            await _botActions.UpdateMessage(sender.Message.Chat, sender.Message.MessageId, formattedMessage, callbackData.Page, callbackData.MessageKeyboardState, false);
        }

        /// <summary>
        /// Handles <see cref="CallbackQuery"/> <see cref="Commands.NEXTPAGE_COMMAND"/>.
        /// This method calls <see cref="BotActions.UpdateMessage"/> method for message with <paramref name="callbackData"/> where Page property increased by 1.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="callbackData"></param>
        /// <returns></returns>
        private async Task HandleCallbackQueryNextPageCommand(CallbackQuery sender, CallbackData callbackData)
        {
            var formattedMessage = await GetMessage(sender.From, callbackData.MessageId);
            if (formattedMessage.FormattedBody.Count <= callbackData.Page)
                throw new InvalidOperationException("Execution of this method is not permissible in this situation");
            var isIgnored = await _dbWorker.IsPresentInIgnoreListAsync(sender.From, formattedMessage.SenderEmail);
            var newPage = callbackData.Page + 1;
            await
                _botActions.UpdateMessage(sender.Message.Chat, sender.Message.MessageId, formattedMessage, newPage,
                    callbackData.MessageKeyboardState, isIgnored);
        }

        /// <summary>
        /// Handles <see cref="CallbackQuery"/> <see cref="Commands.NEXTPAGE_COMMAND"/>.
        /// This method calls <see cref="BotActions.UpdateMessage"/> method for message with <paramref name="callbackData"/> where Page property decreased by 1.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="callbackData"></param>
        /// <returns></returns>
        private async Task HandleCallbackQueryPrevPageCommand(CallbackQuery sender, CallbackData callbackData)
        {
            var formattedMessage = await GetMessage(sender.From, callbackData.MessageId);
            if (callbackData.Page < 2)
                throw new InvalidOperationException("Execution of this method is not permissible in this situation");
            var isIgnored = await _dbWorker.IsPresentInIgnoreListAsync(sender.From, formattedMessage.SenderEmail);
            var newPage = callbackData.Page - 1;
            await _botActions.UpdateMessage(sender.Message.Chat, sender.Message.MessageId, formattedMessage, newPage, callbackData.MessageKeyboardState, isIgnored);
        }

        private Service SearchServiceByUserId(string userId)
        {
            var gmailServiceFactory = ServiceFactory.GetInstanse(ClientSecret);
            var service = gmailServiceFactory.ServiceCollection.FirstOrDefault(s => s.UserCredential.UserId == userId);
            if (service == null)
                throw new ServiceNotFoundException($"Service with credentials from user with id={userId} is not created. User, probably, is not authorized");
            return service;
        }


        private async Task<FormattedGmailMessage> GetMessage(string userId, string messageId)
        {
            var service = SearchServiceByUserId(userId);
            var query = service.GmailService.Users.Messages.Get("me", messageId);
            var messageResponce = await query.ExecuteAsync();
            return new FormattedGmailMessage(messageResponce);
        }

        private async Task<FormattedGmailMessage> ModifyMessageLabels(ModifyLabelsAction action, string userId, string messageId, string eTag = null, params string[] labels)
        {
            var labelsList = labels.ToList();
            if (action == ModifyLabelsAction.Add)
                return await ModifyMessageLabels(userId, messageId, labelsList, null, eTag);
            return await ModifyMessageLabels(userId, messageId, null, labelsList, eTag);
        }

        private async Task<FormattedGmailMessage> ModifyMessageLabels(string userId, string messageId, List<string> addedLabels = null, List<string> removedLabels = null, string eTag = null)
        {
            var service = SearchServiceByUserId(userId);
            var modifyMessageRequest = new ModifyMessageRequest
            {
                ETag = eTag,
                AddLabelIds = addedLabels,
                RemoveLabelIds = removedLabels
            };
            var modifyRequest = service.GmailService.Users.Messages.Modify(modifyMessageRequest, "me", messageId);
            var messageResponce = await modifyRequest.ExecuteAsync();
            var getRequest = service.GmailService.Users.Messages.Get("me", messageResponce.Id);
            messageResponce = await getRequest.ExecuteAsync();
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

    public enum ModifyLabelsAction
    {
        Add,
        Remove
    }
}