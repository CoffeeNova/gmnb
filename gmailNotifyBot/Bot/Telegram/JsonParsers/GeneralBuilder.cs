using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;

namespace CoffeeJelly.gmailNotifyBot.Bot.Telegram.JsonParsers
{
    public static class GeneralBuilder
    {
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
                Length = Convert.ToInt32(entity["length"]),
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
                Length = Convert.ToInt32(entity["length"]),
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
    }
}