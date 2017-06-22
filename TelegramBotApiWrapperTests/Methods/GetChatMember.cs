using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoffeeJelly.TelegramBotApiWrapper.Methods.Tests
{
    public partial class TelegramMethodsTests
    {
        [TestMethod()]
        public void GetChatMember_ChatId_ListOfChatMembers()
        {
            var expected = _githubBotChatMember;
            var actual = _telegramMethods.GetChatMember(_testChannel.Id.ToString(), _githubBotChatMember.User.Id).Result;

            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }
    }
}
