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

            Message message;

            #region 1. as TextUserMessage
            if (typeof(T) == typeof(TextMessage))
                message = BuildTextMessage(messageToken);
            #endregion
            #region 2. as AudioMessage
            else if (typeof(T) == typeof(AudioMessage))
                message = BuildAudioMessage(messageToken);
            #endregion
            #region 3. as DocumentMessage
            else if (typeof(T) == typeof(DocumentMessage))
                message = BuildDocumentMessage(messageToken);
            #endregion
            #region 4 as StickerMessage
            else if (typeof(T) == typeof(StickerMessage))
                message = BuildStickerMessage(messageToken);
            #endregion
            #region 5. as PhotoMessage
            else if (typeof(T) == typeof(PhotoMessage))
                message = BuildPhotoMessage(messageToken);

            #endregion
            #region 6. as GameMessage
            else if (typeof(T) == typeof(GameMessage))
                message = BuildGameMessage(messageToken);
            #endregion
            #region 7. as VideoMessage
            else if (typeof(T) == typeof(VideoMessage))
                message = BuildVideoMessage(messageToken);
            #endregion
            #region 8. as VoiceMessage
            else if (typeof(T) == typeof(VoiceMessage))
                message = BuildVoiceMessage(messageToken);
            #endregion
            #region 9. as ContactMessage
            else if (typeof(T) == typeof(ContactMessage))
                message = BuildContactMessage(messageToken);
            #endregion
            #region 10. as LocationMessage
            else if (typeof(T) == typeof(LocationMessage))
                message = BuildLocationMessage(messageToken);
            #endregion
            #region 11. as VenueMessage
            else if (typeof(T) == typeof(VenueMessage))
                message = BuildVenueMessage(messageToken);
            #endregion
            #region 12. as NewChatMemberMessage
            else if (typeof(T) == typeof(NewChatMemberMessage))
                message = BuildNewChatMemberMessage(messageToken);
            #endregion
            #region 13. as LeftChatMemberMessage
            else if (typeof(T) == typeof(LeftChatMemberMessage))
                message = BuildLeftChatMemberMessage(messageToken);
            #endregion
            #region 14. as NewChatTitleMessage
            else if (typeof(T) == typeof(NewChatTitleMessage))
                message = BuildNewChatTitleMessage(messageToken);
            #endregion
            #region 15. as NewChatPhotoMessage
            else if (typeof(T) == typeof(NewChatPhotoMessage))
                message = BuildNewChatPhotoMessage(messageToken);
            #endregion
            #region 16. as MigrateToChatIdMessage
            else if (typeof(T) == typeof(MigrateToChatIdMessage))
                message = BuildMigrateToChatIdMessage(messageToken);
            #endregion
            #region 17. as MigrateFromChatIdMessage
            else if (typeof(T) == typeof(MigrateFromChatIdMessage))
                message = BuildMigrateFromChatIdMessage(messageToken);
            #endregion
            #region 18. as PinnedMessage
            else if (typeof(T) == typeof(PinnedMessage))
                message = BuildPinnedMessage(messageToken);
            #endregion
            #region 19. as UnknownMessage

            else
                message = BuildUnknownMessage(messageToken);
            #endregion

            return (T)message;
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

            Message message;
            switch ((messageBody as JProperty).Name)
            {
                #region cases
                case "text":
                    message = BuildMessage<TextMessage>(messageToken);
                    break;
                case "audio":
                    message = BuildMessage<AudioMessage>(messageToken);
                    break;
                case "document":
                    message = BuildMessage<DocumentMessage>(messageToken);
                    break;
                case "sticker":
                    message = BuildMessage<StickerMessage>(messageToken);
                    break;
                case "photo":
                    message = BuildMessage<PhotoMessage>(messageToken);
                    break;
                case "game":
                    message = BuildMessage<GameMessage>(messageToken);
                    break;
                case "video":
                    message = BuildMessage<VideoMessage>(messageToken);
                    break;
                case "voice":
                    message = BuildMessage<VoiceMessage>(messageToken);
                    break;
                case "contact":
                    message = BuildMessage<ContactMessage>(messageToken);
                    break;
                case "location":
                    message = BuildMessage<LocationMessage>(messageToken);
                    break;
                case "venue":
                    message = BuildMessage<VenueMessage>(messageToken);
                    break;
                case "new_chat_member":
                    message = BuildMessage<NewChatMemberMessage>(messageToken);
                    break;
                case "left_chat_member":
                    message = BuildMessage<LeftChatMemberMessage>(messageToken);
                    break;
                case "new_chat_title":
                    message = BuildMessage<NewChatTitleMessage>(messageToken);
                    break;
                case "new_chat_photo":
                    message = BuildMessage<NewChatPhotoMessage>(messageToken);
                    break;
                case "migrate_to_chat_id":
                    message = BuildMessage<MigrateToChatIdMessage>(messageToken);
                    break;
                case "migrate_from_chat_id":
                    message = BuildMessage<MigrateFromChatIdMessage>(messageToken);
                    break;
                case "pinned_message":
                    message = BuildMessage<PinnedMessage>(messageToken);
                    break;
                default:
                    message = BuildMessage<UnknownMessage>(messageToken);
                    break;
                    #endregion
            }

            return message;
        }

        private static TextMessage BuildTextMessage(JToken messageToken)
        {

            var message = new TextMessage
            {
                From = BuildUser(messageToken["from"]),
                Text = messageToken["text"]?.ToString(),
                Entities = BuildEntities(messageToken["entities"]),
                ReplyToMessage = messageToken["reply_to_message"] == null
                ? null
                : BuildUnspecifiedMessage(messageToken["reply_to_message"])
            };
            AttachGeneralProperties(message, messageToken);
            return message;
        }

        private static AudioMessage BuildAudioMessage(JToken messageToken)
        {
            var message = new AudioMessage
            {
                From = BuildUser(messageToken["from"]),
                Audio = BuildAudio(messageToken["audio"]),
                Caption = messageToken["caption"]?.ToString()
            };
            AttachGeneralProperties(message, messageToken);
            return message;
        }

        private static DocumentMessage BuildDocumentMessage(JToken messageToken)
        {
            var message = new DocumentMessage
            {
                From = BuildUser(messageToken["from"]),
                Document = BuildDocument(messageToken["document"]),
                Caption = messageToken["caption"]?.ToString()
            };
            AttachGeneralProperties(message, messageToken);
            return message;
        }

        private static StickerMessage BuildStickerMessage(JToken messageToken)
        {
            var message = new StickerMessage
            {
                From = BuildUser(messageToken["from"]),
                Sticker = BuildSticker(messageToken["sticker"])
            };
            AttachGeneralProperties(message, messageToken);
            return message;
        }

        private static PhotoMessage BuildPhotoMessage(JToken messageToken)
        {

            var message = new PhotoMessage
            {
                From = BuildUser(messageToken["from"]),
                Photo = BuildPhoto(messageToken["photo"]),
                Caption = messageToken["caption"]?.ToString()
            };
            AttachGeneralProperties(message, messageToken);
            return message;
        }

        private static GameMessage BuildGameMessage(JToken messageToken)
        {
            var message = new GameMessage
            {
                From = BuildUser(messageToken["from"]),
                Game = BuildGame(messageToken["game"]),
            };
            AttachGeneralProperties(message, messageToken);
            return message;
        }

        private static VideoMessage BuildVideoMessage(JToken messageToken)
        {
            var message = new VideoMessage
            {
                From = BuildUser(messageToken["from"]),
                Video = BuildVideo(messageToken["video"]),
                Caption = messageToken["caption"]?.ToString()
            };
            AttachGeneralProperties(message, messageToken);
            return message;
        }

        private static VoiceMessage BuildVoiceMessage(JToken messageToken)
        {
            var message = new VoiceMessage
            {
                From = BuildUser(messageToken["from"]),
                Voice = BuildVoice(messageToken["voice"])
            };
            AttachGeneralProperties(message, messageToken);
            return message;
        }

        private static ContactMessage BuildContactMessage(JToken messageToken)
        {
            var message = new ContactMessage
            {
                From = BuildUser(messageToken["from"]),
                Contact = BuildContact(messageToken["contact"])
            };
            AttachGeneralProperties(message, messageToken);
            return message;
        }

        private static LocationMessage BuildLocationMessage(JToken messageToken)
        {
            var message = new LocationMessage
            {
                From = BuildUser(messageToken["from"]),
                Location = BuildLocation(messageToken["location"])
            };
            AttachGeneralProperties(message, messageToken);
            return message;
        }

        private static VenueMessage BuildVenueMessage(JToken messageToken)
        {
            var message = new VenueMessage
            {
                From = BuildUser(messageToken["from"]),
                Venue = BuildVenue(messageToken["venue"])
            };
            AttachGeneralProperties(message, messageToken);
            return message;
        }

        private static NewChatMemberMessage BuildNewChatMemberMessage(JToken messageToken)
        {
            var message = new NewChatMemberMessage
            {
                From = BuildUser(messageToken["from"]),
                NewChatMember = BuildUser(messageToken["new_chat_member"]),
                NewChatParticipant = BuildUser(messageToken["new_chat_participant"]),
                NewChatMembers = BuildUsers(messageToken["new_chat_members"])
            };
            AttachGeneralProperties(message, messageToken);
            return message;
        }

        private static LeftChatMemberMessage BuildLeftChatMemberMessage(JToken messageToken)
        {
            var message = new LeftChatMemberMessage
            {
                From = BuildUser(messageToken["from"]),
                LeftChatMember = BuildUser(messageToken["left_chat_member"]),
                LeftChatParticipant = BuildUser(messageToken["left_chat_participant"])
            };
            AttachGeneralProperties(message, messageToken);
            return message;
        }

        private static NewChatTitleMessage BuildNewChatTitleMessage(JToken messageToken)
        {
            var message = new NewChatTitleMessage
            {
                From = BuildUser(messageToken["from"]),
                NewChatTitle = messageToken["new_chat_title"].ToString()
            };
            AttachGeneralProperties(message, messageToken);
            return message;
        }

        private static NewChatPhotoMessage BuildNewChatPhotoMessage(JToken messageToken)
        {
            var message = new NewChatPhotoMessage
            {
                From = BuildUser(messageToken["from"]),
                NewChatPhoto = BuildPhoto(messageToken["new_chat_photo"])
            };
            AttachGeneralProperties(message, messageToken);
            return message;
        }

        private static MigrateToChatIdMessage BuildMigrateToChatIdMessage(JToken messageToken)
        {
            var message = new MigrateToChatIdMessage
            {
                From = BuildUser(messageToken["from"]),
                MigrateToChatId = Convert.ToInt64(messageToken["migrate_to_chat_id"])
            };
            AttachGeneralProperties(message, messageToken);
            return message;
        }

        private static MigrateFromChatIdMessage BuildMigrateFromChatIdMessage(JToken messageToken)
        {
            var message = new MigrateFromChatIdMessage
            {
                From = BuildUser(messageToken["from"]),
                MigrateFromChatId = Convert.ToInt64(messageToken["migrate_from_chat_id"])
            };
            AttachGeneralProperties(message, messageToken);
            return message;
        }

        private static PinnedMessage BuildPinnedMessage(JToken messageToken)
        {
            var message = new PinnedMessage
            {
                From = BuildUser(messageToken["from"]),
                Message = BuildUnspecifiedMessage(messageToken["pinned_message"])
            };
            AttachGeneralProperties(message, messageToken);
            return message;
        }

        private static UnknownMessage BuildUnknownMessage(JToken messageToken)
        {
            var message = new UnknownMessage();
            AttachGeneralProperties(message, messageToken);
            return message;
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