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
        protected LabelsListKeyboard(IEnumerable<LabelInfo> labels)
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
                LabelsListRowList.Add(new List<InlineKeyboardButton> { button });
            }
            BackLabelsListButton = InitButton(InlineKeyboardType.CallbackData, GeneralButtonCaption.Back, CallbackCommand.LABELS_LIST_BACK_COMMAND, SelectedOption.Option10);
        }


        protected override IEnumerable<IEnumerable<InlineKeyboardButton>> DefineInlineKeyboard()
        {
            if (BackLabelsListButton != null)
                BackLabelsListRow.Add(BackLabelsListButton);

            var inlineKeyboard = new List<List<InlineKeyboardButton>>(LabelsListRowList)
            {
                BackLabelsListRow
            };

            return inlineKeyboard;
        }

        protected abstract string LabelButtonCommand { get; set; }

        protected IEnumerable<LabelInfo> Labels;
        protected InlineKeyboardButton BackLabelsListButton { get; set; }

        protected List<List<InlineKeyboardButton>> LabelsListRowList = new List<List<InlineKeyboardButton>>();
        protected List<InlineKeyboardButton> BackLabelsListRow = new List<InlineKeyboardButton>();
    }
}