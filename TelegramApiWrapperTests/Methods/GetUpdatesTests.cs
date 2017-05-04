using System.Collections.Generic;
using CoffeeJelly.TelegramApiWrapper.Types;
using CoffeeJelly.TelegramApiWrapper.Types.General;
using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoffeeJelly.TelegramApiWrapper.Methods.Tests
{
    public partial class TelegramMethodsTests
    {
        [TestMethod()]
        public void GetUpdatesTest()
        {
            var expected = new List<Update>();
            var actual = _telegramMethods.GetUpdates();
            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected.GetType(), actual.GetType());

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
            // _lastUpdateId = actual.Count == 0 ? 0 : actual.Last().UpdateId;
        }

        //[TestMethod()]
        public void GetUpdates_EditedMessagesOnly_ListOfOneUpdate()
        {
            var expected = new List<Update>
            {
                new Update
                {
                    Message = _editedTextMessage
                }
            };
            var allowedUpdates = new List<UpdateType> { UpdateType.AllUpdates};
            var actual = _telegramMethods.GetUpdates(-1, null, null, allowedUpdates);
            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);

        }
    }
}
