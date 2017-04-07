using System;
using System.Collections.Generic;

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
        public int MessageId { get; set; }

        /// <summary>
        /// Date the message was sent.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Conversation the message belongs to.
        /// </summary>
        public Chat Chat { get; set; }

        /// <summary>
        /// Optional. For forwarded messages, sender of the original message.
        /// </summary>
        public User ForwardFrom { get; set; }

        /// <summary>
        /// Optional. For messages forwarded from a channel, information about the original channel.
        /// </summary>
        public Chat ForwardFromChat { get; set; }

        /// <summary>
        /// Optional. For forwarded channel posts, identifier of the original message in the channel.
        /// </summary>
        public int? ForwardFromMessageId { get; set; }

        /// <summary>
        /// Optional. For forwarded messages, date the original message was sent.
        /// </summary>
        public DateTime? ForwardDate { get; set; }

        /// <summary>
        /// Optional. Date the message was last edited.
        /// </summary>
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
        public User From { get; set; }

        /// <summary>
        /// Text message, the actual UTF-8 text of the message, 0-4096 characters.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Special entities like usernames, URLs, bot commands, etc. that appear in the text.
        /// </summary>
        public List<MessageEntity> Entities { get; set; }

        /// <summary>
        /// Optional. For replies, the original message. 
        /// </summary>
        /// <remark>Note that the Message object in this field will not contain further reply_to_message fields even if it itself is a reply.</remark>
        public Message ReplyToMessage { get; set; }

    }

    /// <summary>
    /// A class which represents the Telegram audio message.
    /// </summary>
    public class TelegramAudioMessage : Message, ISenderMessage, IAudioMessage
    {
        /// <summary>
        /// Sender, can be empty for messages sent to channels.
        /// </summary>
        public User From { get; set; }

        /// <summary>
        /// Information about the audio file.
        /// </summary>
        public Audio Audio { get; set; }

        /// <summary>
        /// Caption for the document, 0-200 characters.
        /// </summary>
        public string Caption { get; set; }
    }

    /// <summary>
    /// A class which represents the Telegram document message.
    /// </summary>
    public class TelegramDocumentMessage : Message, ISenderMessage, IDocumentMessage
    {
        /// <summary>
        /// Sender, can be empty for messages sent to channels.
        /// </summary>
        public User From { get; set; }

        /// <summary>
        /// Information about general file.
        /// </summary>
        public Document Document { get; set; }

        /// <summary>
        /// Caption for the document, 0-200 characters.
        /// </summary>
        public string Caption { get; set; }
    }

    /// <summary>
    /// A class which represents the Telegram sticker message.
    /// </summary>
    public class TelegramStickerMessage : Message, ISenderMessage, IStickerMessage
    {
        /// <summary>
        /// Sender, can be empty for messages sent to channels.
        /// </summary>
        public User From { get; set; }

        /// <summary>
        /// Information about the sticker.
        /// </summary>
        public Sticker Sticker { get; set; }
    }

    /// <summary>
    /// A class which represents the Telegram photo message.
    /// </summary>
    public class TelegramPhotoMessage : Message, ISenderMessage, IPhotoMessage
    {
        /// <summary>
        /// Sender, can be empty for messages sent to channels.
        /// </summary>
        public User From { get; set; }

        /// <summary>
        /// Available sizes of the photo.
        /// </summary>
        public List<PhotoSize> Photo { get; set; }

        /// <summary>
        /// Caption for the photo, 0-200 characters.
        /// </summary>
        public string Caption { get; set; }
    }

    /// <summary>
    /// A class which represents the Telegram game message.
    /// </summary>
    public class TelegramGameMessage : Message, ISenderMessage, IGameMessage
    {
        /// <summary>
        /// Sender, can be empty for messages sent to channels.
        /// </summary>
        public User From { get; set; }

        /// <summary>
        /// Information about the game.
        /// </summary>
        public Game Game { get; set; }
    }

    /// <summary>
    /// A class which represents the Telegram video message.
    /// </summary>
    public class TelegramVideoMessage : Message, ISenderMessage, IVideoMessage
    {
        /// <summary>
        /// Sender, can be empty for messages sent to channels.
        /// </summary>
        public User From { get; set; }

        /// <summary>
        /// Information about the video.
        /// </summary>
        public Video Video { get; set; }

        /// <summary>
        /// Caption for the video, 0-200 characters.
        /// </summary>
        public string Caption { get; set; }
    }

    /// <summary>
    /// A class which represents the Telegram voice message.
    /// </summary>
    public class TelegramVoiceMessage : Message, ISenderMessage, IVoiceMessage
    {
        /// <summary>
        /// Sender, can be empty for messages sent to channels.
        /// </summary>
        public User From { get; set; }

        /// <summary>
        /// Information about the voice.
        /// </summary>
        public Voice Voice { get; set; }
    }

    /// <summary>
    /// A class which represents the Telegram shared contact message.
    /// </summary>
    public class TelegramContactMessage : Message, ISenderMessage, ITelegramContactMessage
    {
        /// <summary>
        /// Sender, can be empty for messages sent to channels.
        /// </summary>
        public User From { get; set; }

        /// <summary>
        /// Information about the contact.
        /// </summary>
        public Contact Contact { get; set; }
    }

    /// <summary>
    /// A class which represents the Telegram shared location message.
    /// </summary>
    public class TelegramLocationMessage : Message, ISenderMessage, ILocationMessage
    {
        /// <summary>
        /// Sender, can be empty for messages sent to channels.
        /// </summary>
        public User From { get; set; }

        /// <summary>
        /// Information about the location.
        /// </summary>
        public Location Location { get; set; }
    }

    /// <summary>
    /// A class which represents the Telegram venue message.
    /// </summary>
    public class TelegramVenueMessage : Message, ISenderMessage, IVenueMessage
    {
        /// <summary>
        /// Sender, can be empty for messages sent to channels.
        /// </summary>
        public User From { get; set; }

        /// <summary>
        /// Information about the venue.
        /// </summary>
        public Venue Venue { get; set; }
    }

    /// <summary>
    /// A class which represents the Telegram message about new chat member.
    /// </summary>
    public class TelegramNewChatMemberMessage : Message
    {
        /// <summary>
        /// Sender, can be empty for messages sent to channels.
        /// </summary>
        public User From { get; set; }

        /// <summary>
        /// Information about first member, which been added to the group.
        /// </summary>
        public User NewChatMember { get; set; }

        /// <summary>
        /// Information about first member, which been added to the group.
        /// (<see cref="NewChatMember"/>  copy?)
        /// </summary>
        public User NewChatParticipant { get; set; }

        /// <summary>
        /// Information about members, which been added to the group.
        /// </summary>
        public List<User> NewChatMembers { get; set; }
    }

    /// <summary>
    /// A class which represents the Telegram message about left chat member.
    /// </summary>
    public class TelegramLeftChatMemberMessage : Message
    {
        /// <summary>
        /// Sender, can be empty for messages sent to channels.
        /// </summary>
        public User From { get; set; }

        /// <summary>
        /// Information about member, which been removed from the group.
        /// </summary>
        public User LeftChatMember { get; set; }

        /// <summary>
        /// Information about member, which been removed from the group.
        /// (<see cref="LeftChatMember"/>  copy?)
        /// </summary>
        public User LeftChatParticipant { get; set; }
    }

    /// <summary>
    /// A class which represents the Telegram message about new chat title.
    /// </summary>
    public class TelegramNewChatTitleMessage : Message
    {
        /// <summary>
        /// Sender, can be empty for messages sent to channels.
        /// </summary>
        public User From { get; set; }

        /// <summary>
        /// A Title. :>
        /// </summary>
        public string NewChatTitle { get; set; }
    }

    /// <summary>
    /// A class which represents the Telegram message about new chat photo.
    /// </summary>
    public class TelegramNewChatPhotoMessage : Message
    {
        /// <summary>
        /// Sender, can be empty for messages sent to channels.
        /// </summary>
        public User From { get; set; }

        /// <summary>
        /// Available sizes of the new chat photo.
        /// </summary>
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
    public class TelegramMigrateToChatIdMessage : Message
    {
        /// <summary>
        /// Sender, can be empty for messages sent to channels.
        /// </summary>
        public User From { get; set; }

        /// <summary>
        /// Supergroup identifier.
        /// </summary>
        public long MigrateToChatId { get; set; }
    }

    /// <summary>
    /// A class which represents the Telegram message indicated 
    /// that the supergroup has been migrated from a group with the specified identifier.
    /// </summary>
    public class TelegramMigrateFromChatIdMessage : Message
    {
        /// <summary>
        /// Sender, can be empty for messages sent to channels.
        /// </summary>
        public User From { get; set; }

        /// <summary>
        /// Supergroup identifier.
        /// </summary>
        public long MigrateFromChatId { get; set; }
    }

    /// <summary>
    /// A class which represents the Telegram pinned specified message.
    /// </summary>
    public class TelegramPinnedMessage : Message
    {
        /// <summary>
        /// Sender, can be empty for messages sent to channels.
        /// </summary>
        public User From { get; set; }

        /// <summary>
        /// Pinned message.
        /// </summary>
        public Message PinnedMessage { get; set; }
    }

    /// <summary>
    /// Temprory class for unknown Telegram message.
    /// </summary>
    public class TelegramUnknownMessage : Message
    {

    }
}
