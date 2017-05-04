﻿using System.Collections.Generic;
using CoffeeJelly.TelegramApiWrapper.Converters;
using CoffeeJelly.TelegramApiWrapper.Types.Message;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramApiWrapper.Types.General
{
    /// <summary>
    /// Represents a custom keyboard with reply options.
    /// </summary>
    public class ReplyKeyboardMarkup : IMarkup
    {
        /// <summary>
        /// List of button rows, each represented by an List of KeyboardButton objects
        /// </summary>
        [JsonConverter(typeof(ArrayToListConverter<List<KeyboardButton>>))]
        [JsonProperty("keyboard", Required = Required.Always)]
        public List<List<KeyboardButton>> Keyboard { get; set; }

        /// <summary>
        /// Optional. Requests clients to resize the keyboard vertically for optimal fit (e.g., make the keyboard smaller if there are just two rows of buttons). 
        /// </summary>
        /// <remarks>
        /// Defaults to <see langword="false"/>, in which case the custom keyboard is always of the same height as the app's standard keyboard.
        /// </remarks>
        [JsonProperty("resize_keyboars")]
        public bool? ResizeKeyboard { get; set; } = false;

        /// <summary>
        /// Optional. Requests clients to hide the keyboard as soon as it's been used. 
        /// The keyboard will still be available, but clients will automatically display the usual letter-keyboard in the chat – 
        /// the user can press a special button in the input field to see the custom keyboard again. 
        /// </summary>
        /// <remarks>Defaults to <see langword="false"/>.</remarks>
        [JsonProperty("one_time_keyboard")]
        public bool? OneTimeKeyboard { get; set; }

        /// <summary>
        /// Optional. Use this parameter if you want to show the keyboard to specific users only. 
        /// Targets: 1) users that are @mentioned in the <see cref="TextMessage.Text"/>; 
        /// 2) if the bot's message is a reply (has ReplyToMessageId), sender of the original message.
        /// </summary>
        /// <example>
        /// A user requests to change the bot‘s language, bot replies to the request with a keyboard to select the new language. 
        /// Other users in the group don’t see the keyboard.
        /// </example>
        [JsonProperty("selective")]
        public bool? Selective { get; set; }
    }
}