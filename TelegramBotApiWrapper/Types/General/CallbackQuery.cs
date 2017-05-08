using CoffeeJelly.TelegramBotApiWrapper.Converters;
using CoffeeJelly.TelegramBotApiWrapper.Types.Messages;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Types.General
{
    /// <summary>
    /// Represents an incoming callback query from a callback button in an inline keyboard. 
    /// If the button that originated the query was attached to a message sent by the bot, the field message will be present. 
    /// If the button was attached to a message sent via the bot (in inline mode), the property <see cref="InlineMessageId"/> will be present. 
    /// Exactly one of the properties <see cref="Data"/> or <see cref="GameShortName"/>  will be present.
    /// </summary>
    public class CallbackQuery : ISender
    {
        /// <summary>
        /// Unique identifier for this query.
        /// </summary>
        [JsonProperty("id", Required = Required.Always)]
        public string Id { get; set; }

        /// <summary>
        /// Sender.
        /// </summary>
        [JsonProperty("from", Required = Required.Always)]
        public User From { get; set; }

        /// <summary>
        /// Optional. Message with the callback button that originated the query.
        /// </summary>
        /// <remarks>
        /// Note that message content and message date will not be available if the message is too old.
        /// </remarks>
        [JsonConverter(typeof(MessageConverter))]
        [JsonProperty("message")]
        public Message Message { get; set; }

        /// <summary>
        /// Optional. Identifier of the message sent via the bot in inline mode, that originated the query.
        /// </summary>
        [JsonProperty("inline_message_id")]
        public string InlineMessageId { get; set; }

        /// <summary>
        /// Global identifier, uniquely corresponding to the chat to which the message with the callback button was sent. Useful for high scores in games.
        /// </summary>
        [JsonProperty("chat_instance")]
        public string ChatInstance { get; set; }

        /// <summary>
        /// Optional. Data associated with the callback button. 
        /// </summary>
        /// <remarks>
        /// Be aware that a bad client can send arbitrary data in this field.
        /// </remarks>
        [JsonProperty("data")]
        public string Data { get; set; }

        /// <summary>
        /// Optional. Short name of a Game to be returned, serves as the unique identifier for the game.
        /// </summary>
        [JsonProperty("game_short_namae")]
        public string GameShortName { get; set; }
    }
}