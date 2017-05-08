using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Types.General
{
    /// <summary>
    /// Represents a sticker.
    /// </summary>
    public class Sticker : IFile, IFileImageSize, IFileThumb
    {
        /// <summary>
        /// Unique file identifier.
        /// </summary>
        [JsonProperty("file_id", Required = Required.Always)]
        public string FileId { get; set; }

        /// <summary>
        /// Sticker width.
        /// </summary>
        [JsonProperty("width", Required = Required.Always)]
        public int Width { get; set; }

        /// <summary>
        /// Sticker height.
        /// </summary>
        [JsonProperty("height", Required = Required.Always)]
        public int Height { get; set; }

        /// <summary>
        /// Sticker thumbnail in .webp or .jpg format.
        /// </summary>
        ///<remarks>Optional.</remarks>
        [JsonProperty("thumb")]
        public PhotoSize Thumb { get; set; }

        /// <summary>
        /// Emoji associated with the sticker.
        /// </summary>
        ///<remarks>Optional.</remarks>
        [JsonProperty("emoji")]
        public string Emoji { get; set; }

        /// <summary>
        /// File size.
        /// </summary>
        ///<remarks>Optional.</remarks>
        [JsonProperty("file_size")]
        public int? FileSize { get; set; }
    }
}