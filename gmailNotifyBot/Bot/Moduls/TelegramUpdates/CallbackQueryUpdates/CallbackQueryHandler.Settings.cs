using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels;
using CoffeeJelly.gmailNotifyBot.Bot.Exceptions;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Settings;
using CoffeeJelly.gmailNotifyBot.Bot.Moduls.GoogleRequests;
using CoffeeJelly.gmailNotifyBot.Bot.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;
using Google.Apis.Gmail.v1.Data;
using Label = CoffeeJelly.gmailNotifyBot.Bot.Types.Label;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.CallbackQueryUpdates
{
    public partial class CallbackQueryHandler
    {
        #region main menu handlers
        public async Task HandleCallbackQLabelsMenu(CallbackQuery query)
        {
            var userSettings = await _dbWorker.FindUserSettingsAsync(query.From);
            if (userSettings == null)
                return;

            await
                _botActions.UpdateSettingsMenu(query.From, query.Message.MessageId, SettingsKeyboardState.LabelsMenu, SelectedOption.None, userSettings);
        }

        public async Task HandleCallbackQPermissionsMenu(CallbackQuery query)
        {
            var userSettings = await _dbWorker.FindUserSettingsAsync(query.From);
            if (userSettings == null)
                return;

            await
                _botActions.UpdateSettingsMenu(query.From, query.Message.MessageId, SettingsKeyboardState.PermissionsMenu, SelectedOption.None, userSettings);
        }

        public async Task HandleCallbackQIgnoreMenu(CallbackQuery query)
        {
            var userSettings = await _dbWorker.FindUserSettingsAsync(query.From);
            if (userSettings == null)
                return;

            await
                _botActions.UpdateSettingsMenu(query.From, query.Message.MessageId, SettingsKeyboardState.IgnoreMenu, SelectedOption.None, userSettings);
        }

        public async Task HandleCallbackQAdditionalMenu(CallbackQuery query)
        {
            var userSettings = await _dbWorker.FindUserSettingsAsync(query.From);
            if (userSettings == null)
                return;

            await
                _botActions.UpdateSettingsMenu(query.From, query.Message.MessageId, SettingsKeyboardState.AdditionalMenu, SelectedOption.None, userSettings);
        }

        public async Task HandleCallbackQStartNotify(CallbackQuery query, Service service)
        {
            var userSettings = await UserSettings(query.From);
            if (userSettings == null)
                return;
            if (userSettings.MailNotification)
                return;

            userSettings.MailNotification = true;
            await _dbWorker.UpdateUserSettingsRecordAsync(userSettings);
            if (string.IsNullOrEmpty(BotInitializer.Instance?.BotSettings?.Topic))
                throw new CommandHandlerException($"{nameof(BotInitializer.Instance.BotSettings.Token)} must be not null or empty.");

            var watchRequest = new WatchRequest
            {
                LabelIds = new List<string> { Label.Unread },
                TopicName = BotInitializer.Instance.BotSettings.Topic,
                LabelFilterAction = "include"
            };
            var request = service.GmailService.Users.Watch(watchRequest, "me");
            var watchResponse = await request.ExecuteAsync();
            if (watchResponse.Expiration != null)
                userSettings.Expiration = watchResponse.Expiration.Value;
            if (watchResponse.HistoryId != null)
                userSettings.HistoryId = Convert.ToInt64(watchResponse.HistoryId.Value);

            await _dbWorker.UpdateUserSettingsRecordAsync(userSettings);
            await _botActions.NotificationStartedMessage(service.From);
        }

        public async Task HandleCallbackQStopNotify(CallbackQuery query, Service service)
        {
            var userSettings = await UserSettings(query.From);
            if (userSettings == null)
                return;
            if (!userSettings.MailNotification)
                return;

            var request = service.GmailService.Users.Stop("me");
            await request.ExecuteAsync();

            userSettings.MailNotification = false;
            await _dbWorker.UpdateUserSettingsRecordAsync(userSettings);
            await _botActions.NotificationStopedMessage(service.From);
        }

        public async Task HandleCallbackQAbout(CallbackQuery query, SettingsCallbackData callbackData)
        {
            var userSettings = await UserSettings(query.From);
            if (userSettings == null)
                return;
            await
                _botActions.UpdateSettingsMenu(query.From, query.Message.MessageId, SettingsKeyboardState.MainMenu, callbackData.Option, userSettings);
        }

        #endregion

        #region Labels menu

        public async Task HandleCallbackQEditLabelsMenu(CallbackQuery query, Service service)
        {
            var labels = await Methods.GetLabelsList(service);
            var labelInfos = labels
                .Where(l => l.Type != "system")
                .Select(l => new LabelInfo { Name = l.Name, LabelId = l.Id });
            await
                _botActions.UpdateSettingsMenu(query.From, query.Message.MessageId, SettingsKeyboardState.EditLabelsMenu, SelectedOption.None, null, null, labelInfos);
        }

        public async Task HandleCallbackQCreateNewLabel(CallbackQuery query)
        {
            await _botActions.CreateNewLabelForceReply(query.From);
        }

        public async Task HandleCallbackQWhitelistMenu(CallbackQuery query, Service service)
        {
            var userSettings = await _dbWorker.FindUserSettingsAsync(query.From);
            if (userSettings == null)
                return;
            var labels = await Methods.GetLabelsList(service);
            var labelInfos = labels
                .Where(l => (l.Type != "system") || CatergoriesLabels.List.Any(cl => cl.LabelId == l.Id))
                .Select(l => new LabelInfo { Name = CatergoriesLabels.ReturnLabelName(l.Id) ?? l.Name, LabelId = l.Id });
            await
               _botActions.UpdateSettingsMenu(query.From, query.Message.MessageId, SettingsKeyboardState.WhiteListMenu, SelectedOption.None, userSettings, null, labelInfos);
        }

        public async Task HandleCallbackQBlacklistMenu(CallbackQuery query, Service service)
        {
            var userSettings = await _dbWorker.FindUserSettingsAsync(query.From);
            if (userSettings == null)
                return;
            var labels = await Methods.GetLabelsList(service);
            var labelInfos = labels
                .Where(l => (l.Type != "system") || CatergoriesLabels.List.Any(cl => cl.LabelId == l.Id))
                .Select(l => new LabelInfo { Name = CatergoriesLabels.ReturnLabelName(l.Id) ?? l.Name, LabelId = l.Id });

            await
               _botActions.UpdateSettingsMenu(query.From, query.Message.MessageId, SettingsKeyboardState.BlackListMenu, SelectedOption.None, userSettings, null, labelInfos);
        }

        #endregion

        #region LabelsList

        public async Task HandleCallbackQBackToLabelsMenu(CallbackQuery query)
        {
            var userSettings = await _dbWorker.FindUserSettingsAsync(query.From);
            if (userSettings == null)
                return;

            await
                _botActions.UpdateSettingsMenu(query.From, query.Message.MessageId, SettingsKeyboardState.LabelsMenu, SelectedOption.None, userSettings);
        }

        public async Task HandleCallbackQLabelActionsMenu(CallbackQuery query, SettingsCallbackData callbackData)
        {
            var userSettings = await _dbWorker.FindUserSettingsAsync(query.From);
            if (userSettings == null)
                return;

            var tempData = await _dbWorker.FindTempDataAsync(userSettings.UserId);
            if (tempData == null)
                await
                    _dbWorker.AddNewTempDataAsync(new TempDataModel
                    {
                        UserId = userSettings.UserId,
                        LabelId = callbackData.LabelId,
                    });
            else
            {
                tempData.LabelId = callbackData.LabelId;
                await _dbWorker.UpdateTempDataRecordAsync(tempData);
            }
            await
                _botActions.UpdateSettingsMenu(query.From, query.Message.MessageId, SettingsKeyboardState.LabelActionsMenu, SelectedOption.None, userSettings, tempData, null);
        }

        public async Task HandleCallbackQWhitelistLabelAction(CallbackQuery query, SettingsCallbackData callbackData, Service service)
        {
            var userSettings = await _dbWorker.FindUserSettingsAsync(query.From);
            if (userSettings == null)
                return;

            var label = await Methods.GetLabelAsync(callbackData.LabelId, service);
            var labelModel = userSettings.Whitelist.SingleOrDefault(w => w.LabelId == label.Id);
            if (labelModel == null)
                userSettings.Whitelist.Add(new WhitelistModel { Name = label.Name, LabelId = label.Id });
            else
                userSettings.Whitelist.Remove(labelModel);
            await _dbWorker.UpdateUserSettingsRecordAsync(userSettings);
            await HandleCallbackQWhitelistMenu(query, service);
        }

        public async Task HandleCallbackQBlacklistLabelAction(CallbackQuery query, SettingsCallbackData callbackData, Service service)
        {
            var userSettings = await _dbWorker.FindUserSettingsAsync(query.From);
            if (userSettings == null)
                return;

            var label = await Methods.GetLabelAsync(callbackData.LabelId, service);
            var labelModel = userSettings.Blacklist.SingleOrDefault(b => b.LabelId == label.Id);
            if (labelModel == null)
                userSettings.Blacklist.Add(new BlacklistModel { Name = label.Name, LabelId = label.Id });
            else
                userSettings.Blacklist.Remove(labelModel);
            await _dbWorker.UpdateUserSettingsRecordAsync(userSettings);
            await HandleCallbackQBlacklistMenu(query, service);

        }

        public async Task HandleCallbackQUseBlacklist(CallbackQuery query, Service service)
        {
            var userSettings = await _dbWorker.FindUserSettingsAsync(query.From);
            if (userSettings == null)
                return;

            userSettings.UseWhitelist = false;
            await _dbWorker.UpdateUserSettingsRecordAsync(userSettings);
            await HandleCallbackQBlacklistMenu(query, service);
        }

        public async Task HandleCallbackQUseWhitelist(CallbackQuery query, Service service)
        {
            var userSettings = await _dbWorker.FindUserSettingsAsync(query.From);
            if (userSettings == null)
                return;

            userSettings.UseWhitelist = true;
            await _dbWorker.UpdateUserSettingsRecordAsync(userSettings);
            await HandleCallbackQWhitelistMenu(query, service);
        }
        #endregion

        #region Label actions menu

        public async Task HandleCallbackQEditLabelName(CallbackQuery query)
        {
            await _botActions.EditLabelNameForceReply(query.From);
        }

        public async Task HandleCallbackQRemoveLabel(CallbackQuery query, SettingsCallbackData callbackData, Service service)
        {
            var tempData = await _dbWorker.FindTempDataAsync(query.From);
            await Methods.DeleteLabelAsync(tempData.LabelId, service);
            await HandleCallbackQBackToEditLabelsListMenu(query, service);
        }

        public async Task HandleCallbackQBackToEditLabelsListMenu(CallbackQuery query, Service service)
        {
            var labels = await Methods.GetLabelsList(service);
            var labelInfos = labels
                  .Where(l => l.Type != "system")
                  .Select(l => new LabelInfo { Name = l.Name, LabelId = l.Id });

            await
                _botActions.UpdateSettingsMenu(query.From, query.Message.MessageId, SettingsKeyboardState.EditLabelsMenu, SelectedOption.None, null, null, labelInfos);
        }

        #endregion

        #region ignore menu

        public async Task HandleCallbackQAddToIgnore(CallbackQuery query)
        {
            await _botActions.AddToIgnoreForceReply(query.From);
        }

        public async Task HandleCallbackQRemoveFromIgnore(CallbackQuery query)
        {
            await _botActions.RemoveFromIgnoreForceReply(query.From);
        }

        public async Task HandleCallbackQDisplayIgnoredEmails(CallbackQuery query, SettingsCallbackData callbackData)
        {
            var userSettings = await _dbWorker.FindUserSettingsAsync(query.From);
            if (userSettings == null)
                return;

            await
                _botActions.UpdateSettingsMenu(query.From, query.Message.MessageId, SettingsKeyboardState.IgnoreMenu,
                    callbackData.Option, userSettings);
        }

        #endregion

        #region permissions menu

        public async Task HandleCallbackQSwapPermissions(CallbackQuery query, Service service)
        {
            Authorizer.AuthorizeLinks links;
            links = service.FullUserAccess ? Authorizer.AuthorizeLinks.Notify : Authorizer.AuthorizeLinks.Full;
            await _authorizer.SendAuthorizeLink(query, links);
            await _botActions.DeleteMessage(query.From, query.Message.MessageId);
        }

        public async Task HandleCallbackQRevokePermissions(CallbackQuery query, Service service)
        {
            var result = await service.UserCredential.RevokeTokenAsync(new System.Threading.CancellationToken());
            if (result)
                await _botActions.RevokeTokenSuccessfulMessage(query.From);
            else
                await _botActions.RevokeTokenUnSuccessfulMessage(query.From);
        }

        public async Task HandleCallbackQRevokePermissionsViaWeb(CallbackQuery query)
        {
            await _botActions.SendAccountPermissinsUrl(query.From);
        }

        #endregion

        #region additional menu

        public async Task HandleCallbackQReadAfterReceiving(CallbackQuery query, SettingsCallbackData callbackData)
        {
            var userSettings = await _dbWorker.FindUserSettingsAsync(query.From);
            if (userSettings == null)
                return;

            userSettings.ReadAfterReceiving = !userSettings.ReadAfterReceiving;
            await _dbWorker.UpdateUserSettingsRecordAsync(userSettings);

            await
               _botActions.UpdateSettingsMenu(query.From, query.Message.MessageId, SettingsKeyboardState.AdditionalMenu,
                   callbackData.Option, userSettings);
        }

        #endregion

        public async Task HandleCallbackQBackToMainMenu(CallbackQuery query)
        {
            var userSettings = await _dbWorker.FindUserSettingsAsync(query.From);
            if (userSettings == null)
                return;

            await
                _botActions.UpdateSettingsMenu(query.From, query.Message.MessageId, SettingsKeyboardState.MainMenu, SelectedOption.None, userSettings);
        }

        private async Task<UserSettingsModel> UserSettings(int userId)
        {
            var userSettings = await _dbWorker.FindUserSettingsAsync(userId);
            if (userSettings == null)
                throw new DbDataStoreException(
                    $"Can't find user settings data in database. User record with id {userId} is absent in the database.");
            return userSettings;
        }
    }
}