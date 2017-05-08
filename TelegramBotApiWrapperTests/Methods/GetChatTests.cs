using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoffeeJelly.TelegramBotApiWrapper.Methods.Tests
{
    public partial class TelegramMethodsTests
    {
        [TestMethod()]
        public void GetChat_ChatId_Chat()
        {
            var expected = _testChannel;
            var actual = _telegramMethods.GetChat(_testChannel.Id.ToString());

            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }
    }
}
