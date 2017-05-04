using CoffeeJelly.TelegramApiWrapper.Types.General;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramApiWrapper.Types.Message
{
    /// <summary>
    /// A class which represents the Telegram venue message.
    /// </summary>
    public class VenueMessage : Message, IVenueMessage
    {
        /// <summary>
        /// Information about the venue.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("venue")]
        public Venue Venue { get; set; }
    }
}