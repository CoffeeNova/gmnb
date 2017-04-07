using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Extensions;
using Newtonsoft.Json.Linq;

namespace CoffeeJelly.gmailNotifyBot.Bot.Telegram
{
    public static class MessageBuilder
    {
        public static T BuildMessage<T>(JToken messageToken) where T : Message
        {
            if (messageToken == null)
                throw new ArgumentNullException(nameof(messageToken));

            Message telegramMessage;

            #region 1. as TelegramTextUserMessage
            if (typeof(T) == typeof(TextMessage))
                telegramMessage = BuildTelegramTextMessage(messageToken);
            #endregion
            #region 2. as TelegramAudioMessage
            else if (typeof(T) == typeof(TelegramAudioMessage))
                telegramMessage = BuildTelegramAudioMessage(messageToken);
            #endregion
            #region 3. as TelegramDocumentMessage
            else if (typeof(T) == typeof(TelegramDocumentMessage))
                telegramMessage = BuildTelegramDocumentMessage(messageToken);
            #endregion
            #region 4 as TelegramStickerMessage
            else if (typeof(T) == typeof(TelegramStickerMessage))
                telegramMessage = BuildTelegramStickerMessage(messageToken);
            #endregion
            #region 5. as TelegramPhotoMessage
            else if (typeof(T) == typeof(TelegramPhotoMessage))
                telegramMessage = BuildTelegramPhotoMessage(messageToken);

            #endregion
            #region 6. as TelegramGameMessage
            else if (typeof(T) == typeof(TelegramGameMessage))
                telegramMessage = BuildTelegramGameMessage(messageToken);
            #endregion
            #region 7. as TelegramVideoMessage
            else if (typeof(T) == typeof(TelegramVideoMessage))
                telegramMessage = BuildTelegramVideoMessage(messageToken);
            #endregion
            #region 8. as TelegramVoiceMessage
            else if (typeof(T) == typeof(TelegramVoiceMessage))
                telegramMessage = BuildTelegramVoiceMessage(messageToken);
            #endregion
            #region 9. as TelegramContactMessage
            else if (typeof(T) == typeof(TelegramContactMessage))
                telegramMessage = BuildTelegramContactMessage(messageToken);
            #endregion
            #region 10. as TelegramLocationMessage
            else if (typeof(T) == typeof(TelegramLocationMessage))
                telegramMessage = BuildTelegramLocationMessage(messageToken);
            #endregion
            #region 11. as TelegramVenueMessage
            else if (typeof(T) == typeof(TelegramVenueMessage))
                telegramMessage = BuildTelegramVenueMessage(messageToken);
            #endregion
            #region 12. as TelegramNewChatMemberMessage
            else if (typeof(T) == typeof(TelegramNewChatMemberMessage))
                telegramMessage = BuildTelegramNewChatMemberMessage(messageToken);
            #endregion
            #region 13. as TelegramLeftChatMemberMessage
            else if (typeof(T) == typeof(TelegramLeftChatMemberMessage))
                telegramMessage = BuildTelegramLeftChatMemberMessage(messageToken);
            #endregion
            #region 14. as TelegramNewChatTitleMessage
            else if (typeof(T) == typeof(TelegramNewChatTitleMessage))
                telegramMessage = BuildTelegramNewChatTitleMessage(messageToken);
            #endregion
            #region 15. as TelegramNewChatPhotoMessage
            else if (typeof(T) == typeof(TelegramNewChatPhotoMessage))
                telegramMessage = BuildTelegramNewChatPhotoMessage(messageToken);
            #endregion
            #region 16. as TelegramMigrateToChatIdMessage
            else if (typeof(T) == typeof(TelegramMigrateToChatIdMessage))
                telegramMessage = BuildTelegramMigrateToChatIdMessage(messageToken);
            #endregion
            #region 17. as TelegramMigrateFromChatIdMessage
            else if (typeof(T) == typeof(TelegramMigrateFromChatIdMessage))
                telegramMessage = BuildTelegramMigrateFromChatIdMessage(messageToken);
            #endregion
            #region 18. as TelegramPinnedMessage
            else if (typeof(T) == typeof(TelegramPinnedMessage))
                telegramMessage = BuildTelegramPinnedMessage(messageToken);
            #endregion
            #region 19. as TelegramUnknownMessage

            else
                telegramMessage = BuildTelegramUnknownMessage(messageToken);
            #endregion

            return (T)telegramMessage;
        }

