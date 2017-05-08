using System;
using CoffeeJelly.TelegramBotApiWrapper.Extensions;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Types.Messages
{
    /// <summary>
    /// A class which represents the Telegram document message.
    /// </summary>
    public class DocumentMessage : Message, IDocumentMessage
    {
        /// <summary>
        /// Information about general file.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("document")]
        public Document Document { get; set; }

        private string _caption;

        /// <summary>
        /// Caption for the document, 0-200 characters.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("caption")]
        public string Caption
        {
            get { return _caption; }
            set
            {
                if (value != null && !value.Length.InRange(0, 200))
                    throw new ArgumentOutOfRangeException(nameof(value), $"{value} Length should be from 0 to 200 characters.");
                _caption = value;
            }
        }
    }
}