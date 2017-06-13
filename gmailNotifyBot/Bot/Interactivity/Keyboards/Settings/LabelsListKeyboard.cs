using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Settings
{
    internal abstract class LabelsListKeyboard : SettingsKeyboard
    {
        protected LabelsListKeyboard(Dictionary<string, string> labels)
        {
            if (labels == null)
                throw new ArgumentNullException(nameof(labels));

            Labels = labels;
        }
        protected override void ButtonsInitializer()
        {
            foreach (var label in Labels)
            {
                var button = InitButton(InlineKeyboardType.CallbackData, label.Value, LabelButtonCommand,
                    SelectedOption.None, label.Key);
                LabelsListRow.Add(button);
            }
            BackLabelsListButton = InitButton(InlineKeyboardType.CallbackData, GeneralButtonCaption.Back, CallbackCommand.LABELSLIST_BACK_COMMAND, SelectedOption.Option10);
        }


        protected override IEnumerable<IEnumerable<InlineKeyboardButton>> DefineInlineKeyboard()
        {
            if (BackLabelsListButton != null)
                BackLabelsListRow.Add(BackLabelsListButton);

            var inlineKeyboard = new List<List<InlineKeyboardButton>>
            {
                LabelsListRow, BackLabelsListRow 
            };

            return inlineKeyboard;
        }

        protected abstract string LabelButtonCommand { get; set; }

        protected Dictionary<string, string> Labels;
        protected InlineKeyboardButton BackLabelsListButton { get; set; }

        protected List<InlineKeyboardButton> LabelsListRow = new List<InlineKeyboardButton>();
        protected List<InlineKeyboardButton> BackLabelsListRow = new List<InlineKeyboardButton>();
    }
}