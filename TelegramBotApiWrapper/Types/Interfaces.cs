using System;
using System.Collections.Generic;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Types
{
    /// <summary>
    /// Interface for Telegram's messages.
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// Unique message identifier inside this chat.
        /// </summary>
        [JsonProperty("message_id")]
        int MessageId { get; set; }

        /// <summary>
        /// Date the message was sent in Unix time.
        /// </summary>
        [JsonProperty("date")]
        DateTime Date { get; set; }

        /// <summary>
        /// Conversation the message belongs to.
        /// </summary>
        [JsonProperty("chat")]
        Chat Chat { get; set; }
    }

    /// <summary>
    /// Interface for Telegram's user messages.
    /// </summary>
    public interface ISender
    {
        /// <summary>
        /// Sender.
        /// </summary>
        [JsonProperty("from")]
        User From { get; set; }

    }

    /// <summary>
    /// Interface for Telegram's text messages.
    /// </summary>
    public interface ITextMessage
    {
        /// <summary>
        /// The actual UTF-8 text of the message.
        /// </summary>
        [JsonProperty("text")]
        string Text { get; set; }

        /// <summary>
        /// Special entities like usernames, URLs, bot commands, etc. that appear in the text.
        /// </summary>
        [JsonProperty("entities")]
        IEnumerable<MessageEntity> Entities { get; set; }

    }

    /// <summary>
    /// Interface for Telegram's audio messages.
    /// </summary>
    public interface IAudioMessage
    {
        /// <summary>
        /// Information about the audio file.
        /// </summary>
        [JsonProperty("audio")]
        Audio Audio { get; set; }
    }

    /// <summary>
    /// Interface for Telegram's general file message.
    /// </summary>
    public interface IDocumentMessage
    {
        /// <summary>
        /// Information about general file.
        /// </summary>
        [JsonProperty("document")]
        Document Document { get; set; }

        /// <summary>
        /// Caption for the document.
        /// </summary>
        [JsonProperty("caption")]
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
    public interface IPhotoMessage
    {
        /// <summary>
        /// Available sizes of the photo.
        /// </summary>
        [JsonProperty("photo")]
        IEnumerable<PhotoSize> Photo { get; set; }

        /// <summary>
        /// Caption for the photo.
        /// </summary>
        [JsonProperty("caption")]
        string Caption { get; set; }
    }

    /// <summary>
    /// Interface for Telegram's sticker message.
    /// </summary>
    public interface IStickerMessage
    {
        /// <summary>
        /// information about the sticker.
        /// </summary>
        [JsonProperty("sticker")]
        Sticker Sticker { get; set; }
    }

    /// <summary>
    /// Interface for Telegram's video message.
    /// </summary>
    public interface IVideoMessage
    {
        /// <summary>
        /// Information about the video.
        /// </summary>
        [JsonProperty("video")]
        Video Video { get; set; }

        /// <summary>
        /// Caption for the video.
        /// </summary>
        [JsonProperty("caption")]
        string Caption { get; set; }
    }

    /// <summary>
    /// Interface for Telegram's voice message.
    /// </summary>
    public interface IVoiceMessage
    {
        /// <summary>
        /// Information about the voice.
        /// </summary>
        [JsonProperty("voice")]
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
        [JsonProperty("contact")]
        Contact Contact { get; set; }
    }

    /// <summary>
    /// Interface for Telegram's shared location message.
    /// </summary>
    public interface ILocationMessage
    {
        /// <summary>
        /// Information about the location.
        /// </summary>
        [JsonProperty("location")]
        Location Location { get; set; }
    }

    /// <summary>
    /// Interface for Telegram's venue message.
    /// </summary>
    public interface IVenueMessage
    {
        /// <summary>
        /// Information about the venue.
        /// </summary>
        [JsonProperty("venue")]
        Venue Venue { get; set; }
    }

    /// <summary>
    /// Interface for Telegram's game message.
    /// </summary>
    public interface IGameMessage
    {
        /// <summary>
        /// Information about the game.
        /// </summary>
        [JsonProperty("game")]
        Game Game { get; set; }
    }

    /// <summary>
    /// Interface for Telegram's file message of a file id.
    /// </summary>
    public interface IFile
    {
        /// <summary>
        /// Interface for the Unique identifier of the Telegram message file
        /// </summary>
        [JsonProperty("file_id")]
        string FileId { get; set; }

        /// <summary>
        /// File size.
        /// </summary>
        [JsonProperty("file_size")]
        int? FileSize { get; set; }
    }

    /// <summary>
    /// Interface for the image size of the Telegram's message file (photo, sticker, video)
    /// </summary>
    public interface IFileImageSize
    {
        /// <summary>
        /// File widht.
        /// </summary>
        [JsonProperty("width")]
        int Width { get; set; }

        /// <summary>
        /// File height.
        /// </summary>
        [JsonProperty("height")]
        int Height { get; set; }
    }

    /// <summary>
    /// Interface for the duration of the Telegram's message file
    /// </summary>
    public interface IFileDuration
    {
        /// <summary>
        /// Duration of the file in seconds as defined by sender
        /// </summary>
        [JsonProperty("duration")]
        int Duration { get; set; }
    }

    /// <summary>
    /// Interface for the MIME type of the Telegram's message file.
    /// </summary>
    public interface IFileMimeType
    {
        /// <summary>
        /// MIME type as defined by sender.
        /// </summary>
        [JsonProperty("mime_type")]
        string MimeType { get; set; }
    }

    /// <summary>
    /// Interface for the title of the Telegram's message file.
    /// </summary>
    public interface IFileTitle
    {
        /// <summary>
        /// Title of the file as defined by sender or by audio tags.
        /// </summary>
        [JsonProperty("title")]
        string Title { get; set; }
    }

    /// <summary>
    /// Interface for the thumbnail of the Telegram's message file.
    /// </summary>
    public interface IFileThumb
    {
        /// <summary>
        /// File thumbnail as defined by sender.
        /// </summary>
        [JsonProperty("thumb")]
        PhotoSize Thumb { get; set; }
    }

    /// <summary>
    /// Interface for the file name of the Telegram's message file.
    /// </summary>
    public interface IFileName
    {
        /// <summary>
        /// Original filename as defined by sender.
        /// </summary>
        [JsonProperty("file_name")]
        string FileName { get; set; }
    }

    public interface IMarkup
    {
        
    }

    /// <summary>
    /// Interface represents the thumbnail parameters.
    /// </summary>
    public interface IResultThumb
    {
        /// <summary>
        /// Url of the thumbnail for the result.
        /// </summary>
        [JsonProperty("thumb_url")]
        string ThumbUrl { get; set; }

        /// <summary>
        /// Thumbnail width.
        /// </summary>
        [JsonProperty("thumb_width")]
        int? ThumbWidth { get; set; }

        /// <summary>
        /// Thumbnail height.
        /// </summary>
        [JsonProperty("thumb_height")]
        int? ThumbHeight { get; set; }
    }

    public interface IInputMessageContent
    {

    }

    /// <summary>
    /// Interface represents the photo file parameters.
    /// </summary>
    public interface IResultPhoto
    {
        /// <summary>
        /// A valid URL of the photo.
        /// </summary>
        [JsonProperty("photo_url")]
        string PhotoUrl { get; set; }

        /// <summary>
        /// Width of the photo.
        /// </summary>
        [JsonProperty("photo_width")]
        int? PhotoWidth { get; set; }

        /// <summary>
        /// Height of the photo.
        /// </summary>
        [JsonProperty("photo_height")]
        int? PhotoHeight { get; set; }
    }

    /// <summary>
    /// Interface represents the gif file parameters.
    /// </summary>
    public interface IResultGif
    {
        /// <summary>
        /// A valid URL for the GIF file. File size must not exceed 1MB.
        /// </summary>
        [JsonProperty("gif_url")]
        string GifUrl { get; set; }

        /// <summary>
        /// Width of the GIF.
        /// </summary>
        [JsonProperty("gif_width")]
        int? GifWidth { get; set; }

        /// <summary>
        /// Height of the GIF.
        /// </summary>
        [JsonProperty("gif_height")]
        int? GifHeight { get; set; }
    }

    /// <summary>
    /// Interface for the title of the Telegram's InlineQueryResult file.
    /// </summary>
    public interface IResultTitle
    {
        /// <summary>
        /// Title for the result.
        /// </summary>
        [JsonProperty("title")]
        string Title { get; set; }
    }

    /// <summary>
    /// Interface represents the mpeg4 gif file parameters.
    /// </summary>
    public interface IResultMpeg4
    {
        /// <summary>
        /// A valid URL for the MP4 file. File size must not exceed 1MB.
        /// </summary>
        [JsonProperty("mpeg4_url")]
        string Mpeg4Url { get; set; }

        /// <summary>
        /// Video width.
        /// </summary>
        [JsonProperty("mpeg4_width")]
        int? Mpeg4Width { get; set; }

        /// <summary>
        /// Video height.
        /// </summary>
        [JsonProperty("mpeg4_height")]
        int? Mpeg4Height { get; set; }
    }

    /// <summary>
    /// Interface represents the video file parameters.
    /// </summary>
    public interface IResulVideo
    {
        /// <summary>
        /// A valid URL for the MP4 file. File size must not exceed 1MB.
        /// </summary>
        [JsonProperty("video_url")]
        string VideoUrl { get; set; }

        /// <summary>
        /// Video width.
        /// </summary>
        [JsonProperty("video_width")]
        int? VideoWidth { get; set; }

        /// <summary>
        /// Video height.
        /// </summary>
        [JsonProperty("video_height")]
        int? VideoHeight { get; set; }

        /// <summary>
        /// Video duration in seconds.
        /// </summary>
        [JsonProperty("video_duration")]
        int? VideoDuration { get; set; }
    }

    /// <summary>
    /// Interface represents the audio file parameters.
    /// </summary>
    public interface IResulAudio
    {
        /// <summary>
        /// A valid URL for the audio file.
        /// </summary>
        [JsonProperty("audio_url")]
        string AudioUrl { get; set; }

        /// <summary>
        /// Audio duration in seconds.
        /// </summary>
        [JsonProperty("audio_duration")]
        int? AudioDuration { get; set; }
    }

    /// <summary>
    /// Interface represents the voice recording parameters.
    /// </summary>
    public interface IResulVoice
    {
        /// <summary>
        /// A valid URL for the voice recording.
        /// </summary>
        [JsonProperty("voice_url")]
        string VoiceUrl { get; set; }

        /// <summary>
        /// Voice duration in seconds.
        /// </summary>
        [JsonProperty("voice_duration")]
        int? VoiceDuration { get; set; }
    }
}
