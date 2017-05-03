using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeJelly.gmailNotifyBot.Bot.Telegram.Tests
{
    [TestClass()]
    public class ToolsTests
    {
        [TestMethod()]
        public void OnlyAllowedCharacters1_CorrectString_True()
        {
            var testString = "ASSDf194_-sdfPO";
            var condition = Tools.OnlyAllowedCharacters1(testString);
            Assert.IsTrue(condition, "Test string have only alowed characters.");
        }

        [TestMethod()]
        public void OnlyAllowedCharacters1_IncorrectString_False()
        {
            var testString = "ASS/f194_-sdfPO";
            var condition = Tools.OnlyAllowedCharacters1(testString);
            Assert.IsFalse(condition, "Test string have illegal character '/'.");
        }
    }
}