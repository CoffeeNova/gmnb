using System.Collections.Generic;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;
using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoffeeJelly.TelegramBotApiWrapper.Methods.Tests
{
    public partial class TelegramMethodsTests
    {
        [TestMethod()]
        public void GetChatAdministrators_ChatId_ListOfChatMembers()
        {
            var expected = new List<ChatMember>
            {
              _botChatMember,
              _userChatMember
            };
            var actual = _telegramMethods.GetChatAdministrators(_testChannel.Id.ToString()).Result;

            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }
    }
}
