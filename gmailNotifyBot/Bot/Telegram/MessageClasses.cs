using System;
using System.Collections.Generic;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram.Converters;
using Newtonsoft.Json;

namespace CoffeeJelly.gmailNotifyBot.Bot.Telegram
{
    /// <summary>
    /// A class which represents the general Telegram message.
    /// </summary>
    public abstract class Message : IMessage
    {
        /// <summary>
        /// Unique message identifier inside this chat.
        /// </summary>
        [JsonProperty("message_id")]
        public int MessageId { get; set; }

        /// <summary>
        /// Date the message was sent.
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonProperty("date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Conversation the message belongs to.
        /// </summary>
        [JsonProperty("chat")]
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
    /// A class which represents the Telegram text user's message.
    /// </summary>
    public class TextMessage : Message, ISenderMessage, ITextMessage
    {
        /// <summary>
        /// Sender, can be empty for messages sent to channels.
        /// </summary>
        [JsonProperty("from")]
        public User From { get; set; }

        /// <summary>
        /// Text message, the actual UTF-8 text of the message, 0-4096 characters.
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        /// Special entities like usernames, URLs, bot commands, etc. that appear in the text.
        /// </summary>
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

    /// <summary>
    /// A class which represents the Telegram audio message.
    /// </summary>
    public class AudioMessage : Message, ISenderMessage, IAudioMessage
    {
        /// <summary>
        /// Sender, can be empty for messages sent to channels.
        /// </summary>
        [JsonProperty("from")]
        public User From { get; set; }

        /// <summary>
        /// Information about the audio file.
        /// </summary>
        [JsonProperty("audio")]
        public Audio Audio { get; set; }

        /// <summary>
        /// Caption for the document, 0-200 characters.
        /// </summary>
        [JsonProperty("caption")]
        public string Caption { get; set; }
    }

    /// <summary>
    /// A class which represents the Telegram document message.
    /// </summary>
    public class DocumentMessage : Message, ISenderMessage, IDocumentMessage
    {
        /// <summary>
        /// Sender, can be empty for messages sent to channels.
        /// </summary>
        [JsonProperty("from")]
        public User From { get; set; }

        /// <summary>
        /// Information about general file.
        /// </summary>
        [JsonProperty("document")]
        public Document Document { get; set; }

        /// <summary>
        /// Caption for the document, 0-200 characters.
        /// </summary>
        [JsonProperty("caption")]
        public string Caption { get; set; }
    }

    /// <summary>
    /// A class which represents the Telegram sticker message.
    /// </summary>
    public class StickerMessage : Message, ISenderMessage, IStickerMessage
    {
        /// <summary>
        /// Sender, can be empty for messages sent to channels.
        /// </summary>
        [JsonProperty("from")]
        public User From { get; set; }

        /// <summary>
        /// Information about the sticker.
        /// </summary>
        [JsonProperty("sticker")]
        public Sticker Sticker { get; set; }
    }

    /// <summary>
    /// A class which represents the Telegram photo message.
    /// </summary>
    public class PhotoMessage : Message, ISenderMessage, IPhotoMessage
    {
        /// <summary>
        /// Sender, can be empty for messages sent to channels.
        /// </summary>
        [JsonProperty("from")]
        public User From { get; set; }

        /// <summary>
        /// Available sizes of the photo.
        /// </summary>
        [JsonConverter(typeof(ArrayToListConverter<PhotoSize>))]
        [JsonProperty("photo")]
        public List<PhotoSize> Photo { get; set; }

        /// <summary>
        /// Caption for the photo, 0-200 characters.
        /// </summary>
        [JsonProperty("caption")]
        public string Caption { get; set; }
    }

    /// <summary>
    /// A class which represents the Telegram game message.
    /// </summary>
    public class GameMessage : Message, ISenderMessage, IGameMessage
    {
        /// <summary>
        /// Sender, can be empty for messages sent to channels.
        /// </summary>
        [JsonProperty("from")]
        public User From { get; set; }

        /// <summary>
        /// Information about the game.
        /// </summary>
        [JsonProperty("game")]
        public Game Game { get; set; }
    }

    /// <summary>
    /// A class which represents the Telegram video message.
    /// </summary>
    public class VideoMessage : Message, ISenderMessage, IVideoMessage
    {
        /// <summary>
        /// Sender, can be empty for messages sent to channels.
        /// </summary>
        [JsonProperty("from")]
        public User From { get; set; }

        /// <summary>
        /// Information about the video.
        /// </summary>
        [JsonProperty("video")]
        public Video Video { get; set; }

        /// <summary>
        /// Caption for the video, 0-200 characters.
        /// </summary>
        [JsonProperty("caption")]
        public string Caption { get; set; }
    }

    /// <summary>
    /// A class which represents the Telegram voice message.
    /// </summary>
    public class VoiceMessage : Message, ISenderMessage, IVoiceMessage
    {
        /// <summary>
        /// Sender, can be empty for messages sent to channels.
        /// </summary>
        [JsonProperty("from")]
        public User From { get; set; }

        /// <summary>
        /// Information about the voice.
        /// </summary>
        [JsonProperty("voice")]
        public Voice Voice { get; set; }
    }

    /// <summary>
    /// A class which represents the Telegram shared contact message.
    /// </summary>
    public class ContactMessage : Message, ISenderMessage, ITelegramContactMessage
    {
        /// <summary>
        /// Sender, can be empty for messages sent to channels.
        /// </summary>
        [JsonProperty("from")]
        public User From { get; set; }

        /// <summary>
        /// Information about the contact.
        /// </summary>
        [JsonProperty("contact")]
        public Contact Contact { get; set; }
    }

    /// <summary>
    /// A class which represents the Telegram shared location message.
    /// </summary>
    public class LocationMessage : Message, ISenderMessage, ILocationMessage
    {
        /// <summary>
        /// Sender, can be empty for messages sent to channels.
        /// </summary>
        [JsonProperty("from")]
        public User From { get; set; }

        /// <summary>
        /// Information about the location.
        /// </summary>
        [JsonProperty("location")]
        public Location Location { get; set; }
    }

    /// <summary>
    /// A class which represents the Telegram venue message.
    /// </summary>
    public class VenueMessage : Message, ISenderMessage, IVenueMessage
    {
        /// <summary>
        /// Sender, can be empty for messages sent to channels.
        /// </summary>
        [JsonProperty("from")]
        public User From { get; set; }

        /// <summary>
        /// Information about the venue.
        /// </summary>
        [JsonProperty("venue")]
        public Venue Venue { get; set; }
    }

    /// <summary>
    /// A class which represents the Telegram message about new chat member.
    /// </summary>
    public class NewChatMemberMessage : Message
    {
        /// <summary>
        /// Sender, can be empty for messages sent to channels.
        /// </summary>
        [JsonProperty("from")]
        public User From { get; set; }

        /// <summary>
        /// Information about first member, which been added to the group.
        /// </summary>
        [JsonProperty("new_chat_member")]
        public User NewChatMember { get; set; }

        /// <summary>
        /// Information about first member, which been added to the group.
        /// (<see cref="NewChatMember"/>  copy?)
        /// </summary>
        [JsonProperty("new_chat_participant")]
        public User NewChatParticipant { get; set; }

        /// <summary>
        /// Information about members, which been added to the group.
        /// </summary>
        [JsonConverter(typeof(ArrayToListConverter<User>))]
        [JsonProperty("new_chat_members")]
        public List<User> NewChatMembers { get; set; }
    }

    /// <summary>
    /// A class which represents the Telegram message about left chat member.
    /// </summary>
    public class LeftChatMemberMessage : Message
    {
        /// <summary>
        /// Sender, can be empty for messages sent to channels.
        /// </summary>
        [JsonProperty("from")]
        public User From { get; set; }

        /// <summary>
        /// Information about member, which been removed from the group.
        /// </summary>
        [JsonProperty("left_chat_member")]
        public User LeftChatMember { get; set; }

        /// <summary>
        /// Information about member, which been removed from the group.
        /// (<see cref="LeftChatMember"/>  copy?)
        /// </summary>
        [JsonProperty("left_chat_participant")]
        public User LeftChatParticipant { get; set; }
    }

    /// <summary>
    /// A class which represents the Telegram message about new chat title.
    /// </summary>
    public class NewChatTitleMessage : Message
    {
        /// <summary>
        /// Sender, can be empty for messages sent to channels.
        /// </summary>
        [JsonProperty("from")]
        public User From { get; set; }

        /// <summary>
        /// A Title. :>
        /// </summary>
        [JsonProperty("new_chat_title")]
        public string NewChatTitle { get; set; }
    }

    /// <summary>
    /// A class which represents the Telegram message about new chat photo.
    /// </summary>
    public class NewChatPhotoMessage : Message
    {
        /// <summary>
        /// Sender, can be empty for messages sent to channels.
        /// </summary>
        [JsonProperty("from")]
        public User From { get; set; }

        /// <summary>
        /// Available sizes of the new chat photo.
        /// </summary>
        [JsonConverter(typeof(ArrayToListConverter<PhotoSize>))]
        [JsonProperty("new_chat_photo")]
        public List<PhotoSize> NewChatPhoto { get; set; }
    }

    ///// <summary>
    ///// A class which represents the Telegram service message.
    ///// </summary>
    //public class TelegramServiceMessage : TelegramMessage
    //{
    //    /// <summary>
    //    /// Service chat message.
    //    /// </summary>
    //    public string Message { get; set; }
    //}

    /// <summary>
    /// A class which represents the Telegram message indicated 
    /// that the group has been migrated to a supergroup with the specified identifier.
    /// </summary>
    public class MigrateToChatIdMessage : Message
    {
        /// <summary>
        /// Sender, can be empty for messages sent to channels.
        /// </summary>
        [JsonProperty("from")]
        public User From { get; set; }

        /// <summary>
        /// Supergroup identifier.
        /// </summary>
        [JsonProperty("migrate_to_chat_id")]
        public long MigrateToChatId { get; set; }
    }

    /// <summary>
    /// A class which represents the Telegram message indicated 
    /// that the supergroup has been migrated from a group with the specified identifier.
    /// </summary>
    public class MigrateFromChatIdMessage : Message
    {
        /// <summary>
        /// Sender, can be empty for messages sent to channels.
        /// </summary>
        [JsonProperty("from")]
        public User From { get; set; }

        /// <summary>
        /// Supergroup identifier.
        /// </summary>
        [JsonProperty("migrate_from_chat_id")]
        public long MigrateFromChatId { get; set; }
    }

    /// <summary>
    /// A class which represents the Telegram pinned specified message.
    /// </summary>
    public class PinnedMessage : Message
    {
        /// <summary>
        /// Sender, can be empty for messages sent to channels.
        /// </summary>
        [JsonProperty("from")]
        public User From { get; set; }

        /// <summary>
        /// Pinned message.
        /// </summary>
        [JsonConverter(typeof(MessageConverter))]
        [JsonProperty("pinned_message")]
        public Message Message { get; set; }
    }

    /// <summary>
    /// Temprory class for unknown Telegram message.
    /// </summary>
    public class UnknownMessage : Message
    {

    }
}
