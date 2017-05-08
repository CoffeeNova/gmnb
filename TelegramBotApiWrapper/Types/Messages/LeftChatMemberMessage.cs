using CoffeeJelly.TelegramBotApiWrapper.Types.General;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Types.Messages
{
    /// <summary>
    /// A class which represents the Telegram message about left chat member.
    /// </summary>
    public class LeftChatMemberMessage : Message
    {
        /// <summary>
        /// Information about member, which been removed from the group.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("left_chat_member")]
        public User LeftChatMember { get; set; }

        /// <summary>
        /// Information about member, which been removed from the group.
        /// (<see cref="LeftChatMember"/>  copy?)
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("left_chat_participant")]
        public User LeftChatParticipant { get; set; }
    }
}