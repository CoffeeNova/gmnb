using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoffeeJelly.gmailNotifyBot.Bot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;
using CoffeeJelly.gmailNotifyBot.Extensions;
using KellermanSoftware.CompareNetObjects;

namespace CoffeeJelly.gmailNotifyBot.Bot.Tests
{
    [TestClass]
    public class MessageBuilderTests
    {
        public TestContext TestContext { get; set; }

        private JToken _message;
        private static int _messageId = 100;
        private static Chat _privateChat;
        private static Chat _groupChat;
        private static Chat _superGroupChat;
        private static DateTime _date;
        private static User _user;
        private static User _user1;
        private static User _user2;
        private static List<User> _users;
        private string _text = @"http://www.translate.ru/";
        private static List<MessageEntity> _entities;
        private static MessageEntity _urlEntity;

        private static string _caption = "TestCaption";
        private static JToken _messageToken;
        private static ComparisonConfig _config;
        private static Audio _audio;
        private static Document _document;
        private static Sticker _sticker;
        private static PhotoSize _thumb;
        private static PhotoSize _photoSize1;
        private static PhotoSize _photoSize2;
        private static PhotoSize _photoSize3;
        private static List<PhotoSize> _photo;
        private static Video _video;
        private static Voice _voice;
        private static Contact _contact;
        private static Location _location;
        private static List<PhotoSize> _newChatPhoto;
        private static VoiceMessage _replyTo;
        private static Message _pinnedMessage;

        private static string _textMessageFileName = "SampleTextMessage.json";
        private static string _audioMessageFileName = "SampleAudioMessage.json";
        private static string _documentMessageFileName = "SampleDocumentMessage.json";
        private static string _stickerMessageFileName = "SampleStickerMessage.json";
        private static string _photoMessageFileName = "SamplePhotoMessage.json";
        private static string _gameMessageFileName = "SampleGameMessage.json";
        private static string _videoMessageFileName = "SampleVideoMessage.json";
        private static string _voiceMessageFileName = "SampleVoiceMessage.json";
        private static string _contactMessageFileName = "SampleContactMessage.json";

        private static string _locationMessageFileName = "SampleLocationMessage.json";
        private static string _venueMessageFileName = "SampleVenueMessage.json";
        private static string _newChatMemberMessageFileName = "SampleNewChatMemberMessage.json";
        private static string _leftChatMemberMessageFileName = "SampleLeftChatMemberMessage.json";
        private static string _newChatTitleMessageFileName = "SampleNewChatTitleMessage.json";
        private static string _newChatPhotoMessageFileName = "SampleNewChatPhotoMessage.json";
        private static string _migrateToChatIdMessageFileName = "SampleMigrateToChatIdMessage.json";
        private static string _migrateFromChatIdMessageFileName = "SampleMigrateFromChatIdMessage.json";
        private static string _forwardFromMessageFileName = "SampleForwardFromMessage.json";
        private static string _replyToMessageFileName = "SampleReplyToMessage.json";
        private static string _pinnedMessageFileName = "SamplePinnedMessage.json";
        private static string _forwardedVideoMessageFileName = "SampleForwardedVideoMessage.json";


        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            #region _privateChat

            _privateChat = new Chat
            {
                Id = 170181775,
                FirstName = "Coffee",
                LastName = "Jelly",
                UserName = "CoffeeJelly",
                Type = ChatType.Private
            };

            #endregion
            #region _groupChat

            _groupChat = new Chat
            {
                Id = -200694086,
                Title = "testgrp",
                Type = ChatType.Group,
                AllMembersAreAdministrators = true
            };

            #endregion
            #region _superGroupChat

            _superGroupChat = new Chat
            {
                Id = -1001076966401,
                Title = "testgrp",
                Type = ChatType.Supergroup,
            };

