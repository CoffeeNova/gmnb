using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Types.General
{
    /// <summary>
    /// Represents an animation file to be displayed in the message containing a game.
    /// </summary>
    public class Animation : IFile, IFileThumb
    {
        /// <summary>
        /// 	Unique file identifier
        /// </summary>
        [JsonProperty("file_id", Required = Required.Always)]
        public string FileId { get; set; }

        /// <summary>
        /// Animation thumbnail as defined by sender.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("thumb")]
        public PhotoSize Thumb { get; set; }

        /// <summary>
        /// Original animation filename as defined by sender.
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