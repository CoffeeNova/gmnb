using System.Collections.Generic;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Types.Payments
{
    /// <summary>
    /// Represents one shipping option.
    /// </summary>
    public class ShippingOption
    {
        /// <summary>
        /// Shipping option identifier.
        /// </summary>
        [JsonProperty("id", Required = Required.Always)]
        public string Id { get; set; }

        /// <summary>
        /// Option title.
        /// </summary>
        [JsonProperty("title", Required = Required.Always)]
        public string Title { get; set; }

        /// <summary>
        /// List of price portions.
        /// </summary>
        [JsonProperty("prices", Required = Required.Always)]
        public IEnumerable<LabeledPrice> Prices { get; set; }
    }
}