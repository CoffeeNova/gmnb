﻿using System.Collections.Generic;
using CoffeeJelly.TelegramBotApiWrapper.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;
using CoffeeJelly.TelegramBotApiWrapper.Types.Messages;
using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoffeeJelly.TelegramBotApiWrapper.Methods.Tests
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

            var actual = _telegramMethods.SendMessage(_privateChat.Id.ToString(), message).Result;
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

            var actual = _telegramMethods.SendMessage(_privateChat.Id.ToString(), "_" + message + "_", ParseMode.Markdown).Result;
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

            var actual = _telegramMethods.SendMessage(_privateChat.Id.ToString(), message, null, false, true).Result;
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

            var actual = _telegramMethods.SendMessage(_privateChat.Id.ToString(), message, null, false, false, null, _testInlineKeyboardMarkup).Result;
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

            var actual = _telegramMethods.SendMessage(_privateChat.Id.ToString(), message, null, false, false, null, _testReplyKeyboardMarkup).Result;
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

            var actual = _telegramMethods.SendMessage(_privateChat.Id.ToString(), message, null, false, false, null, _testReplyKeyboardRemove).Result;
            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        [TestMethod()]
        public void SendMessage_ForceReply_TextMessage()
        {
            var message = "ForceReply";
            var expected = new TextMessage
            {
                Chat = _privateChat,
                Text = message,
                From = _botUser,
            };

            var actual = _telegramMethods.SendMessage(_privateChat.Id.ToString(), message, null, false, false, null, _testForceReply).Result;
            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }
    }
}
