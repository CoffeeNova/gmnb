using Newtonsoft.Json;

namespace CoffeeJelly.TelegramApiWrapper.Types.InlineQueryResult
{
    /// <summary>
    /// Represents a link to a photo. By default, this photo will be sent by the user with optional caption. 
    /// Alternatively, you can use <see cref="InlineQueryResultPhoto.InputMessageContent"/> to send a message with the specified content instead of the photo.
    /// </summary>
    public class InlineQueryResultPhoto : InlineQueryResultCaption, IResultPhoto, IResultTitle
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
    }
}