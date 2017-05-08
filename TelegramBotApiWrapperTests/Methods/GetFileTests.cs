using CoffeeJelly.TelegramBotApiWrapper.Types.General;
using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoffeeJelly.TelegramBotApiWrapper.Methods.Tests
{
    public partial class TelegramMethodsTests
    {
        [TestMethod()]
        public void GetFile_FileId_File()
        {
            var expected = _file;
            File actual = _telegramMethods.GetFile(_file.FileId);

            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
            Assert.IsTrue(!string.IsNullOrEmpty(actual.FilePath), "FilePath is empty.");
            _file = actual;
        }
    }
}