        private static void AttachGeneralProperties<T>(T message, JToken messageToken) where T : Message
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
            message.Chat = BuildChat(messageToken["chat"]);
            message.ForwardFrom = BuildUser(messageToken["forward_from"]);
            message.ForwardFromChat = BuildChat(messageToken["forward_from_chat"]);
            message.ForwardFromMessageId = messageToken["forward_from_message_id"] == null
                ? null
                : new int?(Convert.ToInt32(messageToken["forward_from_message_id"]));

            message.ForwardDate = messageToken["forward_date"] == null
                ? null
                : new DateTime?(
                    DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64((messageToken["forward_date"] as JValue)?.Value))
                        .UtcDateTime);
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

        public static dynamic BuildUnspecifiedMessage(JToken messageToken)
        {
            if (messageToken == null)
                throw new Exceptions.TelegramMessageIsUnidentifiedException();
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

            if (messageBody == null || (messageBody as JProperty).Name == null)
                throw new Exceptions.TelegramMessageIsUnidentifiedException();

            Message telegramMessage;
            switch ((messageBody as JProperty).Name)
            {
                #region cases
                case "text":
                    telegramMessage = BuildMessage<TextMessage>(messageToken);
                    break;
                case "audio":
                    telegramMessage = BuildMessage<TelegramAudioMessage>(messageToken);
                    break;
                case "document":
                    telegramMessage = BuildMessage<TelegramDocumentMessage>(messageToken);
                    break;
                case "sticker":
                    telegramMessage = BuildMessage<TelegramStickerMessage>(messageToken);
                    break;
                case "photo":
                    telegramMessage = BuildMessage<TelegramPhotoMessage>(messageToken);
                    break;
                case "game":
                    telegramMessage = BuildMessage<TelegramGameMessage>(messageToken);
                    break;
                case "video":
                    telegramMessage = BuildMessage<TelegramVideoMessage>(messageToken);
                    break;
                case "voice":
                    telegramMessage = BuildMessage<TelegramVoiceMessage>(messageToken);
                    break;
                case "contact":
                    telegramMessage = BuildMessage<TelegramContactMessage>(messageToken);
                    break;
                case "location":
                    telegramMessage = BuildMessage<TelegramLocationMessage>(messageToken);
                    break;
                case "venue":
                    telegramMessage = BuildMessage<TelegramVenueMessage>(messageToken);
                    break;
                case "new_chat_member":
                    telegramMessage = BuildMessage<TelegramNewChatMemberMessage>(messageToken);
                    break;
                case "left_chat_member":
                    telegramMessage = BuildMessage<TelegramLeftChatMemberMessage>(messageToken);
                    break;
                case "new_chat_title":
                    telegramMessage = BuildMessage<TelegramNewChatTitleMessage>(messageToken);
                    break;
                case "new_chat_photo":
                    telegramMessage = BuildMessage<TelegramNewChatPhotoMessage>(messageToken);
                    break;
                case "migrate_to_chat_id":
                    telegramMessage = BuildMessage<TelegramMigrateToChatIdMessage>(messageToken);
                    break;
                case "migrate_from_chat_id":
                    telegramMessage = BuildMessage<TelegramMigrateFromChatIdMessage>(messageToken);
                    break;
                case "pinned_message":
                    telegramMessage = BuildMessage<TelegramPinnedMessage>(messageToken);
                    break;
                default:
                    telegramMessage = BuildMessage<TelegramUnknownMessage>(messageToken);
                    break;
                    #endregion
            }

            return telegramMessage;
        }

        private static TextMessage BuildTelegramTextMessage(JToken messageToken)
        {

            var telegramMessage = new TextMessage
            {
                From = BuildUser(messageToken["from"]),
                Text = messageToken["text"]?.ToString(),
                Entities = BuildEntities(messageToken["entities"]),
                ReplyToMessage = messageToken["reply_to_message"] == null
                ? null
                : BuildUnspecifiedMessage(messageToken["reply_to_message"])
            };
            AttachGeneralProperties(telegramMessage, messageToken);
            return telegramMessage;
        }

        private static TelegramAudioMessage BuildTelegramAudioMessage(JToken messageToken)
        {
            var telegramMessage = new TelegramAudioMessage
            {
                From = BuildUser(messageToken["from"]),
                Audio = BuildAudio(messageToken["audio"]),
                Caption = messageToken["caption"]?.ToString()
            };
            AttachGeneralProperties(telegramMessage, messageToken);
            return telegramMessage;
        }

        private static TelegramDocumentMessage BuildTelegramDocumentMessage(JToken messageToken)
        {
            var telegramMessage = new TelegramDocumentMessage
            {
                From = BuildUser(messageToken["from"]),
                Document = BuildDocument(messageToken["document"]),
                Caption = messageToken["caption"]?.ToString()
            };
            AttachGeneralProperties(telegramMessage, messageToken);
            return telegramMessage;
        }

