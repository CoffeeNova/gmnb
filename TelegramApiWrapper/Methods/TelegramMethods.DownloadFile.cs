using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using CoffeeJelly.TelegramApiWrapper.Exceptions;
using File = CoffeeJelly.TelegramApiWrapper.Types.General.File;

namespace CoffeeJelly.TelegramApiWrapper.Methods
{
    public partial class TelegramMethods
    {
        public void DownloadFileAsync(File file, string path = null)
        {
            if (path == null) path = FileStorage;
            if (!Directory.Exists(path))
                throw new ArgumentException($"Path does not exist. Set {nameof(path)} argument or {nameof(FileStorage)} property.", nameof(path));

            if (DateTime.Now.Subtract(file.FilePathCreated).Hours > 1)
                throw new TelegramFileDownloadException(
                    $"The link's lifetime is expired. It is probably not valid now. Try to call {nameof(GetFile)} method again");

            using (var webClient = new WebClient())
            {
                webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;
                webClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;

                try
                {
                    var fullFileName = Path.Combine(path, file.FilePath.Split('/').Last());
                    _downloadFile.Download(webClient, new Uri(TelegramFileUrl + Token + "/" + file.FilePath),
                        fullFileName);
                }
                catch (WebException ex)
                {
                    throw new TelegramFileDownloadException("Error downloading file or saving to disk.", ex);
                }
            }
        }

        public event DownloadProgressChangedEventHandler DownloadFileProgressChanged;
        public event AsyncCompletedEventHandler DownloadFileCompleted;

        private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            DownloadFileProgressChanged?.Invoke(sender, e);
        }

        private void WebClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            DownloadFileCompleted?.Invoke(sender, e);
        }


        #region inner private classes

        private interface IDownloadFile
        {
            void Download(WebClient webClient, Uri uri, string fileName);
        }

        private class DownloadFile : IDownloadFile
        {
            public void Download(WebClient webClient, Uri uri, string fileName)
            {
                webClient.DownloadFileAsync(uri, fileName);
            }

        }

        //for test only
        private class DownloadFileStub : IDownloadFile
        {
            public void Download(WebClient webClient, Uri uri, string fileName)
            {
                webClient.DownloadFile(uri, fileName);
            }
        }
        #endregion
    }
}