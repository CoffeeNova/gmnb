using System;
using CoffeeJelly.TelegramBotApiWrapper.Extensions;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Types.InlineQueryResult
{
    /// <summary>
    /// Represents one result of an inline query.
    /// </summary>
    public abstract class InlineQueryResult
    {
        /// <summary>
        /// Type of the result.
        /// </summary>
        [JsonProperty("type", Required = Required.Always)]
        public abstract string Type { get; }

        private string _id;

        /// <summary>
        /// Unique identifier for this result, 1-64 Bytes.
        /// </summary>
        [JsonProperty("id", Required = Required.Always)]
        public string Id
        {
            get { return _id; }
            set
            {
                if (value != null && !value.SizeUtf8().InRange(1, 64))
                    throw new ArgumentOutOfRangeException();
                _id = value;
            }
        }

        /// <summary>
        /// <see cref="InlineKeyboardMarkup"/> attached to the message.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("reply_markup")]
        public InlineKeyboardMarkup ReplyMarkup { get; set; }
    }

    public abstract class InlineQueryResultContent : InlineQueryResult
    {
        /// <summary>
        /// Content of the message to be sent.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("input_message_content")]
        public IInputMessageContent InputMessageContent { get; set; }
    }

    public abstract class InlineQueryResultCaption : InlineQueryResultContent
    {
        private string _caption;

        /// <summary>
        /// Optional. Caption of the GIF file to be sent, 0-200 characters
        /// </summary>
        [JsonProperty("caption")]
        public string Caption
        {
            get { return _caption; }
            set
            {
                if (value != null && value.Length.InRange(0, 200))
                    throw new ArgumentOutOfRangeException(nameof(value), "A value should not exceed 200 characters.");
                _caption = value;
            }
        }
    }
}