
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramApiWrapper.Types.InlineQueryResult
{
    /// <summary>
    /// Represents a link to a photo stored on the Telegram servers. 
    /// By default, this photo will be sent by the user with an optional caption. 
    /// Alternatively, you can use <see cref="InlineQueryResultCachedPhoto.InputMessageContent"/> to send a message with the specified content instead of the photo.
    /// </summary>
    public class InlineQueryResultCachedPhoto : InlineQueryResultCaption, IResultTitle
    {
        /// <summary>
        /// Type of the result.
        /// </summary>
        [JsonProperty("type", Required = Required.Always)]
        public override string Type => InlineQueryResultType.Photo;

        /// <summary>
        /// A valid file identifier of the photo.
        /// </summary>
        [JsonProperty("photo_file_id", Required = Required.Always)]
        public string PhotoFileId { get; set; }

        /// <summary>
        /// Optional. Title for the result.
        /// </summary>
        [JsonProperty("title", Required = Required.Default)]
        public string Title { get; set; }

        /// <summary>
        /// Optional. Short description of the result.
        /// </summary>
        [JsonProperty("description", Required = Required.Default)]
        public string Description { get; set; }
    }
}
