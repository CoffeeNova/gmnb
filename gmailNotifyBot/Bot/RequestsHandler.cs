using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram;
using CoffeeJelly.gmailNotifyBot.Extensions;
using Newtonsoft.Json.Linq;
using NLog;

namespace CoffeeJelly.gmailNotifyBot.Bot
{
    /// <summary>
    /// This class acts as a subscriber <see cref="BotRequests.RequestsArrivedEvent"/>.
    /// Recognizes Telegram's messages and forms them as <see cref="TelegramMessage"/> objects.
    /// Triggers self events depending on the message type recieved.
    /// </summary>
    public class RequestsHandler
    {
        public RequestsHandler()
        {
            BotRequests.RequestsArrivedEvent += GmnbRequests_RequestsArrivedEvent;
        }

        private void GmnbRequests_RequestsArrivedEvent(IRequests requests)
        {
            //LogMaker.Log("testlog", false);
            //Console.WriteLine(requests.LastUpdateId);
            foreach (var request in requests.Requests)
            {
                DefineTelegramMessage(request["message"]);
            }
            var dr = new DateTime();



        }

        private ITelegramMessage DefineTelegramMessage(JToken messageToken)
        {
            string[] requestType = { "text", "sticker", "photo", "video", "document", "location", "contact", "voice" };
            var children = messageToken.Children();
            var messageBody = children.FirstOrDefault(j =>
            {
                var jProperty = j as JProperty;
                return jProperty != null && jProperty.Name.EqualsAny(requestType);
            });

            if (messageBody == null)
                throw new Exceptions.UnidentifiedTelegramMessageException();

            TelegramMessage telegramMessage;
            switch ((messageBody as JProperty).Name)
            {
                case "text":
                    FormTelegramMessage<TelegramMessage>(messageToken, out telegramMessage);
                    break;
            }

            return telegramMessage;
        }

        private void FormTelegramMessage<T>(JToken messageToken, out T telegramMessage) where T : TelegramMessage
        {
            if (messageToken == null)
                throw new Exceptions.UnidentifiedTelegramMessageException();
            // var jProperty = messageToken as JProperty;
            #region inner Action 
            var attachProperties = new Action<TelegramMessage>(message =>
            {
                var messageId = messageToken["message_id"] as JValue;
                if (messageId != null)
                    message.MessageId = (int)messageId.Value;
                var date = messageToken["date"] as JValue;
                if (date != null)
                {
                    var dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds((long)date.Value);
                    message.Date = dateTimeOffset.UtcDateTime;
                }
                var chat = messageToken["chat"] as JValue;
                if (chat != null)
                    message.Chat = new Chat
                    {
                        Id = (long)chat["id"],
                        Type = DefineChatType(chat["type"].ToString()),
                        //next are optional properties, should be verified by null value
                        Title = chat["title"]?.ToString(),
                        UserName = chat["username"]?.ToString(),
                        FirstName = chat["first_name"]?.ToString(),
                        LastName = chat["last_name"]?.ToString(),
                        AllMembersAreAdministrators = chat["all_members_are_administrators"] == null
                                                    ? null : (bool?)chat["all_members_are_administrators"],
                    };
            });
            #endregion

            telegramMessage = new TelegramMessage() as T;
            //form TelegramMessage:
            #region 1. as TelegramTextUserMessage
            if (typeof(T) == typeof(TelegramTextUserMessage))
            {
                telegramMessage = new TelegramTextUserMessage
                {
                    From = DefineUser(messageToken["from"]),
                    Text = messageToken["text"]?.ToString(),
                    Entities = DefineEntities(messageToken["entities"])
                } as T;
            }
            #endregion
            #region 2. as TelegramAudioMessage
            else if (typeof(T) == typeof(TelegramAudioMessage))
            {
                telegramMessage = new TelegramAudioMessage
                {
                    From = DefineUser(messageToken["from"]),
                    Audio = DefineAudio(messageToken["audio"]),
                } as T;
            }
            #endregion
            #region 3. as TelegramDocumentMessage
            else if (typeof(T) == typeof(TelegramDocumentMessage))
            {
                telegramMessage = new TelegramDocumentMessage
                {
                    From = DefineUser(messageToken["from"]),
                    Document = DefineDocument(messageToken["document"]),
                    Caption = messageToken["caption"]?.ToString()
                } as T;
            }
            #endregion
            #region 4. as TelegramPhotoMessage
            else if (typeof(T) == typeof(TelegramPhotoMessage))
            {
                telegramMessage = new TelegramPhotoMessage
                {
                    From = DefineUser(messageToken["from"]),
                    Photo = DefinePhoto(messageToken["photo"]),
                    Caption = messageToken["caption"]?.ToString()
                } as T;
            }
            #endregion
            #region 5. as TelegramGameMessage
            else if (typeof(T) == typeof(TelegramGameMessage))
            {
                telegramMessage = new TelegramGameMessage
                {
                    From = DefineUser(messageToken["from"]),
                    Game = DefineGame(messageToken["game"]),
                } as T;
            }
            #endregion
            #region 6. as TelegramVideoMessage
            else if (typeof(T) == typeof(TelegramVideoMessage))
            {
                telegramMessage = new TelegramVideoMessage
                {
                    From = DefineUser(messageToken["from"]),
                    Video = DefineVideo(messageToken["video"])
                } as T;
            }
            #endregion
            #region 7. as TelegramVoiceMessage
            #endregion
            #region 8. as TelegramContactMessage
            #endregion
            #region 9. as TelegramLocationMessage
            #endregion
            #region 10. as TelegramVenueMessage
            #endregion
            #region 11. as TelegramNewChatMemberMessage
            #endregion
            #region 12. as TelegramLeftChatMemberMessage
            #endregion
            #region 13. as TelegramNewChatTitleMessage
            #endregion
            #region 14. as TelegramNewChatPhotoMessage
            #endregion
            #region 15. as TelegramServiceMessage
            #endregion
            #region 16. as TelegramMigrateToChatIdMessage
            #endregion
            #region 17. as TelegramMigrateFromChatIdMessage
            #endregion
            #region 18. as TelegramPinnedMessage
            #endregion
            #region 19. as TelegramUnknownMessage
            #endregion
            attachProperties(telegramMessage);
        }

