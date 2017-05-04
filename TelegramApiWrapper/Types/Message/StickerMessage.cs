using CoffeeJelly.TelegramApiWrapper.Types.General;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramApiWrapper.Types.Message
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