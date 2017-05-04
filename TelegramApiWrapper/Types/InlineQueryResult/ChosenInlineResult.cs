using CoffeeJelly.TelegramApiWrapper.Types.General;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramApiWrapper.Types.InlineQueryResult
{
    /// <summary>
    /// Represents a result of an inline query that was chosen by the user and sent to their chat partner.
    /// </summary>
    public class ChosenInlineResult
    {
        /// <summary>
        /// The unique identifier for the result that was chosen.
        /// </summary>
        [JsonProperty("result_id", Required = Required.Always)]
        public string ResultId { get; set; }

        /// <summary>
        /// The user that chose the result.
        /// </summary>
        [JsonProperty("from", Required = Required.Always)]
        public User From { get; set; }

        /// <summary>
        /// Sender location, only for bots that request user location.
        /// </summary>
        ///<remarks>Optional.</remarks>
        [JsonProperty("location")]
        public Location Location { get; set; }

        /// <summary>
        /// Identifier of the sent inline message.
        /// </summary>
        /// <remarks>
        /// Optional.
        /// Available only if there is an inline keyboard attached to the message. Will be also received in <see cref="CallbackQuery"/> and can be used to edit the message.
        /// </remarks>
        [JsonProperty("inline_message_id")]
        public string InlineMessageId { get; set; }

        /// <summary>
        /// The query that was used to obtain the result.
        /// </summary>
        [JsonProperty("query", Required = Required.Always)]
        public string Query { get; set; }
    }
}