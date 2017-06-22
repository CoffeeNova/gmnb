using System;
using CoffeeJelly.TelegramBotApiWrapper.Types.Messages;
using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoffeeJelly.TelegramBotApiWrapper.Methods.Tests
{
    public partial class TelegramMethodsTests
    {
        [TestMethod()]
        public void SendDocument_DocumentName_DocumentMessage()
        {
            var actual = _telegramMethods.SendDocument(_privateChat.Id.ToString(), _fullFileName).Result;
            Assert.IsInstanceOfType(actual, typeof(DocumentMessage));
        }

        [TestMethod()]
        public void SendDocumentByUri_DocumentUri_DocumentMessage()
        {
            var uri = new Uri(_documentUri);
            var actual = _telegramMethods.SendDocumentUri(_privateChat.Id.ToString(), uri).Result;

            Assert.IsInstanceOfType(actual, typeof(DocumentMessage));
        }

        private static string _documentFileName = "AutoUpdate.zip";
        private static string _documentUri = "https://ssl.gstatic.com/s2/profiles/images/silhouette48.png";
    }
}
