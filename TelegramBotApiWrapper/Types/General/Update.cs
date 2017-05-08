using CoffeeJelly.TelegramBotApiWrapper.Converters;
using CoffeeJelly.TelegramBotApiWrapper.Types.InlineQueryResult;
using CoffeeJelly.TelegramBotApiWrapper.Types.Messages;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Types.General
{
    /// <summary>
    /// Represents an incoming update.
    /// </summary>
    public class Update
    {
        /// <summary>
        /// The update‘s unique identifier.
        /// </summary>
        /// <remarks>
        /// Update identifiers start from a certain positive number and increase sequentially. 
        /// This ID becomes especially handy if you’re using Webhooks, since it allows you to ignore repeated updates or to restore the correct update sequence, 
        /// should they get out of order.
        /// </remarks>
        [JsonProperty("update_id", Required = Required.Always)]
        public long UpdateId { get; set; }

        /// <summary>
        /// New incoming message of any kind — <see cref="TextMessage"/>, <see cref="PhotoMessage"/>, <see cref="StickerMessage"/>, etc.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("message")]
        [JsonConverter(typeof(MessageConverter))]
        public Message Message { get; set; }

        /// <summary>
        /// New version of a message that is known to the bot and was edited.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("edited_message")]
        [JsonConverter(typeof(MessageConverter))]
        public Message EditedMessage { get; set; }

        /// <summary>
        /// New incoming channel post of any kind — <see cref="TextMessage"/>, <see cref="PhotoMessage"/>, <see cref="StickerMessage"/>, etc.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("channel_post")]
        [JsonConverter(typeof(MessageConverter))]
        public Message ChannelPost { get; set; }

        /// <summary>
        /// New version of a channel post that is known to the bot and was edited.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("edited_channel_post")]
        [JsonConverter(typeof(MessageConverter))]
        public Message EditedChannelPost { get; set; }

        /// <summary>
        /// New incoming inline query.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("inline_query")]
        public InlineQuery InlineQuery { get; set; }

        /// <summary>
        /// The result of an inline query that was chosen by a user and sent to their chat partner.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("chosen_inline_result")]
        public ChosenInlineResult ChosenInlineResult { get; set; }

        /// <summary>
        /// New incoming callback query.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("callback_query")]
        public CallbackQuery CallbackQuery { get; set; }
    }
}