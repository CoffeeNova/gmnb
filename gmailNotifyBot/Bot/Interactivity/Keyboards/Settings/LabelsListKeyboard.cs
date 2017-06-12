using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Settings
{
    internal abstract class LabelsListKeyboard : SettingsKeyboard
    {

        protected override void ButtonsInitializer()
        {

            BackLabelsListButton = InitButton(GeneralButtonCaption.Back, CallbackCommand.LABELSLIST_BACK_COMMAND);
        }

        protected override IEnumerable<IEnumerable<InlineKeyboardButton>> DefineInlineKeyboard()
        {
            if (BackLabelsButton != null)
                BackLabelsListRow.Add(BackLabelsButton);

            var inlineKeyboard = new List<List<InlineKeyboardButton>>
            {
                DisplayLabelsRow, CreateLabelRow, RemoveLabelRow, EditLabelRow,
                WhitelistRow, BlacklistRow, BackLabelsRow
            };

            return inlineKeyboard;
        }



        protected InlineKeyboardButton BackLabelsListButton { get; set; }
        protected List<InlineKeyboardButton> BackLabelsListRow = new List<InlineKeyboardButton>();

    }
}