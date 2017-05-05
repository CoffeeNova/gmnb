using System.Collections.Generic;
using CoffeeJelly.TelegramApiWrapper.Converters;
using CoffeeJelly.TelegramApiWrapper.Types.General;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramApiWrapper.Types.Messages
{
    /// <summary>
    /// A class which represents the Telegram message about new chat photo.
    /// </summary>
    public class NewChatPhotoMessage : Message
    {
        /// <summary>
        /// Available sizes of the new chat photo.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonConverter(typeof(ArrayToListConverter<PhotoSize>))]
        [JsonProperty("new_chat_photo")]
        public List<PhotoSize> NewChatPhoto { get; set; }
    }
}