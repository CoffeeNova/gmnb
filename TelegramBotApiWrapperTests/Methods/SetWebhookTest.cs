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
        public void SetWebhook_NoParameters()
        {
            var url = @"https://lazymail.ml/Push/TelegramPath";
            var actual = _telegramMethods.SetWebhook(url, null, 40, new List<Types.UpdateType> {Types.UpdateType.AllUpdates}).Result;

            Assert.IsTrue(actual);
        }
    }
}