            #endregion
            #region _date
            _date = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(1490868721)).UtcDateTime;
            #endregion
            #region _user
            _user = new User
            {
                Id = 170181775,
                FirstName = "Coffee",
                LastName = "Jelly",
                Username = "CoffeeJelly"
            };
            #endregion
            #region _user1

            _user1 = new User
            {
                Id = 208070915,
                FirstName = "testuser1",
                Username = "testusername1"
            };

            #endregion
            #region _user2

            _user2 = new User
            {
                Id = 208070916,
                FirstName = "testuser2",
                Username = "testusername2"
            };

            #endregion
            #region _users

            _users = new List<User>
            {
                _user1,
                _user2
            };

            #endregion
            #region _urlEntity
            _urlEntity = new MessageEntity
            {
                Type = "url",
                Offset = 0,
                Lenght = 24
            };
            #endregion
            #region _entities
            _entities = new List<MessageEntity>
            {
                _urlEntity
            };
            #endregion
            #region _thumb
            _thumb = new PhotoSize
            {
                FileId = "AAQCABOLx4ENAAT8_8aMR6FJKZeyAAIC",
                FileSize = 5056,
                Width = 128,
                Height = 128
            };
            #endregion
            #region _photoSize1
            _photoSize1 = new PhotoSize
            {
                FileId = "AgADAgADvqcxG5h36EqtRLHi7G4IpthVtw0ABHZHxxRGwQQWJp4BAAEC",
                FileSize = 1104,
                Width = 90,
                Height = 66
            };
            #endregion
            #region _photoSize2
            _photoSize2 = new PhotoSize
            {
                FileId = "AgADAgADvqcxG5h36EqtRLHi7G4IpthVtw0ABJcP22BBjoQSJ54BAAEC",
                FileSize = 8848,
                Width = 270,
                Height = 199
            };
            #endregion
            #region _photoSize3
            _photoSize3 = new PhotoSize
            {
                FileId = "AgADAgADeKgxG4_EJAqofB1cykfvRrNJtw0ABAObNR5xowTDVqABAAEC",
                FileSize = 99685,
                Width = 640,
                Height = 640
            };
            #endregion
            #region _config
            _config = new ComparisonConfig
            {
                CompareChildren = true,
                CompareFields = false,
                CompareReadOnly = true,
                ComparePrivateFields = false,
                ComparePrivateProperties = false,
                CompareProperties = true,
                MaxDifferences = 50
            };
            #endregion
            #region _audio
            _audio = new Audio
            {
                Duration = 195,
                MimeType = "TestMimeType",
                Title = "TestTitle",
                Performer = "TestPerformer",
                FileId = "CQADAgADEgADmHfoSvnwZ3irGpuOAg",
                FileSize = 3125626
            };
            #endregion
            #region _document

            _document = new Document
            {
                FileName = "Test.PDF",
                MimeType = "application/pdf",
                FileId = "BQADAgADEwADmHfoSqJcnJwfHNTDAg",
                FileSize = 2972868
            };

            #endregion
            #region _sticker
            _sticker = new Sticker
            {
                Width = 512,
                Height = 512,
                Emoji = "TestEmoji",
                Thumb = _thumb,
                FileId = "CAADAgADdQEAAsxUSQlymelYJqPjNwI",
                FileSize = 31896
            };
            #endregion
            #region _photo

            _photo = new List<PhotoSize>
            {
                _photoSize1,
                _photoSize2
            };

            #endregion
            #region _video

            _video = new Video
            {
                Duration = 7,
                Width = 1280,
                Height = 720,
                Thumb = _thumb,
                FileId = "BAADAgADFwADmHfoSinMsvYz1GJpAg",
                FileSize = 827579
            };

            #endregion
            #region _voice

            _voice = new Voice
            {
                Duration = 3,
                MimeType = "audio/ogg",
                FileId = "AwADAgADFQADmHfoSt8RIvvK5Z47Ag",
                FileSize = 18020
            };

            #endregion
            #region _contact

