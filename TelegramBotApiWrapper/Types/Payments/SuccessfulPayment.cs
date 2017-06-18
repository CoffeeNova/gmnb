using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Types.Payments
{
    /// <summary>
    /// Represents a basic information about a successful payment.
    /// </summary>
    public class SuccessfulPayment : ICurrency
    {
        /// <summary>
        /// Three-letter ISO 4217 <see href="https://core.telegram.org/bots/payments#supported-currencies">currency</see> code.
        /// </summary>
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

        /// <summary>
        /// Bot specified invoice payload.
        /// </summary>
        [JsonProperty("invoice_payload", Required = Required.Always)]
        public string InvoicePayload { get; set; }

        /// <summary>
        /// Optional. Identifier of the shipping option chosen by the user.
        /// </summary>
        [JsonProperty("shipping_option_id")]
        public string ShippingOptionId { get; set; }

        /// <summary>
        /// Optional. Order info provided by the user.
        /// </summary>
        [JsonProperty("order_info")]
        public OrderInfo OrderInfo { get; set; }

        /// <summary>
        /// Telegram payment identifier.
        /// </summary>
        [JsonProperty("telegram_payment_charge_id", Required = Required.Always)]
        public string TelegramPaymentChargeId { get; set; }

        /// <summary>
        /// Provider payment identifier.
        /// </summary>
        [JsonProperty("provider_payment_charge_id", Required = Required.Always)]
        public string ProviderPaymentChargeId { get; set; }
    }
}