using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoffeeJelly.gmailNotifyBot.Bot.Exceptions
{

    [Serializable]
    public class DbDataStoreException : Exception
    {
        public DbDataStoreException() { }
        public DbDataStoreException(string message) : base(message) { }
        public DbDataStoreException(string message, Exception inner) : base(message, inner) { }
        protected DbDataStoreException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}