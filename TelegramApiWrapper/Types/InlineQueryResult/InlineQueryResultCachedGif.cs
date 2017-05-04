using Newtonsoft.Json;

namespace CoffeeJelly.TelegramApiWrapper.Types.InlineQueryResult
{
    /// <summary>
    /// Represents a link to an animated GIF file stored on the Telegram servers.
    /// By default, this animated GIF file will be sent by the user with optional caption. 
    /// Alternatively, you can use <see cref="InlineQueryResultCachedGif.InputMessageContent"/> to send a message with the specified content instead of the animation.
    /// </summary>
    public class InlineQueryResultCachedGif : InlineQueryResultCaption, IResultTitle
    {
        /// <summary>
        /// Type of the result.
        /// </summary>
        [JsonProperty("type", Required = Required.Always)]
        public override string Type => InlineQueryResultType.Gif;

        /// <summary>
        /// A valid URL for the GIF file. File size must not exceed 1MB.
        /// </summary>
        [JsonProperty("gif_file_id", Required = Required.Always)]
        public string GifFileId { get; set; }

        /// <summary>
        /// Optional. Title for the result.
        /// </summary>
        [JsonProperty("title", Required = Required.Default)]
        public string Title { get; set; }

    }
}
