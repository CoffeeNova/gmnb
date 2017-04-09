using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KellermanSoftware.CompareNetObjects;

namespace CoffeeJelly.gmailNotifyBot.Bot.Telegram.Tests
{
    [TestClass()]
    public class TelegramMethodsTests
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _config = new ComparisonConfig
            {
                CompareChildren = true,
                CompareFields = false,
                CompareReadOnly = true,
                ComparePrivateFields = false,
                ComparePrivateProperties = false,
                CompareProperties = true,
                MaxDifferences = 50,
                MembersToIgnore = new List<string> { "MessageId", "Date" }
            };
            string token = App_LocalResources.Tokens.GmailControlBotToken;
            _telegramMethods = new TelegramMethods(token);

            _privateChat = new Chat
            {
                Id = 170181775,
                FirstName = "Coffee",
                LastName = "Jelly",
                UserName = "CoffeeJelly",
                Type = ChatType.Private
            };
            _botUser = new User
            {
                Id = 252886092,
                FirstName = "Gmail control bot",
                Username = "gmailnotifybot"
            };
            _italicTextEntity = new MessageEntity
            {
                Type = "italic",
                Offset = 0,
                Length = 23
            };
            _urlEntity = new MessageEntity
            {
                Type = "url",
                Offset = 0,
                Length = 21
            };
            _testUrlButton = new InlineKeyboardButton
            {
                Text = "URL Button",
                Url = "https://www.twitch.tv"
            };
            _testCallbackDataButton = new InlineKeyboardButton
            {
                Text = "Callback Button",
                CallbackData = "Test callback data"
            };
            _testInlineKeyboardMarkup = new InlineKeyboardMarkup
            {
                InlineKeyboard = new List<List<InlineKeyboardButton>>
                {
                    new List<InlineKeyboardButton>
                    {
                    _testUrlButton,
                    _testCallbackDataButton
                    }
                }
            };
        }

        [TestMethod()]
        public void GetMeTest()
        {
            var expected = _botUser;

            var actual = _telegramMethods.GetMe();
            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        [TestMethod()]
        public void GetMeAsyncTest()
        {
            var expected = new User
            {
                Id = 252886092,
                FirstName = "Gmail control bot",
                Username = "gmailnotifybot"
            };

            var actual = _telegramMethods.GetMeAsync().Result;
            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        [TestMethod()]
        public void SendMessage_TextMessageOnlyString_TextMessage()
        {
            var message = "test sendMessage";
            var expected = new TextMessage
            {
                Chat = _privateChat,
                Text = message,
                From = _botUser
            };

            var actual = _telegramMethods.SendMessage(_privateChat.Id, message);
            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        [TestMethod()]
        public void SendMessage_StyledMessage_TextMessage()
        {
            var message = "test styled sendMessage";
            var expected = new TextMessage
            {
                Chat = _privateChat,
                Text = message,
                From = _botUser,
                Entities = new List<MessageEntity> { _italicTextEntity }
            };

            var actual = _telegramMethods.SendMessage(_privateChat.Id, "_" + message + "_", "Markdown");
            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        [TestMethod()]
        public void SendMessage_InlineKeyboardMarkupMessage_TextMessage()
        {
            var message = "https://www.twitch.tv";
            var expected = new TextMessage
            {
                Chat = _privateChat,
                Text = message,
                From = _botUser,
                Entities = new List<MessageEntity> { _urlEntity }
            };

            var actual = _telegramMethods.SendMessage(_privateChat.Id, message, null, false, false, null, _testInlineKeyboardMarkup);
            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }
        private static ComparisonConfig _config;
        private static TelegramMethods _telegramMethods;
        private static Chat _privateChat;
        private static User _botUser;
        private static MessageEntity _italicTextEntity;
        private static MessageEntity _urlEntity;
        private static InlineKeyboardButton _testUrlButton;
        private static InlineKeyboardButton _testCallbackDataButton;
        private static InlineKeyboardMarkup _testInlineKeyboardMarkup;
    }
}