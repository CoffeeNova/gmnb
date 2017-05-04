using Newtonsoft.Json;

namespace CoffeeJelly.TelegramApiWrapper.Types.General
{
    /// <summary>
    /// Represents one button of the reply keyboard. 
    /// </summary>
    public class KeyboardButton
    {
        /// <summary>
        /// Text of the button. If none of the optional fields are used, it will be sent to the bot as a message when the button is pressed.
        /// </summary>
        [JsonProperty("text", Required = Required.Always)]
        public string Text { get; set; }

        /// <summary>
        /// Optional. If <see langword="true"/>, the user's phone number will be sent as a contact when the button is pressed. 
        /// Available in private chats only
        /// </summary>
        [JsonProperty("request_contact")]
        public bool? RequestContact { get; set; }

        /// <summary>
        /// Optional. If <see langword="true"/>, the user's current location will be sent when the button is pressed. 
        /// Available in private chats only.
        /// </summary>
        [JsonProperty("request_location")]
        public bool? RequestLocation { get; set; }
    }
}