using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace CoffeeJelly.gmailNotifyBot.Bot.Telegram
{
    /// <summary>
    /// Represents an incoming inline query. When the user sends an empty query, your bot could return some default or trending results.
    /// </summary>
    public class InlineQuery
    {
        /// <summary>
        /// Unique identifier for this query.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Sender.
        /// </summary>
        [JsonProperty("from")]
        public User From { get; set; }

        /// <summary>
        /// Sender location, only for bots that request user location.
        /// </summary>
        ///<remarks>Optional.</remarks>
        [JsonProperty("location")]
        public Location Location { get; set; }

        /// <summary>
        /// Text of the query (up to 512 characters).
        /// </summary>
        [JsonProperty("query")]
        public string Query { get; set; }

        /// <summary>
        /// Offset of the results to be returned, can be controlled by the bot.
        /// </summary>
        [JsonProperty("offset")]
        public string Offset { get; set; }
    }

    public class ChosenInlineResult
    {
        /// <summary>
        /// The unique identifier for the result that was chosen.
        /// </summary>
        [JsonProperty("result_id")]
        public string ResultId { get; set; }

        /// <summary>
        /// The user that chose the result.
        /// </summary>
        [JsonProperty("from")]
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
        [JsonProperty("query")]
        public string Query { get; set; }
    }
}