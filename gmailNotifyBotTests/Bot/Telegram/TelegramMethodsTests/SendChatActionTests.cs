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
        public void SendChatAction_UploadDocumentAction_True()
        {
            var expected = true;

            var actual = _telegramMethods.SendChatAction(_privateChat.Id.ToString(), TelegramMethods.Action.UploadDocument);

            Assert.AreEqual(expected, actual);
        }
    }
}
