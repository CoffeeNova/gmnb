using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Types.InlineQueryResult
{
    /// <summary>
    /// Represents a link to a file. By default, this file will be sent by the user with an optional caption. 
    /// Alternatively, you can use <see cref="InlineQueryResultDocument.InputMessageContent"/> to send a message with the specified content instead of the file. 
    /// Currently, only .PDF and .ZIP files can be sent using this method.
    /// </summary>
    /// <remarks>This will only work in Telegram versions released after 9 April, 2016. Older clients will ignore them.</remarks>
    public class InlineQueryResultDocument : InlineQueryResultCaption, IResultThumb, IResultTitle
    {
        /// <summary>
        /// Type of the result.
        /// </summary>
        [JsonProperty("type", Required =Required.Always)]
        public override string Type => InlineQueryResultType.Document;

        /// <summary>
        /// Title for the result.
        /// </summary>
        [JsonProperty("title", Required = Required.Always)]
        public string Title { get; set; }

        /// <summary>
        /// A valid URL for the file.
        /// </summary>
        [JsonProperty("document_url", Required = Required.Always)]
        public string DocumentUrl { get; set; }

        /// <summary>
        /// Mime type of the content of the file, either “application/pdf” or “application/zip”.
        /// </summary>
        [JsonProperty("mime_type", Required = Required.Always)]
        public MimeType MimeType { get; set; }

        /// <summary>
        /// Optional. Short description of the result.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Optional. URL of the thumbnail (jpeg only) for the file.
        /// </summary>
        [JsonProperty("thumb_url")]
        public string ThumbUrl { get; set; }

        /// <summary>
        /// Optional. Thumbnail width.
        /// </summary>
        [JsonProperty("thumb_width")]
        public int? ThumbWidth { get; set; }

        /// <summary>
        /// Optional. Thumbnail height.
        /// </summary>
        [JsonProperty("thumb_height")]
        public int? ThumbHeight { get; set; }
    }
}
