using System.Collections.Generic;
using CoffeeJelly.TelegramBotApiWrapper.Converters;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Types.General
{
    /// <summary>
    /// Represents an inline keyboard that appears right next to the message it belongs to.
    /// </summary>
    public class InlineKeyboardMarkup : IMarkup
    {
        /// <summary>
        /// Array of button rows, each represented by an Array of InlineKeyboardButton objects.
        /// </summary>
        [JsonConverter(typeof(ArrayToListConverter<List<InlineKeyboardButton>>))]
        [JsonProperty("inline_keyboard", Required = Required.Always)]
        public List<List<InlineKeyboardButton>> InlineKeyboard { get; set; }
    }
}