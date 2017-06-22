using System;
using CoffeeJelly.TelegramBotApiWrapper.Types.Messages;
using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoffeeJelly.TelegramBotApiWrapper.Methods.Tests
{
    public partial class TelegramMethodsTests
    {
        [TestMethod()]
        public void SendVideo_FileId_VideoMessage()
        {
            var videoId = "BAADAgADJQADk35oS3hn1l33Em0gAg";
            var actual = _telegramMethods.SendVideo(_privateChat.Id.ToString(), videoId).Result;

            Assert.IsInstanceOfType(actual, typeof(VideoMessage));
        }

        [TestMethod()]
        public void SendVideo_VideoWithParameters_VideoMessage()
        {
            var caption = "video.mp4";
            var duration = 5;
            var width = 560;
            var height = 320;
            var actual = _telegramMethods.SendVideo(_privateChat.Id.ToString(), _fullFileName, caption, duration, width, height).Result;

            Assert.IsInstanceOfType(actual, typeof(VideoMessage));
        }

        [TestMethod()]
        public void SendVideoByUri_VideoUri_VideoMessage()
        {
            var uri = new Uri(_videoUri);
            var actual = _telegramMethods.SendVideoUri(_privateChat.Id.ToString(), uri).Result;

            Assert.IsInstanceOfType(actual, typeof(VideoMessage));
        }

        private static string _videoFileName = "video.mp4";
        private static string _videoId = "BAADAgADJQADk35oS3hn1l33Em0gAg";
        private static string _videoUri = "http://techslides.com/demos/sample-videos/small.mp4";
    }
}
