using System.Collections.Generic;
using CoffeeJelly.TelegramApiWrapper.Converters;
using CoffeeJelly.TelegramApiWrapper.Types.General;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramApiWrapper.Types.Message
{
    /// <summary>
    /// A class which represents the Telegram message about new chat member.
    /// </summary>
    public class NewChatMemberMessage : Message
    {
        /// <summary>
        /// Information about first member, which been added to the group.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("new_chat_member")]
        public User NewChatMember { get; set; }

        /// <summary>
        /// Information about first member, which been added to the group.
        /// (<see cref="NewChatMember"/>  copy?)
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("new_chat_participant")]
        public User NewChatParticipant { get; set; }

        /// <summary>
        /// Information about members, which been added to the group.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonConverter(typeof(ArrayToListConverter<User>))]
        [JsonProperty("new_chat_members")]
        public List<User> NewChatMembers { get; set; }
    }
}