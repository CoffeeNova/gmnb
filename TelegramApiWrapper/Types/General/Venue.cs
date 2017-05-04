using Newtonsoft.Json;

namespace CoffeeJelly.TelegramApiWrapper.Types.General
{
    /// <summary>
    /// Represents a venue.
    /// </summary>
    public class Venue
    {
        /// <summary>
        /// Venue location.
        /// </summary>
        [JsonProperty("location", Required = Required.Always)]
        public Location Location { get; set; }

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
        /// Foursquare identifier of the venue.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("foursquare_id")]
        public string FoursquareId { get; set; }
    }
}