using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CoffeeJelly.TelegramBotApiWrapper.Exceptions;
using File = CoffeeJelly.TelegramBotApiWrapper.Types.General.File;

namespace CoffeeJelly.TelegramBotApiWrapper.Methods
{
    public partial class TelegramMethods
    {
        public async Task DownloadFileAsync(File file, string path = null)
        {
            if (path == null) path = FileStorage;
            if (!Directory.Exists(path))
                throw new ArgumentException($"Path does not exist. Set {nameof(path)} argument or {nameof(FileStorage)} property.", nameof(path));

            if (DateTime.Now.Subtract(file.FilePathCreated).Hours > 1)
                throw new TelegramFileDownloadException(
                    $"The link's lifetime is expired. It is probably not valid now. Try to call {nameof(GetFile)} method again");

            using (var webClient = new WebClient())
            {
                try
                {
                    var fullFileName = Path.Combine(path, file.FilePath.Split('/').Last());
                    await _downloadFile.Download(webClient, new Uri(TelegramFileUrl + Token + "/" + file.FilePath),
                        fullFileName);
                }
                catch (WebException ex)
                {
                    throw new TelegramFileDownloadException("Error downloading file or saving to disk.", ex);
                }
            }
        }

        #region inner private classes

        private interface IDownloadFile
        {
            Task Download(WebClient webClient, Uri uri, string fileName);
        }

        private class DownloadFile : IDownloadFile
        {
            public async Task Download(WebClient webClient, Uri uri, string fileName)
            {
                await webClient.DownloadFileTaskAsync(uri, fileName).ConfigureAwait(false);
            }

        }

        //for test only
        private class DownloadFileStub : IDownloadFile
        {
            public async Task Download(WebClient webClient, Uri uri, string fileName)
            {
                await webClient.DownloadFileTaskAsync(uri, fileName).ConfigureAwait(false);
            }
        }
        #endregion
    }
}