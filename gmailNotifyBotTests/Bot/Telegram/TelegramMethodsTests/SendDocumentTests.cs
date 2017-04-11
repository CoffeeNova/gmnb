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
        public void SendDocument_DocumentName_TextMessage()
        {
            var expected = new TextMessage
            {
                Chat = _privateChat,
                From = _botUser
            };
            var actual = _telegramMethods.SendDocument(_privateChat.Id.ToString(), _fullFileName).Result;

            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        [TestMethod()]
        public void SendDocumentByUri_DocumentUri_TextMessage()
        {
            var uri = new Uri(_documentUri);
            var expected = new TextMessage
            {
                Chat = _privateChat,
                From = _botUser
            };
            var actual = _telegramMethods.SendDocumentByUri(_privateChat.Id.ToString(), uri);

            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        [TestMethod()]
        public void SendDocumentByUriAsync_DocumentUriPlusCaption_TextMessage()
        {
            var uri = new Uri(_documentUri);
            var caption = "Test caption";
            var expected = new TextMessage
            {
                Chat = _privateChat,
                From = _botUser
            };
            var actual = _telegramMethods.SendDocumentByUriAsync(_privateChat.Id.ToString(), uri, caption).Result;

            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        private static string _documentFileName = "AutoUpdate.zip";
        private static string _documentUri = "http://www.icecubesservice.com/TestDownloadDoc.pdf.zip/at_download/file";
    }
}
