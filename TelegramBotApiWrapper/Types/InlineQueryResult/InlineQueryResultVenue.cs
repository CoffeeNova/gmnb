using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Types.InlineQueryResult
{
    /// <summary>
    /// Represents a venue. By default, the venue will be sent by the user.
    /// Alternatively, you can use <see cref="InlineQueryResultVenue.InputMessageContent"/> to send a message with the specified content instead of the venue.
    /// </summary>
    /// <remarks>This will only work in Telegram versions released after 9 April, 2016. Older clients will ignore them.</remarks>
    public class InlineQueryResultVenue : InlineQueryResultContent, IResultThumb, IResultTitle
    {
        /// <summary>
        /// Type of the result.
        /// </summary>
        [JsonProperty("type", Required =Required.Always)]
        public override string Type => InlineQueryResultType.Venue;

        /// <summary>
        /// Location latitude in degrees.
        /// </summary>
        [JsonProperty("latitude", Required = Required.Always)]
        public float Latitude { get; set; }

        /// <summary>
        /// Location longitude in degrees.
        /// </summary>
        [JsonProperty("longitude", Required = Required.Always)]
        public float Longitude { get; set; }

        /// <summary>
        /// Title of the venue.
        /// </summary>
        [JsonProperty("title", Required = Required.Always)]
        public string Title { get; set; }

        /// <summary>
        /// Address of the venue.
        /// </summary>
        [JsonProperty("address", Required = Required.Always)]
        public string Address { get; set; }

        /// <summary>
        /// Optional. Foursquare identifier of the venue if known.
        /// </summary>
        [JsonProperty("foursquare_id", Required = Required.Default)]
        public string FoursquareId { get; set; }

        /// <summary>
        /// Optional. Url of the thumbnail for the result.
        /// </summary>
        [JsonProperty("thumb_url", Required = Required.Default)]
        public string ThumbUrl { get; set; }

        /// <summary>
        /// Optional. Thumbnail width.
        /// </summary>
        [JsonProperty("thumb_width", Required = Required.Default)]
        public int? ThumbWidth { get; set; }

        /// <summary>
        /// Optional. Thumbnail height.
        /// </summary>
        [JsonProperty("thumb_height", Required = Required.Default)]
        public int? ThumbHeight { get; set; }
    }
}
