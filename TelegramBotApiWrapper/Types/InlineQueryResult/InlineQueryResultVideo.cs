using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Types.InlineQueryResult
{
    /// <summary>
    /// Represents a link to a page containing an embedded video player or a video file. 
    /// By default, this video file will be sent by the user with an optional caption. 
    /// Alternatively, you can use <see cref="InlineQueryResultVideo.InputMessageContent"/>  to send a message with the specified content instead of the video.
    /// </summary>
    /// <remarks>This will only work in Telegram versions released after 9 April, 2016. Older clients will ignore them.</remarks>
    public class InlineQueryResultVideo : InlineQueryResultCaption, IResulVideo, IResultTitle
    {
        /// <summary>
        /// Type of the result.
        /// </summary>
        [JsonProperty("type", Required =Required.Always)]
        public override string Type => InlineQueryResultType.Video;

        /// <summary>
        /// A valid URL for the embedded video player or video file.
        /// </summary>
        [JsonProperty("video_url", Required = Required.Always)]
        public string VideoUrl { get; set; }

        /// <summary>
        /// Mime type of the content of video url, “text/html” or “video/mp4”
        /// </summary>
        [JsonProperty("mime_type", Required = Required.Always)]
        public MimeType MimeType { get; set; }

        /// <summary>
        /// Url of the thumbnail (jpeg only) for the result.
        /// </summary>
        ///<remarks>Optional.</remarks>
        [JsonProperty("thumb_nail")]
        public string ThumbUrl { get; set; }

        /// <summary>
        /// Title for the result.
        /// </summary>
        [JsonProperty("title", Required = Required.Always)]
        public string Title { get; set; }

        /// <summary>
        /// Optional. Video width.
        /// </summary>
        [JsonProperty("video_width")]
        public int? VideoWidth { get; set; }

        /// <summary>
        /// Optional. Video height.
        /// </summary>
        [JsonProperty("video_height")]
        public int? VideoHeight { get; set; }

        /// <summary>
        /// Optional. Video duration in seconds.
        /// </summary>
        [JsonProperty("video_duration")]
        public int? VideoDuration { get; set; }

        /// <summary>
        /// Optional. Short description of the result.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }
       
    }
}
