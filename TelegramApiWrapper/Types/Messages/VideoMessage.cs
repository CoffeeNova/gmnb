using System;
using CoffeeJelly.TelegramApiWrapper.Extensions;
using CoffeeJelly.TelegramApiWrapper.Types.General;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramApiWrapper.Types.Messages
{
    /// <summary>
    /// A class which represents the Telegram video message.
    /// </summary>
    public class VideoMessage : Message, IVideoMessage
    {
        /// <summary>
        /// Information about the video.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("video")]
        public Video Video { get; set; }

        private string _caption;

        /// <summary>
        /// Caption for the video, 0-200 characters.
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