using System;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Sendmessage;
using CoffeeJelly.gmailNotifyBot.Bot.Types;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards
{
    [Serializable]
    public class SendCallbackData : CallbackData
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
            Row = callbackData.Row;
            Column = callbackData.Column;
        }

        public SendCallbackData(string serializedCallbackData)
        {
            try
            {
                var splitted = serializedCallbackData.Split(SEPARATOR);
                Command = splitted[0];
                MessageId = splitted[1];
                DraftId = splitted[2];
                MessageKeyboardState = (SendKeyboardState)Enum.Parse(typeof(SendKeyboardState), splitted[3]);
                Row = (NmStoreUnit)Enum.Parse(typeof(NmStoreUnit), splitted[4]);
                Column = int.Parse(splitted[5]);
            }
            catch
            {
                throw new ArgumentException("Must be an implicit operator returned type.", nameof(serializedCallbackData));
            }

        }

        public string MessageId { get; set; } = "";

        public string DraftId { get; set; } = "";

        public NmStoreUnit Row { get; set; }

        public int Column { get; set; } = -1;

        public SendKeyboardState MessageKeyboardState { get; set; } = SendKeyboardState.Init;

        public static implicit operator string(SendCallbackData obj)
        {
            return $"{obj.Command}{SEPARATOR}{obj.MessageId}{SEPARATOR}{obj.DraftId}" +
                   $"{SEPARATOR}{(int)obj.MessageKeyboardState}{SEPARATOR}{(int)obj.Row}" +
                   $"{SEPARATOR}{obj.Column}{SEPARATOR}{(int)obj.Type}";
            
        }

        public override CallbackDataType Type { get; } = CallbackDataType.SendCallbackData;
    }
}