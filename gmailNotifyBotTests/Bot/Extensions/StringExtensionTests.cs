﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeJelly.gmailNotifyBot.Tests;
using KellermanSoftware.CompareNetObjects;

namespace CoffeeJelly.gmailNotifyBot.Bot.Extensions.Tests
{
    [TestClass()]
    public class StringExtensionTests
    {

        [TestMethod()]
        public void DivideByLines_DivideBy3Lines_3ElementsList()
        {
            var expected = new List<string>
            {
                $"{text1}\r\n{text2}\r\n{text3}\r\n",
                $"{text4}\r\n{text5}\r\n{text6}\r\n",
                $"{text7}\r\n"
            };
            var text = $"{text1}\r\n{text2}\r\n{text3}\r\n{text4}\r\n{text5}\r\n{text6}\r\n{text7}\r\n";
            var actual = text.DivideByLines(3).ToList();

            var compareLogic = new CompareLogic();
            var result = compareLogic.Compare(expected, actual);

            Assert.IsTrue(result.AreEqual, result.DifferencesString);
        }

        private static string text1 = "line1";
        private static string text2 = "line2";
        private static string text3 = "line3";
        private static string text4 = "line4";
        private static string text5 = "line5";
        private static string text6 = "line6";
        private static string text7 = "line7";

        
    }
}