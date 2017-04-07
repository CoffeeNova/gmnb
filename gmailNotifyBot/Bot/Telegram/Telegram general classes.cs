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
    public class PhotoSize : IFileId, IFileImageSize, IFileSize
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
    public class Audio : IFileId, IFileDuration, IFileMimeType, 
                        IFileSize
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
    public class Document : IFileId, IFileThumb, IFileName, 
                            IFileMimeType, IFileSize
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
    public class Sticker : IFileId, IFileImageSize, IFileThumb, 
                            IFileSize
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
    public class Video : IFileId, IFileImageSize, IFileDuration, 
                            IFileThumb, IFileMimeType, IFileSize
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
    public class Voice : IFileId, IFileDuration, IFileMimeType,
        IFileSize
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
        public int? UserId { get; set; }
    }

    /// <summary>
    /// Represents a point on the map.
    /// </summary>
    public class Location
    {
        /// <summary>
        /// Longitude as defined by sender.
        /// </summary>
        public float Longitude { get; set; }

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

    /// <summary>
    /// Represents one button of the reply keyboard. 
    /// </summary>
    public class KeyboardButton
    {
        /// <summary>
        /// Text of the button. If none of the optional fields are used, it will be sent to the bot as a message when the button is pressed.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Optional. If <see langword="true"/>, the user's phone number will be sent as a contact when the button is pressed. 
        /// Available in private chats only
        /// </summary>
        public bool? RequestContact { get; set; }

        /// <summary>
        /// Optional. If <see langword="true"/>, the user's current location will be sent when the button is pressed. 
        /// Available in private chats only.
        /// </summary>
        public bool? RequestLocation { get; set; }
    }

    /// <summary>
    /// Represents a custom keyboard with reply options.
    /// </summary>
    public class ReplyKeyboardMarkup
    {
        /// <summary>
        /// List of button rows, each represented by an List of KeyboardButton objects
        /// </summary>
        public List<KeyboardButton> Keyboard { get; set; }

        /// <summary>
        /// Requests clients to resize the keyboard vertically for optimal fit (e.g., make the keyboard smaller if there are just two rows of buttons). 
        /// </summary>
        /// <remarks>
        /// Defaults to <see langword="false"/>, in which case the custom keyboard is always of the same height as the app's standard keyboard.
        /// </remarks>
        public bool? ResizeKeyboard { get; set; } = false;

        /// <summary>
        /// Requests clients to hide the keyboard as soon as it's been used. 
        /// The keyboard will still be available, but clients will automatically display the usual letter-keyboard in the chat – 
        /// the user can press a special button in the input field to see the custom keyboard again. 
        /// </summary>
        /// <remarks>Defaults to <see langword="false"/>.</remarks>
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
        public bool? Selective { get; set; }
    }

    /// <summary>
    /// Upon receiving a message with this object, Telegram clients will remove the current custom keyboard and display the default letter-keyboard. 
    /// By default, custom keyboards are displayed until a new keyboard is sent by a bot. 
    /// An exception is made for one-time keyboards that are hidden immediately after the user presses a button (see <see cref="ReplyKeyboardMarkup"/>).
    /// </summary>
    public class ReplyKeyboardRemove
    {
        /// <summary>
        /// Requests clients to remove the custom keyboard (user will not be able to summon this keyboard; 
        /// if you want to hide the keyboard from sight but keep it accessible, use <see cref="ReplyKeyboardMarkup.OneTimeKeyboard"/>)
        /// </summary>
        public bool RemoveKeyboard { get; } = true;

        /// <summary>
        /// Optional. Use this parameter if you want to show the keyboard to specific users only. 
        /// Targets: 1) users that are @mentioned in the <see cref="TextMessage.Text"/>; 
        /// 2) if the bot's message is a reply (has ReplyToMessageId), sender of the original message.
        /// </summary>
        /// <example>
        /// A user votes in a poll, bot returns confirmation message in reply to the vote and removes the keyboard for that user, 
        /// while still showing the keyboard with poll options to users who haven't voted yet.
        /// </example>
        public bool? Selective { get; set; }
    }

    /// <summary>
    /// Represents an inline keyboard that appears right next to the message it belongs to.
    /// </summary>
    public class InlineKeyboardMarkup
    {
        /// <summary>
        /// Array of button rows, each represented by an Array of InlineKeyboardButton objects.
        /// </summary>
        public List<InlineKeyboardButton> InlineKeyboard { get; set; }
    }

    /// <summary>
    /// Represents one button of an inline keyboard. You must use exactly one of the optional fields.
    /// </summary>
    public class InlineKeyboardButton
    {
        /// <summary>
        /// Label text on the button.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Optional. HTTP url to be opened when button is pressed.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Optional. Data to be sent in a <see cref="CallbackQuery"/> to the bot when button is pressed, 1-64 bytes.
        /// </summary>
        public string CallbackData { get; set; }

        /// <summary>
        /// Optional. If set, pressing the button will prompt the user to select one of their chats, open that chat and insert the bot‘s username and the specified inline query in the input field. 
        /// Can be empty, in which case just the bot’s username will be inserted.
        /// </summary>
        public string SwitchInlineQuery { get; set; }

        /// <summary>
        /// Optional. If set, pressing the button will insert the bot‘s username and the specified inline query in the current chat's input field. 
        /// Can be empty, in which case only the bot’s username will be inserted.
        /// </summary>
        public string SwitchInlineQueryCurrentChat { get; set; }

        /// <summary>
        /// Optional. Description of the game that will be launched when the user presses the button.
        /// </summary>
        /// <remarks>
        /// This type of button must always be the first button in the first row.
        /// </remarks>
        public CallbackGame CallbackGame { get; set; }
    }

    /// <summary>
    /// Represents an incoming callback query from a callback button in an inline keyboard. 
    /// If the button that originated the query was attached to a message sent by the bot, the field message will be present. 
    /// If the button was attached to a message sent via the bot (in inline mode), the property <see cref="InlineMessageId"/> will be present. 
    /// Exactly one of the properties <see cref="Data"/> or <see cref="GameShortName"/>  will be present.
    /// </summary>
    public class CallbackQuery
    {
        /// <summary>
        /// Unique identifier for this query.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Sender.
        /// </summary>
        public User From { get; set; }

        /// <summary>
        /// Optional. Message with the callback button that originated the query.
        /// </summary>
        /// <remarks>
        /// Note that message content and message date will not be available if the message is too old.
        /// </remarks>
        public Message Message { get; set; }

        /// <summary>
        /// Optional. Identifier of the message sent via the bot in inline mode, that originated the query.
        /// </summary>
        public string InlineMessageId { get; set; }

        /// <summary>
        /// Global identifier, uniquely corresponding to the chat to which the message with the callback button was sent. Useful for high scores in games.
        /// </summary>
        public string ChatInstance { get; set; }

        /// <summary>
        /// Optional. Data associated with the callback button. 
        /// </summary>
        /// <remarks>
        /// Be aware that a bad client can send arbitrary data in this field.
        /// </remarks>
        public string Data { get; set; }

        /// <summary>
        /// Optional. Short name of a Game to be returned, serves as the unique identifier for the game.
        /// </summary>
        public string GameShortName { get; set; }
    }

    public class CallbackGame
    {
        
    }
}
