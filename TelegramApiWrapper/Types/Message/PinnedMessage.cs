using CoffeeJelly.TelegramApiWrapper.Converters;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramApiWrapper.Types.Message
{
    /// <summary>
    /// A class which represents the Telegram pinned specified message.
    /// </summary>
    public class PinnedMessage : Message
    {
        /// <summary>
        /// Pinned message.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonConverter(typeof(MessageConverter))]
        [JsonProperty("pinned_message")]
        public Message Message { get; set; }
    }
}