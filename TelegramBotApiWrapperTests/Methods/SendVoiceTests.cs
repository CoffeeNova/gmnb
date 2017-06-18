using System;
using CoffeeJelly.TelegramBotApiWrapper.Types.Messages;
using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoffeeJelly.TelegramBotApiWrapper.Methods.Tests
{
    public partial class TelegramMethodsTests
    {
        [TestMethod()]
        public void SendVoice_FileId_VoiceMessage()
        {
            var actual = _telegramMethods.SendVoice(_privateChat.Id.ToString(), _voiceId).Result;

            Assert.IsInstanceOfType(actual, typeof(VoiceMessage));
        }

        [TestMethod()]
        public void SendVoice_VoiceWithParameters_VoiceMessage()
        {
            var caption = "mpthreetest.ogg";
            var duration = 3;
            var actual = _telegramMethods.SendVoice(_privateChat.Id.ToString(), _fullFileName, caption, duration).Result;

            Assert.IsInstanceOfType(actual, typeof(VoiceMessage));
        }

        [TestMethod()]
        public void SendVoiceByUri_VoiceUri_VoiceMessage()
        {
            var uri = new Uri(_voiceUri);
            var actual = _telegramMethods.SendVoiceByUri(_privateChat.Id.ToString(), uri);

            Assert.IsInstanceOfType(actual, typeof(VoiceMessage));
        }

        private static string _voiceId = "AwADAgADOAADk35oSwABtut-veqa0AI";
        private static string _voiceFileName = "mpthreetest.ogg";
        private static string _voiceUri = "https://upload.wikimedia.org/wikipedia/commons/c/c8/Example.ogg";
    }
}
