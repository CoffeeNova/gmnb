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
            BotRequests.RequestsArrivedEvent += RequestsArrivedEvent;
        }

        private void RequestsArrivedEvent(IRequests requests)
        {
            foreach (var request in requests.Requests)
            {
               dynamic message = MessageBuilder.BuildMessage(request["message"]);
                #region raising events
                if(message.GetType() == typeof(TelegramTextMessage))
                    TelegramTextMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(TelegramAudioMessage))
                    TelegramAudioMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(TelegramDocumentMessage))
                    TelegramDocumentMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(TelegramStickerMessage))
                    TelegramStickerMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(TelegramPhotoMessage))
                    TelegramPhotoMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(TelegramGameMessage))
                    TelegramGameMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(TelegramVideoMessage))
                    TelegramVideoMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(TelegramVoiceMessage))
                    TelegramVoiceMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(TelegramContactMessage))
                    TelegramContactMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(TelegramLocationMessage))
                    TelegramLocationMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(TelegramVenueMessage))
                    TelegramVenueMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(TelegramNewChatMemberMessage))
                    TelegramNewChatMemberMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(TelegramLeftChatMemberMessage))
                    TelegramLeftChatMemberMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(TelegramNewChatTitleMessage))
                    TelegramNewChatTitleMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(TelegramNewChatPhotoMessage))
                    TelegramNewChatPhotoMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(TelegramMigrateToChatIdMessage))
                    TelegramMigrateToChatIdMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(TelegramMigrateFromChatIdMessage))
                    TelegramMigrateFromChatIdMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(TelegramPinnedMessage))
                    TelegramPinnedMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(TelegramUnknownMessage))
                    TelegramUnknownMessageEvent?.Invoke(message);
                #endregion
            }

        }

        public void StopHandleRequests()
        {
            if (RequestsHandlerStopped) return;
            BotRequests.RequestsArrivedEvent -= RequestsArrivedEvent;
            RequestsHandlerStopped = true;
        }

        public void ResumeHandleRequests()
        {
            if (!RequestsHandlerStopped) return;
            BotRequests.RequestsArrivedEvent += RequestsArrivedEvent;
            RequestsHandlerStopped = false;
        }
        #region events

        public delegate void TelegramMessageEventHandler<in T>(T message);

        public event TelegramMessageEventHandler<TelegramTextMessage> TelegramTextMessageEvent;
        public event TelegramMessageEventHandler<TelegramAudioMessage> TelegramAudioMessageEvent;
        public event TelegramMessageEventHandler<TelegramDocumentMessage> TelegramDocumentMessageEvent;
        public event TelegramMessageEventHandler<TelegramStickerMessage> TelegramStickerMessageEvent;
        public event TelegramMessageEventHandler<TelegramPhotoMessage> TelegramPhotoMessageEvent;
        public event TelegramMessageEventHandler<TelegramGameMessage> TelegramGameMessageEvent;
        public event TelegramMessageEventHandler<TelegramVideoMessage> TelegramVideoMessageEvent;
        public event TelegramMessageEventHandler<TelegramVoiceMessage> TelegramVoiceMessageEvent;
        public event TelegramMessageEventHandler<TelegramContactMessage> TelegramContactMessageEvent;
        public event TelegramMessageEventHandler<TelegramLocationMessage> TelegramLocationMessageEvent;
        public event TelegramMessageEventHandler<TelegramVenueMessage> TelegramVenueMessageEvent;
        public event TelegramMessageEventHandler<TelegramNewChatMemberMessage> TelegramNewChatMemberMessageEvent;
        public event TelegramMessageEventHandler<TelegramLeftChatMemberMessage> TelegramLeftChatMemberMessageEvent;
        public event TelegramMessageEventHandler<TelegramNewChatTitleMessage> TelegramNewChatTitleMessageEvent;
        public event TelegramMessageEventHandler<TelegramNewChatPhotoMessage> TelegramNewChatPhotoMessageEvent;
        public event TelegramMessageEventHandler<TelegramMigrateToChatIdMessage> TelegramMigrateToChatIdMessageEvent;
        public event TelegramMessageEventHandler<TelegramMigrateFromChatIdMessage> TelegramMigrateFromChatIdMessageEvent;
        public event TelegramMessageEventHandler<TelegramPinnedMessage> TelegramPinnedMessageEvent;
        public event TelegramMessageEventHandler<TelegramUnknownMessage> TelegramUnknownMessageEvent;
        #endregion

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        //private const string TelegramBotToken = "252886092:AAHxtq8ZINX6WJXcT-MuQFoarH9-8Ppntl8";
        //private bool _anyCommandHandled;
        //private bool _handleConnectCommand;
        //private bool _handleConnectCommand2;


        //private bool AnyCommandHandled
        //{
        //    get { return _anyCommandHandled; }
        //    set
        //    {
        //        if (value)
        //            _anyCommandHandled = true;
        //        if (!value && false.EqualsAll(HandleConnectCommand, _handleConnectCommand2))
        //            _anyCommandHandled = false;
        //    }
        //}

        //public bool HandleConnectCommand
        //{
        //    get { return _handleConnectCommand; }
        //    set
        //    {
        //        if (value != _handleConnectCommand)
        //        {
        //            _handleConnectCommand = value;
        //            AnyCommandHandled = value;
        //            if (value)
        //                AuthorizeCommandEvent += GmnbRequestsHandler_AuthorizeCommandEvent;
        //            else
        //                AuthorizeCommandEvent -= GmnbRequestsHandler_AuthorizeCommandEvent;
        //        }
        //    }
        //}

        //public bool HandleConnectCommand2
        //{
        //    get { return _handleConnectCommand2; }
        //    set
        //    {
        //        if (value != _handleConnectCommand2)
        //        {
        //            _handleConnectCommand2 = value;
        //            if (value)
        //                AuthorizeCommandEvent += GmnbRequestsHandler_AuthorizeCommandEvent;
        //            else
        //                AuthorizeCommandEvent -= GmnbRequestsHandler_AuthorizeCommandEvent;
        //        }
        //    }

        //}

        public bool RequestsHandlerStopped { get; private set; }




        private static class MessageBuilder
        {
            public static T Build<T>(JToken messageToken) where T : TelegramMessage
            {
                if (messageToken == null)
                    throw new Exceptions.TelegramMessageIsUnidentifiedException();

                #region inner Action 
                var attachProperties = new Action<TelegramMessage>(message =>
                {
                    var messageId = messageToken["message_id"] as JValue;
                    if (messageId != null)
                        message.MessageId = Convert.ToInt32(messageId.Value);
                    var date = messageToken["date"] as JValue;
                    if (date != null)
                    {
                        var dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(date.Value));
                        message.Date = dateTimeOffset.UtcDateTime;
                    }
                    var chat = messageToken["chat"] as JValue;
                    if (chat != null)
                        message.Chat = new Chat
                        {
                            Id = Convert.ToInt64(chat["id"]),
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

                TelegramMessage telegramMessage;

                #region 1. as TelegramTextUserMessage
                if (typeof(T) == typeof(TelegramTextMessage))
                {
                    telegramMessage = new TelegramTextMessage
                    {
                        From = BuildUser(messageToken["from"]),
                        Text = messageToken["text"]?.ToString(),
                        Entities = BuildEntities(messageToken["entities"])
                    } as T;
                }
                #endregion
                #region 2. as TelegramAudioMessage
                else if (typeof(T) == typeof(TelegramAudioMessage))
                {
                    telegramMessage = new TelegramAudioMessage
                    {
                        From = BuildUser(messageToken["from"]),
                        Audio = BuildAudio(messageToken["audio"]),
                    } as T;
                }
                #endregion
                #region 3. as TelegramDocumentMessage
                else if (typeof(T) == typeof(TelegramDocumentMessage))
                {
                    telegramMessage = new TelegramDocumentMessage
                    {
                        From = BuildUser(messageToken["from"]),
                        Document = BuildDocument(messageToken["document"]),
                        Caption = messageToken["caption"]?.ToString()
                    } as T;
                }
                #endregion
                #region 4 as TelegramStickerMessage
                else if (typeof(T) == typeof(TelegramStickerMessage))
                {
                    telegramMessage = new TelegramStickerMessage
                    {
                        From = BuildUser(messageToken["from"]),
                        Sticker = BuildSticker(messageToken["sticker"])
                    } as T;
                }
                #endregion
                #region 5. as TelegramPhotoMessage
                else if (typeof(T) == typeof(TelegramPhotoMessage))
                {
                    telegramMessage = new TelegramPhotoMessage
                    {
                        From = BuildUser(messageToken["from"]),
                        Photo = BuildPhoto(messageToken["photo"]),
                        Caption = messageToken["caption"]?.ToString()
                    } as T;
                }
                #endregion
                #region 6. as TelegramGameMessage
                else if (typeof(T) == typeof(TelegramGameMessage))
                {
                    telegramMessage = new TelegramGameMessage
                    {
                        From = BuildUser(messageToken["from"]),
                        Game = BuildGame(messageToken["game"]),
                    } as T;
                }
                #endregion
                #region 7. as TelegramVideoMessage
                else if (typeof(T) == typeof(TelegramVideoMessage))
                {
                    telegramMessage = new TelegramVideoMessage
                    {
                        From = BuildUser(messageToken["from"]),
                        Video = BuildVideo(messageToken["video"])
                    } as T;
                }
                #endregion
                #region 8. as TelegramVoiceMessage
                else if (typeof(T) == typeof(TelegramVoiceMessage))
                {
                    telegramMessage = new TelegramVoiceMessage
                    {
                        From = BuildUser(messageToken["from"]),
                        Voice = BuildVoice(messageToken["voice"])
                    } as T;
                }
                #endregion
                #region 9. as TelegramContactMessage
                else if (typeof(T) == typeof(TelegramContactMessage))
                {
                    telegramMessage = new TelegramContactMessage
                    {
                        From = BuildUser(messageToken["from"]),
                        Contact = BuildContact(messageToken["contact"])
                    } as T;
                }
                #endregion
                #region 10. as TelegramLocationMessage
                else if (typeof(T) == typeof(TelegramLocationMessage))
                {
                    telegramMessage = new TelegramLocationMessage
                    {
                        From = BuildUser(messageToken["from"]),
                        Location = BuildLocation(messageToken["location"])
                    } as T;
                }
                #endregion
                #region 11. as TelegramVenueMessage
                else if (typeof(T) == typeof(TelegramVenueMessage))
                {
                    telegramMessage = new TelegramVenueMessage
                    {
                        From = BuildUser(messageToken["from"]),
                        Venue = BuildVenue(messageToken["venue"])
                    } as T;
                }
                #endregion
                #region 12. as TelegramNewChatMemberMessage
                else if (typeof(T) == typeof(TelegramNewChatMemberMessage))
                {
                    telegramMessage = new TelegramNewChatMemberMessage
                    {
                        NewChatMember = BuildUser(messageToken["new_chat_member"])
                    } as T;
                }
                #endregion
                #region 13. as TelegramLeftChatMemberMessage
                else if (typeof(T) == typeof(TelegramLeftChatMemberMessage))
                {
                    telegramMessage = new TelegramLeftChatMemberMessage
                    {
                        LeftChatMember = BuildUser(messageToken["left_chat_member"])
                    } as T;
                }
                #endregion
                #region 14. as TelegramNewChatTitleMessage
                else if (typeof(T) == typeof(TelegramNewChatTitleMessage))
                {
                    telegramMessage = new TelegramNewChatTitleMessage
                    {
                        NewChatTitle = messageToken["new_chat_title"].ToString()
                    } as T;
                }
                #endregion
                #region 15. as TelegramNewChatPhotoMessage
                else if (typeof(T) == typeof(TelegramNewChatPhotoMessage))
                {
                    telegramMessage = new TelegramNewChatPhotoMessage
                    {
                        NewChatPhoto = BuildPhoto(messageToken["new_chat_photo"])
                    } as T;
                }
                #endregion
                #region 16. as TelegramMigrateToChatIdMessage
                else if (typeof(T) == typeof(TelegramMigrateToChatIdMessage))
                {
                    telegramMessage = new TelegramMigrateToChatIdMessage
                    {
                        MigrateToChatId = Convert.ToInt64(messageToken["migrate_to_chat_id"])
                    } as T;
                }
                #endregion
                #region 17. as TelegramMigrateFromChatIdMessage
                else if (typeof(T) == typeof(TelegramMigrateFromChatIdMessage))
                {
                    telegramMessage = new TelegramMigrateFromChatIdMessage
                    {
                        MigrateFromChatId = Convert.ToInt64(messageToken["migrate_from_chat_id"])
                    } as T;
                }
                #endregion
                #region 18. as TelegramPinnedMessage
                else if (typeof(T) == typeof(TelegramPinnedMessage))
                {
                    telegramMessage = new TelegramPinnedMessage
                    {
                        PinnedMessage = BuildMessage(messageToken["pinned_message"])
                    } as T;
                }
                #endregion
                #region 19. as TelegramUnknownMessage
                else
                {
                    telegramMessage = new TelegramUnknownMessage() as T;
                }
                #endregion
                attachProperties(telegramMessage);

                return (T)telegramMessage;
            }

            private static ChatType DefineChatType(string chatTypeStr)
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

            #region builders

            public static dynamic BuildMessage(JToken messageToken)
            {
                string[] requestType =
                {
                    #region array of request
                    "text",
                    "audio",
                    "document",
                    "sticker",
                    "photo",
                    "game",
                    "video",
                    "voice",
                    "contact",
                    "location",
                    "venue",
                    "new_chat_member",
                    "left_chat_member",
                    "new_chat_title",
                    "new_chat_photo",
                    "migrate_to_chat_id",
                    "migrate_from_chat_id",
                    "pinned_message"
                    #endregion
                };
                var children = messageToken.Children();
                var messageBody = children.FirstOrDefault(j =>
                {
                    var jProperty = j as JProperty;
                    return jProperty != null && jProperty.Name.EqualsAny(requestType);
                });

                if (messageBody == null)
                    throw new Exceptions.TelegramMessageIsUnidentifiedException();

                TelegramMessage telegramMessage;
                switch ((messageBody as JProperty).Name)
                {
                    #region cases
                    case "text":
                        telegramMessage = Build<TelegramTextMessage>(messageToken);
                        break;
                    case "audio":
                        telegramMessage = Build<TelegramAudioMessage>(messageToken);
                        break;
                    case "document":
                        telegramMessage = Build<TelegramDocumentMessage>(messageToken);
                        break;
                    case "sticker":
                        telegramMessage = Build<TelegramStickerMessage>(messageToken);
                        break;
                    case "photo":
                        telegramMessage = Build<TelegramPhotoMessage>(messageToken);
                        break;
                    case "game":
                        telegramMessage = Build<TelegramGameMessage>(messageToken);
                        break;
                    case "video":
                        telegramMessage = Build<TelegramVideoMessage>(messageToken);
                        break;
                    case "voice":
                        telegramMessage = Build<TelegramVoiceMessage>(messageToken);
                        break;
                    case "contact":
                        telegramMessage = Build<TelegramContactMessage>(messageToken);
                        break;
                    case "location":
                        telegramMessage = Build<TelegramLocationMessage>(messageToken);
                        break;
                    case "venue":
                        telegramMessage = Build<TelegramVenueMessage>(messageToken);
                        break;
                    case "new_chat_member":
                        telegramMessage = Build<TelegramNewChatMemberMessage>(messageToken);
                        break;
                    case "left_chat_member":
                        telegramMessage = Build<TelegramLeftChatMemberMessage>(messageToken);
                        break;
                    case "new_chat_title":
                        telegramMessage = Build<TelegramNewChatTitleMessage>(messageToken);
                        break;
                    case "new_chat_photo":
                        telegramMessage = Build<TelegramNewChatPhotoMessage>(messageToken);
                        break;
                    case "migrate_to_chat_id":
                        telegramMessage = Build<TelegramMigrateToChatIdMessage>(messageToken);
                        break;
                    case "migrate_from_chat_id":
                        telegramMessage = Build<TelegramMigrateFromChatIdMessage>(messageToken);
                        break;
                    case "pinned_message":
                        telegramMessage = Build<TelegramPinnedMessage>(messageToken);
                        break;
                    default:
                        telegramMessage = Build<TelegramUnknownMessage>(messageToken);
                        break;
                        #endregion
                }

                return telegramMessage;
            }

            private static User BuildUser(JToken from)
            {
                if (from == null) return null;
                return new User
                {
                    Id = Convert.ToInt32(from["id"]),
                    FirstName = from["first_name"].ToString(),
                    //next are optional properties, should be verified by null value
                    LastName = from["last_name"]?.ToString(),
                    Username = from["username"]?.ToString()
                };
            }

            private static List<MessageEntity> BuildEntities(JToken entities)
            {
                return entities?.Select(entity => new MessageEntity
                {
                    Type = entity["type"].ToString(),
                    Offset = Convert.ToInt32(entity["offset"]),
                    Lenght = Convert.ToInt32(entity["lenght"]),
                    //next are optional properties, should be verified by null value
                    Url = entity["url"]?.ToString(),
                    User = BuildUser(entity["user"])
                }).ToList();
            }

            private static Audio BuildAudio(JToken audio)
            {
                if (audio == null) return null;

                return new Audio
                {
                    FileId = audio["file_id"].ToString(),
                    Duration = Convert.ToInt32(audio["duration"]),
                    Performer = audio["performer"]?.ToString(),
                    Title = audio["title"]?.ToString(),
                    MimeType = audio["mime_type"]?.ToString(),
                    FileSize = audio["file_size"] == null ? null : new int?(Convert.ToInt32(audio["file_size"]))
                };
            }

            private static List<PhotoSize> BuildPhoto(JToken photo)
            {
                return photo?.Select(photoSize => new PhotoSize
                {
                    FileId = photoSize["file_id"].ToString(),
                    Width = Convert.ToInt32(photoSize["width"]),
                    Height = Convert.ToInt32(photoSize["height"]),
                    FileSize = photoSize["file_size"] == null ? null : new int?(Convert.ToInt32(photoSize["file_size"]))
                }).ToList();
            }

            private static Document BuildDocument(JToken document)
            {
                if (document == null) return null;

                return new Document
                {
                    FileId = document["file_id"].ToString(),
                    Thumb = BuildThumb(document["thumb"]),
                    FileName = document["file_name"]?.ToString(),
                    MimeType = document["mime_type"]?.ToString(),
                    FileSize = document["file_size"] == null ? null : new int?(Convert.ToInt32(document["file_size"]))
                };
            }

            private static Sticker BuildSticker(JToken sticker)
            {
                if (sticker == null) return null;

                return new Sticker
                {
                    FileId = sticker["file_id"].ToString(),
                    Width = Convert.ToInt32(sticker["width"]),
                    Height = Convert.ToInt32(sticker["height"]),
                    Thumb = BuildThumb(sticker["thumb"]),
                    Emoji = sticker["emoji"]?.ToString(),
                    FileSize = sticker["file_size"] == null ? null : new int?(Convert.ToInt32(sticker["file_size"]))
                };
            }

            private static PhotoSize BuildThumb(JToken thumb)
            {
                if (thumb == null) return null;

                return new PhotoSize
                {
                    FileId = thumb["file_id"].ToString(),
                    Width = Convert.ToInt32(thumb["width"]),
                    Height = Convert.ToInt32(thumb["height"]),
                    FileSize = thumb["file_size"] == null ? null : new int?(Convert.ToInt32(thumb["file_size"]))
                };
            }

            private static Game BuildGame(JToken game)
            {
                if (game == null) return null;

                return new Game
                {
                    Title = game["title"].ToString(),
                    Description = game["description"].ToString(),
                    Photo = BuildPhoto(game["photo"]),
                    Text = game["text"]?.ToString(),
                    TextEntities = BuildTextEntites(game["text_entities"]),
                    Animation = BuildAnimation(game["animation"])
                };
            }

            private static List<MessageEntity> BuildTextEntites(JToken textEntities)
            {
                return textEntities?.Select(entity => new MessageEntity
                {
                    Type = entity["type"].ToString(),
                    Offset = Convert.ToInt32(entity["offset"]),
                    Lenght = Convert.ToInt32(entity["lenght"]),
                    Url = entity["url"]?.ToString(),
                    User = BuildUser(entity["user"])
                }).ToList();
            }

            private static Animation BuildAnimation(JToken animation)
            {
                if (animation == null) return null;

                return new Animation
                {
                    FileId = animation["file_id"].ToString(),
                    Thumb = BuildThumb(animation["thumb"]),
                    FileName = animation["file_name"]?.ToString(),
                    MimeType = animation["mime_type"]?.ToString(),
                    FileSize = animation["file_size"] == null ? null : new int?(Convert.ToInt32(animation["file_size"]))
                };
            }

            private static Video BuildVideo(JToken video)
            {
                if (video == null) return null;

                return new Video
                {
                    FileId = video["file_id"].ToString(),
                    Width = Convert.ToInt32(video["width"]),
                    Height = Convert.ToInt32(video["height"]),
                    Duration = Convert.ToInt32(video["duration"]),
                    Thumb = BuildThumb(video["thumb"]),
                    MimeType = video["mime_type"]?.ToString(),
                    FileSize = video["file_size"] == null ? null : new int?(Convert.ToInt32(video["file_size"]))
                };
            }

            private static Voice BuildVoice(JToken voice)
            {
                if (voice == null) return null;

                return new Voice
                {
                    FileId = voice["file_id"].ToString(),
                    Duration = Convert.ToInt32(voice["duration"]),
                    MimeType = voice["mime_type"]?.ToString(),
                    FileSize = voice["file_size"] == null ? null : new int?(Convert.ToInt32(voice["file_size"]))
                };
            }

            private static Contact BuildContact(JToken contact)
            {
                if (contact == null) return null;

                return new Contact
                {
                    PhoneNumber = contact["phone_number"].ToString(),
                    FirstName = contact["first_name"].ToString(),
                    LastName = contact["last_name"]?.ToString(),
                    UserId = contact["user_id"] == null ? null : new int?(Convert.ToInt32(contact["user_id"]))
                };
            }

            private static Location BuildLocation(JToken location)
            {
                if (location == null) return null;

                return new Location
                {
                    Longtitude = (float)location["longtitude"],
                    Latitude = (float)location["latitude"]
                };
            }

            private static Venue BuildVenue(JToken venue)
            {
                if (venue == null) return null;

                return new Venue
                {
                    Location = BuildLocation(venue["location"]),
                    Title = venue["title"].ToString(),
                    Address = venue["address"].ToString(),
                    FoursquareId = venue["foursquare_id"]?.ToString()
                };
            }

            #endregion
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