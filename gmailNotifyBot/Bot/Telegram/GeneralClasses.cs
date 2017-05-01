using System;
using System.Collections.Generic;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace CoffeeJelly.gmailNotifyBot.Bot.Telegram
{
    /// <summary>
    /// Represents a Telegram user or bot.
    /// </summary>
    public class User
    {
        public static implicit operator string(User obj)
        {
            return obj.Id.ToString();
        }

        /// <summary>
        /// Unique identifier for this user or bot.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// User's or bot's first name.
        /// </summary>
        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        /// <summary>
        /// User‘s or bot’s last name
        /// </summary>
        [JsonProperty("last_name")]
        public string LastName { get; set; }

        /// <summary>
        /// User‘s or bot’s username.
        /// </summary>
        [JsonProperty("username")]
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
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// Type of chat, can be either “private”, “group”, “supergroup” or “channel”.
        /// </summary>
        [JsonProperty("type")]
        public ChatType Type { get; set; }

        /// <summary>
        /// Title, for supergroups, channels and group chats.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Username, for private chats, supergroups and channels if available.
        /// </summary>
        [JsonProperty("username")]
        public string Username { get; set; }

        /// <summary>
        /// First name of the other party in a private chat.
        /// </summary>
        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        /// <summary>
        /// Last name of the other party in a private chat.
        /// </summary>
        [JsonProperty("last_name")]
        public string LastName { get; set; }

        /// <summary>
        /// <see langword="true"/>: if a group has ‘All Members Are Admins’ enabled.
        /// </summary>
        [JsonProperty("all_members_are_administrators")]
        public bool? AllMembersAreAdministrators { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ChatType
    {
        [EnumMember(Value = "private")]
        Private,
        [EnumMember(Value = "group")]
        Group,
        [EnumMember(Value = "supergroup")]
        Supergroup,
        [EnumMember(Value = "channel")]
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
        [JsonProperty("type")]
        public MessageEntityType Type { get; set; }

        /// <summary>
        /// Offset in UTF-16 code units to the start of the entity.
        /// </summary>
        [JsonProperty("offset")]
        public int Offset { get; set; }

        /// <summary>
        /// Length of the entity in UTF-16 code units.
        /// </summary>
        [JsonProperty("length")]
        public int Length { get; set; }

        /// <summary>
        /// For “text_link” only, url that will be opened after user taps on the text.
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// For “text_mention” only, the mentioned user.
        /// </summary>
        [JsonProperty("user")]
        public User User { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum MessageEntityType
    {
        [EnumMember(Value = "mention")]
        Mention,
        [EnumMember(Value = "hashtag")]
        Hashtag,
        [EnumMember(Value = "bot_command")]
        BotCommand,
        [EnumMember(Value = "url")]
        Url,
        [EnumMember(Value = "email")]
        Email,
        [EnumMember(Value = "bold")]
        Bold,
        [EnumMember(Value = "italic")]
        Italic,
        [EnumMember(Value = "code")]
        Code,
        [EnumMember(Value = "pre")]
        Pre,
        [EnumMember(Value = "text_link")]
        TextLink,
        [EnumMember(Value = "text_mention")]
        TextMention
    }

    /// <summary>
    /// Represents one size of a photo or a file / sticker thumbnail.
    /// </summary>
    public class PhotoSize : IFile, IFileImageSize
    {
        /// <summary>
        /// Unique identifier for this file.
        /// </summary>
        [JsonProperty("file_id")]
        public string FileId { get; set; }

        /// <summary>
        /// Photo width.
        /// </summary>
        [JsonProperty("width")]
        public int Width { get; set; }

        /// <summary>
        /// Photo height.
        /// </summary>
        [JsonProperty("height")]
        public int Height { get; set; }

        /// <summary>
        /// File size.
        /// </summary>
        [JsonProperty("file_size")]
        public int? FileSize { get; set; }
    }

    /// <summary>
    ///  Represents an audio file to be treated as music by the Telegram clients.
    /// </summary>
    public class Audio : IFile, IFileDuration, IFileMimeType
    {
        /// <summary>
        /// Unique identifier for this file.
        /// </summary>
        [JsonProperty("file_id")]
        public string FileId { get; set; }

        /// <summary>
        /// Duration of the audio in seconds as defined by sender.
        /// </summary>
        [JsonProperty("duration")]
        public int Duration { get; set; }

        /// <summary>
        /// Performer of the audio as defined by sender or by audio tags.
        /// </summary>
        [JsonProperty("performer")]
        public string Performer { get; set; }

        /// <summary>
        /// Title of the audio as defined by sender or by audio tags.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// MIME type of the file as defined by sender.
        /// </summary>
        [JsonProperty("mime_type")]
        public string MimeType { get; set; }

        /// <summary>
        /// File size.
        /// </summary>
        [JsonProperty("file_size")]
        public int? FileSize { get; set; }
    }

    /// <summary>
    /// Represents a general file (as opposed to photos, voice messages and audio files).
    /// </summary>
    public class Document : IFile, IFileThumb, IFileName,
                            IFileMimeType
    {
        /// <summary>
        /// Unique file identifier.
        /// </summary>
        [JsonProperty("file_id")]
        public string FileId { get; set; }

        /// <summary>
        /// Document thumbnail as defined by sender.
        /// </summary>
        [JsonProperty("thumb")]
        public PhotoSize Thumb { get; set; }

        /// <summary>
        /// Original filename as defined by sender.
        /// </summary>
        [JsonProperty("file_name")]
        public string FileName { get; set; }

        /// <summary>
        /// MIME type of the file as defined by sender.
        /// </summary>
        [JsonProperty("mime_type")]
        public string MimeType { get; set; }

        /// <summary>
        /// File size.
        /// </summary>
        [JsonProperty("file_size")]
        public int? FileSize { get; set; }
    }

    /// <summary>
    /// Represents a sticker.
    /// </summary>
    public class Sticker : IFile, IFileImageSize, IFileThumb
    {
        /// <summary>
        /// Unique file identifier.
        /// </summary>
        [JsonProperty("file_id")]
        public string FileId { get; set; }

        /// <summary>
        /// Sticker width.
        /// </summary>
        [JsonProperty("width")]
        public int Width { get; set; }

        /// <summary>
        /// Sticker height.
        /// </summary>
        [JsonProperty("height")]
        public int Height { get; set; }

        /// <summary>
        /// Sticker thumbnail in .webp or .jpg format.
        /// </summary>
        [JsonProperty("thumb")]
        public PhotoSize Thumb { get; set; }

        /// <summary>
        /// Emoji associated with the sticker.
        /// </summary>
        [JsonProperty("emoji")]
        public string Emoji { get; set; }

        /// <summary>
        /// File size.
        /// </summary>
        [JsonProperty("file_size")]
        public int? FileSize { get; set; }
    }

    /// <summary>
    /// Represents a video file.
    /// </summary>
    public class Video : IFile, IFileImageSize, IFileDuration,
                            IFileThumb, IFileMimeType
    {
        /// <summary>
        /// Unique identifier for this file.
        /// </summary>
        [JsonProperty("file_id")]
        public string FileId { get; set; }

        /// <summary>
        /// Video width as defined by sender.
        /// </summary>
        [JsonProperty("width")]
        public int Width { get; set; }

        /// <summary>
        /// Video height as defined by sender
        /// </summary>
        [JsonProperty("height")]
        public int Height { get; set; }

        /// <summary>
        /// Duration of the video in seconds as defined by sender.
        /// </summary>
        [JsonProperty("duration")]
        public int Duration { get; set; }

        /// <summary>
        /// Video thumbnail.
        /// </summary>
        [JsonProperty("thumb")]
        public PhotoSize Thumb { get; set; }

        /// <summary>
        /// Mime type of a file as defined by sender.
        /// </summary>
        [JsonProperty("mime_type")]
        public string MimeType { get; set; }

        /// <summary>
        /// File size.
        /// </summary>
        [JsonProperty("file_size")]
        public int? FileSize { get; set; }
    }

    /// <summary>
    /// Represents a voice note.
    /// </summary>
    public class Voice : IFile, IFileDuration, IFileMimeType
    {
        /// <summary>
        /// Unique identifier for this file
        /// </summary>
        [JsonProperty("file_id")]
        public string FileId { get; set; }

        /// <summary>
        /// Duration of the audio in seconds as defined by sender.
        /// </summary>
        [JsonProperty("duration")]
        public int Duration { get; set; }

        /// <summary>
        /// MIME type of the file as defined by sender.
        /// </summary>
        [JsonProperty("mime_type")]
        public string MimeType { get; set; }

        /// <summary>
        /// File size.
        /// </summary>
        [JsonProperty("file_size")]
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
        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Contact's first name.
        /// </summary>
        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        /// <summary>
        /// Contact's last name.
        /// </summary>
        [JsonProperty("last_name")]
        public string LastName { get; set; }

        /// <summary>
        /// Contact's user identifier in Telegram.
        /// </summary>
        [JsonProperty("user_id")]
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
        [JsonProperty("longitude")]
        public float Longitude { get; set; }

        /// <summary>
        /// Latitude as defined by sender.
        /// </summary>
        [JsonProperty("latitude")]
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
        [JsonProperty("location")]
        public Location Location { get; set; }

        /// <summary>
        /// Name of the venue.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Address of the venue.
        /// </summary>
        [JsonProperty("address")]
        public string Address { get; set; }

        /// <summary>
        /// Foursquare identifier of the venue.
        /// </summary>
        [JsonProperty("foursquare_id")]
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
        [JsonProperty("total_count")]
        public int TotalCount { get; set; }

        /// <summary>
        /// Requested profile pictures (in up to 4 sizes each).
        /// </summary>
        [JsonConverter(typeof(ArrayToListConverter<List<PhotoSize>>))]
        [JsonProperty("photos")]
        public List<List<PhotoSize>> Photos { get; set; }
    }

    /// <summary>
    /// Represents a game.
    /// </summary>
    public class Game
    {
        /// <summary>
        /// Title of the game.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Description of the game.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Photo that will be displayed in the game message in chats.
        /// </summary>
        [JsonConverter(typeof(ArrayToListConverter<PhotoSize>))]
        [JsonProperty("photo")]
        public List<PhotoSize> Photo { get; set; }

        /// <summary>
        /// Brief description of the game or high scores included in the game message. 
        /// Can be automatically edited to include current high scores for the game when the bot calls setGameScore, 
        /// or manually edited using editMessageText. 0-4096 characters.
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        /// Special entities that appear in text, such as usernames, URLs, bot commands, etc.
        /// </summary>
        [JsonConverter(typeof(ArrayToListConverter<MessageEntity>))]
        [JsonProperty("text_entities")]
        public List<MessageEntity> TextEntities { get; set; }

        /// <summary>
        /// Animation that will be displayed in the game message in chats.
        /// </summary>
        [JsonProperty("animation")]
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
        [JsonProperty("file_id")]
        public string FileId { get; set; }

        /// <summary>
        /// Animation thumbnail as defined by sender.
        /// </summary>
        [JsonProperty("thumb")]
        public PhotoSize Thumb { get; set; }

        /// <summary>
        /// Original animation filename as defined by sender.
        /// </summary>
        [JsonProperty("file_name")]
        public string FileName { get; set; }

        /// <summary>
        /// MIME type of the file as defined by sender.
        /// </summary>
        [JsonProperty("mime_type")]
        public string MimeType { get; set; }

        /// <summary>
        /// File size.
        /// </summary>
        [JsonProperty("file_size")]
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
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        /// Optional. If <see langword="true"/>, the user's phone number will be sent as a contact when the button is pressed. 
        /// Available in private chats only
        /// </summary>
        [JsonProperty("request_contact")]
        public bool? RequestContact { get; set; }

        /// <summary>
        /// Optional. If <see langword="true"/>, the user's current location will be sent when the button is pressed. 
        /// Available in private chats only.
        /// </summary>
        [JsonProperty("request_location")]
        public bool? RequestLocation { get; set; }
    }

    /// <summary>
    /// Represents a custom keyboard with reply options.
    /// </summary>
    public class ReplyKeyboardMarkup : IMarkup
    {
        /// <summary>
        /// List of button rows, each represented by an List of KeyboardButton objects
        /// </summary>
        [JsonConverter(typeof(ArrayToListConverter<List<KeyboardButton>>))]
        [JsonProperty("keyboard")]
        public List<List<KeyboardButton>> Keyboard { get; set; }

        /// <summary>
        /// Requests clients to resize the keyboard vertically for optimal fit (e.g., make the keyboard smaller if there are just two rows of buttons). 
        /// </summary>
        /// <remarks>
        /// Defaults to <see langword="false"/>, in which case the custom keyboard is always of the same height as the app's standard keyboard.
        /// </remarks>
        [JsonProperty("resize_keyboars")]
        public bool? ResizeKeyboard { get; set; } = false;

        /// <summary>
        /// Requests clients to hide the keyboard as soon as it's been used. 
        /// The keyboard will still be available, but clients will automatically display the usual letter-keyboard in the chat – 
        /// the user can press a special button in the input field to see the custom keyboard again. 
        /// </summary>
        /// <remarks>Defaults to <see langword="false"/>.</remarks>
        [JsonProperty("one_time_keyboard")]
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
        [JsonProperty("selective")]
        public bool? Selective { get; set; }
    }

    /// <summary>
    /// Upon receiving a message with this object, Telegram clients will remove the current custom keyboard and display the default letter-keyboard. 
    /// By default, custom keyboards are displayed until a new keyboard is sent by a bot. 
    /// An exception is made for one-time keyboards that are hidden immediately after the user presses a button (see <see cref="ReplyKeyboardMarkup"/>).
    /// </summary>
    public class ReplyKeyboardRemove : IMarkup
    {
        /// <summary>
        /// Requests clients to remove the custom keyboard (user will not be able to summon this keyboard; 
        /// if you want to hide the keyboard from sight but keep it accessible, use <see cref="ReplyKeyboardMarkup.OneTimeKeyboard"/>)
        /// </summary>
        [JsonProperty("remove_keyboard")]
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
        [JsonProperty("selective")]
        public bool? Selective { get; set; }
    }

    /// <summary>
    /// Represents an inline keyboard that appears right next to the message it belongs to.
    /// </summary>
    public class InlineKeyboardMarkup : IMarkup
    {
        /// <summary>
        /// Array of button rows, each represented by an Array of InlineKeyboardButton objects.
        /// </summary>
        [JsonConverter(typeof(ArrayToListConverter<List<InlineKeyboardButton>>))]
        [JsonProperty("inline_keyboard")]
        public List<List<InlineKeyboardButton>> InlineKeyboard { get; set; }
    }

    /// <summary>
    /// Represents one button of an inline keyboard. You must use exactly one of the optional fields.
    /// </summary>
    public class InlineKeyboardButton
    {
        /// <summary>
        /// Label text on the button.
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        /// Optional. HTTP url to be opened when button is pressed.
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// Optional. Data to be sent in a <see cref="CallbackQuery"/> to the bot when button is pressed, 1-64 bytes.
        /// </summary>
        [JsonProperty("callback_data")]
        public string CallbackData { get; set; }

        /// <summary>
        /// Optional. If set, pressing the button will prompt the user to select one of their chats, open that chat and insert the bot‘s username and the specified inline query in the input field. 
        /// Can be empty, in which case just the bot’s username will be inserted.
        /// </summary>
        [JsonProperty("switch_inline_query")]
        public string SwitchInlineQuery { get; set; }

        /// <summary>
        /// Optional. If set, pressing the button will insert the bot‘s username and the specified inline query in the current chat's input field. 
        /// Can be empty, in which case only the bot’s username will be inserted.
        /// </summary>
        [JsonProperty("switch_inline_query_current_chat")]
        public string SwitchInlineQueryCurrentChat { get; set; }

        /// <summary>
        /// Optional. Description of the game that will be launched when the user presses the button.
        /// </summary>
        /// <remarks>
        /// This type of button must always be the first button in the first row.
        /// </remarks>
        [JsonProperty("callback_game")]
        public CallbackGame CallbackGame { get; set; }
    }

    /// <summary>
    /// Represents an incoming callback query from a callback button in an inline keyboard. 
    /// If the button that originated the query was attached to a message sent by the bot, the field message will be present. 
    /// If the button was attached to a message sent via the bot (in inline mode), the property <see cref="InlineMessageId"/> will be present. 
    /// Exactly one of the properties <see cref="Data"/> or <see cref="GameShortName"/>  will be present.
    /// </summary>
    public class CallbackQuery : ISender
    {
        /// <summary>
        /// Unique identifier for this query.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Sender.
        /// </summary>
        [JsonProperty("from")]
        public User From { get; set; }

        /// <summary>
        /// Optional. Message with the callback button that originated the query.
        /// </summary>
        /// <remarks>
        /// Note that message content and message date will not be available if the message is too old.
        /// </remarks>
        [JsonConverter(typeof(MessageConverter))]
        [JsonProperty("message")]
        public Message Message { get; set; }

        /// <summary>
        /// Optional. Identifier of the message sent via the bot in inline mode, that originated the query.
        /// </summary>
        [JsonProperty("inline_message_id")]
        public string InlineMessageId { get; set; }

        /// <summary>
        /// Global identifier, uniquely corresponding to the chat to which the message with the callback button was sent. Useful for high scores in games.
        /// </summary>
        [JsonProperty("chat_instance")]
        public string ChatInstance { get; set; }

        /// <summary>
        /// Optional. Data associated with the callback button. 
        /// </summary>
        /// <remarks>
        /// Be aware that a bad client can send arbitrary data in this field.
        /// </remarks>
        [JsonProperty("data")]
        public string Data { get; set; }

        /// <summary>
        /// Optional. Short name of a Game to be returned, serves as the unique identifier for the game.
        /// </summary>
        [JsonProperty("game_short_namae")]
        public string GameShortName { get; set; }
    }

    public class CallbackGame
    {

    }

    /// <summary>
    /// Upon receiving a message with this object, Telegram clients will display a reply interface to the user (act as if the user has selected the bot‘s message and tapped ’Reply'). 
    /// This can be extremely useful if you want to create user-friendly step-by-step interfaces without having to sacrifice privacy mode.
    /// </summary>
    public class ForceReply : IMarkup
    {
        /// <summary>
        /// Shows reply interface to the user, as if they manually selected the bot‘s message and tapped ’Reply'
        /// </summary>
        [JsonProperty("force_reply")]
        public bool ForseReply { get; } = true;

        /// <summary>
        /// Optional. Use this parameter if you want to show the keyboard to specific users only. 
        /// Targets: 1) users that are @mentioned in the <see cref="TextMessage.Text"/>; 
        /// 2) if the bot's message is a reply (has ReplyToMessageId), sender of the original message.
        /// </summary>
        /// <example>
        /// A user votes in a poll, bot returns confirmation message in reply to the vote and removes the keyboard for that user, 
        /// while still showing the keyboard with poll options to users who haven't voted yet.
        /// </example>
        [JsonProperty("selective")]
        public bool? Selective { get; set; }
    }

    /// <summary>
    /// Represents a file ready to be downloaded. 
    /// </summary>
    /// <remarks>
    /// The file can be downloaded via the link https://api.telegram.org/file/bot&lt;token&gt;/&lt;file_path&gt;
    /// or by method <see cref="TelegramMethods.DownloadFileAsync(File, string)"/>. 
    /// It is guaranteed that the link will be valid for at least 1 hour. 
    /// When the link expires, a new one can be requested by calling <see cref="TelegramMethods.GetFile(string)"/>.
    /// </remarks>
    public class File : IFile
    {
        /// <summary>
        /// Unique identifier for this file
        /// </summary>
        [JsonProperty("file_id")]
        public string FileId { get; set; }

        /// <summary>
        /// Optional. File size.
        /// </summary>
        [JsonProperty("file_size")]
        public int? FileSize { get; set; }

        /// <summary>
        /// Optional. File path.
        /// </summary>
        /// <remarks>
        /// Use https://api.telegram.org/file/bot&lt;token&gt;/&lt;file_path&gt; to get the file
        /// or method <see cref="TelegramMethods.DownloadFileAsync(File, string)"/>.
        /// </remarks>
        [JsonProperty("file_path")]
        public string FilePath
        {
            get { return _filePath; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _filePath = value;
                    FilePathCreated = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// The date when <see cref="FilePath"/> created on Telegeram's servers by <see cref="TelegramMethods.GetFile(string)"/> method.
        /// </summary>
        [JsonIgnore]
        public DateTime FilePathCreated { get; private set; }

        private string _filePath;
    }

    /// <summary>
    /// Contains information about one member of the chat.
    /// </summary>
    public class ChatMember
    {
        /// <summary>
        /// Information about the user.
        /// </summary>
        [JsonProperty("user")]
        public User User { get; set; }

        /// <summary>
        /// The member's status in the chat.
        /// </summary>
        [JsonProperty("status")]
        public ChatMemberStatus Status { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ChatMemberStatus
    {
        [EnumMember(Value = "creator")]
        Creator,
        [EnumMember(Value = "administrator")]
        Administrator,
        [EnumMember(Value = "member")]
        Member,
        [EnumMember(Value = "left")]
        Left,
        [EnumMember(Value = "kicked")]
        Kicked
    }

    /// <summary>
    /// Contains information about why a request was unsuccessfull.
    /// </summary>
    public class ResponceParameters
    {
        /// <summary>
        /// Optional. The group has been migrated to a supergroup with the specified identifier.
        /// </summary>
        [JsonProperty("migrate_to_chat_id")]
        public long? MigrateToChatId { get; set; }

        /// <summary>
        /// Optional. In case of exceeding flood control, the number of seconds left to wait before the request can be repeated.
        /// </summary>
        [JsonProperty("retry_after")]
        public int? RetryAfter { get; set; }
    }


    /// <summary>
    /// Represents an incoming update.
    /// </summary>
    public class Update
    {
        /// <summary>
        /// The update‘s unique identifier.
        /// </summary>
        /// <remarks>
        /// Update identifiers start from a certain positive number and increase sequentially. 
        /// This ID becomes especially handy if you’re using Webhooks, since it allows you to ignore repeated updates or to restore the correct update sequence, 
        /// should they get out of order.
        /// </remarks>
        [JsonProperty("update_id")]
        public long UpdateId { get; set; }

        /// <summary>
        /// New incoming message of any kind — <see cref="TextMessage"/>, <see cref="PhotoMessage"/>, <see cref="StickerMessage"/>, etc.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("message")]
        [JsonConverter(typeof(MessageConverter))]
        public Message Message { get; set; }

        /// <summary>
        /// New version of a message that is known to the bot and was edited.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("edited_message")]
        [JsonConverter(typeof(MessageConverter))]
        public Message EditedMessage { get; set; }

        /// <summary>
        /// New incoming channel post of any kind — <see cref="TextMessage"/>, <see cref="PhotoMessage"/>, <see cref="StickerMessage"/>, etc.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("channel_post")]
        [JsonConverter(typeof(MessageConverter))]
        public Message ChannelPost { get; set; }

        /// <summary>
        /// New version of a channel post that is known to the bot and was edited.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("edited_channel_post")]
        [JsonConverter(typeof(MessageConverter))]
        public Message EditedChannelPost { get; set; }

        /// <summary>
        /// New incoming inline query.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("inline_query")]
        public InlineQuery InlineQuery { get; set; }

        /// <summary>
        /// The result of an inline query that was chosen by a user and sent to their chat partner.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("chosen_inline_result")]
        public ChosenInlineResult ChosenInlineResult { get; set; }

        /// <summary>
        /// New incoming callback query.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("callback_query")]
        public CallbackQuery CallbackQuery { get; set; }
    }
}
