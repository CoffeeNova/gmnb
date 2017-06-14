using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Settings
{
    internal abstract class LabelsListKeyboard : SettingsKeyboard
    {
        protected LabelsListKeyboard(IEnumerable<ILabelInfo> labels)
        {
            if (labels == null)
                throw new ArgumentNullException(nameof(labels));

            Labels = labels;
        }
        protected override void ButtonsInitializer()
        {
            foreach (var label in Labels)
            {
                var button = InitButton(InlineKeyboardType.CallbackData, label.Name, LabelButtonCommand,
                    SelectedOption.None, label.LabelId);
                LabelsListRow.Add(button);
            }
            BackLabelsListButton = InitButton(InlineKeyboardType.CallbackData, GeneralButtonCaption.Back, CallbackCommand.LABELS_LIST_BACK_COMMAND, SelectedOption.Option10);
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

        protected IEnumerable<ILabelInfo> Labels;
        protected InlineKeyboardButton BackLabelsListButton { get; set; }

        protected List<InlineKeyboardButton> LabelsListRow = new List<InlineKeyboardButton>();
        protected List<InlineKeyboardButton> BackLabelsListRow = new List<InlineKeyboardButton>();
    }
}