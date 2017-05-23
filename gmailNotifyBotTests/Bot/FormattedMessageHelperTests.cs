using CoffeeJelly.gmailNotifyBot.Bot;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoffeeJelly.gmailNotifyBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeJelly.gmailNotifyBot.Bot.Types;
using KellermanSoftware.CompareNetObjects;

namespace CoffeeJelly.gmailNotifyBot.Bot.Tests
{
    using Helper = FormattedMessageHelper;
    [TestClass()]
    public class FormattedMessageHelperTests
    {
        [TestMethod()]
        public void DivideIntoPagesTest_SimpleText_3linesList()
        {
            var text = "aaaaaaaaaabbbbbbbbbbcccccccccc";
            var expected = new List<string>
            {
                "aaaaaaaaaa",
                "bbbbbbbbbb",
                "cccccccccc"
            };
            var actual = Helper.DivideIntoPages(text, 5, 10);

            var compaitLogic = new CompareLogic();
            var result = compaitLogic.Compare(expected, actual);
            Assert.IsTrue(result.AreEqual, result.DifferencesString);
        }

        [TestMethod()]
        public void DivideIntoPagesTest_TextWithNewLines_4linesList()
        {
            var text = "0123456\r\n90123456789012345 789";
            var expected = new List<string>
            {
                "0123456",
                "9012345678",
                "9012345",
                "789"
            };
            var actual = Helper.DivideIntoPages(text, 5, 10);

            var compaitLogic = new CompareLogic();
            var result = compaitLogic.Compare(expected, actual);
            Assert.IsTrue(result.AreEqual, result.DifferencesString);
        }

        [TestMethod()]
        public void DivideIntoPagesTest_TextWithBrackets_3linesList()
        {
            var text = "ab<A>fg</A>lmnopqr<A>vwxyzabcdefgh</A>nopqrstuvwxyz";
            var expected = new List<string>
            {
                "ab<A>fg</A>lmnopqr",
                "<A>vwxyzabcdefgh</A>",
                "nopqrstuvwxyz"
            };
            var actual = Helper.DivideIntoPages(text, 5, 20);

            var compaitLogic = new CompareLogic();
            var result = compaitLogic.Compare(expected, actual);
            Assert.IsTrue(result.AreEqual, result.DifferencesString);
        }

        [TestMethod()]
        public void ParseUserInfo_NamePlusEmailString_UserInfo()
        {
            var value = "\"testname\" <testEmail>";
            var expected = new UserInfo
            {
                Email = "testEmail",
                Name = "\"testname\""
            };
            var actual = Helper.ParseUserInfo(value);
            var compaitLogic = new CompareLogic();
            var result = compaitLogic.Compare(expected, actual);
            Assert.IsTrue(result.AreEqual, result.DifferencesString);
        }

        [TestMethod()]
        public void ParseUserInfo_EmailString_UserInfo()
        {
            var value = "testEmail";
            var expected = new UserInfo
            {
                Email = "testEmail",
                Name = ""
            };
            var actual = Helper.ParseUserInfo(value);
            var compaitLogic = new CompareLogic();
            var result = compaitLogic.Compare(expected, actual);
            Assert.IsTrue(result.AreEqual, result.DifferencesString);
        }
    }
}