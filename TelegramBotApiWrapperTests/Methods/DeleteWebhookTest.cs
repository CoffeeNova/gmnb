using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoffeeJelly.TelegramBotApiWrapper.Methods.Tests
{
    public partial class TelegramMethodsTests
    {
        [TestMethod()]
        public void DeleteWebhookTest()
        {
            var actual = _telegramMethods.DeleteWebhook().Result;
            Assert.IsTrue(actual);
        }
    }
}
