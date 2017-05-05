using Newtonsoft.Json;

namespace CoffeeJelly.TelegramApiWrapper.Types.Messages
{
    /// <summary>
    /// A class which represents the Telegram message indicated 
    /// that the supergroup has been migrated from a group with the specified identifier.
    /// </summary>
    public class MigrateFromChatIdMessage : Message
    {
        /// <summary>
        /// Supergroup identifier.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("migrate_from_chat_id")]
        public long MigrateFromChatId { get; set; }
    }
}