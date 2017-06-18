using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoffeeJelly.TelegramBotApiWrapper.Methods.Tests
{
    public partial class TelegramMethodsTests
    {
        [TestMethod()]
        public void DeleteMessage_DeleteLastMessage_Message()
        {
            var actual =
                _telegramMethods.DeleteMessage(_textMessage.Chat, _textMessage.MessageId);

            Assert.IsTrue(actual);
        }

    }
}
