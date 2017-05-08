using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Types.General
{
    /// <summary>
    /// Represents a video file.
    /// </summary>
    public class Video : IFile, IFileImageSize, IFileDuration,
                            IFileThumb, IFileMimeType
    {
        /// <summary>
        /// Unique identifier for this file.
        /// </summary>
        [JsonProperty("file_id", Required = Required.Always)]
        public string FileId { get; set; }

        /// <summary>
        /// Video width as defined by sender.
        /// </summary>
        [JsonProperty("width", Required = Required.Always)]
        public int Width { get; set; }

        /// <summary>
        /// Video height as defined by sender
        /// </summary>
        [JsonProperty("height", Required = Required.Always)]
        public int Height { get; set; }

        /// <summary>
        /// Duration of the video in seconds as defined by sender.
        /// </summary>
        [JsonProperty("duration", Required = Required.Always)]
        public int Duration { get; set; }

        /// <summary>
        /// Video thumbnail.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("thumb")]
        public PhotoSize Thumb { get; set; }

        /// <summary>
        /// Mime type of a file as defined by sender.
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