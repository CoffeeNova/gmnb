using CoffeeJelly.TelegramBotApiWrapper.Types.Payments;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Types.Messages
{
    /// <summary>
    /// A class which represents the Telegram voice message.
    /// </summary>
    public class InvoiceMessage : Message, IInvoiceMessage
    {
        /// <summary>
        /// Information about the voice.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("voice")]
        public Invoice Invoice { get; set; }
    }
}