            _contact = new Contact
            {
                PhoneNumber = "777777",
                FirstName = "tiny",
                LastName = "beast",
                UserId = 220793091
            };

            #endregion
            #region _location
            _location = new Location
            {
               Latitude = 53.901112F,
               Longitude = 27.562325F
            };
            #endregion
            #region _newChatPhoto

            _newChatPhoto = new List<PhotoSize>
            {
                _photoSize1,
                _photoSize2,
                _photoSize3
            };

            #endregion
            #region _replyTo

            _replyTo = new VoiceMessage
            {
                From = _user,
                Voice = _voice
            };
            AttachGeneralProperties(_replyTo);

            #endregion
            #region _pinnedMessage

            _pinnedMessage = new TextMessage
            {
                From = _user,
                Text = "testpinned"
            };
            AttachGeneralProperties3(_pinnedMessage);

            #endregion
        }

        [TestInitialize]
        public void TestInitialize()
        {
            #region big switch
            switch (TestContext.TestName)
            {
                case nameof(BuildMessage_TextJSON_TelegramTextMessage):
                    _messageToken = ReadJsonMessageFromFile(_textMessageFileName);
                    break;
                case nameof(BuildMessage_AudioJSON_TelegramAudioMessage):
                    _messageToken = ReadJsonMessageFromFile(_audioMessageFileName);
                    break;
                case nameof(BuildMessage_DocumentJSON_TelegramDocumentMessage):
                    _messageToken = ReadJsonMessageFromFile(_documentMessageFileName);
                    break;
                case nameof(BuildMessage_StickerJSON_TelegramStickerMessage):
                    _messageToken = ReadJsonMessageFromFile(_stickerMessageFileName);
                    break;
                case nameof(BuildMessage_PhotoJSON_TelegramPhotoMessage):
                    _messageToken = ReadJsonMessageFromFile(_photoMessageFileName);
                    break;
                //case nameof():
                //    _messageToken = ReadJsonMessageFromFile(_gameMessageFileName);
                //    break;
                case nameof(BuildMessage_VideoJSON_TelegramVideoMessage):
                    _messageToken = ReadJsonMessageFromFile(_videoMessageFileName);
                    break;
                case nameof(BuildMessage_VoiceJSON_TelegramVoiceMessage):
                    _messageToken = ReadJsonMessageFromFile(_voiceMessageFileName);
                    break;
                case nameof(BuildMessage_ContactJSON_TelegramContactMessage):
                    _messageToken = ReadJsonMessageFromFile(_contactMessageFileName);
                    break;
                case nameof(BuildMessage_LocationJSON_TelegramLocationMessage):
                    _messageToken = ReadJsonMessageFromFile(_locationMessageFileName);
                    break;
                //case nameof():
                //    _messageToken = ReadJsonMessageFromFile(_venueMessageFileName);
                //    break;
                case nameof(BuildMessage_NewChatMemberJSON_TelegramNewChatMemberMessage):
                    _messageToken = ReadJsonMessageFromFile(_newChatMemberMessageFileName);
                    break;
                case nameof(BuildMessage_LeftChatMemberJSON_TelegramLeftChatMemberMessage):
                    _messageToken = ReadJsonMessageFromFile(_leftChatMemberMessageFileName);
                    break;
                case nameof(BuildMessage_NewChatTitleJSON_TelegramNewChatTitleMessage):
                    _messageToken = ReadJsonMessageFromFile(_newChatTitleMessageFileName);
                    break;
                case nameof(BuildMessage_NewChatPhotoJSON_TelegramNewChatPhotoMessage):
                    _messageToken = ReadJsonMessageFromFile(_newChatPhotoMessageFileName);
                    break;
                case nameof(BuildMessage_MigrateToChatIdJSON_TelegramMigrateToChatIdMessage):
                    _messageToken = ReadJsonMessageFromFile(_migrateToChatIdMessageFileName);
                    break;
                case nameof(BuildMessage_MigrateFromChatIdJSON_TelegramMigrateFromChatIdMessage):
                    _messageToken = ReadJsonMessageFromFile(_migrateFromChatIdMessageFileName);
                    break;
                case nameof(BuildMessage_ForwardFromJSON_TelegraForwardFromMessage):
                    _messageToken = ReadJsonMessageFromFile(_forwardFromMessageFileName);
                    break;
                case nameof(BuildMessage_ReplyToJSON_TelegramReplyToMessage):
                    _messageToken = ReadJsonMessageFromFile(_replyToMessageFileName);
                    break;
                case nameof(BuildMessage_PinnedJSON_TelegramPinnedMessage):
                    _messageToken = ReadJsonMessageFromFile(_pinnedMessageFileName);
                    break;
                case nameof(BuildMessage_ForwardedVideoJSON_TelegramForwardedVideoMessage):
                    _messageToken = ReadJsonMessageFromFile(_forwardedVideoMessageFileName);
                    break;
                    #endregion
            }

        }

        private static void AttachGeneralProperties<T>(T message) where T : Message
        {
            message.MessageId = _messageId;
            message.Chat = _privateChat;
            message.Date = _date;
        }

        private static void AttachGeneralProperties2<T>(T message) where T : Message
        {
            message.MessageId = _messageId;
            message.Chat = _groupChat;
            message.Date = _date;
        }

        private static void AttachGeneralProperties3<T>(T message) where T : Message
        {
            message.MessageId = _messageId;
            message.Chat = _superGroupChat;
            message.Date = _date;
        }
        private static JToken ReadJsonMessageFromFile(string filename)
        {
            var path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\"));
            var sampleJsonRequest = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(path + @"\\TestFiles\\" + filename));
            return sampleJsonRequest["message"];
        }

        [TestMethod]
        public void BuildMessage_TextJSON_TelegramTextMessage()
        {
            var expected = new TextMessage
            {
                From = _user,
                Text = _text,
                Entities = _entities
            };
            AttachGeneralProperties(expected);
            var actual = MessageBuilder.BuildMessage<TextMessage>(_messageToken);

            var compareLogic = new CompareLogic(_config);
            var comparisonResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparisonResult.AreEqual, comparisonResult.DifferencesString);
        }


        [TestMethod]
        public void BuildMessage_AudioJSON_TelegramAudioMessage()
        {
            var expected = new AudioMessage
            {
                From = _user,
                Audio = _audio,
                Caption = _caption
            };
            AttachGeneralProperties(expected);
            var actual = MessageBuilder.BuildMessage<AudioMessage>(_messageToken);

            var compareLogic = new CompareLogic(_config);
            var comparisonResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparisonResult.AreEqual, comparisonResult.DifferencesString);
        }

        [TestMethod]
        public void BuildMessage_DocumentJSON_TelegramDocumentMessage()
        {
            var expected = new DocumentMessage
            {
                From = _user,
                Document = _document,
                Caption = _caption
            };
            AttachGeneralProperties(expected);
            var actual = MessageBuilder.BuildMessage<DocumentMessage>(_messageToken);
            var compareLogic = new CompareLogic(_config);
            var comparisonResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparisonResult.AreEqual, comparisonResult.DifferencesString);
        }

        [TestMethod]
        public void BuildMessage_StickerJSON_TelegramStickerMessage()
        {
            var expected = new StickerMessage
            {
                From = _user,
                Sticker = _sticker
            };
            AttachGeneralProperties(expected);
            var actual = MessageBuilder.BuildMessage<StickerMessage>(_messageToken);
            var compareLogic = new CompareLogic(_config);
            var comparisonResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparisonResult.AreEqual, comparisonResult.DifferencesString);
        }

        [TestMethod]
        public void BuildMessage_PhotoJSON_TelegramPhotoMessage()
        {
            var expected = new PhotoMessage
            {
                From = _user,
                Photo = _photo,
                Caption = _caption
            };
            AttachGeneralProperties(expected);
            var actual = MessageBuilder.BuildMessage<PhotoMessage>(_messageToken);
            var compareLogic = new CompareLogic(_config);
            var comparisonResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparisonResult.AreEqual, comparisonResult.DifferencesString);
        }

        [TestMethod]
        public void BuildMessage_VideoJSON_TelegramVideoMessage()
        {
            var expected = new VideoMessage
            {
                From = _user,
                Video = _video,
                Caption = _caption
            };
            AttachGeneralProperties(expected);
            var actual = MessageBuilder.BuildMessage<VideoMessage>(_messageToken);
            var compareLogic = new CompareLogic(_config);
            var comparisonResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparisonResult.AreEqual, comparisonResult.DifferencesString);
        }

        [TestMethod]
        public void BuildMessage_VoiceJSON_TelegramVoiceMessage()
        {
            var expected = new VoiceMessage
            {
                From = _user,
                Voice = _voice
            };
            AttachGeneralProperties(expected);
            var actual = MessageBuilder.BuildMessage<VoiceMessage>(_messageToken);
            var compareLogic = new CompareLogic(_config);
            var comparisonResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparisonResult.AreEqual, comparisonResult.DifferencesString);
        }

        [TestMethod]
        public void BuildMessage_ContactJSON_TelegramContactMessage()
        {
            var expected = new ContactMessage
            {
                From = _user,
                Contact = _contact
            };
            AttachGeneralProperties(expected);
            var actual = MessageBuilder.BuildMessage<ContactMessage>(_messageToken);
            var compareLogic = new CompareLogic(_config);
            var comparisonResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparisonResult.AreEqual, comparisonResult.DifferencesString);
        }

        [TestMethod]
        public void BuildMessage_LocationJSON_TelegramLocationMessage()
        {
            var expected = new LocationMessage
            {
                From = _user,
                Location = _location
            };
            AttachGeneralProperties(expected);
            var actual = MessageBuilder.BuildMessage<LocationMessage>(_messageToken);
            var compareLogic = new CompareLogic(_config);
            var comparisonResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparisonResult.AreEqual, comparisonResult.DifferencesString);
        }

        [TestMethod]
        public void BuildMessage_NewChatMemberJSON_TelegramNewChatMemberMessage()
        {
            var expected = new NewChatMemberMessage
            {
                From = _user,
                NewChatParticipant = _user1,
                NewChatMember = _user1,
                NewChatMembers = _users
            };
            AttachGeneralProperties2(expected);
            var actual = MessageBuilder.BuildMessage<NewChatMemberMessage>(_messageToken);
            var compareLogic = new CompareLogic(_config);
            var comparisonResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparisonResult.AreEqual, comparisonResult.DifferencesString);
        }

        [TestMethod]
        public void BuildMessage_LeftChatMemberJSON_TelegramLeftChatMemberMessage()
        {
            var expected = new LeftChatMemberMessage
            {
                From = _user,
                LeftChatParticipant = _user1,
                LeftChatMember = _user1,
            };
            AttachGeneralProperties2(expected);
            var actual = MessageBuilder.BuildMessage<LeftChatMemberMessage>(_messageToken);
            var compareLogic = new CompareLogic(_config);
            var comparisonResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparisonResult.AreEqual, comparisonResult.DifferencesString);
        }

        [TestMethod]
        public void BuildMessage_NewChatTitleJSON_TelegramNewChatTitleMessage()
        {
            var expected = new NewChatTitleMessage
            {
                From = _user,
                NewChatTitle = "testgrp"
            };
            AttachGeneralProperties2(expected);
            var actual = MessageBuilder.BuildMessage<NewChatTitleMessage>(_messageToken);
            var compareLogic = new CompareLogic(_config);
            var comparisonResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparisonResult.AreEqual, comparisonResult.DifferencesString);
        }

        [TestMethod]
        public void BuildMessage_NewChatPhotoJSON_TelegramNewChatPhotoMessage()
        {
            var expected = new NewChatPhotoMessage
            {
                From = _user,
                NewChatPhoto = _newChatPhoto
            };
            AttachGeneralProperties2(expected);
            var actual = MessageBuilder.BuildMessage<NewChatPhotoMessage>(_messageToken);
            var compareLogic = new CompareLogic(_config);
            var comparisonResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparisonResult.AreEqual, comparisonResult.DifferencesString);
        }

        [TestMethod]
        public void BuildMessage_MigrateToChatIdJSON_TelegramMigrateToChatIdMessage()
        {
            var expected = new MigrateToChatIdMessage
            {
                From = _user,
                MigrateToChatId = -1001076966401
            };
            AttachGeneralProperties2(expected);
            var actual = MessageBuilder.BuildMessage<MigrateToChatIdMessage>(_messageToken);
            var compareLogic = new CompareLogic(_config);
            var comparisonResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparisonResult.AreEqual, comparisonResult.DifferencesString);
        }

        [TestMethod]
        public void BuildMessage_MigrateFromChatIdJSON_TelegramMigrateFromChatIdMessage()
        {
            var expected = new MigrateFromChatIdMessage
            {
                From = _user,
                MigrateFromChatId = -200694086
            };
            AttachGeneralProperties3(expected);
            var actual = MessageBuilder.BuildMessage<MigrateFromChatIdMessage>(_messageToken);
            var compareLogic = new CompareLogic(_config);
            var comparisonResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparisonResult.AreEqual, comparisonResult.DifferencesString);
        }

        [TestMethod]
        public void BuildMessage_ForwardFromJSON_TelegraForwardFromMessage()
        {
            var expected = new TextMessage
            {
                From = _user,
                ForwardFrom = _user1,
                ForwardDate = _date,
                Text = ":D"
            };
            AttachGeneralProperties3(expected);
            var actual = MessageBuilder.BuildMessage<TextMessage>(_messageToken);
            var compareLogic = new CompareLogic(_config);
            var comparisonResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparisonResult.AreEqual, comparisonResult.DifferencesString);
        }

        [TestMethod]
        public void BuildMessage_ReplyToJSON_TelegramReplyToMessage()
        {
            var expected = new TextMessage
            {
                From = _user,
                ReplyToMessage = _replyTo,
                Text = "testreply"
            };
            AttachGeneralProperties(expected);
            var actual = MessageBuilder.BuildMessage<TextMessage>(_messageToken);
            var compareLogic = new CompareLogic(_config);
            var comparisonResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparisonResult.AreEqual, comparisonResult.DifferencesString);
        }

        [TestMethod]
        public void BuildMessage_PinnedJSON_TelegramPinnedMessage()
        {
            var expected = new PinnedMessage
            {
                From = _user,
                Message = _pinnedMessage
            };
            AttachGeneralProperties3(expected);
            var actual = MessageBuilder.BuildMessage<PinnedMessage>(_messageToken);
            var compareLogic = new CompareLogic(_config);
            var comparisonResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparisonResult.AreEqual, comparisonResult.DifferencesString);
        }

        [TestMethod]
        public void BuildMessage_ForwardedVideoJSON_TelegramForwardedVideoMessage()
        {
            var expected = new VideoMessage
            {
                From = _user,
                ForwardFrom = _user,
                ForwardDate = _date,
                Video = _video
            };
            AttachGeneralProperties3(expected);
            var actual = MessageBuilder.BuildMessage<VideoMessage>(_messageToken);
            var compareLogic = new CompareLogic(_config);
            var comparisonResult = compareLogic.Compare(expected, actual);

            Assert.IsTrue(comparisonResult.AreEqual, comparisonResult.DifferencesString);
        }
    }
}