using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram.Exceptions;
using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoffeeJelly.gmailNotifyBot.Bot.Telegram.Tests
{
    public partial class TelegramMethodsTests
    {
        [TestMethod()]
        public void SendMessage_TextMessageOnlyString_TextMessage()
        {
            var message = "test sendMessage";
            var expected = new TextMessage
            {
                Chat = _privateChat,
                Text = message,
                From = _botUser
            };

            var actual = _telegramMethods.SendMessage(_privateChat.Id.ToString(), message);
            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        [TestMethod()]
        public void SendMessageAsync_TextMessageOnlyString_TextMessage()
        {
            var message = "test sendMessage";
            var expected = new TextMessage
            {
                Chat = _privateChat,
                Text = message,
                From = _botUser
            };

            var actual = _telegramMethods.SendMessageAsync(_privateChat.Id.ToString(), message).Result;
            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        [TestMethod()]
        public void SendMessage_StyledMessage_TextMessage()
        {
            var message = "test styled sendMessage";
            var expected = new TextMessage
            {
                Chat = _privateChat,
                Text = message,
                From = _botUser,
                Entities = new List<MessageEntity> { _italicTextEntity }
            };

            var actual = _telegramMethods.SendMessage(_privateChat.Id.ToString(), "_" + message + "_", "Markdown");
            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        [TestMethod()]
        public void SendMessage_SilentMessage_TextMessage()
        {
            var message = "test silent sendMessage";
            var expected = new TextMessage
            {
                Chat = _privateChat,
                Text = message,
                From = _botUser
            };

            var actual = _telegramMethods.SendMessage(_privateChat.Id.ToString(), message, null, false, true);
            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        [TestMethod()]
        public void SendMessage_InlineKeyboardMarkupMessage_TextMessage()
        {
            var message = "https://www.twitch.tv";
            var expected = new TextMessage
            {
                Chat = _privateChat,
                Text = message,
                From = _botUser,
                Entities = new List<MessageEntity> { _urlEntity }
            };

            var actual = _telegramMethods.SendMessage(_privateChat.Id.ToString(), message, null, false, false, null, _testInlineKeyboardMarkup);
            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        [TestMethod()]
        public void SendMessage_ReplyKeyboardMarkupMessage_TextMessage()
        {
            var message = "test";
            var expected = new TextMessage
            {
                Chat = _privateChat,
                Text = message,
                From = _botUser,
            };

            var actual = _telegramMethods.SendMessage(_privateChat.Id.ToString(), message, null, false, false, null, _testReplyKeyboardMarkup);
            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        [TestMethod()]
        public void SendMessage_ReplyKeyboardRemove_TextMessage()
        {
            var message = "test";
            var expected = new TextMessage
            {
                Chat = _privateChat,
                Text = message,
                From = _botUser,
            };

            var actual = _telegramMethods.SendMessage(_privateChat.Id.ToString(), message, null, false, false, null, _testReplyKeyboardRemove);
            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        [TestMethod()]
        public void SendMessage_ForseReply_TextMessage()
        {
            var message = "ForseReply";
            var expected = new TextMessage
            {
                Chat = _privateChat,
                Text = message,
                From = _botUser,
            };

            var actual = _telegramMethods.SendMessage(_privateChat.Id.ToString(), message, null, false, false, null, _testForceReply);
            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }
    }
}
