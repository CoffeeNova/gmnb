using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoffeeJelly.gmailNotifyBot.Bot.Exceptions
{

    [Serializable]
    public class RevokeTokenException : Exception
    {
        public RevokeTokenException() { }
        public RevokeTokenException(string message) : base(message) { }
        public RevokeTokenException(string message, Exception inner) : base(message, inner) { }
        protected RevokeTokenException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}