using System.Collections.Generic;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Settings
{
    internal class EditLabelsListKeyboard : LabelsListKeyboard
    {
        public EditLabelsListKeyboard(Dictionary<string, string> labels) : base(labels)
        {
        }

        protected override SettingsKeyboardState State { get; } = SettingsKeyboardState.EditLabelsList;

        protected override string LabelButtonCommand { get; set; } = CallbackCommand.LABELSLIST_EDIT_COMMAND;
    }
}