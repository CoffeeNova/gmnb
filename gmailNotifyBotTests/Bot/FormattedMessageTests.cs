using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoffeeJelly.gmailNotifyBot.Bot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.gmailNotifyBot.Tests;
using KellermanSoftware.CompareNetObjects;
using Google.Apis.Gmail.v1.Data;

namespace CoffeeJelly.gmailNotifyBot.Bot.Tests
{
    [TestClass()]
    public class FormattedGmailMessageTests
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
                // MembersToIgnore = new List<string> { "MessageId", "Date", "ForwardDate", "UpdateId" }
            };
            //_formattedMessage = new FormattedMessage
            //{
            //    Id = _id,
            //    ThreadId = _threadId,
            //    SenderName = _senderName,
            //    SenderEmail = _senderAddress,
            //    Body = new List<BodyForm>
            //        {
            //            _bodyPart1,
            //            _bodyPart2,
            //            _bodyPart3,
            //            _bodyPart4
            //        },
            //    Date = _date,
            //    Snippet = _snippet,
            //    Subject = _subject
            //};
        }

        [TestInitialize]
        public void TestInitialize()
        {
            if (TestContext.TestName == nameof(FormattedGmailMessage_2lvlPartedMessage_FormattedMessage))
            {
                _message = new Message
                {
                    #region sick message construction
                    Id = _id,
                    ThreadId = _threadId,
                    Snippet = _snippet,
                    Payload = new MessagePart
                    {
                        Headers = new List<MessagePartHeader>
                        {
                            new MessagePartHeader
                            {
                                Name = "From",
                                Value = _sender,
                            },
                            new MessagePartHeader
                            {
                                Name = "Subject",
                                Value = _subject,
                            },
                            new MessagePartHeader
                            {
                                Name = "Date",
                                Value = _date,
                            }
                        },
                        Parts = new List<MessagePart>
                        {
                            new MessagePart
                            {
                                Body = new MessagePartBody
                                {
                                    Data = Base64.Encode(_bodyPart1.Value)
                                },
                                MimeType = _bodyPart1.MimeType
                            },
                            new MessagePart
                            {
                                Parts = new List<MessagePart>
                                {
                                    new MessagePart
                                    {
                                        Body = new MessagePartBody
                                        {
                                            Data = Base64.Encode(_bodyPart2.Value)
                                        },
                                        MimeType = _bodyPart2.MimeType
                                    },
                                    new MessagePart
                                    {
                                        Body = new MessagePartBody
                                        {
                                            Data = Base64.Encode(_bodyPart3.Value)
                                        },
                                        MimeType = _bodyPart3.MimeType
                                    }
                                }
                            },
                            new MessagePart
                            {
                                Body = new MessagePartBody
                                {
                                    Data = Base64.Encode(_bodyPart4.Value)
                                },
                                MimeType = _bodyPart4.MimeType
                            }
                        }
                    }
                    #endregion
                };

            }
            if (TestContext.TestName ==nameof(FormattedGmailMessage_1lvlPartedMessage_FormattedMessage))
            {
                _message = new Message
                {
                    #region sick message construction
                    Id = _id,
                    ThreadId = _threadId,
                    Snippet = _snippet,
                    Payload = new MessagePart
                    {
                        Headers = new List<MessagePartHeader>
                        {
                            new MessagePartHeader
                            {
                                Name = "From",
                                Value = _sender,
                            },
                            new MessagePartHeader
                            {
                                Name = "Subject",
                                Value = _subject,
                            },
                            new MessagePartHeader
                            {
                                Name = "Date",
                                Value = _date,
                            }
                        },
                        Parts = new List<MessagePart>
                        {
                            new MessagePart
                            {
                                Body = new MessagePartBody
                                {
                                    Data = Base64.Encode(_bodyPart1.Value)
                                },
                                MimeType = _bodyPart1.MimeType
                            },
                            new MessagePart
                            {
                                Body = new MessagePartBody
                                {
                                    Data = Base64.Encode(_bodyPart2.Value)
                                },
                                MimeType = _bodyPart2.MimeType
                            },
                            new MessagePart
                            {
                                Body = new MessagePartBody
                                {
                                    Data = Base64.Encode(_bodyPart3.Value)
                                },
                                MimeType = _bodyPart3.MimeType
                            },
                            new MessagePart
                            {
                                Body = new MessagePartBody
                                {
                                    Data = Base64.Encode(_bodyPart4.Value)
                                },
                                MimeType = _bodyPart4.MimeType
                            }
                        }
                    }
                    #endregion
                };
            }
        }

        [TestMethod()]
        public void FormattedGmailMessage_2lvlPartedMessage_FormattedMessage()
        {
            var expected = _formattedMessage;
            var actual = new FormattedMessage(_message);
            var compaitLogic = new CompareLogic(_config);
            var result = compaitLogic.Compare(expected, actual);
            Assert.IsTrue(result.AreEqual, result.DifferencesString);
        }

        [TestMethod()]
        public void FormattedGmailMessage_1lvlPartedMessage_FormattedMessage()
        {
            var expected = _formattedMessage;
            var actual = new FormattedMessage(_message);
            var compaitLogic = new CompareLogic(_config);
            var result = compaitLogic.Compare(expected, actual);
            Assert.IsTrue(result.AreEqual, result.DifferencesString);
        }

        public TestContext TestContext { get; set; }

        private static string _id = "1";
        private static string _threadId = "1";
        private static string _snippet = "testSnippet";
        private static string _sender = "testSender <testaddr@gmail.com>";
        private static string _senderName = "testSender";
        private static string _senderAddress = "testaddr@gmail.com";
        private static string _subject = "testSubject";
        private static string _date = "testDate";
        private static BodyForm _bodyPart1 = new BodyForm("text/plain", "part1\r\npart1");
        private static BodyForm _bodyPart2 = new BodyForm("text/plain", "part2");
        private static BodyForm _bodyPart3 = new BodyForm("text/plain", "part3\r\npart3\r\npart3\r\n");
        private static BodyForm _bodyPart4 = new BodyForm("text/plain", "part4\r\n");
        private static ComparisonConfig _config;

        private Message _message;
        private static FormattedMessage _formattedMessage;
    }
}