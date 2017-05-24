using System.Collections.Generic;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Types.Payments
{
    /// <summary>
    /// Represents a portion of the price for goods or services.
    /// </summary>
    public class LabeledPrice
    {
        /// <summary>
        /// Portion label.
        /// </summary>
        [JsonProperty("label")]
        public string Label { get; set; }

        /// <summary>
        /// Price of the product in the smallest units of the <see href="https://core.telegram.org/bots/payments#supported-currencies">currency</see>.
        /// </summary>
        /// <remarks>
        /// For example, for a price of US$ 1.45 pass amount = 145. 
        /// See the exp parameter in <see href="https://core.telegram.org/bots/payments/currencies.json">currencies.json</see>/>,
        /// it shows the number of digits past the decimal point for each currency (2 for the majority of currencies).
        /// </remarks>
        [JsonProperty("amount")]
        public string Amount { get; set; }
    }
}