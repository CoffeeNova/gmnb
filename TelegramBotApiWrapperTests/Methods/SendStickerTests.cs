using System;
using CoffeeJelly.TelegramBotApiWrapper.Types.Messages;
using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoffeeJelly.TelegramBotApiWrapper.Methods.Tests
{
    public partial class TelegramMethodsTests
    {
        [TestMethod()]
        public void SendSticker_StickerName_StickerMessage()
        {
            var actual = _telegramMethods.SendSticker(_privateChat.Id.ToString(), _fullFileName).Result;

            Assert.IsInstanceOfType(actual, typeof(StickerMessage));
        }

        [TestMethod()]
        public void SendSticker_FileId_StickerMessage()
        {
            var actual = _telegramMethods.SendSticker(_privateChat.Id.ToString(), _stickerId).Result;

            Assert.IsInstanceOfType(actual, typeof(StickerMessage));
        }

        [TestMethod()]
        public void SendStickerByUri_StickerUri_StickerMessage()
        {
            var uri = new Uri(_stickerUri);
            var actual = _telegramMethods.SendStickerUri(_privateChat.Id.ToString(), uri).Result;

            Assert.IsInstanceOfType(actual, typeof(StickerMessage));
        }

        private static string _stickerFileName = "sticker.webp";
        private static string _stickerId = "CAADAgADcwEAAsxUSQmpOO9BbxEBVgI";
        private static string _stickerUri = "https://www.gstatic.com/webp/gallery/1.webp";
    }
}
