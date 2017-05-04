using Newtonsoft.Json;

namespace CoffeeJelly.TelegramApiWrapper.Types.Message
{
    /// <summary>
    /// A class which represents the Telegram message indicated 
    /// that the group has been migrated to a supergroup with the specified identifier.
    /// </summary>
    public class MigrateToChatIdMessage : Message
    {
        /// <summary>
        /// Supergroup identifier.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("migrate_to_chat_id")]
        public long MigrateToChatId { get; set; }
    }
}