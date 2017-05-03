using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeJelly.gmailNotifyBot.Bot.Telegram.Extensions.Tests
{
    [TestClass()]
    public class StringExtensionTests
    {
        [TestMethod()]
        public void SizeInBytesTest()
        {
            int size = 2;
            var testStr = "aa";
            var actual = testStr.SizeUtf8();
            Assert.AreEqual(size, actual);
        }
    }
}