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
        public void GetUserProfilePhotos_UserId_UserProfilePhotos()
        {
            var expected = _userProfilePhotos;

            var actual = _telegramMethods.GetUserProfilePhotos(_user.Id);

            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }
    }
}
