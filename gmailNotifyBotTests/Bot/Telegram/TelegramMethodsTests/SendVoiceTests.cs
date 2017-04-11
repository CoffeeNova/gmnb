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
        public void SendVoice_FileId_TextMessage()
        {
            var expected = new TextMessage
            {
                Chat = _privateChat,
                From = _botUser
            };
            var actual = _telegramMethods.SendVoice(_privateChat.Id.ToString(), _voiceId).Result;

            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        [TestMethod()]
        public void SendVoice_VoiceWithParameters_TextMessage()
        {
            var caption = "mpthreetest.ogg";
            var duration = 3;

            var expected = new TextMessage
            {
                Chat = _privateChat,
                From = _botUser
            };
            var actual = _telegramMethods.SendVoice(_privateChat.Id.ToString(), _fullFileName, caption, duration).Result;

            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        [TestMethod()]
        public void SendVoiceByUri_VoiceUri_TextMessage()
        {
            var expected = new TextMessage
            {
                Chat = _privateChat,
                From = _botUser
            };
            var uri = new Uri(_voiceUri);
            var actual = _telegramMethods.SendVoiceByUri(_privateChat.Id.ToString(), uri);

            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        private static string _voiceId = "AwADAgADOAADk35oSwABtut-veqa0AI";
        private static string _voiceFileName = "mpthreetest.ogg";
        private static string _voiceUri = "https://upload.wikimedia.org/wikipedia/commons/c/c8/Example.ogg";
    }
}
