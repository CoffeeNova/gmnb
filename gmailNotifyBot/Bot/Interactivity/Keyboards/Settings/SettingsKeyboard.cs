using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;
using CoffeeJelly.TelegramBotApiWrapper.Types.Messages;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Settings
{
    internal abstract class SettingsKeyboard : Keyboard
    {
        public override void CreateInlineKeyboard()
        {
            GeneralCallbackData = new SettingsCallbackData
            {
                MessageKeyboardState = State
            };
            ButtonsInitializer();
            base.InlineKeyboard = DefineInlineKeyboard();
        }

        protected virtual InlineKeyboardButton InitButton(InlineKeyboardType type, string text, string command, SelectedOption option = default(SelectedOption), 
            string labelId = "", bool forceCallbackData = true)
        {
            if (!forceCallbackData)
                return base.InitButton(type, text, command);
            var callbackData = new SettingsCallbackData(GeneralCallbackData)
            {
                Command = command,
                Option = option,
                LabelId = labelId
            };
            return base.InitButton(type, text, callbackData);
        }

        protected abstract SettingsKeyboardState State { get; }
        protected SettingsCallbackData GeneralCallbackData;
    }

}