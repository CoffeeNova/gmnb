using System;
using System.Threading.Tasks;
using CoffeeJelly.gmailNotifyBot.Bot.Exceptions;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls
{
    public sealed partial class CommandHandler
    {
        private async void _updatesHandler_TelegramCallbackQueryEvent(CallbackQuery callbackQuery)
        {
            if (callbackQuery?.Data == null)
                throw new ArgumentNullException(nameof(callbackQuery));

            if (!callbackQuery.Data.StartsWithAny(StringComparison.CurrentCultureIgnoreCase,
                    Commands.CONNECT_COMMAND, Commands.EXPAND_COMMAND,
                    Commands.HIDE_COMMAND, Commands.EXPAND_ACTIONS_COMMAND, Commands.HIDE_ACTIONS_COMMAND,
                    Commands.TO_READ_COMMAND, Commands.TO_UNREAD_COMMAND, Commands.TO_SPAM_COMMAND,
                    Commands.TO_INBOX_COMMAND, Commands.TO_TRASHCOMMAND, Commands.ARCHIVE_COMMAND,
                    Commands.UNIGNORE_COMMAND, Commands.IGNORE_COMMAND, Commands.NEXTPAGE_COMMAND,
                      Commands.PREVPAGE_COMMAND, Commands.ADD_SUBJECT_COMMAND)) return;

            LogMaker.Log(Logger,
                $"{callbackQuery.Data} command received from user with id {(string)callbackQuery.From}", false);
            try
            {
                var callbackData = new CallbackData(callbackQuery.Data);

                if (callbackData.Command.Equals(Commands.CONNECT_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                    await _authorizer.SendAuthorizeLink(callbackQuery);

                else if (callbackData.Command.Equals(Commands.EXPAND_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                    await HandleCallbackQueryExpandCommand(callbackQuery, callbackData);

                else if (callbackData.Command.Equals(Commands.HIDE_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                    await HandleCallbackQueryHideCommand(callbackQuery, callbackData);

                else if (callbackData.Command.Equals(Commands.EXPAND_ACTIONS_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                    await HandleCallbackQueryExpandActionsCommand(callbackQuery, callbackData);

                else if (callbackData.Command.Equals(Commands.HIDE_ACTIONS_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                    await HandleCallbackQueryHideActionsCommand(callbackQuery, callbackData);

                else if (callbackData.Command.Equals(Commands.TO_READ_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                    await HandleCallbackQueryToReadCommand(callbackQuery, callbackData);

                else if (callbackData.Command.Equals(Commands.TO_UNREAD_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                    await HandleCallbackQueryToUnReadCommand(callbackQuery, callbackData);

                else if (callbackData.Command.Equals(Commands.TO_SPAM_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                    await HandleCallbackQueryToSpamCommand(callbackQuery, callbackData);

                else if (callbackData.Command.Equals(Commands.TO_INBOX_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                    await HandleCallbackQueryToInboxCommand(callbackQuery, callbackData);

                else if (callbackData.Command.Equals(Commands.TO_TRASHCOMMAND, StringComparison.CurrentCultureIgnoreCase))
                    await HandleCallbackQueryToTrashCommand(callbackQuery, callbackData);

                else if (callbackData.Command.Equals(Commands.ARCHIVE_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                    await HandleCallbackQueryArchiveCommand(callbackQuery, callbackData);

                else if (callbackData.Command.Equals(Commands.UNIGNORE_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                    await HandleCallbackQueryUnignoreCommand(callbackQuery, callbackData);

                else if (callbackData.Command.Equals(Commands.IGNORE_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                    await HandleCallbackQueryIgnoreCommand(callbackQuery, callbackData);

                else if (callbackData.Command.Equals(Commands.NEXTPAGE_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                    await HandleCallbackQueryNextPageCommand(callbackQuery, callbackData);

                else if (callbackData.Command.Equals(Commands.PREVPAGE_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                    await HandleCallbackQueryPrevPageCommand(callbackQuery, callbackData);

                else if (callbackData.Command.Equals(Commands.ADD_SUBJECT_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                    await HandleAddSubjectCommand(callbackQuery, callbackData);
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
        /// Handles <see cref="CallbackQuery"/> <see cref="Commands.TO_INBOX_COMMAND"/>.
        /// This method adds "INBOX" label to message and calls <see cref="BotActions.UpdateMessage"/> method for message with <paramref name="callbackData"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="callbackData"></param>
        /// <returns></returns>
        private async Task HandleCallbackQueryToInboxCommand(CallbackQuery sender, CallbackData callbackData)
        {
            var formattedMessage = await ModifyMessageLabels(ModifyLabelsAction.Add, sender.From, callbackData.MessageId, callbackData.Etag, "INBOX");
            var isIgnored = await _dbWorker.IsPresentInIgnoreListAsync(sender.From, formattedMessage.SenderEmail);

            await _botActions.UpdateMessage(sender.Message.Chat, sender.Message.MessageId, formattedMessage, callbackData.Page, callbackData.MessageKeyboardState, isIgnored);
        }

        /// <summary>
        /// Handles <see cref="CallbackQuery"/> <see cref="Commands.TO_TRASHCOMMAND"/>.
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
        /// Handles <see cref="CallbackQuery"/> <see cref="Commands.ARCHIVE_COMMAND"/>.
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
            if (formattedMessage.Pages <= callbackData.Page)
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

        private async Task HandleAddSubjectCommand(CallbackQuery sender, CallbackData callbackData)
        {

        }


    }


}