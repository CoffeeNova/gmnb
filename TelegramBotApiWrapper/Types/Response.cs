
using CoffeeJelly.TelegramBotApiWrapper.Types.General;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Types
{
	/// <summary>
    /// The Telegram Api response.
    /// </summary>
    public class Response<T>
    {
		/// <summary>
        /// Indicates whether the request was successful.
        /// </summary>
        [JsonProperty("ok", Required = Required.Always)]
        public bool Ok { get; set; }

        /// <summary>
        /// The description of the <see cref="Result"/> or <see cref="ErrorCode"/>.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

		/// <summary>
        /// The result object of the request.
        /// </summary>
        [JsonProperty("result")]
        public T Result { get; set; }

		/// <summary>
        /// The error code.
        /// </summary>
        [JsonProperty("error_code")]
        public int ErrorCode { get; set;}

        /// <summary>
        /// Additional information which can help to automatically handle the error.
        /// </summary>
        [JsonProperty("parameters")]
        public ResponseParameters Parameters { get; set; }
    }
}
