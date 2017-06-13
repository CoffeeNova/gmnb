﻿using System.Collections.Generic;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Settings
{
    internal class WhiteListKeyboard : LabelsListKeyboard
    {
        public WhiteListKeyboard(Dictionary<string, string> labels) : base(labels)
        {
        }

        protected override SettingsKeyboardState State { get; } = SettingsKeyboardState.WhiteList;

        protected override string LabelButtonCommand { get; set; } = CallbackCommand.LABELSLIST_WHITELIST_COMMAND;
    }
}