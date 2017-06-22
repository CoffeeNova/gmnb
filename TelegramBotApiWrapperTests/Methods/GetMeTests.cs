using System.Threading.Tasks;
using CoffeeJelly.TelegramBotApiWrapper.Exceptions;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;
using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoffeeJelly.TelegramBotApiWrapper.Methods.Tests
{
    public partial class TelegramMethodsTests
    {
        [TestMethod()]
        public void GetMeTest()
        {
            var expected = _botUser;

            var actual = _telegramMethods.GetMe().Result;
            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        [TestMethod()]
        [ExpectedException(typeof(TelegramMethodsException), "Should be exception, because wrong token")]
        public async Task GetMe_TelegramMethodsGetMeException()
        {
            var telegramMethods = new TelegramMethods("fakeToken");
            var actual = await telegramMethods.GetMe();
        }

        //[TestMethod()]
        //public void GetMeAsyncTest()
        //{
        //    var expected = new User
        //    {
        //        Id = 252886092,
        //        FirstName = "Gmail control bot",
        //        Username = "gmailnotifybot"
        //    };

        //    var actual = _telegramMethods.GetMeAsync().Result;
        //    var compareLogic = new CompareLogic(_config);
        //    var comparationResult = compareLogic.Compare(expected, actual);

        //    Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        //}
    }
}
