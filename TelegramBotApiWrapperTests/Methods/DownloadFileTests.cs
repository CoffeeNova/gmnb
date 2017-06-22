using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoffeeJelly.TelegramBotApiWrapper.Methods.Tests
{
    public partial class TelegramMethodsTests
    {
        [TestMethod()]
        public void DownloadFile_FileId_FileExists()
        {
            _file = _telegramMethods.GetFile(_file.FileId).Result;

            _telegramMethods.DownloadFileAsync(_file, _fullFileName).Wait();
            var fileName = Path.GetFileName(_file.FilePath);
            _fullFileName = Path.Combine(_fullFileName, fileName);
            Assert.IsTrue(System.IO.File.Exists(_fullFileName), $"File {_fullFileName} is not exist!");
        }

    }
}
