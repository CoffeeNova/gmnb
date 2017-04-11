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
        public void SendVenue_LatAndLongitude_TextMessage()
        {
            var title = "test venue";
            var address = "test address";
            var expected = new TextMessage
            {
                Chat = _privateChat,
                From = _botUser
            };
            var actual = _telegramMethods.SendVenue(_privateChat.Id.ToString(), _location.Latitude, _location.Longitude, title, address);

            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        [TestMethod()]
        public void SendVenueAsync_LatAndLongitude_TextMessage()
        {
            var title = "test venue";
            var address = "test address";
            var expected = new TextMessage
            {
                Chat = _privateChat,
                From = _botUser
            };
            var actual = _telegramMethods.SendVenueAsync(_privateChat.Id.ToString(), _location.Latitude, _location.Longitude, title, address).Result;

            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }
    }
}
