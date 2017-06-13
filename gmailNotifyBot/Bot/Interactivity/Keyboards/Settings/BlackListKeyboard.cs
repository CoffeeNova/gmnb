using System.Collections.Generic;


namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Settings
{
    internal class BlackListKeyboard : LabelsListKeyboard
    {
        public BlackListKeyboard(Dictionary<string, string> labels) : base(labels)
        {
        }

        protected override SettingsKeyboardState State { get; } = SettingsKeyboardState.BlackList;

        protected override string LabelButtonCommand { get; set; } = CallbackCommand.LABELSLIST_WHITELIST_COMMAND;
    }
}