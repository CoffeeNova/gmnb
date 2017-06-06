using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Getmessage;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Sendmessage;
using CoffeeJelly.gmailNotifyBot.Bot.Moduls.GoogleRequests;
using CoffeeJelly.gmailNotifyBot.Bot.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types;
using Google.Apis.Gmail.v1.Data;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.CallbackQueryUpdates
{
    using Query = TelegramBotApiWrapper.Types.General.CallbackQuery;

    public partial class CallbackQueryHandler
    {
        /// <summary>
        /// Handles <see cref="Query"/> <see cref="Commands.AUTHORIZE_COMMAND"/>.
        /// This method calls <see cref="Authorizer.SendAuthorizeLink"/> that forms URL link and provides it to the chat as a message.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task HandleCallbackQAuthorize(Query query)
        {
            await _authorizer.SendAuthorizeLink(query);
        }

        /// <summary>
        /// Handles <see cref="Query"/> <see cref="Commands.EXPAND_COMMAND"/>.
        /// This method calls <see cref="BotActions.UpdateMessage"/> method for message with <paramref name="callbackData"/>
        ///  which <see cref="GetCallbackData.MessageKeyboardState"/> equals <see langword="MessageKeyboardState.Maximized"/> or 
        /// <see langword="MessageKeyboardState.MaximizedActions"/> which updates it to the 1st page.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="callbackData"></param>
        /// <returns></returns>
        public async Task HandleCallbackQExpand(Query query, GetCallbackData callbackData)
        {
            if (callbackData.MessageKeyboardState.EqualsAny(GetKeyboardState.Maximized, GetKeyboardState.MaximizedActions))
                throw new ArgumentException("Must be a Minimized or MinimizedAction state.", nameof(callbackData.MessageKeyboardState));

            var formattedMessage = await Methods.GetMessage(query.From, callbackData.MessageId);
            var isIgnored = await _dbWorker.IsPresentInIgnoreListAsync(query.From, formattedMessage.From.Email);
            var newState = callbackData.MessageKeyboardState == GetKeyboardState.Minimized
                ? GetKeyboardState.Maximized
                : GetKeyboardState.MaximizedActions;
            var newPage = 1;
            await _botActions.UpdateMessage(query.From, query.Message.MessageId, newState, formattedMessage, newPage, isIgnored);
        }

        /// <summary>
        /// Handles <see cref="Query"/> <see cref="Commands.HIDE_COMMAND"/>.
        /// This method calls <see cref="BotActions.UpdateMessage"/> method for message with <paramref name="callbackData"/>
        /// which <see cref="GetCallbackData.MessageKeyboardState"/> equals <see langword="MessageKeyboardState.Minimized"/> or 
        /// <see langword="MessageKeyboardState.MinimizedActions"/> which updates it to the 0 page.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="callbackData"></param>
        /// <returns></returns>
        public async Task HandleCallbackQHide(Query query, GetCallbackData callbackData)
        {
            if (callbackData.MessageKeyboardState.EqualsAny(GetKeyboardState.Minimized, GetKeyboardState.MinimizedActions))
                throw new ArgumentException("Must be a Maximized or MaximizedAction state.", nameof(callbackData.MessageKeyboardState));

            var formattedMessage = await Methods.GetMessage(query.From, callbackData.MessageId);
            var isIgnored = await _dbWorker.IsPresentInIgnoreListAsync(query.From, formattedMessage.From.Email);
            var newState = callbackData.MessageKeyboardState == GetKeyboardState.Maximized
                ? GetKeyboardState.Minimized
                : GetKeyboardState.MinimizedActions;
            var newPage = 0;
            await _botActions.UpdateMessage(query.From, query.Message.MessageId, newState, formattedMessage, newPage, isIgnored);

        }

        /// <summary>
        /// Handles <see cref="Query"/> <see cref="Commands.EXPAND_ACTIONS_COMMAND"/>.
        /// This method calls <see cref="BotActions.UpdateMessage"/> method for message with <paramref name="callbackData"/>
        /// which <see cref="GetCallbackData.MessageKeyboardState"/> equals <see langword="MessageKeyboardState.MinimizedActions"/> or 
        /// <see langword="MessageKeyboardState.MaximizedActions"/> which updates it on the set <see cref="GetCallbackData.Page"/>.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="callbackData"></param>
        /// <returns></returns>
        public async Task HandleCallbackQExpandActions(Query query, GetCallbackData callbackData)
        {
            if (callbackData.MessageKeyboardState.EqualsAny(GetKeyboardState.MinimizedActions, GetKeyboardState.MaximizedActions))
                throw new ArgumentException("Must be a Minimized or Maximized state.", nameof(callbackData.MessageKeyboardState));

            var formattedMessage = await Methods.ModifyMessageLabels(ModifyLabelsAction.Remove, query.From, callbackData.MessageId, null, "UNREAD");
            var isIgnored = await _dbWorker.IsPresentInIgnoreListAsync(query.From, formattedMessage.From.Email);
            var newState = callbackData.MessageKeyboardState == GetKeyboardState.Minimized
                ? GetKeyboardState.MinimizedActions
                : GetKeyboardState.MaximizedActions;
            await _botActions.UpdateMessage(query.From, query.Message.MessageId, newState, formattedMessage, callbackData.Page, isIgnored);
        }

        /// <summary>
        /// Handles <see cref="Query"/> <see cref="Commands.HIDE_ACTIONS_COMMAND"/>.
        /// This method calls <see cref="BotActions.UpdateMessage"/> method for message with <paramref name="callbackData"/>
        /// which <see cref="GetCallbackData.MessageKeyboardState"/> equals <see langword="MessageKeyboardState.Minimized"/> or 
        /// <see langword="MessageKeyboardState.Maximized"/> which updates it on the set <see cref="GetCallbackData.Page"/>.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="callbackData"></param>
        /// <returns></returns>
        public async Task HandleCallbackQHideActions(Query query, GetCallbackData callbackData)
        {
            if (callbackData.MessageKeyboardState.EqualsAny(GetKeyboardState.Minimized, GetKeyboardState.Maximized))
                throw new ArgumentException("Must be a MinimizedActions or MaximizedActions state.", nameof(callbackData.MessageKeyboardState));

            var formattedMessage = await Methods.GetMessage(query.From, callbackData.MessageId);
            var isIgnored = await _dbWorker.IsPresentInIgnoreListAsync(query.From, formattedMessage.From.Email);
            var newState = callbackData.MessageKeyboardState == GetKeyboardState.MinimizedActions
                ? GetKeyboardState.Minimized
                : GetKeyboardState.Maximized;
            await _botActions.UpdateMessage(query.From, query.Message.MessageId, newState, formattedMessage, callbackData.Page, isIgnored);
        }

        /// <summary>
        /// Handles <see cref="Query"/> <see cref="Commands.TO_READ_COMMAND"/>.
        /// This method removes message's "UNREAD" label and calls <see cref="BotActions.UpdateMessage"/> method for message with <paramref name="callbackData"/>.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="callbackData"></param>
        /// <returns></returns>
        public async Task HandleCallbackQToRead(Query query, GetCallbackData callbackData)
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
        public async Task HandleCallbackQToUnRead(Query query, GetCallbackData callbackData)
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
        public async Task HandleCallbackQToSpam(Query query, GetCallbackData callbackData)
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
        public async Task HandleCallbackQToInbox(Query query, GetCallbackData callbackData)
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
        public async Task HandleCallbackQToTrash(Query query, GetCallbackData callbackData)
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
        public async Task HandleCallbackQArchive(Query query, GetCallbackData callbackData)
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
        public async Task HandleCallbackQUnignore(Query query, GetCallbackData callbackData)
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
        public async Task HandleCallbackQIgnore(Query query, GetCallbackData callbackData)
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
        public async Task HandleCallbackQNextPage(Query query, GetCallbackData callbackData)
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
        /// Handles <see cref="Query"/> <see cref="Commands.NEXTPAGE_COMMAND"/>.
        /// This method calls <see cref="BotActions.UpdateMessage"/> method for message with <paramref name="callbackData"/> where Page property decreased by 1.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="callbackData"></param>
        /// <returns></returns>
        public async Task HandleCallbackQPrevPage(Query query, GetCallbackData callbackData)
        {
            var formattedMessage = await Methods.GetMessage(query.From, callbackData.MessageId);
            if (callbackData.Page < 2)
                throw new InvalidOperationException("Execution of this method is not permissible in this situation");
            var isIgnored = await _dbWorker.IsPresentInIgnoreListAsync(query.From, formattedMessage.From.Email);
            var newPage = callbackData.Page - 1;
            await _botActions.UpdateMessage(query.From, query.Message.MessageId, callbackData.MessageKeyboardState, formattedMessage, newPage, isIgnored);
        }

        /// <summary>
        /// Handles <see cref="Query"/> <see cref="Commands.SHOW_ATTACHMENTS_COMMAND"/>.
        /// This method calls <see cref="BotActions.SendAttachmentsListMessage"/> method for message with <paramref name="callbackData"/> where <see cref="GetCallbackData.MessageKeyboardState"/>
        /// equals <see cref="GetKeyboardState.Attachments"/>.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="callbackData"></param>
        /// <returns></returns>
        public async Task HandleCallbackQShowAttachments(Query query, GetCallbackData callbackData)
        {
            var message = await Methods.GetMessage(query.From, callbackData.MessageId);
            var newState = GetKeyboardState.Attachments;
            await _botActions.SendAttachmentsListMessage(query.From, query.Message.MessageId, message, newState);
        }

        /// <summary>
        /// Handles <see cref="Query"/> <see cref="Commands.HIDE_ATTACHMENTS_COMMAND"/>.
        /// This method calls <see cref="BotActions.UpdateMessage"/> method for message with <paramref name="callbackData"/> where <see cref="GetCallbackData.MessageKeyboardState"/>
        /// equals <see cref="GetKeyboardState.Minimized"/> (restores the original state).
        /// </summary>
        /// <param name="query"></param>
        /// <param name="callbackData"></param>
        /// <returns></returns>
        public async Task HandleCallbackQHideAttachments(Query query, GetCallbackData callbackData)
        {
            var formattedMessage = await Methods.GetMessage(query.From, callbackData.MessageId);
            var newState = GetKeyboardState.Minimized;
            await _botActions.UpdateMessage(query.From, query.Message.MessageId, newState, formattedMessage);
        }

        /// <summary>
        /// Handles <see cref="Query"/> <see cref="Commands.GET_ATTACHMENT_COMMAND"/>.
        /// Downloads from gmail server attachment defined in <paramref name="callbackData"/> to temp folder and send in to telegram recipient by 
        /// <see cref="BotActions.SendAttachmentToChat"/> method.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="callbackData"></param>
        /// <returns></returns>
        public async Task HandleCallbackQGetAttachment(ISender query, GetCallbackData callbackData)
        {
            var formattedMessage = await Methods.GetMessage(query.From, callbackData.MessageId);
            var attachmentInfo = new List<AttachmentInfo>(formattedMessage.Attachments)[callbackData.AttachmentIndex];
            var service = Methods.SearchServiceByUserId(query.From);
            var attachment = await Methods.GetAttachment(service, callbackData.MessageId, attachmentInfo);
            if (attachment.Length > _botSettings.MaxAttachmentSize)
                await _botActions.SendErrorAboutMaxAttachmentSizeToChat(query.From);

            string randomFolder = Tools.RandomString(8);
            var tempDirName = Path.Combine(_botSettings.AttachmentsTempFolder, randomFolder);
            var attachFullName = Path.Combine(tempDirName, attachmentInfo.FileName);
            try
            {
                Methods.CreateDirectory(tempDirName);
                await Methods.WriteAttachmentToTemp(attachFullName, attachment);
                await _botActions.SendAttachmentToChat(query.From, attachFullName, attachmentInfo.FileName);
            }
            finally
            {
                var dInfo = new DirectoryInfo(tempDirName);
                if (dInfo.Exists)
                    await dInfo.DeleteAsync(true);
            }
        }

        public async Task HandleCallbackQSaveAsDraft(Query query, SendCallbackData callbackData)
        {
            var nmModel = await _dbWorker.FindNmStoreAsync(query.From);
            if (nmModel == null)
                return;
            var localFileNames = new List<string>();
            try
            {
                if (nmModel.File != null)
                {
                    //download file and save to local temp
                    foreach (var fileModel in nmModel.File)
                    {
                        string randomFolder = Tools.RandomString(8);
                        var tempDirName = Path.Combine(_botSettings.AttachmentsTempFolder, randomFolder);
                        Methods.CreateDirectory(tempDirName);
                        var file = await _botActions.GetFile(fileModel.FileId);
                        await _botActions.DownloadFile(file, tempDirName);
                        var attachFullName = Path.Combine(tempDirName, file.FileName);
                        localFileNames.Add(attachFullName);
                    }
                }
               // var draft = await Methods.GetDraft(query.From, callbackData.DraftId);
                Draft draft = null;
                if (callbackData.DraftId == "")
                {
                    var body = Methods.CreateNewDraftBody(nmModel.Subject, nmModel.Message, nmModel.To.ToList(),
                        nmModel.Cc.ToList(),
                        nmModel.Bcc.ToList(), localFileNames);
                    draft = await Methods.CreateDraft(body, query.From);
                }
                else
                {
                    draft = await Methods.GetDraft(query.From, callbackData.DraftId);
                    var body = Methods.AddToDraftBody(draft, nmModel.Subject, nmModel.Message, nmModel.To.ToList(),
                        nmModel.Cc.ToList(),
                        nmModel.Bcc.ToList(), localFileNames);
                    draft = await Methods.UpdateDraft(body, query.From, draft.Id);
                }
                if (draft == null)
                    throw new NotImplementedException("BotAction message: error to save message as draft");

                await _botActions.UpdateNewMailMessage(query.From, SendKeyboardState.Drafted, nmModel, draft.Id);
            }
            finally
            {
                localFileNames.ForEach(async f =>
                {
                    var dInfo = new DirectoryInfo(Path.GetFullPath(f));
                    if (dInfo.Exists)
                        await dInfo.DeleteAsync(true);
                });
            }
            await HandleCallbackQNotSaveAsDraft(query, callbackData);
        }

        public async Task HandleCallbackQNotSaveAsDraft(Query query, SendCallbackData callbackData)
        {
            Methods.SearchServiceByUserId(query.From);
            var nmModel = await _dbWorker.FindNmStoreAsync(query.From);
            await _dbWorker.RemoveNmStoreAsync(nmModel);
            //there is the place to delete old message from chat by query.Message.MessageId
            var textMessage = await _botActions.SpecifyNewMailMessage(query.From, SendKeyboardState.Init);
            await _dbWorker.AddNewNmStoreAsync(new NmStoreModel { UserId = query.From, MessageId = textMessage.MessageId });
        }

        public async Task HandleCallbackQContinueWithOld(Query query, SendCallbackData callbackData)
        {
            Methods.SearchServiceByUserId(query.From);
            var nmModel = await _dbWorker.FindNmStoreAsync(query.From);
            //there is the place to delete old message from chat by query.Message.MessageId
            var textMessage = await _botActions.SpecifyNewMailMessage(query.From, SendKeyboardState.Continue, nmModel);
            nmModel.MessageId = textMessage.MessageId;
            await _dbWorker.UpdateNmStoreRecordAsync(nmModel);
        }

        //public async Task HandleCallbackQAddSubject(Query query, SendCallbackData callbackData)
        //{
        //    var nmModel = await _dbWorker.FindNmStoreAsync(query.From);
        //    //nmModel.Subject= query.Data
        //    await _botActions.UpdateNewMailMessage(query.From, callbackData.MessageKeyboardState, nmModel);
        //}

        public async Task HandleCallbackQAddTextMessage(Query query, SendCallbackData callbackData)
        {
            Methods.SearchServiceByUserId(query.From);
            var model = await _dbWorker.FindNmStoreAsync(query.From);
            if (model == null)
            {
                await _botActions.SendLostInfoMessage(query.From);
                return;
            }

            await _botActions.ChangeTextMessageForceReply(query.From);
        }

        public async Task HandleCallbackQAddSubject(Query query, SendCallbackData callbackData)
        {
            Methods.SearchServiceByUserId(query.From);
            var model = await _dbWorker.FindNmStoreAsync(query.From);
            if (model == null)
            {
                await _botActions.SendLostInfoMessage(query.From);
                return;
            }

            await _botActions.ChangeSubjectForceReply(query.From).ConfigureAwait(false); ;
        }

    }
}