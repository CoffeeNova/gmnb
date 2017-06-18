using System.Collections.Generic;
using CoffeeJelly.TelegramBotApiWrapper.Exceptions;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;
using CoffeeJelly.TelegramBotApiWrapper.Types.Messages;
using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoffeeJelly.TelegramBotApiWrapper.Methods.Tests
{
    public partial class TelegramMethodsTests
    {
        [TestMethod()]
        public void ForwardMessageTest_ForwardTextMessage()
        {
            var chatId = 170181775;
            var fromChatId = 170181775;
            var messageId = 1;
            var expected = new TextMessage
            {
                Chat = _privateChat,
                From = _botUser,
                Entities = new List<MessageEntity> { _botStartCommandEntity },
                Text = "/start",
                ForwardFrom = _user
            };

            var actual = _telegramMethods.ForwardMessage<TextMessage>(chatId.ToString(), fromChatId.ToString(), messageId);
            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        [ExpectedException(typeof(TelegramMethodsException))]
        [TestMethod()]
        public void ForwardMessage_TelegramMethodsException()
        {
            _telegramMethods.ForwardMessage<TextMessage>(_privateChat.Id.ToString(), _privateChat.Id.ToString(), int.MaxValue);
        }

        [TestMethod()]
        public void ForwardMessageTest_ForwardStickerMessage()
        {
            var chatId = 170181775;
            var fromChatId = 170181775;
            var messageId = 4218;
            var expected = new StickerMessage
            {
                Chat = _privateChat,
                From = _botUser,
                Sticker = new Sticker
                {
                    Emoji = "\u2764",
                    FileId = "CAADAgADowEAAsxUSQmFay8ykxLf6QI",
                    FileSize = 35206,
                    Height = 512,
                    Width = 512,
                    Thumb = new PhotoSize
                    {
                        FileId = "AAQCABPNBoINAAQjOFnKsKPVNYe6AAIC",
                        FileSize = 5544,
                        Width = 128,
                        Height = 128
                    }
                },
                MessageId = messageId,
                ForwardFrom = _user
            };
            var actual = _telegramMethods.ForwardMessage<StickerMessage>(chatId.ToString(), fromChatId.ToString(), messageId);
            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }
    }
}
