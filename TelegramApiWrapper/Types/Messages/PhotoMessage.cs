using System;
using System.Collections.Generic;
using CoffeeJelly.TelegramApiWrapper.Converters;
using CoffeeJelly.TelegramApiWrapper.Extensions;
using CoffeeJelly.TelegramApiWrapper.Types.General;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramApiWrapper.Types.Messages
{
    /// <summary>
    /// A class which represents the Telegram photo message.
    /// </summary>
    public class PhotoMessage : Message, IPhotoMessage
    {
        /// <summary>
        /// Available sizes of the photo.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonConverter(typeof(ArrayToListConverter<PhotoSize>))]
        [JsonProperty("photo")]
        public List<PhotoSize> Photo { get; set; }

        private string _caption;

        /// <summary>
        /// Caption for the photo, 0-200 characters.
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