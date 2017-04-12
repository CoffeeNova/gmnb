﻿using System;
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
        public void ForwardMessageTest()
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

            var actual = _telegramMethods.ForwardMessage(chatId.ToString(), fromChatId.ToString(), messageId);
            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        [ExpectedException(typeof(TelegramMethodsException))]
        [TestMethod()]
        public void ForwardMessage_TelegramMethodsException()
        {
            _telegramMethods.ForwardMessage(_privateChat.Id.ToString(), _privateChat.Id.ToString(), int.MaxValue);
        }
    }
}
