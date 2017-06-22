using System;
using CoffeeJelly.TelegramBotApiWrapper.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoffeeJelly.TelegramBotApiWrapper.Methods.Tests
{
    public partial class TelegramMethodsTests
    {
        [TestMethod()]
        public void SendChatAction_UploadDocumentAction_True()
        {
            var expected = true;

            var actual = _telegramMethods.SendChatAction(_privateChat.Id.ToString(), ChatAction.UploadDocument).Result;

            Assert.AreEqual(expected, actual);
        }
    }
}
