using System;
using CoffeeJelly.TelegramBotApiWrapper.JsonParsers;
using CoffeeJelly.TelegramBotApiWrapper.Types.Messages;
using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoffeeJelly.TelegramBotApiWrapper.Methods.Tests
{
    public partial class TelegramMethodsTests
    {
        [TestMethod()]
        public void SendPhoto_PhotoName_PhotoMessage()
        {
            var actual = _telegramMethods.SendPhoto(_privateChat.Id.ToString(), _fullFileName).Result;

            Assert.IsInstanceOfType(actual, typeof(PhotoMessage));
        }

        [TestMethod()]
        public void SendPhoto_PhotoAndInlineKeyboard_PhotoMessage()
        {
            var text = "gabba-gabba-hey";
            var actual = _telegramMethods.SendPhoto(_privateChat.Id.ToString(), _fullFileName, text, true, null, _testInlineKeyboardMarkup).Result;

            Assert.IsInstanceOfType(actual, typeof(PhotoMessage));
        }

        [TestMethod()]
        public void SendPhotoByUri_PhotoUri_PhotoMessage()
        {
            var uri = new Uri(_photoUri);
            var actual = _telegramMethods.SendPhotoUri(_privateChat.Id.ToString(), uri).Result;

            Assert.IsInstanceOfType(actual, typeof(PhotoMessage));
        }

        private static string _photoFileName = "gabba.jpg";
        private static string _photoUri =
           "https://upload.wikimedia.org/wikipedia/commons/thumb/0/0d/C_Sharp_wordmark.svg/117px-C_Sharp_wordmark.svg.png";
    }
}
