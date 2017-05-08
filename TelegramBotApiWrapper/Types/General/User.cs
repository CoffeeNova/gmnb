using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Types.General
{
    /// <summary>
    /// Represents a Telegram user or bot.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Performs an implicit conversion from <see cref="User"/> to <see cref="string"/>.
        /// </summary>
        /// <param name="obj">The <see cref="User"/> instance.</param>
        /// <returns><see cref="User.Id"/> as <see cref="string"/></returns>
        public static implicit operator string(User obj)
        {
            return obj.Id.ToString();
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="User"/> to <see cref="int"/>.
        /// </summary>
        /// <param name="obj">The <see cref="User"/> instance.</param>
        /// <returns><see cref="User.Id"/> as <see cref="int"/></returns>
        public static implicit operator int(User obj)
        {
            return obj.Id;
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