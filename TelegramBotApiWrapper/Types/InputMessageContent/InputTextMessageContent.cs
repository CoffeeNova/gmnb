using System;
using CoffeeJelly.TelegramBotApiWrapper.Extensions;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Types.InputMessageContent
{
    /// <summary>
    /// Represents the content of a text message to be sent as the result of an inline query.
    /// </summary>
    public class InputTextMessageContent : IInputMessageContent
    {
        private string _messageText;
        /// <summary>
        /// Text of the message to be sent, 1-4096 characters.
        /// </summary>
        [JsonProperty("message_text", Required =Required.Always)]
        public string MessageText
        {
            get { return _messageText; }
            set
            {
                if (value != null && !value.Length.InRange(1, 4096))
                    throw new ArgumentOutOfRangeException(nameof(value), $"{value} Length should be from 1 to 4096 characters.");
                _messageText = value;
            }
        }

        /// <summary>
        /// Send Markdown or HTML, if you want Telegram apps to show bold, italic, fixed-width text or inline URLs in your bot's message.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("parse_mode")]
        public string ParseMode { get; set; }

        /// <summary>
        /// Disables link previews for links in the sent message.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("disable_web_page_preview")]
        public bool? DisableWebPagePreview { get; set; }
    }
}
