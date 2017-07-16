using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels;
using CoffeeJelly.gmailNotifyBot.Bot.Exceptions;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Settings;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Sendmessage;
using CoffeeJelly.gmailNotifyBot.Bot.Moduls.GoogleRequests;
using CoffeeJelly.gmailNotifyBot.Bot.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;
using CoffeeJelly.TelegramBotApiWrapper.Types.Messages;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Message = CoffeeJelly.TelegramBotApiWrapper.Types.Messages.Message;
using Format = Google.Apis.Gmail.v1.UsersResource.MessagesResource.GetRequest.FormatEnum;
using Label = CoffeeJelly.gmailNotifyBot.Bot.Types.Label;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.MessageUpdates
{
    public partial class MessageHandler
    {
        public async Task HandleStartCommand(ISender sender)
        {
            try
            {
                var service = Methods.SearchServiceByUserId(sender.From);
                var email = await _dbWorker.FindUserAsync(sender.From);
                await _botActions.StartMessage(sender.From, email.Email);
            }
            catch (ServiceNotFoundException ex)
            {
                await _botActions.StartMessage(sender.From);

            }
        }

        /// <summary>
        /// Handles <see cref="TextCommand.AUTHORIZE_COMMAND"/>.
        /// This method calls <see cref="Authorizer.SendAuthorizeLink"/> that forms URL link and provides it to the chat as a message.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task HandleAuthorizeCommand(ISender query)
        {
            await _authorizer.SendAuthorizeLink(query, Authorizer.AuthorizeLinks.Both);
        }

        public async Task HandleTestMessageCommand(ISender sender, Service service)
        {
            sender.NullInspect(nameof(sender));

            string messageId = "";
            if (sender.GetType() == typeof(TextMessage))
            {
                // ReSharper disable once PossibleNullReferenceException
                var splittedtext = (sender as TextMessage).Text.Split(' ');
                messageId = splittedtext.Length > 1 ? splittedtext[1] : "";
            }

            var query = service.GmailService.Users.Messages.List("me");
            var listMessagesResponse = await query.ExecuteAsync();
            if (listMessagesResponse?.Messages == null) return;

            var message = listMessagesResponse.Messages.First();
            if (!string.IsNullOrEmpty(messageId))
                message = listMessagesResponse.Messages.SingleOrDefault(m => m.Id == messageId);
            if (message == null)
                return;

            var getMailRequest = service.GmailService.Users.Messages.Get("me", message.Id);
            getMailRequest.Format = service.FullUserAccess ? Format.Full : Format.Metadata;
            var mailInfoResponse = await getMailRequest.ExecuteAsync();
            if (mailInfoResponse == null) return;

            var formattedMessage = new FormattedMessage(mailInfoResponse, service.FullUserAccess);
            await _botActions.ShowShortMessageAsync(sender.From, formattedMessage, service.FullUserAccess);
        }

        public async Task HandleTestNameCommand(Service service)
        {
            var query = service.Oauth2Service.Userinfo.Get();
            var userinfo = await query.ExecuteAsync();
            await _botActions.EmailAddressMessage(service.From, userinfo.Name);
        }

        public async Task HandleTestThreadCommand(Service service)
        {
            var query = service.GmailService.Users.Threads.List("me");
            query.LabelIds = "INBOX";
            var listThreadsResponse = await query.ExecuteAsync();
            if (listThreadsResponse?.Threads == null) return;

            var thread = listThreadsResponse.Threads.First();
            var getMailRequest = service.GmailService.Users.Messages.Get("me", thread.Id);
            getMailRequest.Format = service.FullUserAccess ? Format.Full : Format.Metadata;
            var mailInfoResponse = await getMailRequest.ExecuteAsync();
            if (mailInfoResponse == null) return;

            var formattedMessage = new FormattedMessage(mailInfoResponse, service.FullUserAccess);
            await _botActions.ShowShortMessageAsync(service.From, formattedMessage, service.FullUserAccess);
        }

        public async Task HandleTestDraftCommand(ISender sender, Service service)
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
                var textMessage =
                    await _botActions.SpecifyNewMailMessage(sender.From, SendKeyboardState.Continue, nmStore);
                nmStore.MessageId = textMessage.MessageId;
                nmStore.DraftId = draft.Id;
                await _dbWorker.UpdateNmStoreRecordAsync(nmStore);
            }
            else
            {
                await _botActions.SaveAsDraftQuestionMessage(sender.From, SendKeyboardState.Store);
                await _botActions.DeleteMessage(sender.From, nmStore.MessageId);
            }
        }

        public async Task HandleStartNotifyCommand(Service service)
        {
            var userSettings = await UserSettings(service.From);
            if (userSettings.MailNotification)
                return;
            userSettings.MailNotification = true;
            await _dbWorker.UpdateUserSettingsRecordAsync(userSettings);
            await HandleStartWatchCommandAsync(service);
            await _botActions.NotificationStartedMessage(service.From);
        }

        public async Task HandleStopNotifyCommand(Service service)
        {
            var userSettings = await UserSettings(service.From);
            if (!userSettings.MailNotification)
                return;
            await HandleStopWatchCommandAsync(service);
            userSettings.MailNotification = false;
            await _dbWorker.UpdateUserSettingsRecordAsync(userSettings);
            await _botActions.NotificationStopedMessage(service.From);
        }

        public async Task HandleStartWatchCommandAsync(Service service)
        {
            if (string.IsNullOrEmpty(BotInitializer.Instance?.BotSettings?.Topic))
                throw new CommandHandlerException($"{nameof(BotInitializer.Instance.BotSettings.Token)} must be not null or empty.");

            var userSettings = await UserSettings(service.From);
            if (!userSettings.MailNotification)
                return;

            var watchRequest = new WatchRequest
            {
                LabelIds = new List<string> { Label.Unread },
                TopicName = BotInitializer.Instance.BotSettings.Topic,
                LabelFilterAction = "include"
            };
            var query = service.GmailService.Users.Watch(watchRequest, "me");
            var watchResponse = await query.ExecuteAsync();
            if (watchResponse.Expiration != null)
                userSettings.Expiration = watchResponse.Expiration.Value;
            if (watchResponse.HistoryId != null)
                userSettings.HistoryId = Convert.ToInt64(watchResponse.HistoryId.Value);

            await _dbWorker.UpdateUserSettingsRecordAsync(userSettings);
        }

        public void HandleStartWatchCommand(Service service)
        {
            if (string.IsNullOrEmpty(BotInitializer.Instance?.BotSettings?.Topic))
                throw new CommandHandlerException($"{nameof(BotInitializer.Instance.BotSettings.Token)} must be not null or empty.");

            var userSettings = _dbWorker.FindUserSettings(service.From);
            if (userSettings == null)
                throw new DbDataStoreException(
                    $"Can't find user settings data in database. User record with id {service.From} is absent in the database.");
            if (!userSettings.MailNotification) return;

            var watchRequest = new WatchRequest
            {
                LabelIds = new List<string> { Label.Unread },
                TopicName = BotInitializer.Instance.BotSettings.Topic,
                LabelFilterAction = "include"
            };
            var query = service.GmailService.Users.Watch(watchRequest, "me");
            var watchResponse = query.Execute();
            if (watchResponse.Expiration != null)
                userSettings.Expiration = watchResponse.Expiration.Value;
            if (watchResponse.HistoryId != null)
                userSettings.HistoryId = Convert.ToInt64(watchResponse.HistoryId.Value);

            _dbWorker.UpdateUserSettingsRecord(userSettings);
        }

        public async Task HandleStopWatchCommandAsync(Service service)
        {
            var userSettings = await UserSettings(service.From);
            if (!userSettings.MailNotification)
                return;

            var query = service.GmailService.Users.Stop("me");
            await query.ExecuteAsync();
        }

        public async Task HandleNewMessageCommand(ISender sender)
        {
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

        public async Task HandleNewMessageCommand(Service service, string arguments)
        {
            var argTypes = arguments.GetBetween('\"').ToList();
            
            if (argTypes.Count != 3)
            {
                await _botActions.NewMessageArgumentsError(service.From);
                return;
            }
            var recipients = argTypes[0].Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (!recipients.Any() || !recipients.All(Methods.EmailAddressValidation))
            {
                await _botActions.NewMessageRecipientsArgumentError(service.From);
                return;
            }
            var subject = argTypes[1];
            if (string.IsNullOrWhiteSpace(subject))
            {
                await _botActions.NewMessageSubjectArgumentError(service.From);
                return;
            }
            var text = argTypes[2];
            if (string.IsNullOrWhiteSpace(text))
            {
                await _botActions.NewMessageTextArgumentError(service.From);
                return;
            }
            var to = recipients.Select(r => new UserInfo { Email = r })
                .ToList<IUserInfo>();
            var messageBody = Methods.CreateNewMessageBody(subject, text, to);
            var request = service.GmailService.Users.Messages.Send(messageBody, "me");
            await request.ExecuteAsync();
            await _botActions.NewMessageSentSuccessfull(service.From);

        }

        public async Task HandleHelpCommand(ISender sender)
        {
            await _botActions.HelpMessage(sender.From);
        }

        public async Task HandleMessageForceReply(TextMessage message)
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

            if (model.Message != message.Text)
            {
                model.Message = message.Text;
                await _dbWorker.UpdateNmStoreRecordAsync(model);
                await _botActions.UpdateNewMailMessage(message.From, SendKeyboardState.Continue, model);
            }
            await _botActions.DeleteMessage(message.ReplyToMessage.Chat, message.ReplyToMessage.MessageId);
        }

        public async Task HandleSubjectForceReply(TextMessage message)
        {
            var model = await _dbWorker.FindNmStoreAsync(message.From);
            if (model == null)
            {
                await _botActions.SendLostInfoMessage(message.From);
                return;
            }
            if (message.Text == null)
                return;

            if (model.Subject != message.Text)
            {
                model.Subject = message.Text;
                await _dbWorker.UpdateNmStoreRecordAsync(model);
                await _botActions.UpdateNewMailMessage(message.From, SendKeyboardState.Continue, model);
            }
            await _botActions.DeleteMessage(message.ReplyToMessage.Chat, message.ReplyToMessage.MessageId);
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
                    if (message.Document.FileSize > _botSettings.MaxAttachmentSizeBytes)
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
                    await _botActions.DeleteMessage(message.ReplyToMessage.Chat, message.ReplyToMessage.MessageId);
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
            }

        }

        public async Task HandleDeleteMark(TextMessage message)
        {
            await _botActions.DeleteMessage(message.From, message.MessageId);
        }

        #region Settings

        public async Task HandleSettingsCommand(ISender sender)
        {
            var settings = await UserSettings(sender.From);
            await _botActions.ShowSettingsMenu(sender.From, settings);
        }

        public async Task HandleCreateNewLabelForceReply(TextMessage message, Service service)
        {
            try
            {
                var label = await Methods.CreateLabelAsync(message.Text, service);
                if (label == null)
                    throw new Exception();
                await _botActions.CreateLabelSuccessful(message.From, label.Name);
                await _botActions.DeleteMessage(message.ReplyToMessage.Chat, message.ReplyToMessage.MessageId);
            }
            catch (Exception ex)
            {
                await _botActions.CreateLabelError(message.From, message.Text);
                throw new Exception("CreateLabelError", ex);
            }
        }

        public async Task HandleEditLabelNameForceReply(TextMessage message, Service service)
        {
            var tempData = await _dbWorker.FindTempDataAsync(message.From);
            var label = await Methods.GetLabelAsync(tempData.LabelId, service);
            label.Name = message.Text;
            var editedLabel = await Methods.EditLabelAsync(label, service);

            var labels = await Methods.GetLabelsList(service);
            var labelInfos = labels
                .Where(l => l.Type != "system")
                .Select(l => new LabelInfo { Name = l.Name, LabelId = l.Id });

            await
                _botActions.ShowSettingsMenu(message.From, SettingsKeyboardState.EditLabelsMenu, SelectedOption.None,
                    null, labelInfos);
            await _botActions.DeleteMessage(message.ReplyToMessage.Chat, message.ReplyToMessage.MessageId);
        }

        public async Task HandleAddToIgnoreForceReply(TextMessage message)
        {
            var userSettings = await UserSettings(message.From);
            if (userSettings.IgnoreList == null)
                return;

            if (!Methods.EmailAddressValidation(message.Text))
            {
                await _botActions.NotRecognizedEmailMessage(message.From, message.Text);
                return;
            }
            if (userSettings.IgnoreList.Exists(i => i.Address == message.Text))
            {
                await _botActions.AlreadyInIgnoreListMessage(message.From, message.Text);
                return;
            }

            await _dbWorker.AddToIgnoreListAsync(message.From, message.Text);
            await _botActions.AddToIgnoreListSuccessMessage(message.From, message.Text);
            await _botActions.DeleteMessage(message.ReplyToMessage.Chat, message.ReplyToMessage.MessageId);
        }

        public async Task HandleRemoveFromIgnoreForceReply(TextMessage message)
        {
            var userSettings = await UserSettings(message.From);
            if (userSettings.IgnoreList == null)
                return;

            int number;
            IgnoreModel emailModel;
            if (int.TryParse(message.Text, out number))
            {
                if (number < 1 || number > userSettings.IgnoreList.Count)
                {
                    await _botActions.EmailAbsentInIgnoreMessage(message.From, number);
                    return;
                }
                emailModel = userSettings.IgnoreList.ElementAt(number - 1);
            }
            else
            {
                if (!Methods.EmailAddressValidation(message.Text))
                {
                    await _botActions.NotRecognizedEmailMessage(message.From, message.Text);
                    return;
                }
                if (!userSettings.IgnoreList.Exists(i => i.Address == message.Text))
                {
                    await _botActions.EmailAbsentInIgnoreMessage(message.From, message.Text);
                    return;
                }
                emailModel = userSettings.IgnoreList.Find(i => i.Address == message.Text);
            }
            userSettings.IgnoreList.Remove(emailModel);
            await _dbWorker.UpdateUserSettingsRecordAsync(userSettings);
            await _botActions.RemoveFromIgnoreListSuccessMessage(message.From, emailModel.Address);
            await _botActions.DeleteMessage(message.ReplyToMessage.Chat, message.ReplyToMessage.MessageId);
        }

        #endregion

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

        private async Task<UserSettingsModel> UserSettings(int userId)
        {
            var userSettings = await _dbWorker.FindUserSettingsAsync(userId);
            if (userSettings == null)
                throw new DbDataStoreException(
                    $"Can't find user settings data in database. User record with id {userId} is absent in the database.");
            return userSettings;
        }

        private readonly SemaphoreSlim _fileForceReplySemaphore = new SemaphoreSlim(1);
        private static readonly List<string> FileForceReplyLockers = new List<string>();

    }
}