        private static TelegramStickerMessage BuildTelegramStickerMessage(JToken messageToken)
        {
            var telegramMessage = new TelegramStickerMessage
            {
                From = BuildUser(messageToken["from"]),
                Sticker = BuildSticker(messageToken["sticker"])
            };
            AttachGeneralProperties(telegramMessage, messageToken);
            return telegramMessage;
        }

        private static TelegramPhotoMessage BuildTelegramPhotoMessage(JToken messageToken)
        {

            var telegramMessage = new TelegramPhotoMessage
            {
                From = BuildUser(messageToken["from"]),
                Photo = BuildPhoto(messageToken["photo"]),
                Caption = messageToken["caption"]?.ToString()
            };
            AttachGeneralProperties(telegramMessage, messageToken);
            return telegramMessage;
        }

        private static TelegramGameMessage BuildTelegramGameMessage(JToken messageToken)
        {
            var telegramMessage = new TelegramGameMessage
            {
                From = BuildUser(messageToken["from"]),
                Game = BuildGame(messageToken["game"]),
            };
            AttachGeneralProperties(telegramMessage, messageToken);
            return telegramMessage;
        }

        private static TelegramVideoMessage BuildTelegramVideoMessage(JToken messageToken)
        {
            var telegramMessage = new TelegramVideoMessage
            {
                From = BuildUser(messageToken["from"]),
                Video = BuildVideo(messageToken["video"]),
                Caption = messageToken["caption"]?.ToString()
            };
            AttachGeneralProperties(telegramMessage, messageToken);
            return telegramMessage;
        }

        private static TelegramVoiceMessage BuildTelegramVoiceMessage(JToken messageToken)
        {
            var telegramMessage = new TelegramVoiceMessage
            {
                From = BuildUser(messageToken["from"]),
                Voice = BuildVoice(messageToken["voice"])
            };
            AttachGeneralProperties(telegramMessage, messageToken);
            return telegramMessage;
        }

        private static TelegramContactMessage BuildTelegramContactMessage(JToken messageToken)
        {
            var telegramMessage = new TelegramContactMessage
            {
                From = BuildUser(messageToken["from"]),
                Contact = BuildContact(messageToken["contact"])
            };
            AttachGeneralProperties(telegramMessage, messageToken);
            return telegramMessage;
        }

        private static TelegramLocationMessage BuildTelegramLocationMessage(JToken messageToken)
        {
            var telegramMessage = new TelegramLocationMessage
            {
                From = BuildUser(messageToken["from"]),
                Location = BuildLocation(messageToken["location"])
            };
            AttachGeneralProperties(telegramMessage, messageToken);
            return telegramMessage;
        }

        private static TelegramVenueMessage BuildTelegramVenueMessage(JToken messageToken)
        {
            var telegramMessage = new TelegramVenueMessage
            {
                From = BuildUser(messageToken["from"]),
                Venue = BuildVenue(messageToken["venue"])
            };
            AttachGeneralProperties(telegramMessage, messageToken);
            return telegramMessage;
        }

        private static TelegramNewChatMemberMessage BuildTelegramNewChatMemberMessage(JToken messageToken)
        {
            var telegramMessage = new TelegramNewChatMemberMessage
            {
                From = BuildUser(messageToken["from"]),
                NewChatMember = BuildUser(messageToken["new_chat_member"]),
                NewChatParticipant = BuildUser(messageToken["new_chat_participant"]),
                NewChatMembers = BuildUsers(messageToken["new_chat_members"])
            };
            AttachGeneralProperties(telegramMessage, messageToken);
            return telegramMessage;
        }

        private static TelegramLeftChatMemberMessage BuildTelegramLeftChatMemberMessage(JToken messageToken)
        {
            var telegramMessage = new TelegramLeftChatMemberMessage
            {
                From = BuildUser(messageToken["from"]),
                LeftChatMember = BuildUser(messageToken["left_chat_member"]),
                LeftChatParticipant = BuildUser(messageToken["left_chat_participant"])
            };
            AttachGeneralProperties(telegramMessage, messageToken);
            return telegramMessage;
        }

        private static TelegramNewChatTitleMessage BuildTelegramNewChatTitleMessage(JToken messageToken)
        {
            var telegramMessage = new TelegramNewChatTitleMessage
            {
                From = BuildUser(messageToken["from"]),
                NewChatTitle = messageToken["new_chat_title"].ToString()
            };
            AttachGeneralProperties(telegramMessage, messageToken);
            return telegramMessage;
        }

