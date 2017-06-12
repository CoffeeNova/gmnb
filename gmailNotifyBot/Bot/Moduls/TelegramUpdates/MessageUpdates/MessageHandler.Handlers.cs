using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels;
using CoffeeJelly.gmailNotifyBot.Bot.Exceptions;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Sendmessage;
using CoffeeJelly.gmailNotifyBot.Bot.Moduls.GoogleRequests;
using CoffeeJelly.gmailNotifyBot.Bot.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;
using CoffeeJelly.TelegramBotApiWrapper.Types.Messages;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Message = CoffeeJelly.TelegramBotApiWrapper.Types.Messages.Message;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.MessageUpdates
{
    public partial class MessageHandler
    {
        /// <summary>
        /// Handles <see cref="TextTextCommandHORIZE_COMMAND"/>.
        /// This method calls <see cref="Authorizer.SendAuthorizeLink"/> that forms URL link and provides it to the chat as a message.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task HandleAuthorizeCommand(ISender query)
        {
            await _authorizer.SendAuthorizeLink(query);
        }

        public async Task HandleTestMessageCommand(ISender sender)
        {
            sender.NullInspect(nameof(sender));

            string messageId = "";
            if (sender.GetType() == typeof(TextMessage))
            {
                // ReSharper disable once PossibleNullReferenceException
                var splittedtext = (sender as TextMessage).Text.Split(' ');
                messageId = splittedtext.Length > 1 ? splittedtext[1] : "";
            }

            var service = Methods.SearchServiceByUserId(sender.From);
            var query = service.GmailService.Users.Messages.List("me");
            //query.LabelIds = "INBOX";
            var listMessagesResponse = await query.ExecuteAsync();
            if (listMessagesResponse?.Messages == null) return;

            var message = listMessagesResponse.Messages.First();
            if (!string.IsNullOrEmpty(messageId))
                message = listMessagesResponse.Messages.SingleOrDefault(m => m.Id == messageId);
            if (message == null)
                return;

            var getMailRequest = service.GmailService.Users.Messages.Get("me", message.Id);
            var mailInfoResponse = await getMailRequest.ExecuteAsync();
            if (mailInfoResponse == null) return;
            var formattedMessage = new FormattedMessage(mailInfoResponse);
            await _botActions.ShowShortMessageAsync(sender.From, formattedMessage);
        }

        public async Task HandleTestNameCommand(ISender sender)
        {
            var service = Methods.SearchServiceByUserId(sender.From);
            var query = service.Oauth2Service.Userinfo.Get();
            var userinfo = await query.ExecuteAsync();
            await _botActions.EmailAddressMessage(sender.From, userinfo.Name);
        }

        public async Task HandleTestThreadCommand(ISender sender)
        {
            var service = Methods.SearchServiceByUserId(sender.From);
            var query = service.GmailService.Users.Threads.List("me");
            query.LabelIds = "INBOX";
            var listThreadsResponse = await query.ExecuteAsync();
            if (listThreadsResponse?.Threads == null) return;

            var thread = listThreadsResponse.Threads.First();
            var getMailRequest = service.GmailService.Users.Messages.Get("me", thread.Id);
            var mailInfoResponse = await getMailRequest.ExecuteAsync();
            if (mailInfoResponse == null) return;
            var formattedMessage = new FormattedMessage(mailInfoResponse);
            await _botActions.ShowShortMessageAsync(sender.From, formattedMessage);
        }

        public async Task HandleTestDraftCommand(ISender sender)
        {
            sender.NullInspect(nameof(sender));

            string draftId = "";
            int draftIndex = -1;
            if (sender.GetType() == typeof(TextMessage))
            {
                // ReSharper disable once PossibleNullReferenceException
                var splittedtext = (sender as TextMessage).Text.Split(' ');
                if (splittedtext.Length > 1)
                {
                    if (splittedtext[1].StartsWith("="))
                    {
                        int draftNum;
                        int.TryParse(splittedtext[1].Remove(0, 1), out draftNum);
                        draftIndex = draftNum - 1;
                    }
                    else
                        draftId = splittedtext[1];
                }
            }

            var service = Methods.SearchServiceByUserId(sender.From);
            var query = service.GmailService.Users.Drafts.List("me");
            //query.LabelIds = "INBOX";
            var listDraftsResponse = await query.ExecuteAsync();
            if (listDraftsResponse?.Drafts == null) return;

            Draft draftInfo;
            if (draftIndex > -1)
                draftInfo = listDraftsResponse.Drafts[draftIndex];
            else if (!string.IsNullOrEmpty(draftId))
                draftInfo = listDraftsResponse.Drafts.SingleOrDefault(m => m.Id == draftId);
            else
                draftInfo = listDraftsResponse.Drafts.First();
            if (draftInfo == null)
                return;

            var nmStore = await _dbWorker.FindNmStoreAsync(sender.From);
            if (nmStore == null)
            {
                var draft = await Methods.GetDraft(sender.From, draftInfo.Id,
                                    UsersResource.DraftsResource.GetRequest.FormatEnum.Full);
                nmStore = await _dbWorker.AddNewNmStoreAsync(sender.From);
                var formattedMessage = new FormattedMessage(draft.Message);
                Methods.ComposeNmStateModel(nmStore, formattedMessage);
                var textMessage = await _botActions.SpecifyNewMailMessage(sender.From, SendKeyboardState.Continue, nmStore);
                nmStore.MessageId = textMessage.MessageId;
                nmStore.DraftId = draft.Id;
                await _dbWorker.UpdateNmStoreRecordAsync(nmStore);
            }
            else
                await _botActions.SaveAsDraftQuestionMessage(sender.From, SendKeyboardState.Store);
        }

        public async Task HandleStartNotifyCommand(ISender sender)
        {
            var userSettings = await _dbWorker.FindUserSettingsAsync(sender.From);
            if (userSettings == null)
                throw new DbDataStoreException(
                    $"Can't find user settings data in database. User record with id {sender.From} is absent in the database.");
            userSettings.MailNotification = true;
            await _dbWorker.UpdateUserSettingsRecordAsync(userSettings);
            await HandleStartWatchCommandAsync(sender);
            //message to chat about start notification
        }

        public async Task HandleStopNotifyCommand(ISender sender)
        {
            var userSettings = await _dbWorker.FindUserSettingsAsync(sender.From);
            if (userSettings == null)
                throw new DbDataStoreException(
                    $"Can't find user settings data in database. User record with id {sender.From} is absent in the database.");
            await _dbWorker.UpdateUserSettingsRecordAsync(userSettings);
            await HandleStopWatchCommandAsync(sender);
            userSettings.MailNotification = false;
            //message to chat about stop notification
        }

        public async Task HandleStartWatchCommandAsync(ISender sender)
        {
            if (string.IsNullOrEmpty(BotInitializer.Instance?.BotSettings?.Topic))
                throw new CommandHandlerException($"{nameof(BotInitializer.Instance.BotSettings.Token)} must be not null or empty.");

            var service = Methods.SearchServiceByUserId(sender.From);
            var userSettings = await _dbWorker.FindUserSettingsAsync(sender.From);
            if (userSettings == null)
                throw new DbDataStoreException(
                    $"Can't find user settings data in database. User record with id {sender.From} is absent in the database.");
            if (!userSettings.MailNotification) return;

            var watchRequest = new WatchRequest
            {
                LabelIds = new List<string> { "INBOX" },
                TopicName = BotInitializer.Instance.BotSettings.Topic
            };
            var query = service.GmailService.Users.Watch(watchRequest, "me");
            var watchResponse = await query.ExecuteAsync();
            if (watchResponse.Expiration != null)
                userSettings.Expiration = watchResponse.Expiration.Value;
            if (watchResponse.HistoryId != null)
                userSettings.HistoryId = Convert.ToInt64(watchResponse.HistoryId.Value);

            await _dbWorker.UpdateUserSettingsRecordAsync(userSettings);
        }

        public void HandleStartWatchCommand(ISender sender)
        {
            if (string.IsNullOrEmpty(BotInitializer.Instance?.BotSettings?.Topic))
                throw new CommandHandlerException($"{nameof(BotInitializer.Instance.BotSettings.Token)} must be not null or empty.");

            var service = Methods.SearchServiceByUserId(sender.From);
            var userSettings = _dbWorker.FindUserSettings(sender.From);
            if (userSettings == null)
                throw new DbDataStoreException(
                    $"Can't find user settings data in database. User record with id {sender.From} is absent in the database.");
            if (!userSettings.MailNotification) return;

            var watchRequest = new WatchRequest
            {
                LabelIds = new List<string> { "INBOX" },
                TopicName = BotInitializer.Instance.BotSettings.Topic
            };
            var query = service.GmailService.Users.Watch(watchRequest, "me");
            var watchResponse = query.Execute();
            if (watchResponse.Expiration != null)
                userSettings.Expiration = watchResponse.Expiration.Value;
            if (watchResponse.HistoryId != null)
                userSettings.HistoryId = Convert.ToInt64(watchResponse.HistoryId.Value);

            _dbWorker.UpdateUserSettingsRecord(userSettings);
        }


        public async Task HandleStopWatchCommandAsync(ISender sender)
        {
            var service = Methods.SearchServiceByUserId(sender.From);
            var userSettings = await _dbWorker.FindUserSettingsAsync(sender.From);
            if (userSettings == null)
                throw new DbDataStoreException(
                    $"Can't find user settings data in database. User record with id {sender.From} is absent in the database.");
            if (!userSettings.MailNotification) return;

            var query = service.GmailService.Users.Stop("me");
            var stopResponse = await query.ExecuteAsync();
        }

        public async Task HandleNewMessageCommand(ISender sender)
        {
            Methods.SearchServiceByUserId(sender.From);
            var nmStore = await _dbWorker.FindNmStoreAsync(sender.From);
            if (nmStore == null)
            {
                var textMessage = await _botActions.SpecifyNewMailMessage(sender.From, SendKeyboardState.Init);
                nmStore = await _dbWorker.AddNewNmStoreAsync(sender.From);
                nmStore.MessageId = textMessage.MessageId;
                await _dbWorker.UpdateNmStoreRecordAsync(nmStore);
            }
            else
                await _botActions.SaveAsDraftQuestionMessage(sender.From, SendKeyboardState.Store);
        }

        public async Task HandleGetInboxMessagesCommand(ISender sender)
        {
            Methods.SearchServiceByUserId(sender.From);
            await _botActions.GmailInlineInboxCommandMessage(sender.From);
        }

        public async Task HandleGetAllMessagesCommand(ISender sender)
        {
            Methods.SearchServiceByUserId(sender.From);
            await _botActions.GmailInlineAllCommandMessage(sender.From);
        }

        public async Task HandleGetDraftMessagesCommand(ISender sender)
        {
            Methods.SearchServiceByUserId(sender.From);
            await _botActions.GmailInlineDraftCommandMessage(sender.From);
        }

        public async Task HandleMessageForceReply(TextMessage message)
        {
            try
            {
                var model = await _dbWorker.FindNmStoreAsync(message.From);
                if (model == null)
                {
                    await _botActions.SendLostInfoMessage(message.From);
                    return;
                }
                if (message.ReplyToMessage == null)
                    return;
                if (message.Text == null)
                    return;

                model.Message = message.Text;
                await _dbWorker.UpdateNmStoreRecordAsync(model);
                await _botActions.UpdateNewMailMessage(message.From, SendKeyboardState.Continue, model);
            }
            finally
            {
                await _botActions.RemoveKeyboard(message.From);
            }
        }

        public async Task HandleSubjectForceReply(TextMessage message)
        {
            try
            {
                var model = await _dbWorker.FindNmStoreAsync(message.From);
                if (model == null)
                {
                    await _botActions.SendLostInfoMessage(message.From);
                    return;
                }
                if (message.Text == null)
                    return;

                model.Subject = message.Text;
                await _dbWorker.UpdateNmStoreRecordAsync(model);
                await _botActions.UpdateNewMailMessage(message.From, SendKeyboardState.Continue, model);
            }
            finally
            {
                await _botActions.RemoveKeyboard(message.From);
            }
        }

        public async Task HandleFileForceReply(DocumentMessage message)
        {
            if (message.ReplyToMessage == null)
                return;

            try
            {
                FileForceReplyLockers.Add(message.From);

                #region handleAction

                var handleAction = new Func<Task>(async () =>
                {
                    var model = await _dbWorker.FindNmStoreAsync(message.From);
                    if (model == null)
                    {
                        await _botActions.SendLostInfoMessage(message.From);
                        return;
                    }
                    if (message.Document.FileSize > _botSettings.MaxAttachmentSize)
                    {
                        await _botActions.SendErrorAboutMaxAttachmentSizeToChat(message.From, message.Document.FileName);
                        return;
                    }
                    model.File.Add(new FileModel
                    {
                        FileId = message.Document.FileId,
                        OriginalName = message.Document.FileName
                    });
                    await _dbWorker.UpdateNmStoreRecordAsync(model);
                    await _botActions.UpdateNewMailMessage(message.From, SendKeyboardState.Continue, model);
                });

                #endregion

                if (FileForceReplyLockers.Any(l => l == message.From))
                {
                    await _fileForceReplySemaphore.WaitAsync();
                    try
                    {
                        await handleAction();
                    }
                    finally
                    {
                        _fileForceReplySemaphore.Release();
                    }
                }
                else
                    await handleAction();
            }
            finally
            {
                FileForceReplyLockers.Remove(message.From);
                await _botActions.RemoveKeyboard(message.From);
            }

        }

        private void UpdateNmStoreModel(NmStoreModel model, Message message)
        {
            if (message.GetType() == typeof(TextMessage))
                model.Message = (message as TextMessage)?.Text;
            else if (message.GetType() == typeof(DocumentMessage))
            {
                model.Message = (message as DocumentMessage)?.Document.FileName;
                model.Message = (message as DocumentMessage)?.Document.FileId;
            }

        }

        private readonly SemaphoreSlim _fileForceReplySemaphore = new SemaphoreSlim(1);
        private static readonly List<string> FileForceReplyLockers = new List<string>();

    }
}