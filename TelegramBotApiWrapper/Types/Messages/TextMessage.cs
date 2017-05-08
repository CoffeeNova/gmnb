using System;
using System.Collections.Generic;
using CoffeeJelly.TelegramBotApiWrapper.Converters;
using CoffeeJelly.TelegramBotApiWrapper.Extensions;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Types.Messages
{
    /// <summary>
    /// A class which represents the Telegram text user's message.
    /// </summary>
    public class TextMessage : Message, ITextMessage
    {
        private string _text;
        /// <summary>
        /// Text message, the actual UTF-8 text of the message, 0-4096 characters.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("text")]
        public string Text
        {
            get { return _text; }
            set
            {
                if (value != null && !value.Length.InRange(0, 4096))
                    throw new ArgumentOutOfRangeException(nameof(value), $"{value} Length should be from 0 to 4096 characters.");
                _text = value;
            }
        }
        /// <summary>
        /// Special entities like usernames, URLs, bot commands, etc. that appear in the text.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonConverter(typeof(ArrayToListConverter<MessageEntity>))]
        [JsonProperty("entities")]
        public List<MessageEntity> Entities { get; set; }

        /// <summary>
        /// Optional. For replies, the original message. 
        /// </summary>
        /// <remark>Note that the <see cref="Message"/> object in this field will not contain further <see cref="ReplyToMessage"/> properties even if it itself is a reply.</remark>
        [JsonConverter(typeof(MessageConverter))]
        [JsonProperty("reply_to_message")]
        public Message ReplyToMessage { get; set; }

    }
}