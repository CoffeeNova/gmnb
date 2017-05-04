using CoffeeJelly.TelegramApiWrapper.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoffeeJelly.TelegramApiWrapper.Extensions.Tests
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