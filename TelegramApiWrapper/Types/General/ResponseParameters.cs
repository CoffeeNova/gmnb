using Newtonsoft.Json;

namespace CoffeeJelly.TelegramApiWrapper.Types.General
{
    /// <summary>
    /// Contains information about why a request was unsuccessfull.
    /// </summary>
    public class ResponceParameters
    {
        /// <summary>
        /// Optional. The group has been migrated to a supergroup with the specified identifier.
        /// </summary>
        [JsonProperty("migrate_to_chat_id")]
        public long? MigrateToChatId { get; set; }

        /// <summary>
        /// Optional. In case of exceeding flood control, the number of seconds left to wait before the request can be repeated.
        /// </summary>
        [JsonProperty("retry_after")]
        public int? RetryAfter { get; set; }
    }
}