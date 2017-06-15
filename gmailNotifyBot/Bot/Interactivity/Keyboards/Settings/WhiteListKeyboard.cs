using System.Collections.Generic;
using CoffeeJelly.gmailNotifyBot.Bot.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Settings
{
    internal class WhiteListKeyboard : LabelsListKeyboard
    {
        public WhiteListKeyboard(IEnumerable<LabelInfo> labels, List<LabelInfo> whitelistedLabels, bool whitelistEnabled) : base(labels)
        {
            WhitelistedLabels = whitelistedLabels;
            WhitelistEnabled = whitelistEnabled;
        }

        protected override void ButtonsInitializer()
        {
            if (WhitelistEnabled)
                foreach (var label in Labels)
                {
                    string sign = string.Empty;
                    if (WhitelistedLabels.Exists(b => b.LabelId == label.LabelId))
                        sign = Emoji.GRAY_CHECKED_BOX;

                    var button = InitButton(InlineKeyboardType.CallbackData, $"{sign} {label.Name}", LabelButtonCommand,
                        SelectedOption.None, label.LabelId);
                    LabelsListRowList.Add(new List<InlineKeyboardButton> { button });
                }
            else
            {
                var useButton = InitButton(InlineKeyboardType.CallbackData, LabelListButtonCaption.UseWhitelist, UseWhitelistButtonCommand, SelectedOption.Option1);
                LabelsListRowList.Add(new List<InlineKeyboardButton> { useButton });
            }
            BackLabelsListButton = InitButton(InlineKeyboardType.CallbackData, GeneralButtonCaption.Back, CallbackCommand.LABELS_LIST_BACK_COMMAND, SelectedOption.Option10);
        }

        protected override SettingsKeyboardState State { get; } = SettingsKeyboardState.WhiteListMenu;

        protected override string LabelButtonCommand { get; set; } = CallbackCommand.WHITELIST_ACTION_COMMAND;

        protected List<LabelInfo> WhitelistedLabels { get; set; }

        protected bool WhitelistEnabled { get; set; }
        protected string UseWhitelistButtonCommand { get; set; } = CallbackCommand.USE_WHITELIST_COMMAND;
    }
}