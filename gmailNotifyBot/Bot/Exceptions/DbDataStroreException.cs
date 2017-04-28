using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoffeeJelly.gmailNotifyBot.Bot.Exceptions
{

    [Serializable]
    public class DbDataStroreException : Exception
    {
        public DbDataStroreException() { }
        public DbDataStroreException(string message) : base(message) { }
        public DbDataStroreException(string message, Exception inner) : base(message, inner) { }
        protected DbDataStroreException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}