        private ChatType DefineChatType(string chatTypeStr)
        {
            ChatType chatType;
            switch (chatTypeStr)
            {
                case "private":
                    chatType = ChatType.Private;
                    break;
                case "group":
                    chatType = ChatType.Group;
                    break;
                case "supergroup":
                    chatType = ChatType.Supergroup;
                    break;
                case "channel":
                    chatType = ChatType.Channel;
                    break;
                default:
                    chatType = ChatType.Private;
                    break;
            }
            return chatType;
        }

        private User DefineUser(JToken from)
        {
            if (from == null) return null;
            return new User
            {
                Id = (int)from["id"],
                FirstName = from["first_name"].ToString(),
                //next are optional properties, should be verified by null value
                LastName = from["last_name"]?.ToString(),
                Username = from["username"]?.ToString()
            };
        }

        private List<MessageEntity> DefineEntities(JToken entities)
        {
            return entities?.Select(entity => new MessageEntity
            {
                Type = entity["type"].ToString(), Offset = (int) entity["offset"], Lenght = (int) entity["lenght"],
                //next are optional properties, should be verified by null value
                Url = entity["url"]?.ToString(), User = DefineUser(entity["user"])
            }).ToList();
        }

        public Audio DefineAudio(JToken audio)
        {
            if (audio == null) return null;

            return new Audio
            {
                FileId = audio["file_id"].ToString(),
                Duration = (int)audio["duration"],
                Performer = audio["performer"]?.ToString(),
                Title = audio["title"]?.ToString(),
                MimeType = audio["mime_type"]?.ToString(),
                FileSize = audio["file_size"] == null ? null : (int?)audio["file_size"]
            };
        }

        public List<PhotoSize> DefinePhoto(JToken photo)
        {
            return photo?.Select(photoSize => new PhotoSize
            {
                FileId = photoSize["file_id"].ToString(),
                Width = (int) photoSize["width"],
                Height = (int) photoSize["height"],
                FileSize = photoSize["file_size"] == null ? null : (int?) photoSize["file_size"]
            }).ToList();
        }

        public Document DefineDocument(JToken document)
        {
            if (document == null) return null;

            return new Document
            {
                FileId = document["file_id"].ToString(),
                Thumb = DefineThumb(document["thumb"]),
                FileName = document["file_name"]?.ToString(),
                MimeType = document["mime_type"]?.ToString(),
                FileSize = document["file_size"] == null ? null : (int?)document["file_size"]
            };
        }

        public PhotoSize DefineThumb(JToken thumb)
        {
            if (thumb == null) return null;

            return new PhotoSize
            {
                FileId = thumb["file_id"].ToString(),
                Width = (int)thumb["width"],
                Height = (int)thumb["height"],
                FileSize = thumb["file_size"] == null ? null : (int?)thumb["file_size"]
            };
        }

