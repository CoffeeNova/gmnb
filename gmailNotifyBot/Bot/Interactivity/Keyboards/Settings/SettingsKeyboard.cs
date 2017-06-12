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

        protected virtual InlineKeyboardButton InitButton(string text, string callbackCommand)
        {
            return new InlineKeyboardButton
            {
                Text = text,
                CallbackData = new SettingsCallbackData(GeneralCallbackData)
                {
                    Command = callbackCommand
                }
            };
        }

        protected abstract SettingsKeyboardState State { get; }
        protected SettingsCallbackData GeneralCallbackData;
    }
}