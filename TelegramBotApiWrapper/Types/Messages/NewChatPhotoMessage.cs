using System.Collections.Generic;
using CoffeeJelly.TelegramBotApiWrapper.Converters;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Types.Messages
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
        [JsonConverter(typeof(ArrayToIEnumerableConverter<PhotoSize>))]
        [JsonProperty("new_chat_photo")]
        public IEnumerable<PhotoSize> NewChatPhoto { get; set; }
    }
}