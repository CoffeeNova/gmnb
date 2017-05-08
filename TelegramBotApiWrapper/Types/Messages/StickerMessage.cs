using CoffeeJelly.TelegramBotApiWrapper.Types.General;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Types.Messages
{
    /// <summary>
    /// A class which represents the Telegram sticker message.
    /// </summary>
    public class StickerMessage : Message, IStickerMessage
    {
        /// <summary>
        /// Information about the sticker.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("sticker")]
        public Sticker Sticker { get; set; }
    }
}