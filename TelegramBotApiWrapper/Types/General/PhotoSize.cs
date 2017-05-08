using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Types.General
{
    /// <summary>
    /// Represents one size of a photo or a file / sticker thumbnail.
    /// </summary>
    public class PhotoSize : IFile, IFileImageSize
    {
        /// <summary>
        /// Unique identifier for this file.
        /// </summary>
        [JsonProperty("file_id", Required = Required.Always)]
        public string FileId { get; set; }

        /// <summary>
        /// Photo width.
        /// </summary>
        [JsonProperty("width", Required = Required.Always)]
        public int Width { get; set; }

        /// <summary>
        /// Photo height.
        /// </summary>
        [JsonProperty("height", Required = Required.Always)]
        public int Height { get; set; }

        /// <summary>
        /// File size.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("file_size")]
        public int? FileSize { get; set; }
    }
}