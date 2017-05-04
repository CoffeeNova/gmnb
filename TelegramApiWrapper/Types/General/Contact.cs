using Newtonsoft.Json;

namespace CoffeeJelly.TelegramApiWrapper.Types.General
{
    /// <summary>
    /// Represents a phone contact.
    /// </summary>
    public class Contact
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
        /// Contact's last name.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("last_name")]
        public string LastName { get; set; }

        /// <summary>
        /// Contact's user identifier in Telegram.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("user_id")]
        public int? UserId { get; set; }
    }
}