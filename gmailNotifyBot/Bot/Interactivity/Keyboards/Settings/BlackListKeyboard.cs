using System.Collections.Generic;
using CoffeeJelly.gmailNotifyBot.Bot.Types;


namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Settings
{
    internal class BlackListKeyboard : LabelsListKeyboard
    {
        public BlackListKeyboard(IEnumerable<ILabelInfo> labels, List<ILabelInfo> blacklistedLabels) : base(labels)
        {
            BlacklistedLabels = blacklistedLabels;
        }

        protected override void ButtonsInitializer()
        {
            foreach (var label in Labels)
            {
                string sign = string.Empty;
                if (BlacklistedLabels.Exists(b => b.LabelId == label.LabelId))
                    sign = Emoji.RESTRICTION_SIGN;

                var button = InitButton(InlineKeyboardType.CallbackData, $"{sign} {label.Name}", LabelButtonCommand,
                    SelectedOption.None, label.LabelId);
                LabelsListRow.Add(button);
            }
            BackLabelsListButton = InitButton(InlineKeyboardType.CallbackData, GeneralButtonCaption.Back, CallbackCommand.LABELS_LIST_BACK_COMMAND, SelectedOption.Option10);
        }

        protected override SettingsKeyboardState State { get; } = SettingsKeyboardState.BlackListMenu;

        protected override string LabelButtonCommand { get; set; } = CallbackCommand.BLACKLIST_ACTION_COMMAND;

        protected List<ILabelInfo> BlacklistedLabels { get; set; }
    }
}