using CoffeeJelly.TelegramBotApiWrapper.Types.Messages;
using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoffeeJelly.TelegramBotApiWrapper.Methods.Tests
{
    public partial class TelegramMethodsTests
    {
        [TestMethod()]
        public void SendVenue_LatAndLongitude_VenueMessage()
        {
            var title = "test venue";
            var address = "test address";
            var actual = _telegramMethods.SendVenue(_privateChat.Id.ToString(), _location.Latitude, _location.Longitude, title, address).Result;

            Assert.IsInstanceOfType(actual, typeof(VenueMessage));
        }
    }
}
