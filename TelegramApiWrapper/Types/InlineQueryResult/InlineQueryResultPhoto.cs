using System;
using CoffeeJelly.TelegramApiWrapper.Extensions;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramApiWrapper.Types.InlineQueryResult
{
    /// <summary>
    /// Represents a link to a photo. By default, this photo will be sent by the user with optional caption. 
    /// Alternatively, you can use <see cref="IInputMessageContent"/> to send a message with the specified content instead of the photo.
    /// </summary>
    public class InlineQueryResultPhoto : InlineQueryResult, IResultPhoto, IResultTitle
    {
        /// <summary>
        /// Type of the result.
        /// </summary>
        [JsonProperty("type", Required = Required.Always)]
        public override string Type => InlineQueryResultType.Photo;

        /// <summary>
        /// A valid URL of the photo. Photo must be in jpeg format.
        /// </summary>
        /// <remarks>Photo size must not exceed 5MB.</remarks>
        [JsonProperty("photo_url", Required = Required.Always)]
        public string PhotoUrl { get; set; }

        /// <summary>
        /// URL of the thumbnail for the photo
        /// </summary>
        [JsonProperty("thumb_url", Required = Required.Always)]
        public string ThumbUrl { get; set; }

        /// <summary>
        /// Width of the photo
        /// </summary>
        [JsonProperty("photo_width")]
        public int? PhotoWidth { get; set; }

        /// <summary>
        /// Height of the photo
        /// </summary>
        [JsonProperty("photo_height")]
        public int? PhotoHeight { get; set; }

        /// <summary>
        /// Title for the result.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Short description of the result.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        private string _caption;

        /// <summary>
        /// Caption of the photo to be sent, 0-200 characters
        /// </summary>
        [JsonProperty("caption")]
        public string Caption
        {
            get { return _caption; }
            set
            {
                if (value != null && !value.Length.InRange(0, 200))
                    throw new ArgumentOutOfRangeException(nameof(value),
                        $"{value} Length should be from 0 to 200 characters.");
                _caption = value;

            }
        }
    }
}