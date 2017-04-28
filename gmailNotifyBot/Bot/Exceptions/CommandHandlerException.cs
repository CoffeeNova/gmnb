using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoffeeJelly.gmailNotifyBot.Bot.Exceptions
{

    [Serializable]
    public class CommandHandlerException : Exception
    {
        public CommandHandlerException() { }
        public CommandHandlerException(string message) : base(message) { }
        public CommandHandlerException(string message, Exception inner) : base(message, inner) { }
        protected CommandHandlerException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}