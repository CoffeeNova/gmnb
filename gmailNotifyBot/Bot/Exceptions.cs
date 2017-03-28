using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoffeeJelly.gmailNotifyBot.Bot
{
    public class Exceptions
    {

        [Serializable]
        public class UnidentifiedTelegramMessageException : Exception
        {
            public UnidentifiedTelegramMessageException() { }
            public UnidentifiedTelegramMessageException(string message) : base(message) { }
            public UnidentifiedTelegramMessageException(string message, Exception inner) : base(message, inner) { }
            protected UnidentifiedTelegramMessageException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        }
    }
}