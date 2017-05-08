using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Types.InlineQueryResult
{
    /// <summary>
    /// Represents a link to a file stored on the Telegram servers. By default, this file will be sent by the user with an optional caption. 
    /// Alternatively, you can use <see cref="InlineQueryResultCachedDocument.InputMessageContent"/> to send a message with the specified content instead of the file. 
    /// </summary>
    /// <remarks>This will only work in Telegram versions released after 9 April, 2016. Older clients will ignore them.</remarks>
    public class InlineQueryResultCachedDocument : InlineQueryResultCaption, IResultTitle
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
        /// A valid file identifier for the file.
        /// </summary>
        [JsonProperty("document_file_id", Required = Required.Always)]
        public string DocumentFileId { get; set; }

        /// <summary>
        /// Optional. Short description of the result.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
