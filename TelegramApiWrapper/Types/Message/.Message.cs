using System;
using CoffeeJelly.TelegramApiWrapper.Converters;
using CoffeeJelly.TelegramApiWrapper.Types.General;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramApiWrapper.Types.Message
{
    /// <summary>
    /// A class which represents the general Telegram message.
    /// </summary>
    public abstract class Message : IMessage, ISender
    {
        /// <summary>
        /// Unique message identifier inside this chat.
        /// </summary>
        [JsonProperty("message_id", Required =Required.Always)]
        public int MessageId { get; set; }

        /// <summary>
        /// Optional. Sender, can be empty for messages sent to channels.
        /// </summary>
        [JsonProperty("from")]
        public User From { get; set; }

        /// <summary>
        /// Date the message was sent.
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonProperty("date", Required = Required.Always)]
        public DateTime Date { get; set; }

        /// <summary>
        /// Conversation the message belongs to.
        /// </summary>
        [JsonProperty("chat", Required = Required.Always)]
        public Chat Chat { get; set; }

        /// <summary>
        /// Optional. For forwarded messages, sender of the original message.
        /// </summary>
        [JsonProperty("forward_from")]
        public User ForwardFrom { get; set; }

        /// <summary>
        /// Optional. For messages forwarded from a channel, information about the original channel.
        /// </summary>
        [JsonProperty("forward_from_chat")]
        public Chat ForwardFromChat { get; set; }

        /// <summary>
        /// Optional. For forwarded channel posts, identifier of the original message in the channel.
        /// </summary>
        [JsonProperty("forward_from_message_id")]
        public int? ForwardFromMessageId { get; set; }

        /// <summary>
        /// Optional. For forwarded messages, date the original message was sent.
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonProperty("forward_date")]
        public DateTime? ForwardDate { get; set; }

        /// <summary>
        /// Optional. Date the message was last edited.
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonProperty("edit_date")]
        public DateTime? EditDate { get; set; }
    }

    /// <summary>
    /// Temprory class for unknown Telegram message.
    /// </summary>
    public class UnknownMessage : Message
    {

    }
}