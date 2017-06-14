using System.Collections.Generic;
using CoffeeJelly.gmailNotifyBot.Bot.Types;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Settings
{
    internal class WhiteListKeyboard : LabelsListKeyboard
    {
        public WhiteListKeyboard(IEnumerable<ILabelInfo> labels, List<ILabelInfo> whitelistedLabels) : base(labels)
        {
            WhitelistedLabels = whitelistedLabels;
        }

        protected override void ButtonsInitializer()
        {
            foreach (var label in Labels)
            {
                string sign = string.Empty;
                if (WhitelistedLabels.Exists(b => b.LabelId == label.LabelId))
                    sign = Emoji.GRAY_CHECKED_BOX;

                var button = InitButton(InlineKeyboardType.CallbackData, $"{sign} {label.Name}", LabelButtonCommand,
                    SelectedOption.None, label.LabelId);
                LabelsListRow.Add(button);
            }
            BackLabelsListButton = InitButton(InlineKeyboardType.CallbackData, GeneralButtonCaption.Back, CallbackCommand.LABELS_LIST_BACK_COMMAND, SelectedOption.Option10);
        }

        protected override SettingsKeyboardState State { get; } = SettingsKeyboardState.WhiteListMenu;

        protected override string LabelButtonCommand { get; set; } = CallbackCommand.WHITELIST_ACTION_COMMAND;

        protected List<ILabelInfo> WhitelistedLabels { get; set; }
    }
}