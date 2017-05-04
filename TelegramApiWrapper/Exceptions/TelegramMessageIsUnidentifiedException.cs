using System;

namespace CoffeeJelly.TelegramApiWrapper.Exceptions
{
        [Serializable]
        public class TelegramMessageIsUnidentifiedException : Exception
        {
            public TelegramMessageIsUnidentifiedException() { }
            public TelegramMessageIsUnidentifiedException(string message) : base(message) { }
            public TelegramMessageIsUnidentifiedException(string message, Exception inner) : base(message, inner) { }
            protected TelegramMessageIsUnidentifiedException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        }
}