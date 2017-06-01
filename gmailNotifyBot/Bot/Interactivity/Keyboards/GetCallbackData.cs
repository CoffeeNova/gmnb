using System;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Getmessage;
using CoffeeJelly.gmailNotifyBot.Bot.Types;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards
{


    [Serializable]
    public class GetCallbackData : CallbackData
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
                MessageKeyboardState = (GetKeyboardState) Enum.Parse(typeof(GetKeyboardState), splitted[3]);
                AttachmentIndex = Int32.Parse(splitted[4]);
            }
            catch
            {
                throw new ArgumentException("Must be an implicit operator returned type.", nameof(serializedCallbackData));
            }

        }


        public string MessageId { get; set; } = "";

        public int Page { get; set; }

        public int AttachmentIndex { get; set; }

        public GetKeyboardState MessageKeyboardState { get; set; } = GetKeyboardState.Minimized;

        public static implicit operator string(GetCallbackData obj)
        {
            return $"{obj.Command}{SEPARATOR}{obj.MessageId}{SEPARATOR}{obj.Page}{SEPARATOR}" +
                   $"{(int)obj.MessageKeyboardState}{SEPARATOR}{obj.AttachmentIndex}{SEPARATOR}{(int)obj.Type}";
        }

        public override CallbackDataType Type { get; } = CallbackDataType.GetCallbackData;
    }
}