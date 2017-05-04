using Newtonsoft.Json;

namespace CoffeeJelly.TelegramApiWrapper.Types.Message
{
    /// <summary>
    /// A class which represents the Telegram message about new chat title.
    /// </summary>
    public class NewChatTitleMessage : Message
    {
        /// <summary>
        /// A Title. :>
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("new_chat_title")]
        public string NewChatTitle { get; set; }
    }
}