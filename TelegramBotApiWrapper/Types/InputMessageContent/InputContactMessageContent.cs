using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Types.InputMessageContent
{
    /// <summary>
    /// Represents the content of a contact message to be sent as the result of an inline query.
    /// </summary>
    /// <remarks>This will only work in Telegram versions released after 9 April, 2016. Older clients will ignore them.</remarks>
    public class InputContactMessageContent : IInputMessageContent
    {
        /// <summary>
        /// Contact's phone number.
        /// </summary>
        [JsonProperty("phone_number", Required = Required.Always)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Contact's first name.
        /// </summary>
        [JsonProperty("first_name", Required = Required.Always)]
        public string FirstName { get; set; }

        /// <summary>
        /// Contact's last name
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("last_name")]
        public string LastName { get; set; }
    }
}