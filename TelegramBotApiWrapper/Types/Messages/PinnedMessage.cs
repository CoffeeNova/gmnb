using CoffeeJelly.TelegramBotApiWrapper.Converters;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Types.Messages
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