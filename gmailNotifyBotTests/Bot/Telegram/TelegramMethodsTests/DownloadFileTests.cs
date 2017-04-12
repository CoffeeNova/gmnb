using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram.Exceptions;
using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoffeeJelly.gmailNotifyBot.Bot.Telegram.Tests
{
    public partial class TelegramMethodsTests
    {
        [TestMethod()]
        public void DownloadFile_File_File()
        {
            if (string.IsNullOrEmpty(_file.FilePath)) GetFile_FileId_File();

            _telegramMethods.DownloadFileCompleted +=  
            //var actual = _telegramMethods.DownloadFileAsync();

           
            Assert.IsTrue(System.IO.File.Exists(_fullFileName + _file.FileId), comparationResult.DifferencesString);
        }
    }
}
