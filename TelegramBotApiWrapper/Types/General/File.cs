using System;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Types.General
{
    /// <summary>
    /// Represents a file ready to be downloaded. 
    /// </summary>
    /// <remarks>
    /// The file can be downloaded via the link https://api.telegram.org/file/bot&lt;token&gt;/&lt;file_path&gt;
    /// or by method <see cref="Methods.TelegramMethods.DownloadFileAsync(System.IO.File,string)"/>. 
    /// It is guaranteed that the link will be valid for at least 1 hour. 
    /// When the link expires, a new one can be requested by calling <see cref="Methods.TelegramMethods.GetFile(string)"/>.
    /// </remarks>
    public class File : IFile
    {
        /// <summary>
        /// Unique identifier for this file
        /// </summary>
        [JsonProperty("file_id", Required = Required.Always)]
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
        /// or method <see cref="Methods.TelegramMethods.DownloadFileAsync(System.IO.File, string)"/>.
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
        /// The date when <see cref="FilePath"/> created on Telegeram's servers by <see cref="Methods.TelegramMethods.GetFile(string)"/> method.
        /// </summary>
        [JsonIgnore]
        public DateTime FilePathCreated { get; private set; }

        private string _filePath;
    }
}