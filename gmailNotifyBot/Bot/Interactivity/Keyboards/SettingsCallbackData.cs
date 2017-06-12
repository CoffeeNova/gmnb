using System;
using System.Collections.Generic;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Settings;
using CoffeeJelly.gmailNotifyBot.Bot.Types;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards
{
    public class SettingsCallbackData : CallbackData
    {
        public SettingsCallbackData()
        {
            
        }

        public SettingsCallbackData(SettingsCallbackData callbackData)
        {
            callbackData.NullInspect(nameof(callbackData));
            AttachProperties(callbackData);
        }

        public SettingsCallbackData(string serializedCallbackData)
        {
            try
            {
                var splitted = serializedCallbackData.Split(SEPARATOR);
                Command = splitted[0];
                MessageKeyboardState = (SettingsKeyboardState)Enum.Parse(typeof(SettingsKeyboardState), splitted[1]);
            }
            catch
            {
                throw new ArgumentException("Must be an implicit operator returned type.", nameof(serializedCallbackData));
            }
        }

        public void AttachProperties(SettingsCallbackData callbackData)
        {
            Command = callbackData.Command;
        }

        public static implicit operator string(SettingsCallbackData obj)
        {
            return $"{obj.Command}{SEPARATOR}" +
                   $"{(int)obj.MessageKeyboardState}{SEPARATOR}";
        }

        public SettingsKeyboardState MessageKeyboardState { get; set; } = SettingsKeyboardState.MainMenu;
        public override CallbackDataType Type { get; } = CallbackDataType.Settings;
    }
}