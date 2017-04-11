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
    public class TelegramMethodsTests
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
            }
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            if (!TestContext.TestName.EqualsAny(nameof(GetMeTest), nameof(GetMe_TelegramMethodsGetMeException), nameof(GetMeAsyncTest)))
                _telegramMessagesCount++;
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
        [ExpectedException(typeof(TelegramMethodsException), "Should be exception, because wrong token")]
        public void GetMe_TelegramMethodsGetMeException()
        {
            var telegramMethods = new TelegramMethods("fakeToken");
            var actual = telegramMethods.GetMe();
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

            var actual = _telegramMethods.SendMessage(_privateChat.Id.ToString(), message);
            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        [TestMethod()]
        public void SendMessageAsync_TextMessageOnlyString_TextMessage()
        {
            var message = "test sendMessage";
            var expected = new TextMessage
            {
                Chat = _privateChat,
                Text = message,
                From = _botUser
            };

            var actual = _telegramMethods.SendMessageAsync(_privateChat.Id.ToString(), message).Result;
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

            var actual = _telegramMethods.SendMessage(_privateChat.Id.ToString(), "_" + message + "_", "Markdown");
            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        [TestMethod()]
        public void SendMessage_SilentMessage_TextMessage()
        {
            var message = "test silent sendMessage";
            var expected = new TextMessage
            {
                Chat = _privateChat,
                Text = message,
                From = _botUser
            };

            var actual = _telegramMethods.SendMessage(_privateChat.Id.ToString(), message, null, false, true);
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

            var actual = _telegramMethods.SendMessage(_privateChat.Id.ToString(), message, null, false, false, null, _testInlineKeyboardMarkup);
            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        [TestMethod()]
        public void SendMessage_ReplyKeyboardMarkupMessage_TextMessage()
        {
            var message = "test";
            var expected = new TextMessage
            {
                Chat = _privateChat,
                Text = message,
                From = _botUser,
            };

            var actual = _telegramMethods.SendMessage(_privateChat.Id.ToString(), message, null, false, false, null, _testReplyKeyboardMarkup);
            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        [TestMethod()]
        public void SendMessage_ReplyKeyboardRemove_TextMessage()
        {
            var message = "test";
            var expected = new TextMessage
            {
                Chat = _privateChat,
                Text = message,
                From = _botUser,
            };

            var actual = _telegramMethods.SendMessage(_privateChat.Id.ToString(), message, null, false, false, null, _testReplyKeyboardRemove);
            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        [TestMethod()]
        public void SendMessage_ForseReply_TextMessage()
        {
            var message = "ForseReply";
            var expected = new TextMessage
            {
                Chat = _privateChat,
                Text = message,
                From = _botUser,
            };

            var actual = _telegramMethods.SendMessage(_privateChat.Id.ToString(), message, null, false, false, null, _testForceReply);
            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        [TestMethod()]
        public void ForwardMessageTest()
        {
            var chatId = 170181775;
            var fromChatId = 170181775;
            var messageId = 1;
            var expected = new TextMessage
            {
                Chat = _privateChat,
                From = _botUser,
                Entities = new List<MessageEntity> { _botStartCommandEntity },
                Text = "/start",
                ForwardFrom = _user
            };

            var actual = _telegramMethods.ForwardMessage(chatId.ToString(), fromChatId.ToString(), messageId);
            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        [ExpectedException(typeof(TelegramMethodsException))]
        [TestMethod()]
        public void ForwardMessage_TelegramMethodsException()
        {
            var actual = _telegramMethods.ForwardMessage(_privateChat.Id.ToString(), _privateChat.Id.ToString(), int.MaxValue);
        }

        [TestMethod()]
        public void SendPhoto_PhotoName_TextMessage()
        {
            var expected = new TextMessage
            {
                Chat = _privateChat,
                From = _botUser
            };
            var actual = _telegramMethods.SendPhoto(_privateChat.Id.ToString(), _fullFileName).Result;

            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        [TestMethod()]
        public void SendPhoto_PhotoAndInlineKeyboard_TextMessage()
        {
            var text = "gabba-gabba-hey";
            var expected = new TextMessage
            {
                Chat = _privateChat,
                From = _botUser
            };
            var actual = _telegramMethods.SendPhoto(_privateChat.Id.ToString(), _fullFileName, text, true, null, _testInlineKeyboardMarkup).Result;

            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        [TestMethod()]
        public void SendPhotoByUri_PhotoUri_TextMessage()
        {
            var expected = new TextMessage
            {
                Chat = _privateChat,
                From = _botUser
            };
            var uri = new Uri(_photoUri);
            var actual = _telegramMethods.SendPhotoByUri(_privateChat.Id.ToString(), uri);

            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        [TestMethod()]
        public void SendPhotoByUriAsync_PhotoUriPlusInlineKeyboard_TextMessage()
        {
            var caption = "pict with keyboard";
            var expected = new TextMessage
            {
                Chat = _privateChat,
                From = _botUser
            };
            var uri = new Uri(_photoUri);
            var actual = _telegramMethods.SendPhotoByUriAsync(_privateChat.Id.ToString(), uri, caption, true, null, _testInlineKeyboardMarkup).Result;

            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        [TestMethod()]
        public void SendAudio_AudioName_TextMessage()
        {
            var expected = new TextMessage
            {
                Chat = _privateChat,
                From = _botUser
            };
            var actual = _telegramMethods.SendAudio(_privateChat.Id.ToString(), _fullFileName).Result;

            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        [TestMethod()]
        public void SendAudio_AudioWithParameters_TextMessage()
        {
            var caption = "mpthreetest.mp3";
            var duration = 12;
            var performer = "unknown";
            var title = "mpthreetest";

            var expected = new TextMessage
            {
                Chat = _privateChat,
                From = _botUser
            };
            var actual = _telegramMethods.SendAudio(_privateChat.Id.ToString(), _fullFileName, caption, duration, performer, title).Result;

            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        [TestMethod()]
        public void SendAudioByUri_AudioUri_TextMessage()
        {
            var expected = new TextMessage
            {
                Chat = _privateChat,
                From = _botUser
            };
            var uri = new Uri(_audioUri);
            var actual = _telegramMethods.SendAudioByUri(_privateChat.Id.ToString(), uri);

            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        [TestMethod()]
        public void SendAudioByUriAsync_AudioUriWithParams_TextMessage()
        {
            var caption = "mpthreetest.mp3";
            var duration = 12;
            var performer = "unknown";
            var title = "mpthreetest";

            var expected = new TextMessage
            {
                Chat = _privateChat,
                From = _botUser
            };
            var uri = new Uri(_audioUri);
            var actual = _telegramMethods.SendAudioByUriAsync(_privateChat.Id.ToString(), uri, caption, duration, performer, title).Result;

            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        [TestMethod()]
        public void SendDocument_DocumentName_TextMessage()
        {
            var expected = new TextMessage
            {
                Chat = _privateChat,
                From = _botUser
            };
            var actual = _telegramMethods.SendDocument(_privateChat.Id.ToString(), _fullFileName).Result;

            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        [TestMethod()]
        public void SendDocumentByUri_DocumentUri_TextMessage()
        {
            var uri = new Uri(_documentUri);
            var expected = new TextMessage
            {
                Chat = _privateChat,
                From = _botUser
            };
            var actual = _telegramMethods.SendDocumentByUri(_privateChat.Id.ToString(), uri);

            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        [TestMethod()]
        public void SendDocumentByUriAsync_DocumentUriPlusCaption_TextMessage()
        {
            var uri = new Uri(_documentUri);
            var caption = "Test caption";
            var expected = new TextMessage
            {
                Chat = _privateChat,
                From = _botUser
            };
            var actual = _telegramMethods.SendDocumentByUriAsync(_privateChat.Id.ToString(), uri, caption).Result;

            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        [TestMethod()]
        public void SendSticker_StickerName_TextMessage()
        {
            var expected = new TextMessage
            {
                Chat = _privateChat,
                From = _botUser
            };
            var actual = _telegramMethods.SendSticker(_privateChat.Id.ToString(), _fullFileName).Result;

            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        [TestMethod()]
        public void SendSticker_FileId_TextMessage()
        {
            var expected = new TextMessage
            {
                Chat = _privateChat,
                From = _botUser
            };
            var actual = _telegramMethods.SendSticker(_privateChat.Id.ToString(), _stickerId).Result;

            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

        [TestMethod()]
        public void SendStickerByUri_StickerUri_TextMessage()
        {
            var uri = new Uri(_stickerUri);
            var expected = new TextMessage
            {
                Chat = _privateChat,
                From = _botUser
            };
            var actual = _telegramMethods.SendStickerByUri(_privateChat.Id.ToString(), uri);

            var compareLogic = new CompareLogic(_config);
            var comparationResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparationResult.AreEqual, comparationResult.DifferencesString);
        }

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
        private static string _photoFileName = "gabba.jpg";
        private static string _audioFileName = "mpthreetest.mp3";
        private static string _documentFileName = "AutoUpdate.zip";
        private static string _stickerFileName = "sticker.webp";
        private static string _stickerId = "CAADAgADcwEAAsxUSQmpOO9BbxEBVgI";
        private static string _videoFileName = "video.mp4";

        private static string _photoUri =
            "https://upload.wikimedia.org/wikipedia/commons/thumb/0/0d/C_Sharp_wordmark.svg/117px-C_Sharp_wordmark.svg.png";
        private static string _audioUri =
            "https://ia802508.us.archive.org/5/items/testmp3testfile/mpthreetest.mp3";

        private static string _documentUri = "http://www.icecubesservice.com/TestDownloadDoc.pdf.zip/at_download/file";
        private static string _stickerUri = "https://www.gstatic.com/webp/gallery/1.webp";
        private static string _videoUri = "http://techslides.com/demos/sample-videos/small.mp4";

        private static int _telegramMessagesCount;
    }
}