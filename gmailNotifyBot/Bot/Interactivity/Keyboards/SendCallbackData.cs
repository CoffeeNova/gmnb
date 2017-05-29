using System;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Sendmessage;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards
{
    [Serializable]
    public class SendCallbackData : ICallbackCommand
    {
        public SendCallbackData()
        {

        }
        public SendCallbackData(SendCallbackData callbackData)
        {
            callbackData.NullInspect(nameof(callbackData));
            AttachProperties(callbackData);
        }

        public void AttachProperties(SendCallbackData callbackData)
        {
            Command = callbackData.Command;
            MessageId = callbackData.MessageId;
            DraftId = callbackData.DraftId;
            MessageKeyboardState = callbackData.MessageKeyboardState;
        }

        public SendCallbackData(string serializedCallbackData)
        {
            try
            {
                var splitted = serializedCallbackData.Split(SEPARATOR);
                Command = splitted[0];
                MessageId = splitted[1];
                DraftId = splitted[2];
                MessageKeyboardState = splitted[3].ToEnum<SendKeyboardState>();
            }
            catch
            {
                throw new ArgumentException("Must be an implicit operator returned type.", nameof(serializedCallbackData));
            }

        }

        public string Command { get; set; } = "";

        public string MessageId { get; set; } = "";

        public string DraftId { get; set; } = "";

        public SendKeyboardState MessageKeyboardState { get; set; } = SendKeyboardState.Init;

        protected const char SEPARATOR = ':';

        public static implicit operator string(SendCallbackData obj)
        {
            return $"{obj.Command}{SEPARATOR}{obj.MessageId}{SEPARATOR}{obj.DraftId}{SEPARATOR}{obj.MessageKeyboardState.ToEnumString()}{SEPARATOR}";
        }
    }
}