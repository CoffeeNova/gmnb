using System;
using System.Collections;
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
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Settings;
using CoffeeJelly.gmailNotifyBot.Bot.Moduls.GoogleRequests;
using CoffeeJelly.gmailNotifyBot.Bot.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.CallbackQueryUpdates
{
    public partial class CallbackQueryHandler
    {
        #region main menu handlers
        public async Task HandleCallbackQLabels(CallbackQuery query)
        {
            Methods.SearchServiceByUserId(query.From);
            var userSettings = await _dbWorker.FindUserSettingsAsync(query.From);
            if (userSettings == null)
                return;

            await 
                _botActions.UpdateSettingsMenu(query.From, query.Message.MessageId, SettingsKeyboardState.Labels, SelectedOption.None, userSettings);
        }

        public async Task HandleCallbackQPermissions(CallbackQuery query)
        {
            Methods.SearchServiceByUserId(query.From);
            var userSettings = await _dbWorker.FindUserSettingsAsync(query.From);
            if (userSettings == null)
                return;

            await 
                _botActions.UpdateSettingsMenu(query.From, query.Message.MessageId, SettingsKeyboardState.Labels, SelectedOption.None, userSettings);
        }

        public async Task HandleCallbackIgnore(CallbackQuery query)
        {
            Methods.SearchServiceByUserId(query.From);
            await 
                _botActions.UpdateSettingsMenu(query.From, query.Message.MessageId, SettingsKeyboardState.Ignore);
        }

        public async Task HandleCallbackAbout(CallbackQuery query, SettingsCallbackData callbackData)
        {
            Methods.SearchServiceByUserId(query.From);
            await
                _botActions.UpdateSettingsMenu(query.From, query.Message.MessageId, SettingsKeyboardState.MainMenu, callbackData.Option);
        }

        #endregion

        #region Labels menu

        public async Task HandleCallbackQDisplayLabels(CallbackQuery query)
        {
            Methods.SearchServiceByUserId(query.From);
            var userSettings = await _dbWorker.FindUserSettingsAsync(query.From);
            if (userSettings == null)
                return;

            await
                _botActions.UpdateSettingsMenu(query.From, query.Message.MessageId, SettingsKeyboardState.Labels, SelectedOption.None, userSettings);
        }

        public async Task HandleCallbackQCreateLabel(CallbackQuery query)
        {
            Methods.SearchServiceByUserId(query.From);
            var userSettings = await _dbWorker.FindUserSettingsAsync(query.From);
            if (userSettings == null)
                return;

            await
                _botActions.UpdateSettingsMenu(query.From, query.Message.MessageId, SettingsKeyboardState.Labels, SelectedOption.None, userSettings);
        }

        public async Task HandleCallbackQWhitelist(CallbackQuery query)
        {
            Methods.SearchServiceByUserId(query.From);
            var userSettings = await _dbWorker.FindUserSettingsAsync(query.From);
            if (userSettings == null)
                return;

            await
               _botActions.UpdateSettingsMenu(query.From, query.Message.MessageId, SettingsKeyboardState.WhiteList, SelectedOption.None, userSettings);
        }

        public async Task HandleCallbackQBlacklist(CallbackQuery query)
        {
            Methods.SearchServiceByUserId(query.From);
            var userSettings = await _dbWorker.FindUserSettingsAsync(query.From);
            if (userSettings == null)
                return;

            await
               _botActions.UpdateSettingsMenu(query.From, query.Message.MessageId, SettingsKeyboardState.BlackList, SelectedOption.None, userSettings);
        }

        public async Task HandleCallbackQBackToMainMenu(CallbackQuery query, SettingsCallbackData callbackData)
        {
            Methods.SearchServiceByUserId(query.From);
            var userSettings = await _dbWorker.FindUserSettingsAsync(query.From);
            if (userSettings == null)
                return;

            await
                _botActions.UpdateSettingsMenu(query.From, query.Message.MessageId, SettingsKeyboardState.MainMenu, SelectedOption.None, userSettings);
        }


        #endregion
        }
}