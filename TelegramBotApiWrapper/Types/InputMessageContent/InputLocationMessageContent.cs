using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Types.InputMessageContent
{
    /// <summary>
    /// Represents the content of a location message to be sent as the result of an inline query.
    /// </summary>
    /// <remarks>This will only work in Telegram versions released after 9 April, 2016. Older clients will ignore them.</remarks>
    public class InputLocationMessageContent : IInputMessageContent
    {

        /// <summary>
        /// Latitude of the location in degrees.
        /// </summary>
        [JsonProperty("latitude", Required = Required.Always)]
        public float Latitude { get; set; }

        /// <summary>
        /// Longitude of the location in degrees.
        /// </summary>
        [JsonProperty("longitude", Required = Required.Always)]
        public float Longitude { get; set; }
    }
}