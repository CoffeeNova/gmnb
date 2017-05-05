using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoffeeJelly.TelegramApiWrapper.Methods.Tests
{
    public partial class TelegramMethodsTests
    {
        [TestMethod()]
        public void EditMessageText_EditLastMessage_Message()
        {
            var expected = _textMessage;
            var actual =
                _telegramMethods.EditMessageText(_editedTextMessage.Text, _editedTextMessage.Chat.Id.ToString(), _editedTextMessage.MessageId.ToString());
            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

    }
}