        public Game DefineGame(JToken game)
        {
            if (game == null) return null;

            return new Game
            {
                Title = game["title"].ToString(),
                Description = game["description"].ToString(),
                Photo = DefinePhoto(game["photo"]),
                Text = game["text"]?.ToString(),
                TextEntities = DefineTextEntites(game["text_entities"]),
                Animation = DefineAnimation(game["animation"])
            };
        }

        public List<MessageEntity> DefineTextEntites(JToken textEntities)
        {
            return textEntities?.Select(entity => new MessageEntity
            {
                Type = entity["type"].ToString(),
                Offset = (int)entity["offset"],
                Lenght = (int)entity["lenght"],
                Url = entity["url"]?.ToString(),
                User = DefineUser(entity["user"])
            }).ToList();
        }

        public Animation DefineAnimation(JToken animation)
        {
            if (animation == null) return null;

            return new Animation
            {
                FileId = animation["file_id"].ToString(),
                Thumb = DefineThumb(animation["thumb"]),
                FileName = animation["file_name"]?.ToString(),
                MimeType = animation["mime_type"]?.ToString(),
                FileSize = animation["file_size"] == null ? null : (int?)animation["file_size"]
            };
        }

        public Video DefineVideo(JToken video)
        {
            if (video == null) return null;

            return new Video
            {
                FileId = video["file_id"].ToString(),
                Width = (int)video["width"],
                Height = (int)video["height"],
                Duration = (int)video["duration"],
                Thumb = DefineThumb(video["thumb"]),
                MimeType = video["mime_type"]?.ToString(),
                FileSize = video["file_size"] == null ? null : (int?)video["file_size"]
            };
        }

        public void StopHandleRequests()
        {
            if (GmnbRequestsHandlerStopped) return;
            BotRequests.RequestsArrivedEvent -= GmnbRequests_RequestsArrivedEvent;
            GmnbRequestsHandlerStopped = true;
        }

        #region Command events callbacks

        private void GmnbRequestsHandler_AuthorizeCommandEvent()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region events

        private delegate void AuthorizeCommandEventHandler();


        private event AuthorizeCommandEventHandler AuthorizeCommandEvent;

        #endregion


        private const string TelegramBotToken = "252886092:AAHxtq8ZINX6WJXcT-MuQFoarH9-8Ppntl8";
        private BotRequests _gmnbRequests = new BotRequests(TelegramBotToken);

        private bool _anyCommandHandled;
        private bool _handleConnectCommand;
        private bool _handleConnectCommand2;


        private bool AnyCommandHandled
        {
            get { return _anyCommandHandled; }
            set
            {
                if (value)
                    _anyCommandHandled = true;
                if (!value && false.EqualsAll(HandleConnectCommand, _handleConnectCommand2))
                    _anyCommandHandled = false;
            }
        }

        public bool HandleConnectCommand
        {
            get { return _handleConnectCommand; }
            set
            {
                if (value != _handleConnectCommand)
                {
                    _handleConnectCommand = value;
                    AnyCommandHandled = value;
                    if (value)
                        AuthorizeCommandEvent += GmnbRequestsHandler_AuthorizeCommandEvent;
                    else
                        AuthorizeCommandEvent -= GmnbRequestsHandler_AuthorizeCommandEvent;
                }
            }
        }

        public bool HandleConnectCommand2
        {
            get { return _handleConnectCommand2; }
            set
            {
                if (value != _handleConnectCommand2)
                {
                    _handleConnectCommand2 = value;
                    if (value)
                        AuthorizeCommandEvent += GmnbRequestsHandler_AuthorizeCommandEvent;
                    else
                        AuthorizeCommandEvent -= GmnbRequestsHandler_AuthorizeCommandEvent;
                }
            }

        }

        public bool GmnbRequestsHandlerStopped { get; private set; }


        public static class LogMaker
        {
            public static void Log(string message, bool isError)
            {
                DateTime currentDate = DateTime.Now;
                Logger.Info(message);
                NewMessage?.Invoke(message, currentDate, isError);
            }

            public static void Log(Exception ex)
            {
                DateTime currentDate = DateTime.Now;
                Logger.Error(ex);
                NewMessage?.Invoke(ex.Message, currentDate, true);
            }

            public delegate void MessageDelegate(string message, DateTime time, bool isError);
            public static event BotRequests.LogMaker.MessageDelegate NewMessage;
            private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        }
    }

    public enum RequestsType
    {
        Text,
        Sticker,
        Photo,
        Video,
        Document,
        Location,
        Contact,
        Voice
    }
}