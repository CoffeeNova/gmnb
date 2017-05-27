using System;
using System.Threading.Tasks;
using CoffeeJelly.gmailNotifyBot.Bot.Exceptions;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity;
using CoffeeJelly.gmailNotifyBot.Bot.Moduls.GoogleRequests;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.CallbackQuery
{
    using Query = TelegramBotApiWrapper.Types.General.CallbackQuery;
    public partial class CallbackQueryHandler
    {
        /// <summary>
        /// Handles <see cref="Query"/> <see cref="Commands.CONNECT_COMMAND"/>.
        /// This method calls <see cref="Authorizer.SendAuthorizeLink"/> that forms URL link and provides it to the chat as a message.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task HandleCallbackQAuthorize(Query query)
        {
            var authorizer = BotInitializer.Instance?.Authorizer;
            if (authorizer == null)
                throw new AuthorizeException();
            await authorizer.SendAuthorizeLink(query);
        }

        /// <summary>
        /// Handles <see cref="Query"/> <see cref="Commands.EXPAND_COMMAND"/>.
        /// This method calls <see cref="BotActions.UpdateMessage"/> method for message with <paramref name="callbackData"/>
        ///  which <see cref="CallbackData.MessageKeyboardState"/> equals <see langword="MessageKeyboardState.Maximized"/> or 
        /// <see langword="MessageKeyboardState.MaximizedActions"/> which updates it to the 1st page.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="callbackData"></param>
        /// <returns></returns>
        public async Task HandleCallbackQExpand(Query query, CallbackData callbackData)
        {
            if (callbackData.MessageKeyboardState.EqualsAny(MessageKeyboardState.Maximized, MessageKeyboardState.MaximizedActions))
                throw new ArgumentException("Must be a Minimized or MinimizedAction state.", nameof(callbackData.MessageKeyboardState));

            var formattedMessage = await Methods.GetMessage(query.From, callbackData.MessageId);
            var isIgnored = await _dbWorker.IsPresentInIgnoreListAsync(query.From, formattedMessage.From.Email);
            var newState = callbackData.MessageKeyboardState == MessageKeyboardState.Minimized
                ? MessageKeyboardState.Maximized
                : MessageKeyboardState.MaximizedActions;
            var newPage = 1;
            await _botActions.UpdateMessage(query.From, query.Message.MessageId, newState, formattedMessage, newPage, isIgnored);
        }

        /// <summary>
        /// Handles <see cref="Query"/> <see cref="Commands.HIDE_COMMAND"/>.
        /// This method calls <see cref="BotActions.UpdateMessage"/> method for message with <paramref name="callbackData"/>
        /// which <see cref="CallbackData.MessageKeyboardState"/> equals <see langword="MessageKeyboardState.Minimized"/> or 
        /// <see langword="MessageKeyboardState.MinimizedActions"/> which updates it to the 0 page.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="callbackData"></param>
        /// <returns></returns>
        public async Task HandleCallbackQHide(Query query, CallbackData callbackData)
        {
            if (callbackData.MessageKeyboardState.EqualsAny(MessageKeyboardState.Minimized, MessageKeyboardState.MinimizedActions))
                throw new ArgumentException("Must be a Maximized or MaximizedAction state.", nameof(callbackData.MessageKeyboardState));

            var formattedMessage = await Methods.GetMessage(query.From, callbackData.MessageId);
            var isIgnored = await _dbWorker.IsPresentInIgnoreListAsync(query.From, formattedMessage.From.Email);
            var newState = callbackData.MessageKeyboardState == MessageKeyboardState.Maximized
                ? MessageKeyboardState.Minimized
                : MessageKeyboardState.MinimizedActions;
            var newPage = 0;
            await _botActions.UpdateMessage(query.From, query.Message.MessageId, newState, formattedMessage, newPage, isIgnored);

        }

        /// <summary>
        /// Handles <see cref="Query"/> <see cref="Commands.EXPAND_ACTIONS_COMMAND"/>.
        /// This method calls <see cref="BotActions.UpdateMessage"/> method for message with <paramref name="callbackData"/>
        /// which <see cref="CallbackData.MessageKeyboardState"/> equals <see langword="MessageKeyboardState.MinimizedActions"/> or 
        /// <see langword="MessageKeyboardState.MaximizedActions"/> which updates it on the set <see cref="CallbackData.Page"/>.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="callbackData"></param>
        /// <returns></returns>
        public async Task HandleCallbackQExpandActions(Query query, CallbackData callbackData)
        {
            if (callbackData.MessageKeyboardState.EqualsAny(MessageKeyboardState.MinimizedActions, MessageKeyboardState.MaximizedActions))
                throw new ArgumentException("Must be a Minimized or Maximized state.", nameof(callbackData.MessageKeyboardState));

            var formattedMessage = await Methods.ModifyMessageLabels(ModifyLabelsAction.Remove, query.From, callbackData.MessageId, null, "UNREAD");
            var isIgnored = await _dbWorker.IsPresentInIgnoreListAsync(query.From, formattedMessage.From.Email);
            var newState = callbackData.MessageKeyboardState == MessageKeyboardState.Minimized
                ? MessageKeyboardState.MinimizedActions
                : MessageKeyboardState.MaximizedActions;
            await _botActions.UpdateMessage(query.From, query.Message.MessageId, newState, formattedMessage, callbackData.Page, isIgnored);
        }

        /// <summary>
        /// Handles <see cref="Query"/> <see cref="Commands.HIDE_ACTIONS_COMMAND"/>.
        /// This method calls <see cref="BotActions.UpdateMessage"/> method for message with <paramref name="callbackData"/>
        /// which <see cref="CallbackData.MessageKeyboardState"/> equals <see langword="MessageKeyboardState.Minimized"/> or 
        /// <see langword="MessageKeyboardState.Maximized"/> which updates it on the set <see cref="CallbackData.Page"/>.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="callbackData"></param>
        /// <returns></returns>
        public async Task HandleCallbackQHideActions(Query query, CallbackData callbackData)
        {
            if (callbackData.MessageKeyboardState.EqualsAny(MessageKeyboardState.Minimized, MessageKeyboardState.Maximized))
                throw new ArgumentException("Must be a MinimizedActions or MaximizedActions state.", nameof(callbackData.MessageKeyboardState));

            var formattedMessage = await Methods.GetMessage(query.From, callbackData.MessageId);
            var isIgnored = await _dbWorker.IsPresentInIgnoreListAsync(query.From, formattedMessage.From.Email);
            var newState = callbackData.MessageKeyboardState == MessageKeyboardState.MinimizedActions
                ? MessageKeyboardState.Minimized
                : MessageKeyboardState.Maximized;
            await _botActions.UpdateMessage(query.From, query.Message.MessageId, newState, formattedMessage, callbackData.Page, isIgnored);
        }

        /// <summary>
        /// Handles <see cref="Query"/> <see cref="Commands.TO_READ_COMMAND"/>.
        /// This method removes message's "UNREAD" label and calls <see cref="BotActions.UpdateMessage"/> method for message with <paramref name="callbackData"/>.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="callbackData"></param>
        /// <returns></returns>
        public async Task HandleCallbackQToRead(Query query, CallbackData callbackData)
        {
            var formattedMessage = await Methods.ModifyMessageLabels(ModifyLabelsAction.Remove, query.From, callbackData.MessageId, null, "UNREAD");
            var isIgnored = await _dbWorker.IsPresentInIgnoreListAsync(query.From, formattedMessage.From.Email);

            await _botActions.UpdateMessage(query.From, query.Message.MessageId, callbackData.MessageKeyboardState, formattedMessage, callbackData.Page, isIgnored);
        }

        /// <summary>
        /// Handles <see cref="Query"/> <see cref="Commands.TO_READ_COMMAND"/>.
        /// This method adds "UNREAD" label to message and calls <see cref="BotActions.UpdateMessage"/> method for message with <paramref name="callbackData"/>.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="callbackData"></param>
        /// <returns></returns>
        public async Task HandleCallbackQToUnRead(Query query, CallbackData callbackData)
        {
            var formattedMessage = await Methods.ModifyMessageLabels(ModifyLabelsAction.Add, query.From, callbackData.MessageId, null, "UNREAD");
            var isIgnored = await _dbWorker.IsPresentInIgnoreListAsync(query.From, formattedMessage.From.Email);

            await _botActions.UpdateMessage(query.From, query.Message.MessageId, callbackData.MessageKeyboardState, formattedMessage, callbackData.Page, isIgnored);
        }

        /// <summary>
        /// Handles <see cref="Query"/> <see cref="Commands.TO_SPAM_COMMAND"/>.
        /// This method adds "SPAM" label to message and calls <see cref="BotActions.UpdateMessage"/> method for message with <paramref name="callbackData"/>.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="callbackData"></param>
        /// <returns></returns>
        public async Task HandleCallbackQToSpam(Query query, CallbackData callbackData)
        {
            var formattedMessage = await Methods.ModifyMessageLabels(ModifyLabelsAction.Add, query.From, callbackData.MessageId, null, "SPAM");
            var isIgnored = await _dbWorker.IsPresentInIgnoreListAsync(query.From, formattedMessage.From.Email);

            await _botActions.UpdateMessage(query.From, query.Message.MessageId, callbackData.MessageKeyboardState, formattedMessage, callbackData.Page, isIgnored);
        }

        /// <summary>
        /// Handles <see cref="Query"/> <see cref="Commands.TO_INBOX_COMMAND"/>.
        /// This method adds "INBOX" label to message and calls <see cref="BotActions.UpdateMessage"/> method for message with <paramref name="callbackData"/>.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="callbackData"></param>
        /// <returns></returns>
        public async Task HandleCallbackQToInbox(Query query, CallbackData callbackData)
        {
            var formattedMessage = await Methods.ModifyMessageLabels(ModifyLabelsAction.Add, query.From, callbackData.MessageId, null, "INBOX");
            var isIgnored = await _dbWorker.IsPresentInIgnoreListAsync(query.From, formattedMessage.From.Email);

            await _botActions.UpdateMessage(query.From, query.Message.MessageId, callbackData.MessageKeyboardState, formattedMessage, callbackData.Page, isIgnored);
        }

        /// <summary>
        /// Handles <see cref="Query"/> <see cref="Commands.TO_TRASH_COMMAND"/>.
        /// This method adds "TRASH" label to message and calls <see cref="BotActions.UpdateMessage"/> method for message with <paramref name="callbackData"/>.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="callbackData"></param>
        /// <returns></returns>
        public async Task HandleCallbackQToTrash(Query query, CallbackData callbackData)
        {
            var formattedMessage = await Methods.ModifyMessageLabels(ModifyLabelsAction.Add, query.From, callbackData.MessageId, null, "TRASH");
            var isIgnored = await _dbWorker.IsPresentInIgnoreListAsync(query.From, formattedMessage.From.Email);

            await _botActions.UpdateMessage(query.From, query.Message.MessageId, callbackData.MessageKeyboardState, formattedMessage, callbackData.Page, isIgnored);
        }

        /// <summary>
        /// Handles <see cref="Query"/> <see cref="Commands.ARCHIVE_COMMAND"/>.
        /// This method removes "INBOX" label to message and calls <see cref="BotActions.UpdateMessage"/> method for message with <paramref name="callbackData"/>.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="callbackData"></param>
        /// <returns></returns>
        public async Task HandleCallbackQArchive(Query query, CallbackData callbackData)
        {
            var formattedMessage = await Methods.ModifyMessageLabels(ModifyLabelsAction.Remove, query.From, callbackData.MessageId, null, "INBOX");
            var isIgnored = await _dbWorker.IsPresentInIgnoreListAsync(query.From, formattedMessage.From.Email);

            await _botActions.UpdateMessage(query.From, query.Message.MessageId, callbackData.MessageKeyboardState, formattedMessage, callbackData.Page, isIgnored);
        }

        /// <summary>
        /// Handles <see cref="Query"/> <see cref="Commands.UNIGNORE_COMMAND"/>.
        /// Removes senders email address from db.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="callbackData"></param>
        /// <returns></returns>
        public async Task HandleCallbackQUnignore(Query query, CallbackData callbackData)
        {
            var formattedMessage = await Methods.GetMessage(query.From, callbackData.MessageId);
            await _dbWorker.RemoveFromIgnoreListAsync(query.From, formattedMessage.From.Email);
            await _botActions.UpdateMessage(query.From, query.Message.MessageId, callbackData.MessageKeyboardState, formattedMessage, callbackData.Page, false);
        }

        /// <summary>
        /// Handles <see cref="Query"/> <see cref="Commands.IGNORE_COMMAND"/>.
        /// Adds senders email address to db.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="callbackData"></param>
        /// <returns></returns>
        public async Task HandleCallbackQIgnore(Query query, CallbackData callbackData)
        {
            var formattedMessage = await Methods.GetMessage(query.From, callbackData.MessageId);
            await _dbWorker.AddToIgnoreListAsync(query.From, formattedMessage.From.Email);
            await _botActions.UpdateMessage(query.From, query.Message.MessageId, callbackData.MessageKeyboardState, formattedMessage, callbackData.Page, false);
        }

        /// <summary>
        /// Handles <see cref="Query"/> <see cref="Commands.NEXTPAGE_COMMAND"/>.
        /// This method calls <see cref="BotActions.UpdateMessage"/> method for message with <paramref name="callbackData"/> where Page property increased by 1.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="callbackData"></param>
        /// <returns></returns>
        public async Task HandleCallbackQNextPage(Query query, CallbackData callbackData)
        {
            var formattedMessage = await Methods.GetMessage(query.From, callbackData.MessageId);
            if (formattedMessage.Pages <= callbackData.Page)
                throw new InvalidOperationException("Execution of this method is not permissible in this situation");
            var isIgnored = await _dbWorker.IsPresentInIgnoreListAsync(query.From, formattedMessage.From.Email);
            var newPage = callbackData.Page + 1;
            await
                _botActions.UpdateMessage(query.From, query.Message.MessageId, callbackData.MessageKeyboardState, formattedMessage,
                newPage, isIgnored);
        }

        /// <summary>
        /// Handles <see cref="CallbackQuery"/> <see cref="Commands.NEXTPAGE_COMMAND"/>.
        /// This method calls <see cref="BotActions.UpdateMessage"/> method for message with <paramref name="callbackData"/> where Page property decreased by 1.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="callbackData"></param>
        /// <returns></returns>
        public async Task HandleCallbackQPrevPage(Query query, CallbackData callbackData)
        {
            var formattedMessage = await Methods.GetMessage(query.From, callbackData.MessageId);
            if (callbackData.Page < 2)
                throw new InvalidOperationException("Execution of this method is not permissible in this situation");
            var isIgnored = await _dbWorker.IsPresentInIgnoreListAsync(query.From, formattedMessage.From.Email);
            var newPage = callbackData.Page - 1;
            await _botActions.UpdateMessage(query.From, query.Message.MessageId, callbackData.MessageKeyboardState, formattedMessage, newPage, isIgnored);
        }

        public async Task HandleCallbackQAddSubject(Query query, CallbackData callbackData)
        {

        }

        public async Task HandleCallbackQGetAttachments(Query query, CallbackData callbackData)
        {
            var message = await Methods.GetMessage(query.From, callbackData.MessageId);
            var newState = MessageKeyboardState.Attachments;
            await _botActions.SendAttachmentsListMessage(query.From, query.Message.MessageId, message, newState);
        }

        public async Task HandleCallbackQHideAttachments(Query query, CallbackData callbackData)
        {
            var message = await Methods.GetMessage(query.From, callbackData.MessageId);
            var newState = MessageKeyboardState.Minimized;
            await _botActions.SendAttachmentsListMessage(query.From, query.Message.MessageId, message, newState);
        }
    }
}