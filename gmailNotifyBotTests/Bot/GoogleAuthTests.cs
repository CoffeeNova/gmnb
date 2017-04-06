using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoffeeJelly.gmailNotifyBot.Bot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeJelly.gmailNotifyBot.Bot.Tests
{
    [TestClass()]
    public class GoogleAuthTests
    {
        [TestMethod()]
        public void GetTest()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void GetAutenticationUriTest()
        {
            string clientId = "748733091307-i3fd77bgjbv52qnv41qvq2enkstcg54v.apps.googleusercontent.com";
            string clientSecret = "CB7t0jQ35U9FjEKvvixS7-eM";
            string scopes =
                @"https://www.googleapis.com/auth/gmail.compose" +
                @"+https://mail.google.com/" +
                @"+https://www.googleapis.com/auth/userinfo.profile";
            string redirectUri = "http://gmailnotify.com/OAuth";

            var expected = $"https://accounts.google.com/o/oauth2/auth?" +
                $"client_id={clientId}" +
                $"&client_secret={clientSecret}" +
                $"&redirect_uri={redirectUri}" +
                $"&scope={scopes}" +
                $"&response_type=code";

            var actual = GoogleAuth.GetAuthenticationUri(clientId, clientSecret);
            Assert.AreEqual(expected, actual.OriginalString, expected + "/r/n" + actual.OriginalString);
        }
    }
}