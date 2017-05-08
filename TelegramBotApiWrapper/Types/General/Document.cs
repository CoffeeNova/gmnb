using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Types.General
{
    /// <summary>
    /// Represents a general file (as opposed to photos, voice messages and audio files).
    /// </summary>
    public class Document : IFile, IFileThumb, IFileName,
                            IFileMimeType
    {
        /// <summary>
        /// Unique file identifier.
        /// </summary>
        [JsonProperty("file_id", Required = Required.Always)]
        public string FileId { get; set; }

        /// <summary>
        /// Document thumbnail as defined by sender.
        /// </summary>
        /// <remarks>Optional.</remarks>
        public PhotoSize Thumb { get; set; }

        /// <summary>
        /// Original filename as defined by sender.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("file_name")]
        public string FileName { get; set; }

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