using Newtonsoft.Json;

namespace CoffeeJelly.TelegramApiWrapper.Types.General
{
    /// <summary>
    /// This class represents one special entity in a text message. For example, hashtags, usernames, URLs, etc.
    /// </summary>
    public class MessageEntity
    {
        /// <summary>
        /// Type of the entity.
        /// </summary>
        /// <remarks>
        /// Can be mention (@username), hashtag, bot_command, url, email, bold (bold text), italic (italic text), 
        /// code (monowidth string), pre (monowidth block), text_link (for clickable text URLs), 
        /// text_mention (for users without usernames)
        /// </remarks>
        [JsonProperty("type", Required = Required.Always)]
        public MessageEntityType Type { get; set; }

        /// <summary>
        /// Offset in UTF-16 code units to the start of the entity.
        /// </summary>
        [JsonProperty("offset", Required = Required.Always)]
        public int Offset { get; set; }

        /// <summary>
        /// Length of the entity in UTF-16 code units.
        /// </summary>
        [JsonProperty("length", Required = Required.Always)]
        public int Length { get; set; }

        /// <summary>
        /// For “text_link” only, url that will be opened after user taps on the text.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// For “text_mention” only, the mentioned user.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("user")]
        public User User { get; set; }
    }
}