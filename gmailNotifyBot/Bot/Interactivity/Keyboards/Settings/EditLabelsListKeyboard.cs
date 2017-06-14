using System.Collections.Generic;
using CoffeeJelly.gmailNotifyBot.Bot.Types;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Settings
{
    internal class EditLabelsListKeyboard : LabelsListKeyboard
    {
        public EditLabelsListKeyboard(IEnumerable<ILabelInfo> labels) : base(labels)
        {
        }

        protected override SettingsKeyboardState State { get; } = SettingsKeyboardState.EditLabelsList;

        protected override string LabelButtonCommand { get; set; } = CallbackCommand.LABEL_ACTIONS_MENU_COMMAND;
    }
}