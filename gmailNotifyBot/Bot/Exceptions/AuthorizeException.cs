using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoffeeJelly.gmailNotifyBot.Bot.Exceptions
{

    [Serializable]
    public class AuthorizeException : Exception
    {
        public AuthorizeException() { }
        public AuthorizeException(string message) : base(message) { }
        public AuthorizeException(string message, Exception inner) : base(message, inner) { }
        protected AuthorizeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}