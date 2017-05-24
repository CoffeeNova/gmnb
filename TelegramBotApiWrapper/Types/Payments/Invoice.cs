using System.Collections.Generic;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Types.Payments
{
    /// <summary>
    /// Contains basic information about an invoice.
    /// </summary>
    public class Invoice : ICurrency
    {
        /// <summary>
        /// Product name.
        /// </summary>
        [JsonProperty("title", Required = Required.Always)]
        public string Title { get; set; }

        /// <summary>
        /// Product description.
        /// </summary>
        [JsonProperty("description", Required =Required.Always)]
        public string Description { get; set; }

        /// <summary>
        /// Unique bot deep-linking parameter that can be used to generate this invoice.
        /// </summary>
        [JsonProperty("start_parameter", Required = Required.Always)]
        public string StartParameter { get; set; }

        /// <summary>
        /// Three-letter ISO 4217 <see href="https://core.telegram.org/bots/payments#supported-currencies">currency</see> code.
        /// </summary>
        [JsonProperty("currency", Required = Required.Always)]
        public string Currency { get; set; }

        /// <summary>
        /// Total price in the smallest units of the currency.
        /// </summary>
        /// <remarks>
        /// For example, for a price of US$ 1.45 pass amount = 145. 
        /// See the exp parameter in <see href="https://core.telegram.org/bots/payments/currencies.json">currencies.json</see>/>,
        /// it shows the number of digits past the decimal point for each currency (2 for the majority of currencies).
        /// </remarks>
        [JsonProperty("total_amount", Required = Required.Always)]
        public int TotalAmount { get; set; }
    }
}