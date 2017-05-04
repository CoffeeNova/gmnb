using Newtonsoft.Json;

namespace CoffeeJelly.TelegramApiWrapper.Types.General
{
    /// <summary>
    /// Represents a Telegram user or bot.
    /// </summary>
    public class User
    {
        public static implicit operator string(User obj)
        {
            return obj.Id.ToString();
        }

        /// <summary>
        /// Unique identifier for this user or bot.
        /// </summary>
        [JsonProperty("id", Required = Required.Always)]
        public int Id { get; set; }

        /// <summary>
        /// User's or bot's first name.
        /// </summary>
        [JsonProperty("first_name", Required = Required.Always)]
        public string FirstName { get; set; }

        /// <summary>
        /// User‘s or bot’s last name
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("last_name")]
        public string LastName { get; set; }

        /// <summary>
        /// User‘s or bot’s username.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("username")]
        public string Username { get; set; }
    }
}