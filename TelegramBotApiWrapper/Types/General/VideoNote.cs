using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Types.General
{
    /// <summary>
    /// Represents a video message (available in Telegram apps as of v.4.0).
    /// </summary>
    public class VideoNote : IFile, IFileDuration, IFileThumb
    {
        /// <summary>
        /// Unique identifier for this file.
        /// </summary>
        [JsonProperty("file_id", Required = Required.Always)]
        public string FileId { get; set; }


        /// <summary>
        /// Video width and height as defined by sender.
        /// </summary>
        [JsonProperty("length", Required = Required.Always)]
        public int Length { get; set; }

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
        /// File size.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("file_size")]
        public int? FileSize { get; set; }
    }
}