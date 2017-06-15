using System.Collections.Generic;
using CoffeeJelly.gmailNotifyBot.Bot.Types;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Settings
{
    internal class EditLabelsKeyboard : LabelsListKeyboard
    {
        public EditLabelsKeyboard(IEnumerable<LabelInfo> labels) : base(labels)
        {
        }

        protected override SettingsKeyboardState State { get; } = SettingsKeyboardState.EditLabelsMenu;

        protected override string LabelButtonCommand { get; set; } = CallbackCommand.LABEL_ACTIONS_MENU_COMMAND;
    }
}