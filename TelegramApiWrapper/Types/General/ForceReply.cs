using CoffeeJelly.TelegramApiWrapper.Types.Message;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramApiWrapper.Types.General
{
    /// <summary>
    /// Upon receiving a message with this object, Telegram clients will display a reply interface to the user (act as if the user has selected the bot‘s message and tapped ’Reply'). 
    /// This can be extremely useful if you want to create user-friendly step-by-step interfaces without having to sacrifice privacy mode.
    /// </summary>
    public class ForceReply : IMarkup
    {
        /// <summary>
        /// Shows reply interface to the user, as if they manually selected the bot‘s message and tapped ’Reply'
        /// </summary>
        [JsonProperty("force_reply", Required = Required.Always)]
        public bool ForseReply { get; } = true;

        /// <summary>
        /// Optional. Use this parameter if you want to show the keyboard to specific users only. 
        /// Targets: 1) users that are @mentioned in the <see cref="TextMessage.Text"/>; 
        /// 2) if the bot's message is a reply (has ReplyToMessageId), sender of the original message.
        /// </summary>
        /// <example>
        /// A user votes in a poll, bot returns confirmation message in reply to the vote and removes the keyboard for that user, 
        /// while still showing the keyboard with poll options to users who haven't voted yet.
        /// </example>
        [JsonProperty("selective")]
        public bool? Selective { get; set; }
    }
}