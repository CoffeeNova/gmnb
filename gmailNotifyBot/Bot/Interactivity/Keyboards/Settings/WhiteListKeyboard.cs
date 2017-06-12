using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Settings
{
    internal class WhiteListKeyboard : LabelsListKeyboard
    {
        protected override void ButtonsInitializer()
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<IEnumerable<InlineKeyboardButton>> DefineInlineKeyboard()
        {
            throw new NotImplementedException();
        }



        protected override SettingsKeyboardState State { get; } = SettingsKeyboardState.WhiteList;
    }
}