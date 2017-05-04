using System;
using CoffeeJelly.TelegramApiWrapper.Extensions;
using CoffeeJelly.TelegramApiWrapper.Types.General;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramApiWrapper.Types.Message
{
    /// <summary>
    /// A class which represents the Telegram audio message.
    /// </summary>
    public class AudioMessage : Message, IAudioMessage
    {
        /// <summary>
        /// Information about the audio file.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("audio")]
        public Audio Audio { get; set; }

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