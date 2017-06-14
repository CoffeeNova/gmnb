using System.Linq;
using System.Threading.Tasks;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Settings;
using CoffeeJelly.gmailNotifyBot.Bot.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.CallbackQueryUpdates
{
    public partial class CallbackQueryHandler
    {
        #region main menu handlers
        public async Task HandleCallbackQLabelsMenu(CallbackQuery query)
        {
            Methods.SearchServiceByUserId(query.From);
            var userSettings = await _dbWorker.FindUserSettingsAsync(query.From);
            if (userSettings == null)
                return;

            await
                _botActions.UpdateSettingsMenu(query.From, query.Message.MessageId, SettingsKeyboardState.LabelsMenu, SelectedOption.None, userSettings);
        }

        public async Task HandleCallbackQPermissionsMenu(CallbackQuery query)
        {
            Methods.SearchServiceByUserId(query.From);
            var userSettings = await _dbWorker.FindUserSettingsAsync(query.From);
            if (userSettings == null)
                return;

            await
                _botActions.UpdateSettingsMenu(query.From, query.Message.MessageId, SettingsKeyboardState.LabelsMenu, SelectedOption.None, userSettings);
        }

        public async Task HandleCallbackQIgnoreMenu(CallbackQuery query)
        {
            Methods.SearchServiceByUserId(query.From);
            await
                _botActions.UpdateSettingsMenu(query.From, query.Message.MessageId, SettingsKeyboardState.IgnoreMenu);
        }

        public async Task HandleCallbackQAbout(CallbackQuery query, SettingsCallbackData callbackData)
        {
            Methods.SearchServiceByUserId(query.From);
            await
                _botActions.UpdateSettingsMenu(query.From, query.Message.MessageId, SettingsKeyboardState.MainMenu, callbackData.Option);
        }

        #endregion

        #region Labels menu

        public async Task HandleCallbackQEditLabelsMenu(CallbackQuery query)
        {
            var service = Methods.SearchServiceByUserId(query.From);
            var labels = await Methods.GetLabelsList(service);
            var labelInfoList = labels.Select(l => new LabelInfo { Name = l.Name, LabelId = l.Id } as ILabelInfo);

            await
                _botActions.UpdateSettingsMenu(query.From, query.Message.MessageId, SettingsKeyboardState.EditLabelsMenu, SelectedOption.None, null, null, labelInfoList);
        }

        public async Task HandleCallbackQWhitelistMenu(CallbackQuery query)
        {
            var service = Methods.SearchServiceByUserId(query.From);
            var userSettings = await _dbWorker.FindUserSettingsAsync(query.From);
            if (userSettings == null)
                return;
            var labels = await Methods.GetLabelsList(service);
            var labelInfoList = labels.Select(l => new LabelInfo { Name = l.Name, LabelId = l.Id } as ILabelInfo);
            await
               _botActions.UpdateSettingsMenu(query.From, query.Message.MessageId, SettingsKeyboardState.WhiteListMenu, SelectedOption.None, userSettings, null, labelInfoList);
        }

        public async Task HandleCallbackQBlacklistMenu(CallbackQuery query)
        {
            var service = Methods.SearchServiceByUserId(query.From);
            var userSettings = await _dbWorker.FindUserSettingsAsync(query.From);
            if (userSettings == null)
                return;
            var labels = await Methods.GetLabelsList(service);
            var labelInfoList = labels.Select(l => new LabelInfo { Name = l.Name, LabelId = l.Id } as ILabelInfo);

            await
               _botActions.UpdateSettingsMenu(query.From, query.Message.MessageId, SettingsKeyboardState.BlackListMenu, SelectedOption.None, userSettings, null, labelInfoList);
        }

        #endregion

        #region LabelsList

        public async Task HandleCallbackQBackToLabelsMenu(CallbackQuery query)
        {
            Methods.SearchServiceByUserId(query.From);
            var userSettings = await _dbWorker.FindUserSettingsAsync(query.From);
            if (userSettings == null)
                return;

            await
                _botActions.UpdateSettingsMenu(query.From, query.Message.MessageId, SettingsKeyboardState.LabelsMenu, SelectedOption.None, userSettings);
        }

        public async Task HandleCallbackQLabelActionsMenu(CallbackQuery query, SettingsCallbackData callbackData)
        {
            Methods.SearchServiceByUserId(query.From);
            var userSettings = await _dbWorker.FindUserSettingsAsync(query.From);
            if (userSettings == null)
                return;

            var tempData = await _dbWorker.FindTempDataAsync(userSettings.UserId);
            if (tempData == null)
                await
                    _dbWorker.AddNewTempDataAsync(new TempDataModel
                    {
                        UserId = userSettings.UserId,
                        EditableLabelId = callbackData.LabelId,
                    });
            else
            {
                tempData.EditableLabelId = callbackData.LabelId;
                await _dbWorker.UpdateTempDataRecordAsync(tempData);
            }
            await
                _botActions.UpdateSettingsMenu(query.From, query.Message.MessageId, SettingsKeyboardState.LabelActionsMenu, SelectedOption.None, userSettings, tempData);
        }

        public async Task HandleCallbackQWhitelistLabelAction(CallbackQuery query, SettingsCallbackData callbackData)
        {
            var service = Methods.SearchServiceByUserId(query.From);
            var userSettings = await _dbWorker.FindUserSettingsAsync(query.From);
            if (userSettings == null)
                return;

            var label = await Methods.GetLabelAsync(callbackData.LabelId, service);
            var labelModel = userSettings.Whitelist.SingleOrDefault(w => w.Name == label.Name);
            if (labelModel == null)
                userSettings.Whitelist.Add(new LabelModel { Name = label.Name, LabelId = label.Id });
            else
                userSettings.Whitelist.Remove(labelModel);
            await _dbWorker.UpdateUserSettingsRecordAsync(userSettings);
            await
                _botActions.UpdateSettingsMenu(query.From, query.Message.MessageId, SettingsKeyboardState.WhiteListMenu, SelectedOption.None, userSettings);
        }

        public async Task HandleCallbackQBlacklistLabelAction(CallbackQuery query, SettingsCallbackData callbackData)
        {
            var service = Methods.SearchServiceByUserId(query.From);
            var userSettings = await _dbWorker.FindUserSettingsAsync(query.From);
            if (userSettings == null)
                return;

            var label = await Methods.GetLabelAsync(callbackData.LabelId, service);
            var labelModel = userSettings.Blacklist.SingleOrDefault(b => b.Name == label.Name);
            if (labelModel == null)
                userSettings.Blacklist.Add(new LabelModel { Name = label.Name, LabelId = label.Id });
            else
                userSettings.Blacklist.Remove(labelModel);
            await _dbWorker.UpdateUserSettingsRecordAsync(userSettings);
            await
                _botActions.UpdateSettingsMenu(query.From, query.Message.MessageId, SettingsKeyboardState.BlackListMenu, SelectedOption.None, userSettings);
        }
        #endregion

        #region Label actions menu

        public async Task HandleCallbackQRemoveLabel(CallbackQuery query, SettingsCallbackData callbackData)
        {
            var service = Methods.SearchServiceByUserId(query.From);
            await Methods.DeleteLabelAsync(callbackData.LabelId, service);
            await HandleCallbackQBackToEditLabelsListMenu(query);
        }

        public async Task HandleCallbackQBackToEditLabelsListMenu(CallbackQuery query)
        {
            var service = Methods.SearchServiceByUserId(query.From);
            var labels = await Methods.GetLabelsList(service);
            var labelInfoList = labels.Select(l => new LabelInfo { Name = l.Name, LabelId = l.Id } as ILabelInfo);

            await
                _botActions.UpdateSettingsMenu(query.From, query.Message.MessageId, SettingsKeyboardState.EditLabelsMenu, SelectedOption.None, null, null, labelInfoList);
        }

        #endregion

        #region ignore menu

        public async Task HandleCallbackQDisplayIgnoredEmails(CallbackQuery query, SettingsCallbackData callbackData)
        {
            var settings = await _dbWorker.FindUserSettingsAsync(query.From);

            await
                _botActions.UpdateSettingsMenu(query.From, query.Message.MessageId, SettingsKeyboardState.IgnoreMenu,
                    callbackData.Option);
        }

        #endregion

        #region permissions menu

        public async Task HandleCallbackQSwapPermissions(CallbackQuery query)
        {
            await _authorizer.SendAuthorizeLink(query);
            await _botActions.DeleteMessage(query.From, query.Message.MessageId);
        }

        public async Task HandleCallbackQRevokePermissions(CallbackQuery query)
        {
            var service = Methods.SearchServiceByUserId(query.From);
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

        public async Task HandleCallbackQBackToMainMenu(CallbackQuery query)
        {
            Methods.SearchServiceByUserId(query.From);
            var userSettings = await _dbWorker.FindUserSettingsAsync(query.From);
            if (userSettings == null)
                return;

            await
                _botActions.UpdateSettingsMenu(query.From, query.Message.MessageId, SettingsKeyboardState.MainMenu, SelectedOption.None, userSettings);
        }
    }
}