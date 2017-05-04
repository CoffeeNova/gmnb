using CoffeeJelly.TelegramApiWrapper.Types.General;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramApiWrapper.Types.Message
{
    /// <summary>
    /// A class which represents the Telegram shared location message.
    /// </summary>
    public class LocationMessage : Message, ILocationMessage
    {
        /// <summary>
        /// Information about the location.
        /// </summary>
        [JsonProperty("location")]
        public Location Location { get; set; }
    }
}