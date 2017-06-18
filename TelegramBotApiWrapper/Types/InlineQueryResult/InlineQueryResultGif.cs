
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Types.InlineQueryResult
{
    /// <summary>
    /// Represents a link to an animated GIF file. 
    /// By default, this animated GIF file will be sent by the user with optional caption. 
    /// Alternatively, you can use <see cref="InlineQueryResultGif.InputMessageContent"/> to send a message with the specified content instead of the animation.
    /// </summary>
    public class InlineQueryResultGif : InlineQueryResultCaption, IResultGif, IResultTitle
    {
        /// <summary>
        /// Type of the result
        /// </summary>
        [JsonProperty("type", Required = Required.Always)]
        public override string Type => InlineQueryResultType.Gif;

        /// <summary>
        /// A valid URL for the GIF file. File size must not exceed 1MB.
        /// </summary>
        [JsonProperty("gif_url", Required = Required.Always)]
        public string GifUrl { get; set; }

        /// <summary>
        /// Optional. Width of the GIF.
        /// </summary>
        [JsonProperty("gif_width")]
        public int? GifWidth { get; set; }

        /// <summary>
        /// Optional. Height of the GIF.
        /// </summary>
        [JsonProperty("gif_height")]
        public int? GifHeight { get; set; }

        /// <summary>
        /// Optional. Duration of the GIF.
        /// </summary>
        [JsonProperty("gif_duration")]
        public int? GifDuration { get; set; }

        /// <summary>
        /// URL of the static thumbnail for the result (jpeg or gif)
        /// </summary>
        [JsonProperty("thumb_url", Required = Required.Always)]
        public string ThumbUrl { get; set; }

        /// <summary>
        /// Optional. Title for the result
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }
    }
}
