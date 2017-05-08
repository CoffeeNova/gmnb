using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Types.InputMessageContent
{
    /// <summary>
    /// Represents the content of a venue message to be sent as the result of an inline query.
    /// </summary>
    /// <remarks>This will only work in Telegram versions released after 9 April, 2016. Older clients will ignore them.</remarks>
    public class InputVenueMessageContent : IInputMessageContent
    {
        /// <summary>
        /// Latitude of the venue in degrees.
        /// </summary>
        [JsonProperty("latitude", Required = Required.Always)]
        public float Latitude { get; set; }

        /// <summary>
        /// Longitude of the venue in degrees.
        /// </summary>
        [JsonProperty("longitude", Required = Required.Always)]
        public float Longitude { get; set; }

        /// <summary>
        /// Name of the venue.
        /// </summary>
        [JsonProperty("title", Required = Required.Always)]
        public string Title { get; set; }

        /// <summary>
        /// Address of the venue.
        /// </summary>
        [JsonProperty("address", Required = Required.Always)]
        public string Address { get; set; }

        /// <summary>
        /// Foursquare identifier of the venue, if known.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("foursquare_id")]
        public string FoursquareId { get; set; }
    }
}