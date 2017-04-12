using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram.Exceptions;
using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoffeeJelly.gmailNotifyBot.Bot.Telegram.Tests
{
    public partial class TelegramMethodsTests
    {
        [TestMethod()]
        public void DownloadFile_FileId_FileExists()
        {
            _file = _telegramMethods.GetFile(_file.FileId);

            _telegramMethods.DownloadFileCompleted +=
                delegate (object sender, System.ComponentModel.AsyncCompletedEventArgs e)
                {
                    Assert.IsTrue(System.IO.File.Exists(_fullFileName + _file.FilePath), $"File {Path.Combine(_fullFileName, _file.FilePath)} is not exist!");
                };
            _telegramMethods.DownloadFileAsync(_file, _fullFileName);
        }

    }
}
