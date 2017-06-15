using System.Collections.Generic;
using CoffeeJelly.gmailNotifyBot.Bot.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;


namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Settings
{
    internal class BlackListKeyboard : LabelsListKeyboard
    {
        public BlackListKeyboard(IEnumerable<LabelInfo> labels, List<LabelInfo> blacklistedLabels, bool blackListEnabled) : base(labels)
        {
            BlacklistedLabels = blacklistedLabels;
            BlacklistEnabled = blackListEnabled;
        }

        protected override void ButtonsInitializer()
        {
            if (BlacklistEnabled)
                foreach (var label in Labels)
                {
                    string sign = string.Empty;
                    if (BlacklistedLabels.Exists(b => b.LabelId == label.LabelId))
                        sign = Emoji.RESTRICTION_SIGN;

                    var button = InitButton(InlineKeyboardType.CallbackData, $"{sign} {label.Name}", LabelButtonCommand,
                        SelectedOption.None, label.LabelId);
                    LabelsListRowList.Add(new List<InlineKeyboardButton> {button});
                }
            else
            {
                var useButton = InitButton(InlineKeyboardType.CallbackData, LabelListButtonCaption.UseBlacklist, UseBlacklistButtonCommand, SelectedOption.Option1);
                LabelsListRowList.Add(new List<InlineKeyboardButton> {useButton});
            }
            BackLabelsListButton = InitButton(InlineKeyboardType.CallbackData, GeneralButtonCaption.Back, CallbackCommand.LABELS_LIST_BACK_COMMAND, SelectedOption.Option10);
        }

        protected override SettingsKeyboardState State { get; } = SettingsKeyboardState.BlackListMenu;

        protected override string LabelButtonCommand { get; set; } = CallbackCommand.BLACKLIST_ACTION_COMMAND;


        protected List<LabelInfo> BlacklistedLabels { get; set; }

        protected bool BlacklistEnabled { get; set; }
        protected string UseBlacklistButtonCommand { get; set; } = CallbackCommand.USE_BLACKLIST_COMMAND;
    }
}