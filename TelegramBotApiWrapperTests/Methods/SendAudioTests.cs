using System;
using CoffeeJelly.TelegramBotApiWrapper.Types.Messages;
using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoffeeJelly.TelegramBotApiWrapper.Methods.Tests
{
    public partial class TelegramMethodsTests
    {
        [TestMethod()]
        public void SendAudio_AudioName_AudioMessage()
        {
            var actual = _telegramMethods.SendAudio(_privateChat.Id.ToString(), _fullFileName).Result;
            Assert.IsInstanceOfType(actual, typeof(AudioMessage));
        }

        [TestMethod()]
        public void SendAudio_AudioWithParameters_AudioMessage()
        {
            var caption = "mpthreetest.mp3";
            var duration = 12;
            var performer = "unknown";
            var title = "mpthreetest";
            var actual = _telegramMethods.SendAudio(_privateChat.Id.ToString(), _fullFileName, caption, duration, performer, title).Result;

            Assert.IsInstanceOfType(actual, typeof(AudioMessage));
        }

        [TestMethod()]
        public void SendAudioByUri_AudioUri_AudioMessage()
        {
            var uri = new Uri(_audioUri);
            var actual = _telegramMethods.SendAudioUri(_privateChat.Id.ToString(), uri).Result;
            Assert.IsInstanceOfType(actual, typeof(AudioMessage));
        }

        private static string _audioFileName = "mpthreetest.mp3";
        private static string _audioUri =
            "https://ia802508.us.archive.org/5/items/testmp3testfile/mpthreetest.mp3";
    }
}
