using CoffeeJelly.TelegramApiWrapper.Exceptions;
using CoffeeJelly.TelegramApiWrapper.Types.General;
using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoffeeJelly.TelegramApiWrapper.Methods.Tests
{
    public partial class TelegramMethodsTests
    {
        [TestMethod()]
        public void GetMeTest()
        {
            var expected = _botUser;

            var actual = _telegramMethods.GetMe();
            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        [TestMethod()]
        [ExpectedException(typeof(TelegramMethodsException), "Should be exception, because wrong token")]
        public void GetMe_TelegramMethodsGetMeException()
        {
            var telegramMethods = new TelegramMethods("fakeToken");
            var actual = telegramMethods.GetMe();
        }

        [TestMethod()]
        public void GetMeAsyncTest()
        {
            var expected = new User
            {
                Id = 252886092,
                FirstName = "Gmail control bot",
                Username = "gmailnotifybot"
            };

            var actual = _telegramMethods.GetMeAsync().Result;
            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }
    }
}
