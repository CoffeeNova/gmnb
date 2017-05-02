using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoffeeJelly.gmailNotifyBot.Bot.Exceptions
{
    [Serializable]
    public class FormattedGmailMessageException : Exception
    {
        public FormattedGmailMessageException() { }
        public FormattedGmailMessageException(string message) : base(message) { }
        public FormattedGmailMessageException(string message, Exception inner) : base(message, inner) { }
        protected FormattedGmailMessageException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}