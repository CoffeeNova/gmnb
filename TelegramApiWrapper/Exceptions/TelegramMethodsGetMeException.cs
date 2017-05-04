using System;

namespace CoffeeJelly.TelegramApiWrapper.Exceptions
{
        [Serializable]
        public class TelegramMethodsException : Exception
        {
            public TelegramMethodsException() { }
            public TelegramMethodsException(string message) : base(message) { }
            public TelegramMethodsException(string message, Exception inner) : base(message, inner) { }
            protected TelegramMethodsException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        }
}