        private static TelegramNewChatPhotoMessage BuildTelegramNewChatPhotoMessage(JToken messageToken)
        {
            var telegramMessage = new TelegramNewChatPhotoMessage
            {
                From = BuildUser(messageToken["from"]),
                NewChatPhoto = BuildPhoto(messageToken["new_chat_photo"])
            };
            AttachGeneralProperties(telegramMessage, messageToken);
            return telegramMessage;
        }

        private static TelegramMigrateToChatIdMessage BuildTelegramMigrateToChatIdMessage(JToken messageToken)
        {
            var telegramMessage = new TelegramMigrateToChatIdMessage
            {
                From = BuildUser(messageToken["from"]),
                MigrateToChatId = Convert.ToInt64(messageToken["migrate_to_chat_id"])
            };
            AttachGeneralProperties(telegramMessage, messageToken);
            return telegramMessage;
        }

        private static TelegramMigrateFromChatIdMessage BuildTelegramMigrateFromChatIdMessage(JToken messageToken)
        {
            var telegramMessage = new TelegramMigrateFromChatIdMessage
            {
                From = BuildUser(messageToken["from"]),
                MigrateFromChatId = Convert.ToInt64(messageToken["migrate_from_chat_id"])
            };
            AttachGeneralProperties(telegramMessage, messageToken);
            return telegramMessage;
        }

        private static TelegramPinnedMessage BuildTelegramPinnedMessage(JToken messageToken)
        {
            var telegramMessage = new TelegramPinnedMessage
            {
                From = BuildUser(messageToken["from"]),
                PinnedMessage = BuildUnspecifiedMessage(messageToken["pinned_message"])
            };
            AttachGeneralProperties(telegramMessage, messageToken);
            return telegramMessage;
        }

        private static TelegramUnknownMessage BuildTelegramUnknownMessage(JToken messageToken)
        {
            var telegramMessage = new TelegramUnknownMessage();
            AttachGeneralProperties(telegramMessage, messageToken);
            return telegramMessage;
        }

        public static User BuildUser(JToken user)
        {
            if (user == null) return null;
            return new User
            {
                Id = Convert.ToInt32(user["id"]),
                FirstName = user["first_name"].ToString(),
                //next are optional properties, should be verified by null value
                LastName = user["last_name"]?.ToString(),
                Username = user["username"]?.ToString()
            };
        }

        public static Chat BuildChat(JToken chat)
        {
            if (chat == null) return null;

            return new Chat
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
        }

        public static List<User> BuildUsers(JToken users)
        {
            return users?.Select(user => new User
            {
                Id = Convert.ToInt32(user["id"]),
                FirstName = user["first_name"].ToString(),
                //next are optional properties, should be verified by null value
                LastName = user["last_name"]?.ToString(),
                Username = user["username"]?.ToString()
            }).ToList();
        }

        public static List<MessageEntity> BuildEntities(JToken entities)
        {
            return entities?.Select(entity => new MessageEntity
            {
                Type = entity["type"].ToString(),
                Offset = Convert.ToInt32(entity["offset"]),
                Lenght = Convert.ToInt32(entity["length"]),
                //next are optional properties, should be verified by null value
                Url = entity["url"]?.ToString(),
                User = BuildUser(entity["user"])
            }).ToList();
        }

        public static Audio BuildAudio(JToken audio)
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

        public static List<PhotoSize> BuildPhoto(JToken photo)
        {
            return photo?.Select(photoSize => new PhotoSize
            {
                FileId = photoSize["file_id"].ToString(),
                Width = Convert.ToInt32(photoSize["width"]),
                Height = Convert.ToInt32(photoSize["height"]),
                FileSize = photoSize["file_size"] == null ? null : new int?(Convert.ToInt32(photoSize["file_size"]))
            }).ToList();
        }

        public static Document BuildDocument(JToken document)
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

        public static Sticker BuildSticker(JToken sticker)
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

        public static PhotoSize BuildThumb(JToken thumb)
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

        public static Game BuildGame(JToken game)
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

        public static List<MessageEntity> BuildTextEntites(JToken textEntities)
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

        public static Animation BuildAnimation(JToken animation)
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

        public static Video BuildVideo(JToken video)
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

        public static Voice BuildVoice(JToken voice)
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

        public static Contact BuildContact(JToken contact)
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

        public static Location BuildLocation(JToken location)
        {
            if (location == null) return null;

            return new Location
            {
                Longitude = (float)location["longitude"],
                Latitude = (float)location["latitude"]
            };
        }

        public static Venue BuildVenue(JToken venue)
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