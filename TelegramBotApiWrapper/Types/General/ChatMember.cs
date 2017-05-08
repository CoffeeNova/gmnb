using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Types.General
{
    /// <summary>
    /// Contains information about one member of the chat.
    /// </summary>
    public class ChatMember
    {
        /// <summary>
        /// Information about the user.
        /// </summary>
        [JsonProperty("user", Required = Required.Always)]
        public User User { get; set; }

        /// <summary>
        /// The member's status in the chat.
        /// </summary>
        [JsonProperty("status", Required = Required.Always)]
        public ChatMemberStatus Status { get; set; }
    }
}