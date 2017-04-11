using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram.Exceptions;
using KellermanSoftware.CompareNetObjects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CoffeeJelly.gmailNotifyBot.Extensions;

namespace CoffeeJelly.gmailNotifyBot.Bot.Telegram.Tests
{
    [TestClass()]
    public partial class TelegramMethodsTests
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _telegramMessagesCount = 0;
            _config = new ComparisonConfig
            {
                CompareChildren = true,
                CompareFields = false,
                CompareReadOnly = true,
                ComparePrivateFields = false,
                ComparePrivateProperties = false,
                CompareProperties = true,
                MaxDifferences = 50,
                MembersToIgnore = new List<string> { "MessageId", "Date", "ForwardDate" }
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
            _testTextKeyboardButton = new KeyboardButton
            {
                Text = "Test keyboard button"
            };
            _testContactKeyboardButton = new KeyboardButton
            {
                Text = "Test contact keyboard button",
                RequestContact = true

            };
            _testReplyKeyboardMarkup = new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                    new List<KeyboardButton>
                    {
                        _testTextKeyboardButton,
                        _testContactKeyboardButton,
                    },
                    new List<KeyboardButton>
                    {
                        _testContactKeyboardButton
                    }
                }
            };
            _testReplyKeyboardRemove = new ReplyKeyboardRemove();
            _testForceReply = new ForceReply();
            _botStartCommandEntity = new MessageEntity
            {
                Type = "bot_command",
                Offset = 0,
                Length = 6
            };
            _user = new User
            {
                Id = 170181775,
                FirstName = "Coffee",
                LastName = "Jelly",
                Username = "CoffeeJelly"
            };
            _location = new Location
            {
                Latitude = 53.901112F,
                Longitude = 27.562325F
            };
        }

        [ClassCleanup]
        public static void ClassCleanUp()
        {
        }

        [TestInitialize]
        public void TestInitialize()
        {
           var path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\")) + "TestFiles\\";
            switch (TestContext.TestName)
            {
                case nameof(SendPhoto_PhotoName_TextMessage):
                    _fullFileName = path + _photoFileName;
                    break;
                case nameof(SendPhoto_PhotoAndInlineKeyboard_TextMessage):
                    _fullFileName = path + _photoFileName;
                    break;
                case nameof(SendAudio_AudioName_TextMessage):
                    _fullFileName = path + _audioFileName;
                    break;
                case nameof(SendAudio_AudioWithParameters_TextMessage):
                    _fullFileName = path + _audioFileName;
                    break;
                case nameof(SendDocument_DocumentName_TextMessage):
                    _fullFileName = path + _documentFileName;
                    break;
                case nameof(SendSticker_StickerName_TextMessage):
                    _fullFileName = path + _stickerFileName;
                    break;
                case nameof(SendVideo_VideoWithParameters_TextMessage):
                    _fullFileName = path + _videoFileName;
                    break;
                case nameof(SendVoice_VoiceWithParameters_TextMessage):
                    _fullFileName = path + _voiceFileName;
                    break;
            }
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            if (!TestContext.TestName.EqualsAny(nameof(GetMeTest), nameof(GetMe_TelegramMethodsGetMeException), nameof(GetMeAsyncTest)))
                _telegramMessagesCount++;
        }

        public TestContext TestContext { get; set; }

        private static ComparisonConfig _config;
        private static TelegramMethods _telegramMethods;
        private static Chat _privateChat;
        private static User _botUser;
        private static User _user;
        private static MessageEntity _italicTextEntity;
        private static MessageEntity _urlEntity;
        private static MessageEntity _botStartCommandEntity;
        private static InlineKeyboardButton _testUrlButton;
        private static InlineKeyboardButton _testCallbackDataButton;
        private static KeyboardButton _testTextKeyboardButton;
        private static KeyboardButton _testContactKeyboardButton;
        private static InlineKeyboardMarkup _testInlineKeyboardMarkup;
        private static ReplyKeyboardMarkup _testReplyKeyboardMarkup;
        private static ReplyKeyboardRemove _testReplyKeyboardRemove;
        private static ForceReply _testForceReply;
        private static string _fullFileName;
        private static Location _location;
        private static int _telegramMessagesCount;
    }
}