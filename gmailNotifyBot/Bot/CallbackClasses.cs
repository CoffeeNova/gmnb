using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.gmailNotifyBot.Bot.Types;
using MsgPack.Serialization;

namespace CoffeeJelly.gmailNotifyBot.Bot
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
            AttachProperties(callbackData);
        }

        public void AttachProperties(CallbackData callbackData)
        {
            Command = callbackData.Command;
            MessageId = callbackData.MessageId;
            Page = callbackData.Page;
            MessageKeyboardState = callbackData.MessageKeyboardState;
            Etag = callbackData.Etag;
            Attachments = callbackData.Attachments;
        }

        public CallbackData(string serializedCallbackData)
        {
            try
            {
                var buffer = Base64.DecodeToBytesUrl(serializedCallbackData);
                using (var ms = new MemoryStream(buffer))
                {
                    var serializer = MessagePackSerializer.Get<CallbackData>();
                    var data = serializer.Unpack(ms);
                    AttachProperties(data);
                }
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

        public string Etag { get; set; } = "";

        public List<AttachmentInfo> Attachments { get; set; }

        public static implicit operator string(CallbackData obj)
        {
            using (var ms = new MemoryStream())
            {
                var serializer = MessagePackSerializer.Get<CallbackData>();
                serializer.Pack(ms, obj);
                return Base64.EncodeUrl(ms.ToArray());
            }
        }
    }
}