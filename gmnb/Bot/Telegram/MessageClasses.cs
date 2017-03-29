using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeJelly.Bot.Bot.Telegram
{
    /// <summary>
    /// A class which represents the telegram text user's message.
    /// </summary>
    public class TelegramTextUserMessage : ITelegramMessage, IUserMessage, ITelegramTextMessage
    {
        public int MessageId { get; set; }
        public DateTime Date { get; set; }
        public Chat Chat { get; set; }
        public User From { get; set; }


        public string Text { get; set; }
        public List<MessageEntity> Entities { get; set; }
    }

    /// <summary>
    /// A class which represents the telegram message about new chat member.
    /// </summary>
    public class TelegramNewChatMemberMessage : ITelegramMessage
    {
        public int MessageId { get; set; }
        public DateTime Date { get; set; }
        public Chat Chat { get; set; }

        /// <summary>
        /// Information about member, which been added to the group.
        /// </summary>
        public User NewChatMember { get; set; }
    }

    /// <summary>
    /// A class which represents the telegram message about left chat member.
    /// </summary>
    public class TelegramLeftChatMemberMessage : ITelegramMessage
    {
        public int MessageId { get; set; }
        public DateTime Date { get; set; }
        public Chat Chat { get; set; }

        /// <summary>
        /// Information about member, which been removed from the group.
        /// </summary>
        public User LeftChatMember { get; set; }
    }

    /// <summary>
    /// A class which represents the telegram message about new chat title.
    /// </summary>
    public class TelegramNewChatTitleMessage : ITelegramMessage
    {
        public int MessageId { get; set; }
        public DateTime Date { get; set; }
        public Chat Chat { get; set; }

        /// <summary>
        /// A Title. :>
        /// </summary>
        public string NewChatTitle { get; set; }
    }

    /// <summary>
    /// A class which represents the telegram message about new chat photo.
    /// </summary>
    public class TelegramNewChatPhotoMessage : ITelegramMessage
    {
        public int MessageId { get; set; }
        public DateTime Date { get; set; }
        public Chat Chat { get; set; }

        /// <summary>
        /// Available sizes of the new chat photo.
        /// </summary>
        public List<PhotoSize> NewChatPhoto { get; set; }
    }

    /// <summary>
    /// A class which represents the telegram service message.
    /// </summary>
    public class TelegramServiceMessage : ITelegramMessage
    {
        public int MessageId { get; set; }
        public DateTime Date { get; set; }
        public Chat Chat { get; set; }

        /// <summary>
        /// Service chat message.
        /// </summary>
        public string Message { get; set; }
    }

    /// <summary>
    /// A class which represents that the group has been migrated to a supergroup with the specified identifier.
    /// </summary>
    public class TelegramMigrateToChatIdMessage : ITelegramMessage
    {
        public int MessageId { get; set; }
        public DateTime Date { get; set; }
        public Chat Chat { get; set; }

        /// <summary>
        /// Supergroup identifier.
        /// </summary>
        public long MigrateToChatId { get; set; }
    }

    /// <summary>
    /// A class which represents that the supergroup has been migrated from a group with the specified identifier.
    /// </summary>
    public class TelegramMigrateFromChatIdMessage : ITelegramMessage
    {
        public int MessageId { get; set; }
        public DateTime Date { get; set; }
        public Chat Chat { get; set; }

        /// <summary>
        /// Supergroup identifier.
        /// </summary>
        public long MigrateGromChatId { get; set; }
    }

    /// <summary>
    /// A class which represents the pinned specified message.
    /// </summary>
    public class TelegramPinnedMessage : ITelegramMessage
    {
        public int MessageId { get; set; }
        public DateTime Date { get; set; }
        public Chat Chat { get; set; }

        /// <summary>
        /// Pinned message.
        /// </summary>
        public long PinnedMessage { get; set; }
    }

    /// <summary>
    /// Temprory class for unknown telegram message.
    /// </summary>
    public class TelegramUnknownMessage : ITelegramMessage
    {
        public int MessageId { get; set; }
        public DateTime Date { get; set; }
        public Chat Chat { get; set; }
    }
}
