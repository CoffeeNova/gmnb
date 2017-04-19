using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoffeeJelly.gmailNotifyBot.Bot.Exceptions
{

    [Serializable]
    public class ExchangeException : Exception
    {
        public ExchangeException() { }
        public ExchangeException(string message) : base(message) { }
        public ExchangeException(string message, Exception inner) : base(message, inner) { }
        protected ExchangeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}