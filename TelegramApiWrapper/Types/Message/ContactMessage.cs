using CoffeeJelly.TelegramApiWrapper.Types.General;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramApiWrapper.Types.Message
{
    /// <summary>
    /// A class which represents the Telegram shared contact message.
    /// </summary>
    public class ContactMessage : Message, ITelegramContactMessage
    {
        /// <summary>
        /// Information about the contact.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("contact")]
        public Contact Contact { get; set; }
    }
}