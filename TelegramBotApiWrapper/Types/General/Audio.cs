using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Types.General
{
    /// <summary>
    ///  Represents an audio file to be treated as music by the Telegram clients.
    /// </summary>
    public class Audio : IFile, IFileDuration, IFileMimeType
    {
        /// <summary>
        /// Unique identifier for this file.
        /// </summary>
        [JsonProperty("file_id", Required = Required.Always)]
        public string FileId { get; set; }

        /// <summary>
        /// Duration of the audio in seconds as defined by sender.
        /// </summary>
        [JsonProperty("duration", Required = Required.Always)]
        public int Duration { get; set; }

        /// <summary>
        /// Performer of the audio as defined by sender or by audio tags.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("performer")]
        public string Performer { get; set; }

        /// <summary>
        /// Title of the audio as defined by sender or by audio tags.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// MIME type of the file as defined by sender.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("mime_type")]
        public string MimeType { get; set; }

        /// <summary>
        /// File size.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("file_size")]
        public int? FileSize { get; set; }
    }
}