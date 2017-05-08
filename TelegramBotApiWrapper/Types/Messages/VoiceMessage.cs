using CoffeeJelly.TelegramBotApiWrapper.Types.General;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Types.Messages
{
    /// <summary>
    /// A class which represents the Telegram voice message.
    /// </summary>
    public class VoiceMessage : Message, IVoiceMessage
    {
        /// <summary>
        /// Information about the voice.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("voice")]
        public Voice Voice { get; set; }
    }
}