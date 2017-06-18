using System;
using CoffeeJelly.TelegramBotApiWrapper.Extensions;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Types.Messages
{
    /// <summary>
    /// A class which represents the Telegram video note message.
    /// </summary>
    public class VideoNoteMessage : Message, IVideoNoteMessage
    {
        /// <summary>
        /// Information about the video message.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("video_note")]
        public VideoNote VideoNote { get; set; }
    }
}