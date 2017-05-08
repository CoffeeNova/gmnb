using System;
using CoffeeJelly.TelegramBotApiWrapper.Extensions;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Types
{
    /// <summary>
    /// Represents an incoming inline query. When the user sends an empty query, your bot could return some default or trending results.
    /// </summary>
    public class InlineQuery : ISender
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

        private string _query;

        /// <summary>
        /// Text of the query (up to 512 characters).
        /// </summary>
        [JsonProperty("query")]
        public string Query
        {
            get { return _query; }
            set
            {
                if (value != null && !value.Length.InRange(0, 4096))
                    throw new ArgumentOutOfRangeException(nameof(value), $"{value} Length should be up to 512 characters.");
                _query = value;
            }
        }

        /// <summary>
        /// Offset of the results to be returned, can be controlled by the bot.
        /// </summary>
        [JsonProperty("offset")]
        public string Offset { get; set; }
    }
}