using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.Types;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.General;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Settings;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards
{
    public class GeneralCallbackData : CallbackData
    {
        public GeneralCallbackData()
        {

        }

        public GeneralCallbackData(GeneralCallbackData callbackData)
        {
            callbackData.NullInspect(nameof(callbackData));
            AttachProperties(callbackData);
        }

        public GeneralCallbackData(string serializedCallbackData)
        {
            try
            {
                var splitted = serializedCallbackData.Split(SEPARATOR);
                Command = splitted[0];
                MessageKeyboardState = (GeneralKeyboardState)Enum.Parse(typeof(GeneralKeyboardState), splitted[1]);
            }
            catch
            {
                throw new ArgumentException("Must be an implicit operator returned type.", nameof(serializedCallbackData));
            }
        }

        public void AttachProperties(GeneralCallbackData callbackData)
        {
            Command = callbackData.Command;
        }

        public static implicit operator string(GeneralCallbackData obj)
        {
            return $"{obj.Command}{SEPARATOR}" +
                   $"{(int)obj.MessageKeyboardState}{SEPARATOR}" +
                   $"{(int)obj.Type}";
        }

        public GeneralKeyboardState MessageKeyboardState { get; set; } = GeneralKeyboardState.ResumeNotifications;
        public override CallbackDataType Type { get; } = CallbackDataType.GeneralCallbackData;
    }
}