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
                Option = (SelectedOption)Enum.Parse(typeof(SelectedOption), splitted[1]);
                LabelId = splitted[2];
                MessageKeyboardState = (SettingsKeyboardState)Enum.Parse(typeof(SettingsKeyboardState), splitted[3]);
            }
            catch
            {
                throw new ArgumentException("Must be an implicit operator returned type.", nameof(serializedCallbackData));
            }
        }

        public void AttachProperties(SettingsCallbackData callbackData)
        {
            Command = callbackData.Command;
            Option = callbackData.Option;
            LabelId = callbackData.LabelId;
        }

        public static implicit operator string(SettingsCallbackData obj)
        {
            return $"{obj.Command}{SEPARATOR}" +
                $"{obj.Option}{SEPARATOR}" +
                $"{ obj.LabelId}{ SEPARATOR}" +
                   $"{(int)obj.MessageKeyboardState}{SEPARATOR}" +
                   $"{(int)obj.Type}";
        }

        public SettingsKeyboardState MessageKeyboardState { get; set; } = SettingsKeyboardState.MainMenu;
        public SelectedOption Option { get; set; }
        public string LabelId { get; set; }
        public override CallbackDataType Type { get; } = CallbackDataType.SettingsCallbackData;
    }
}