using System;
using System.Collections.Generic;

namespace CoffeeJelly.gmailNotifyBot.Bot.Telegram
{
    /// <summary>
    /// A class which represents the general Telegram message.
    /// </summary>
    public abstract class TelegramMessage : ITelegramMessage
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
    }

    /// <summary>
    /// A class which represents the Telegram text user's message.
    /// </summary>
    public class TelegramTextMessage : TelegramMessage, ISenderMessage, ITelegramTextMessage
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
    }

    /// <summary>
    /// A class which represents the Telegram audio message.
    /// </summary>
    public class TelegramAudioMessage : TelegramMessage, ISenderMessage, ITelegramAudioMessage
    {
        /// <summary>
        /// Sender, can be empty for messages sent to channels.
        /// </summary>
        public User From { get; set; }

        /// <summary>
        /// Information about the audio file.
        /// </summary>
        public Audio Audio { get; set; }
    }

    /// <summary>
    /// A class which represents the Telegram document message.
    /// </summary>
    public class TelegramDocumentMessage : TelegramMessage, ISenderMessage, ITelegramDocumentMessage
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
    public class TelegramStickerMessage : TelegramMessage, ISenderMessage, ITelegramStickerMessage
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
    public class TelegramPhotoMessage : TelegramMessage, ISenderMessage, ITelegramPhotoMessage
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
    public class TelegramGameMessage : TelegramMessage, ISenderMessage, ITelegramGameMessage
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
    public class TelegramVideoMessage : TelegramMessage, ISenderMessage, ITelegramVideoMessage
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
    public class TelegramVoiceMessage : TelegramMessage, ISenderMessage, ITelegramVoiceMessage
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
    public class TelegramContactMessage : TelegramMessage, ISenderMessage, ITelegramContactMessage
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
    public class TelegramLocationMessage : TelegramMessage, ISenderMessage, ITelegramLocationMessage
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
    public class TelegramVenueMessage : TelegramMessage, ISenderMessage, ITelegramVenueMessage
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
    public class TelegramNewChatMemberMessage : TelegramMessage
    {
        /// <summary>
        /// Information about member, which been added to the group.
        /// </summary>
        public User NewChatMember { get; set; }
    }

    /// <summary>
    /// A class which represents the Telegram message about left chat member.
    /// </summary>
    public class TelegramLeftChatMemberMessage : TelegramMessage
    {
        /// <summary>
        /// Information about member, which been removed from the group.
        /// </summary>
        public User LeftChatMember { get; set; }
    }

    /// <summary>
    /// A class which represents the Telegram message about new chat title.
    /// </summary>
    public class TelegramNewChatTitleMessage : TelegramMessage
    {
        /// <summary>
        /// A Title. :>
        /// </summary>
        public string NewChatTitle { get; set; }
    }

    /// <summary>
    /// A class which represents the Telegram message about new chat photo.
    /// </summary>
    public class TelegramNewChatPhotoMessage : TelegramMessage
    {
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
    public class TelegramMigrateToChatIdMessage : TelegramMessage
    {
        /// <summary>
        /// Supergroup identifier.
        /// </summary>
        public long MigrateToChatId { get; set; }
    }

    /// <summary>
    /// A class which represents the Telegram message indicated 
    /// that the supergroup has been migrated from a group with the specified identifier.
    /// </summary>
    public class TelegramMigrateFromChatIdMessage : TelegramMessage
    {
        /// <summary>
        /// Supergroup identifier.
        /// </summary>
        public long MigrateFromChatId { get; set; }
    }

    /// <summary>
    /// A class which represents the Telegram pinned specified message.
    /// </summary>
    public class TelegramPinnedMessage : TelegramMessage
    {
        /// <summary>
        /// Pinned message.
        /// </summary>
        public TelegramMessage PinnedMessage { get; set; }
    }

    /// <summary>
    /// Temprory class for unknown Telegram message.
    /// </summary>
    public class TelegramUnknownMessage : TelegramMessage
    {

    }
}
