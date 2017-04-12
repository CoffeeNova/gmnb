using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoffeeJelly.gmailNotifyBot.Bot.Telegram.Exceptions
{
    [Serializable]
    public class TelegramFileDownloadException : Exception
    {
        public TelegramFileDownloadException() { }
        public TelegramFileDownloadException(string message) : base(message) { }
        public TelegramFileDownloadException(string message, Exception inner) : base(message, inner) { }
        protected TelegramFileDownloadException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}