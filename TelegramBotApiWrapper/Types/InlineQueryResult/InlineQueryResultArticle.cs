using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Types.InlineQueryResult
{
    /// <summary>
    /// Represents a link to an article or web page.
    /// </summary>
    public class InlineQueryResultArticle : InlineQueryResultContent, IResultThumb, IResultTitle
    {
        /// <summary>
        /// Type of the result.
        /// </summary>
        [JsonProperty("type", Required = Required.Always)]
        public override string Type => InlineQueryResultType.Article;

        /// <summary>
        /// Title of the result.
        /// </summary>
        [JsonProperty("title", Required = Required.Always)]
        public string Title { get; set; }

        /// <summary>
        /// URL of the result.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// Pass True, if you don't want the URL to be shown in the message.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("hide_url")]
        public bool? HideUrl { get; set; }

        /// <summary>
        /// Short description of the result.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Url of the thumbnail for the result.
        /// </summary>
        ///<remarks>Optional.</remarks>
        [JsonProperty("thumb_url")]
        public string ThumbUrl { get; set; }

        /// <summary>
        /// Thumbnail width.
        /// </summary>
        ///<remarks>Optional.</remarks>
        [JsonProperty("thumb_width")]
        public int? ThumbWidth { get; set; }

        /// <summary>
        /// Thumbnail height.
        /// </summary>
        ///<remarks>Optional.</remarks>
        [JsonProperty("thumb_height")]
        public int? ThumbHeight { get; set; }
    }
}