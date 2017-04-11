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
        public void SendPhoto_PhotoName_TextMessage()
        {
            var expected = new TextMessage
            {
                Chat = _privateChat,
                From = _botUser
            };
            var actual = _telegramMethods.SendPhoto(_privateChat.Id.ToString(), _fullFileName).Result;

            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        [TestMethod()]
        public void SendPhoto_PhotoAndInlineKeyboard_TextMessage()
        {
            var text = "gabba-gabba-hey";
            var expected = new TextMessage
            {
                Chat = _privateChat,
                From = _botUser
            };
            var actual = _telegramMethods.SendPhoto(_privateChat.Id.ToString(), _fullFileName, text, true, null, _testInlineKeyboardMarkup).Result;

            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        [TestMethod()]
        public void SendPhotoByUri_PhotoUri_TextMessage()
        {
            var expected = new TextMessage
            {
                Chat = _privateChat,
                From = _botUser
            };
            var uri = new Uri(_photoUri);
            var actual = _telegramMethods.SendPhotoByUri(_privateChat.Id.ToString(), uri);

            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        [TestMethod()]
        public void SendPhotoByUriAsync_PhotoUriPlusInlineKeyboard_TextMessage()
        {
            var caption = "pict with keyboard";
            var expected = new TextMessage
            {
                Chat = _privateChat,
                From = _botUser
            };
            var uri = new Uri(_photoUri);
            var actual = _telegramMethods.SendPhotoByUriAsync(_privateChat.Id.ToString(), uri, caption, true, null, _testInlineKeyboardMarkup).Result;

            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        private static string _photoFileName = "gabba.jpg";
        private static string _photoUri =
           "https://upload.wikimedia.org/wikipedia/commons/thumb/0/0d/C_Sharp_wordmark.svg/117px-C_Sharp_wordmark.svg.png";
    }
}
