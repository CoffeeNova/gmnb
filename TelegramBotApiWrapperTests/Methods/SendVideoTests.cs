using System;
using CoffeeJelly.TelegramBotApiWrapper.Types.Messages;
using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoffeeJelly.TelegramBotApiWrapper.Methods.Tests
{
    public partial class TelegramMethodsTests
    {
        [TestMethod()]
        public void SendVideo_FileId_TextMessage()
        {
            var videoId = "BAADAgADJQADk35oS3hn1l33Em0gAg";
            var expected = new TextMessage
            {
                Chat = _privateChat,
                From = _botUser
            };
            var actual = _telegramMethods.SendVideo(_privateChat.Id.ToString(), videoId).Result;

            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        [TestMethod()]
        public void SendVideo_VideoWithParameters_TextMessage()
        {
            var caption = "video.mp4";
            var duration = 5;
            var width = 560;
            var height = 320;

            var expected = new TextMessage
            {
                Chat = _privateChat,
                From = _botUser
            };
            var actual = _telegramMethods.SendVideo(_privateChat.Id.ToString(), _fullFileName, caption, duration, width, height).Result;

            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        [TestMethod()]
        public void SendVideoByUri_VideoUri_TextMessage()
        {
            var expected = new TextMessage
            {
                Chat = _privateChat,
                From = _botUser
            };
            var uri = new Uri(_videoUri);
            var actual = _telegramMethods.SendVideoByUri(_privateChat.Id.ToString(), uri);

            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        private static string _videoFileName = "video.mp4";
        private static string _videoId = "BAADAgADJQADk35oS3hn1l33Em0gAg";
        private static string _videoUri = "http://techslides.com/demos/sample-videos/small.mp4";
    }
}
