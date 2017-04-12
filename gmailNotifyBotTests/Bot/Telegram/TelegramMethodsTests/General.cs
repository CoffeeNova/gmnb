﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            _photoSize1 = new PhotoSize
            {
                FileId = "AgADAgADqqcxG4_EJAo-Knthid_Ygy4XSA0ABHFCoFI0_mWuqGEBAAEC",
                FileSize = 10311,
                Width = 160,
                Height = 160
            };
            _photoSize2 = new PhotoSize
            {
                FileId = "AgADAgADqqcxG4_EJAo-Knthid_Ygy4XSA0ABII7yIDVjT7yqWEBAAEC",
                FileSize = 22094,
                Width = 320,
                Height = 320
            };
            _photoSize3 = new PhotoSize
            {
                FileId = "AgADAgADqqcxG4_EJAo-Knthid_Ygy4XSA0ABEN3DhgGCbBFqmEBAAEC",
                FileSize = 57644,
                Width = 640,
                Height = 640
            };
            _userProfilePhotos = new UserProfilePhotos
            {
                TotalCount = 1,
                Photos = new List<List<PhotoSize>>
                {
                    new List<PhotoSize>
                    {
                        _photoSize1,
                        _photoSize2,
                        _photoSize3
                    }
                }
            };
            _file = new File
            {
                FileId = "CAADAgADLgADk35wS_5j0ImZMegiAg";
            };
        }

        [ClassCleanup]
        public static void ClassCleanUp()
        {
        }

        [TestInitialize]
        public void TestInitialize()
        {
           var testFilesPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\")) + "TestFiles\\";
           var testStorageFilesPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\")) + "TestStorage\\";
            switch (TestContext.TestName)
            {
                case nameof(SendPhoto_PhotoName_TextMessage):
                    _fullFileName = testFilesPath + _photoFileName;
                    break;
                case nameof(SendPhoto_PhotoAndInlineKeyboard_TextMessage):
                    _fullFileName = testFilesPath + _photoFileName;
                    break;
                case nameof(SendAudio_AudioName_TextMessage):
                    _fullFileName = testFilesPath + _audioFileName;
                    break;
                case nameof(SendAudio_AudioWithParameters_TextMessage):
                    _fullFileName = testFilesPath + _audioFileName;
                    break;
                case nameof(SendDocument_DocumentName_TextMessage):
                    _fullFileName = testFilesPath + _documentFileName;
                    break;
                case nameof(SendSticker_StickerName_TextMessage):
                    _fullFileName = testFilesPath + _stickerFileName;
                    break;
                case nameof(SendVideo_VideoWithParameters_TextMessage):
                    _fullFileName = testFilesPath + _videoFileName;
                    break;
                case nameof(SendVoice_VoiceWithParameters_TextMessage):
                    _fullFileName = testFilesPath + _voiceFileName;
                    break;
                case nameof(GetFile_FileId_File):
                    _config.MembersToIgnore.Add("FilePath");
                    _config.MembersToIgnore.Add("FilePathCreated");
                    _config.MembersToIgnore.Add("FileSize");
                    break;
                case nameof(DownloadFile_File_File):
                    _fullFileName = testStorageFilesPath;
                    break;
            }
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            if (!TestContext.TestName.EqualsAny(nameof(GetMeTest), nameof(GetMe_TelegramMethodsGetMeException), nameof(GetMeAsyncTest)))
                _telegramMessagesCount++;

            switch (TestContext.TestName)
            {
                case nameof(GetFile_FileId_File):
                    _config.MembersToIgnore.Remove("FilePath");
                    _config.MembersToIgnore.Remove("FilePathCreated");
                    _config.MembersToIgnore.Remove("FileSize");
                    break;
            }
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
        private static UserProfilePhotos _userProfilePhotos;
        private static int _telegramMessagesCount;
        private static PhotoSize _photoSize1;
        private static PhotoSize _photoSize2;
        private static PhotoSize _photoSize3;
        private static File _file;
    }
}