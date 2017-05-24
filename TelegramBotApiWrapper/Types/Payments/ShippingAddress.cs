using System.Collections.Generic;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Types.Payments
{
    /// <summary>
    /// Represents a shipping address.
    /// </summary>
    public class ShippingAddress
    {
        /// <summary>
        /// ISO 3166-1 alpha-2 country code.
        /// </summary>
        [JsonProperty("country_code", Required = Required.Always)]
        public string CountryCode { get; set; }

        /// <summary>
        /// State, if applicable.
        /// </summary>
        [JsonProperty("state", Required =Required.Always)]
        public string State { get; set; }

        /// <summary>
        /// City.
        /// </summary>
        [JsonProperty("city", Required = Required.Always)]
        public string City { get; set; }

        /// <summary>
        /// First line for the address.
        /// </summary>
        [JsonProperty("street_line1", Required = Required.Always)]
        public string StreetLine1 { get; set; }

        /// <summary>
        /// Second line for the address.
        /// </summary>
        [JsonProperty("street_line2", Required = Required.Always)]
        public string StreetLine2 { get; set; }

        /// <summary>
        /// Address post code.
        /// </summary>
        [JsonProperty("post_code", Required = Required.Always)]
        public string PostCode { get; set; }

    }
}