using System;
using CoffeeJelly.TelegramBotApiWrapper.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;


namespace CoffeeJelly.TelegramBotApiWrapper.Exceptions
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


        public static TelegramMethodsException CreateException<T>(Response<T> response)
        {
            return new TelegramMethodsException(response.Description)
            {
                ErrorCode = response.ErrorCode,
                Parameters = response.Parameters
            };
        }
        public int ErrorCode { get; set; }
        public ResponseParameters Parameters { get; set; }

    }
}