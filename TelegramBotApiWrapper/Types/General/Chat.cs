﻿using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Types.General
{
    /// <summary>
    /// Represents a chat.
    /// </summary>
    public class Chat
    {
        /// <summary>
        /// Performs an implicit conversion from <see cref="Chat"/> to <see cref="string"/>.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns><see cref="Chat.Id"/> as <see cref="string"/></returns>
        public static implicit operator string(Chat obj)
        {
            return obj.Id.ToString();
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Chat"/> to <see cref="long"/>.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns><see cref="Chat.Id"/> as <see cref="long"/></returns>
        public static implicit operator long(Chat obj)
        {
            return obj.Id;
        }
        /// <summary>
        /// Unique identifier for this chat.
        /// </summary>
        [JsonProperty("id", Required = Required.Always)]
        public long Id { get; set; }

        /// <summary>
        /// Type of chat, can be either “private”, “group”, “supergroup” or “channel”.
        /// </summary>
        [JsonProperty("type", Required = Required.Always)]
        public ChatType Type { get; set; }

        /// <summary>
        /// Title, for supergroups, channels and group chats.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Username, for private chats, supergroups and channels if available.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("username")]
        public string Username { get; set; }

        /// <summary>
        /// First name of the other party in a private chat.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        /// <summary>
        /// Last name of the other party in a private chat.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("last_name")]
        public string LastName { get; set; }

        /// <summary>
        /// <see langword="true"/>: if a group has ‘All Members Are Admins’ enabled.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("all_members_are_administrators")]
        public bool? AllMembersAreAdministrators { get; set; }
    }
}