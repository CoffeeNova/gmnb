using System.Collections.Generic;

namespace CoffeeJelly.gmailNotifyBot.Bot.Telegram
{
    /// <summary>
    /// Represents a Telegram user or bot.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Unique identifier for this user or bot.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// User's or bot's first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// User‘s or bot’s last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// User‘s or bot’s username.
        /// </summary>
        public string Username { get; set; }
    }

    /// <summary>
    /// Represents a chat.
    /// </summary>
    public class Chat
    {
        /// <summary>
        /// Unique identifier for this chat.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Type of chat, can be either “private”, “group”, “supergroup” or “channel”.
        /// </summary>
        public ChatType Type { get; set; }

        /// <summary>
        /// Title, for supergroups, channels and group chats.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Username, for private chats, supergroups and channels if available.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// First name of the other party in a private chat.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name of the other party in a private chat.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// <see langword="true"/>: if a group has ‘All Members Are Admins’ enabled.
        /// </summary>
        public bool? AllMembersAreAdministrators { get; set; }
    }

    public enum ChatType
    {
        Private,
        Group,
        Supergroup,
        Channel
    }
    /// <summary>
    /// This class represents one special entity in a text message. For example, hashtags, usernames, URLs, etc.
    /// </summary>
    public class MessageEntity
    {
        /// <summary>
        /// Type of the entity.
        /// </summary>
        /// <remarks>
        /// Can be mention (@username), hashtag, bot_command, url, email, bold (bold text), italic (italic text), 
        /// code (monowidth string), pre (monowidth block), text_link (for clickable text URLs), 
        /// text_mention (for users without usernames)
        /// </remarks>
        public string Type { get; set; }

        /// <summary>
        /// Offset in UTF-16 code units to the start of the entity.
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Length of the entity in UTF-16 code units.
        /// </summary>
        public int Lenght { get; set; }

        /// <summary>
        /// For “text_link” only, url that will be opened after user taps on the text.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// For “text_mention” only, the mentioned user.
        /// </summary>
        public User User { get; set; }
    }

    /// <summary>
    /// Represents one size of a photo or a file / sticker thumbnail.
    /// </summary>
    public class PhotoSize : ITelegramMessageFileId, ITelegramMessageFileImageSize, ITelegramMessageFileSize
    {
        /// <summary>
        /// Unique identifier for this file.
        /// </summary>
        public string FileId { get; set; }

        /// <summary>
        /// Photo width.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Photo height.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// File size.
        /// </summary>
        public int? FileSize { get; set; }
    }

    /// <summary>
    ///  Represents an audio file to be treated as music by the Telegram clients.
    /// </summary>
    public class Audio : ITelegramMessageFileId, ITelegramMessageFileDuration, ITelegramMessageFileMimeType, 
                        ITelegramMessageFileSize
    {
        /// <summary>
        /// Unique identifier for this file.
        /// </summary>
        public string FileId { get; set; }

        /// <summary>
        /// Duration of the audio in seconds as defined by sender.
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// Performer of the audio as defined by sender or by audio tags.
        /// </summary>
        public string Performer { get; set; }

        /// <summary>
        /// Title of the audio as defined by sender or by audio tags.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// MIME type of the file as defined by sender.
        /// </summary>
        public string MimeType { get; set; }

        /// <summary>
        /// File size.
        /// </summary>
        public int? FileSize { get; set; }
    }

    /// <summary>
    /// Represents a general file (as opposed to photos, voice messages and audio files).
    /// </summary>
    public class Document : ITelegramMessageFileId, ITelegramMessageFileThumb, ITelegramFileName, 
                            ITelegramMessageFileMimeType, ITelegramMessageFileSize
    {
        /// <summary>
        /// Unique file identifier.
        /// </summary>
        public string FileId { get; set; }

        /// <summary>
        /// Document thumbnail as defined by sender.
        /// </summary>
        public PhotoSize Thumb { get; set; }

        /// <summary>
        /// Original filename as defined by sender.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// MIME type of the file as defined by sender.
        /// </summary>
        public string MimeType { get; set; }

        /// <summary>
        /// File size.
        /// </summary>
        public int? FileSize { get; set; }
    }

    /// <summary>
    /// Represents a sticker.
    /// </summary>
    public class Sticker : ITelegramMessageFileId, ITelegramMessageFileImageSize, ITelegramMessageFileThumb, 
                            ITelegramMessageFileSize
    {
        /// <summary>
        /// Unique file identifier.
        /// </summary>
        public string FileId { get; set; }

        /// <summary>
        /// Sticker width.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Sticker height.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Sticker thumbnail in .webp or .jpg format.
        /// </summary>
        public PhotoSize Thumb { get; set; }

        /// <summary>
        /// Emoji associated with the sticker.
        /// </summary>
        public string Emoji { get; set; }

        /// <summary>
        /// File size.
        /// </summary>
        public int? FileSize { get; set; }
    }

    /// <summary>
    /// Represents a video file.
    /// </summary>
    public class Video : ITelegramMessageFileId, ITelegramMessageFileImageSize, ITelegramMessageFileDuration, 
                            ITelegramMessageFileThumb, ITelegramMessageFileMimeType, ITelegramMessageFileSize
    {
        /// <summary>
        /// Unique identifier for this file.
        /// </summary>
        public string FileId { get; set; }

        /// <summary>
        /// Video width as defined by sender.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Video height as defined by sender
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Duration of the video in seconds as defined by sender.
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// Video thumbnail.
        /// </summary>
        public PhotoSize Thumb { get; set; }

        /// <summary>
        /// Mime type of a file as defined by sender.
        /// </summary>
        public string MimeType { get; set; }

        /// <summary>
        /// File size.
        /// </summary>
        public int? FileSize { get; set; }
    }

    /// <summary>
    /// Represents a voice note.
    /// </summary>
    public class Voice : ITelegramMessageFileId, ITelegramMessageFileDuration, ITelegramMessageFileMimeType,
        ITelegramMessageFileSize
    {
        /// <summary>
        /// Unique identifier for this file
        /// </summary>
        public string FileId { get; set; }

        /// <summary>
        /// Duration of the audio in seconds as defined by sender.
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// MIME type of the file as defined by sender.
        /// </summary>
        public string MimeType { get; set; }

        /// <summary>
        /// File size.
        /// </summary>
        public int? FileSize { get; set; }
    }

    /// <summary>
    /// Represents a phone contact.
    /// </summary>
    public class Contact
    {
        /// <summary>
        /// Contact's phone number.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Contact's first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Contact's last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Contact's user identifier in Telegram.
        /// </summary>
        public int UserId { get; set; }
    }

    /// <summary>
    /// Represents a point on the map.
    /// </summary>
    public class Location
    {
        /// <summary>
        /// Longitude as defined by sender.
        /// </summary>
        public float Longtitude { get; set; }

        /// <summary>
        /// Latitude as defined by sender.
        /// </summary>
        public float Latitude { get; set; }
    }

    /// <summary>
    /// Represents a venue.
    /// </summary>
    public class Venue
    {
        /// <summary>
        /// Venue location.
        /// </summary>
        public Location Location { get; set; }

        /// <summary>
        /// Name of the venue.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Address of the venue.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Foursquare identifier of the venue.
        /// </summary>
        public string FoursquareId { get; set; }
    }

    /// <summary>
    /// Represent a user's profile pictures.
    /// </summary>
    public class UserProfilePhotos
    {
        /// <summary>
        /// Total number of profile pictures the target user has.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Requested profile pictures (in up to 4 sizes each).
        /// </summary>
        public List<PhotoSize> Photos { get; set; }
    }

    /// <summary>
    /// Represents a game.
    /// </summary>
    public class Game
    {
        /// <summary>
        /// Title of the game.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Description of the game.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Photo that will be displayed in the game message in chats.
        /// </summary>
        public List<PhotoSize> Photo { get; set; }

        /// <summary>
        /// Brief description of the game or high scores included in the game message. 
        /// Can be automatically edited to include current high scores for the game when the bot calls setGameScore, 
        /// or manually edited using editMessageText. 0-4096 characters.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Special entities that appear in text, such as usernames, URLs, bot commands, etc.
        /// </summary>
        public List<MessageEntity> TextEntities { get; set; }

        /// <summary>
        /// Animation that will be displayed in the game message in chats.
        /// </summary>
        public Animation Animation { get; set; }
    }

    /// <summary>
    /// Represents an animation file to be displayed in the message containing a game.
    /// </summary>
    public class Animation
    {
        /// <summary>
        /// 	Unique file identifier
        /// </summary>
        public string FileId { get; set; }

        /// <summary>
        /// Animation thumbnail as defined by sender.
        /// </summary>
        public PhotoSize Thumb { get; set; }

        /// <summary>
        /// Original animation filename as defined by sender.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// MIME type of the file as defined by sender.
        /// </summary>
        public string MimeType { get; set; }

        /// <summary>
        /// File size.
        /// </summary>
        public int? FileSize { get; set; }
    }
}
