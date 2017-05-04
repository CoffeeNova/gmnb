using System;
using CoffeeJelly.TelegramApiWrapper.Types.Message;
using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoffeeJelly.TelegramApiWrapper.Methods.Tests
{
    public partial class TelegramMethodsTests
    {
        [TestMethod()]
        public void SendAudio_AudioName_TextMessage()
        {
            var expected = new TextMessage
            {
                Chat = _privateChat,
                From = _botUser
            };
            var actual = _telegramMethods.SendAudio(_privateChat.Id.ToString(), _fullFileName).Result;

            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        [TestMethod()]
        public void SendAudio_AudioWithParameters_TextMessage()
        {
            var caption = "mpthreetest.mp3";
            var duration = 12;
            var performer = "unknown";
            var title = "mpthreetest";

            var expected = new TextMessage
            {
                Chat = _privateChat,
                From = _botUser
            };
            var actual = _telegramMethods.SendAudio(_privateChat.Id.ToString(), _fullFileName, caption, duration, performer, title).Result;

            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        [TestMethod()]
        public void SendAudioByUri_AudioUri_TextMessage()
        {
            var expected = new TextMessage
            {
                Chat = _privateChat,
                From = _botUser
            };
            var uri = new Uri(_audioUri);
            var actual = _telegramMethods.SendAudioByUri(_privateChat.Id.ToString(), uri);

            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        [TestMethod()]
        public void SendAudioByUriAsync_AudioUriWithParams_TextMessage()
        {
            var caption = "mpthreetest.mp3";
            var duration = 12;
            var performer = "unknown";
            var title = "mpthreetest";

            var expected = new TextMessage
            {
                Chat = _privateChat,
                From = _botUser
            };
            var uri = new Uri(_audioUri);
            var actual = _telegramMethods.SendAudioByUriAsync(_privateChat.Id.ToString(), uri, caption, duration, performer, title).Result;

            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        private static string _audioFileName = "mpthreetest.mp3";
        private static string _audioUri =
            "https://ia802508.us.archive.org/5/items/testmp3testfile/mpthreetest.mp3";
    }
}
