using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeJelly.Bot.Bot.Telegram
{
    /// <summary>
    /// Interface for Telegram's messages.
    /// </summary>
    public interface ITelegramMessage
    {
        /// <summary>
        /// Unique message identifier inside this chat.
        /// </summary>
        int MessageId { get; set; }

        /// <summary>
        /// Date the message was sent in Unix time.
        /// </summary>
        DateTime Date { get; set; }

        /// <summary>
        /// Conversation the message belongs to.
        /// </summary>
        Chat Chat { get; set; }
 

    }

    /// <summary>
    /// Interface for Telegram's user messages.
    /// </summary>
    public interface IUserMessage
    {
        /// <summary>
        /// Sender.
        /// </summary>
        User From { get; set; }

    }

    /// <summary>
    /// Interface for Telegram's text messages.
    /// </summary>
    public interface ITelegramTextMessage
    {
        /// <summary>
        /// The actual UTF-8 text of the message, 0-4096 characters.
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Special entities like usernames, URLs, bot commands, etc. that appear in the text.
        /// </summary>
        List<MessageEntity> Entities { get; set; }

    }

    /// <summary>
    /// Interface for Telegram's audio messages.
    /// </summary>
    public interface ITelegramAudioMessage
    {
        /// <summary>
        /// Information about the file
        /// </summary>
        Audio Audio { get; set; }
    }

    /// <summary>
    /// Interface for Telegram's general file message.
    /// </summary>
    public interface ITelegramDocumentMessage
    {
        /// <summary>
        /// Information about general file.
        /// </summary>
        Document Document { get; set; }

        /// <summary>
        /// Caption for the document, 0-200 characters.
        /// </summary>
        string Caption { get; set; }
    }

    /// <summary>
    /// Interface for Telegram's game message.
    /// </summary>
    //public interface ITelegramGameMessage
    //{
    //    /// <summary>
    //    /// Information about the game.
    //    /// </summary>
    //    Game Game { get; set; }
    //}

    /// <summary>
    /// Interface for Telegram's photo message.
    /// </summary>
    public interface ITelegramPhotoMessage
    {
        /// <summary>
        /// Available sizes of the photo.
        /// </summary>
        List<PhotoSize> Photo { get; set; }

        /// <summary>
        /// Caption for the photo, 0-200 characters.
        /// </summary>
        string Caption { get; set; }
    }

    /// <summary>
    /// Interface for Telegram's sticker message.
    /// </summary>
    public interface ITelegramStickerMessage
    {
        /// <summary>
        /// information about the sticker.
        /// </summary>
        Sticker Sticker { get; set; }
    }

    /// <summary>
    /// Interface for Telegram's video message.
    /// </summary>
    public interface ITelegramVideoMessage
    {
        /// <summary>
        /// Information about the video.
        /// </summary>
        Video Video { get; set; }

        /// <summary>
        /// Caption for the video, 0-200 characters.
        /// </summary>
        string Caption { get; set; }
    }

    /// <summary>
    /// Interface for Telegram's voice message.
    /// </summary>
    public interface ITelegramVoiceMessage
    {
        /// <summary>
        /// Information about the voice.
        /// </summary>
        Voice Voice { get; set; }
    }

    /// <summary>
    /// Interface for Telegram's shared contact message.
    /// </summary>
    public interface ITelegramContactMessage
    {
        /// <summary>
        /// Information about the contact.
        /// </summary>
        Contact Contact { get; set; }
    }

    /// <summary>
    /// Interface for Telegram's shared location message.
    /// </summary>
    public interface ITelegramLocationMessage
    {
        /// <summary>
        /// Information about the location.
        /// </summary>
        Location Location { get; set; }
    }

    /// <summary>
    /// Interface for Telegram's venue message.
    /// </summary>
    public interface ITelegramVenueMessage
    {
        /// <summary>
        /// Information about the venue.
        /// </summary>
        Venue Venue { get; set; }
    }

    /// <summary>
    /// Interface for Telegram's file message of a file id.
    /// </summary>
    public interface ITelegramMessageFileId
    {
        /// <summary>
        /// Interface for the Unique identifier of the Telegram message file
        /// </summary>
        string FileId { get; set; }
    }

    /// <summary>
    /// Interface for the image size of the Telegram's message file (photo, sticker, video)
    /// </summary>
    public interface ITelegramMessageFileImageSize
    {
        /// <summary>
        /// File widht.
        /// </summary>
        int Width { get; set; }

        /// <summary>
        /// File height.
        /// </summary>
        int Height { get; set; }
    }

    /// <summary>
    /// Interface for the duration of the Telegram's message file
    /// </summary>
    public interface ITelegramMessageFileDuration
    {
        /// <summary>
        /// Duration of the file in seconds as defined by sender
        /// </summary>
        int Duration { get; set; }
    }

    /// <summary>
    /// Interface for the size of the Telegram's message file
    /// </summary>
    public interface ITelegramMessageFileSize
    {
        /// <summary>
        /// File size.
        /// </summary>
        int FileSize { get; set; }
    }

    /// <summary>
    /// Interface for the MIME type of the Telegram's message file.
    /// </summary>
    public interface ITelegramMessageFileMimeType
    {
        /// <summary>
        /// MIME type as defined by sender.
        /// </summary>
        string MimeType { get; set; }
    }

    /// <summary>
    /// Interface for the title of the Telegram's message file.
    /// </summary>
    public interface ITelegramMessageFileTitle
    {
        /// <summary>
        /// Title of the file as defined by sender or by audio tags.
        /// </summary>
        string Title { get; set; }
    }

    /// <summary>
    /// Interface for the thumbnail of the Telegram's message file.
    /// </summary>
    public interface ITelegramMessageFileThumb
    {
        /// <summary>
        /// File thumbnail as defined by sender.
        /// </summary>
        PhotoSize Thumb { get; set; }
    }

    /// <summary>
    /// Interface for the file name of the Telegram's message file.
    /// </summary>
    public interface ITelegramFileName
    {
        /// <summary>
        /// Original filename as defined by sender.
        /// </summary>
        string FileName { get; set; }
    }


}
