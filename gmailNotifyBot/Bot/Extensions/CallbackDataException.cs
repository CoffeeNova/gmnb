using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoffeeJelly.gmailNotifyBot.Bot.Extensions
{
    [Serializable]
    public class CallbackDataException : Exception
    {
        public CallbackDataException() { }
        public CallbackDataException(string message) : base(message) { }
        public CallbackDataException(string message, Exception inner) : base(message, inner) { }
        protected CallbackDataException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}