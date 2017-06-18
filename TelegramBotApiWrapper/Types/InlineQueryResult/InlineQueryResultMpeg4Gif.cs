using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Types.InlineQueryResult
{
    /// <summary>
    /// Represents a link to a video animation (H.264/MPEG-4 AVC video without sound). 
    /// By default, this animated MPEG-4 file will be sent by the user with optional caption. 
    /// Alternatively, you can use <see cref="InlineQueryResultMpeg4Gif.InputMessageContent"/>  to send a message with the specified content instead of the animation.
    /// </summary>
    public class InlineQueryResultMpeg4Gif : InlineQueryResultCaption, IResultTitle, IResultMpeg4
    {
        /// <summary>
        /// Type of the result.
        /// </summary>
        [JsonProperty("type", Required = Required.Always)]
        public override string Type => InlineQueryResultType.Mpeg4Gif;

        /// <summary>
        /// A valid URL for the MP4 file. File size must not exceed 1MB.
        /// </summary>
        [JsonProperty("mpeg4_url", Required = Required.Always)]
        public string Mpeg4Url { get; set; }

        /// <summary>
        /// Optional. Video width.
        /// </summary>
        [JsonProperty("mpeg4_width")]
        public int? Mpeg4Width { get; set; }

        /// <summary>
        /// Optional. Video height.
        /// </summary>
        [JsonProperty("mpeg4_height")]
        public int? Mpeg4Height { get; set; }

        /// <summary>
        /// Optional. Video duration.
        /// </summary>
        [JsonProperty("mpeg4_duration")]
        public int? Mpeg4Duration { get; set; }

        /// <summary>
        /// URL of the static thumbnail (jpeg or gif) for the result.
        /// </summary>
        [JsonProperty("thumb_url", Required = Required.Always)]
        public string ThumbUrl { get; set; }

        /// <summary>
        /// Optional. Title for the result.
        /// </summary>
        public string Title { get; set; }


    }
}
