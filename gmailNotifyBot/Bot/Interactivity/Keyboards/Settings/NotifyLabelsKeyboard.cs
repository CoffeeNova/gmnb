using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Settings
{
    internal class NotifyLabelsKeyboard : LabelsKeyboard
    {
        public NotifyLabelsKeyboard(bool whitelistEnabled) : base(whitelistEnabled)
        {
        }

        protected override void ButtonsInitializer()
        {
            WhitelistButton = InitButton(InlineKeyboardType.CallbackData, WhitelistButtonCaption, CallbackCommand.WHITELIST_MENU_COMMAND, SelectedOption.Option3);
            BlacklistButton = InitButton(InlineKeyboardType.CallbackData, BlacklistButtonCaption, CallbackCommand.BLACKLIST_MENU_COMMAND, SelectedOption.Option4);
            BackLabelsButton = InitButton(InlineKeyboardType.CallbackData, GeneralButtonCaption.Back, CallbackCommand.LABELS_BACK_COMMAND, SelectedOption.Option10);
        }
    }
}