using System;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity
{
    public interface ICallbackCommand
    {
        string Command { get; set; }
    }


    [Serializable]
    public class CallbackData : ICallbackCommand
    {
        public CallbackData()
        {

        }
        public CallbackData(CallbackData callbackData)
        {
            callbackData.NullInspect(nameof(callbackData));
            AttachProperties(callbackData);
        }

        public void AttachProperties(CallbackData callbackData)
        {
            Command = callbackData.Command;
            MessageId = callbackData.MessageId;
            Page = callbackData.Page;
            MessageKeyboardState = callbackData.MessageKeyboardState;
        }

        public CallbackData(string serializedCallbackData)
        {
            try
            {
                var splitted = serializedCallbackData.Split(SEPARATOR);
                Command = splitted[0];
                MessageId = splitted[1];
                Page = Int32.Parse(splitted[2]);
                MessageKeyboardState = splitted[3].ToEnum<MessageKeyboardState>();
            }
            catch
            {
                throw new ArgumentException("Must be an implicit operator returned type.", nameof(serializedCallbackData));
            }

        }

        public string Command { get; set; } = "";

        public string MessageId { get; set; } = "";

        public int Page { get; set; }

        public MessageKeyboardState MessageKeyboardState { get; set; } = MessageKeyboardState.Minimized;

        protected const char SEPARATOR = ':';

        public static implicit operator string(CallbackData obj)
        {
            return $"{obj.Command}{SEPARATOR}{obj.MessageId}{SEPARATOR}{obj.Page}{SEPARATOR}{obj.MessageKeyboardState.ToEnumString()}";
        }
    }
}