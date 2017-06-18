using CoffeeJelly.TelegramBotApiWrapper.Types.Messages;
using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoffeeJelly.TelegramBotApiWrapper.Methods.Tests
{
    public partial class TelegramMethodsTests
    {
        [TestMethod()]
        public void SendLocation_LatAndLongitude_LocationMessage()
        {
            var actual = _telegramMethods.SendLocation(_privateChat.Id.ToString(), _location.Latitude, _location.Longitude);

            Assert.IsInstanceOfType(actual, typeof(LocationMessage));
        }

        [TestMethod()]
        public void SendLocationAsync_LatAndLongitude_LocationMessage()
        {
            var actual = _telegramMethods.SendLocationAsync(_privateChat.Id.ToString(), _location.Latitude, _location.Longitude).Result;

            Assert.IsInstanceOfType(actual, typeof(LocationMessage));
        }
    }
}
