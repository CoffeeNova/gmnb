using System;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Getmessage;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards
{


    [Serializable]
    public class GetCallbackData : ICallbackData
    {
        public GetCallbackData()
        {

        }
        public GetCallbackData(GetCallbackData callbackData)
        {
            callbackData.NullInspect(nameof(callbackData));
            AttachProperties(callbackData);
        }

        public void AttachProperties(GetCallbackData callbackData)
        {
            Command = callbackData.Command;
            MessageId = callbackData.MessageId;
            Page = callbackData.Page;
            MessageKeyboardState = callbackData.MessageKeyboardState;
            AttachmentIndex = callbackData.AttachmentIndex;
        }

        public GetCallbackData(string serializedCallbackData)
        {
            try
            {
                var splitted = serializedCallbackData.Split(SEPARATOR);
                Command = splitted[0];
                MessageId = splitted[1];
                Page = Int32.Parse(splitted[2]);
                MessageKeyboardState = splitted[3].ToEnum<GetKeyboardState>();
                AttachmentIndex = Int32.Parse(splitted[4]);
            }
            catch
            {
                throw new ArgumentException("Must be an implicit operator returned type.", nameof(serializedCallbackData));
            }

        }

        public string Command { get; set; } = "";

        public string MessageId { get; set; } = "";

        public int Page { get; set; }

        public int AttachmentIndex { get; set; }

        public GetKeyboardState MessageKeyboardState { get; set; } = GetKeyboardState.Minimized;

        protected const char SEPARATOR = ':';

        public static implicit operator string(GetCallbackData obj)
        {
            return $"{obj.Command}{SEPARATOR}{obj.MessageId}{SEPARATOR}{obj.Page}{SEPARATOR}{obj.MessageKeyboardState.ToEnumString()}{SEPARATOR}{obj.AttachmentIndex}";
        }
    }
}