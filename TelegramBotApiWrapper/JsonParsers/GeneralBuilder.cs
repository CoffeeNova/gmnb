using System.Collections.Generic;
using CoffeeJelly.TelegramBotApiWrapper.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CoffeeJelly.TelegramBotApiWrapper.JsonParsers
{
    public static class GeneralBuilder
    {
        public static User BuildUser(JToken user)
        {
            if (user == null) return null;
            return JsonConvert.DeserializeObject<User>(user.ToString(), Settings);
        }

        public static Chat BuildChat(JToken chat)
        {
            if (chat == null) return null;
            return JsonConvert.DeserializeObject<Chat>(chat.ToString(), Settings);
        }

        public static List<User> BuildUsers(JToken users)
        {
            if (users == null) return null;
            return JsonConvert.DeserializeObject<List<User>>(users.ToString(), Settings);
        }

        public static List<MessageEntity> BuildEntities(JToken entities)
        {
            if (entities == null) return null;
            return JsonConvert.DeserializeObject<List<MessageEntity>>(entities.ToString(), Settings);
        }

        public static Audio BuildAudio(JToken audio)
        {
            if (audio == null) return null;
            return JsonConvert.DeserializeObject<Audio>(audio.ToString(), Settings);
        }

        public static List<PhotoSize> BuildPhoto(JToken photo)
        {
            if (photo == null) return null;
            return JsonConvert.DeserializeObject<List<PhotoSize>>(photo.ToString(), Settings);
        }

        public static Document BuildDocument(JToken document)
        {
            if (document == null) return null;
            return JsonConvert.DeserializeObject<Document>(document.ToString(), Settings);
        }

        public static Sticker BuildSticker(JToken sticker)
        {
            if (sticker == null) return null;
            return JsonConvert.DeserializeObject<Sticker>(sticker.ToString(), Settings);
        }

        public static PhotoSize BuildThumb(JToken thumb)
        {
            if (thumb == null) return null;
            return JsonConvert.DeserializeObject<PhotoSize>(thumb.ToString(), Settings);
        }

        public static Game BuildGame(JToken game)
        {
            if (game == null) return null;
            return JsonConvert.DeserializeObject<Game>(game.ToString(), Settings);
        }

        public static List<MessageEntity> BuildTextEntites(JToken textEntities)
        {
            if (textEntities == null) return null;
            return JsonConvert.DeserializeObject<List<MessageEntity>>(textEntities.ToString(), Settings);
        }

        public static Animation BuildAnimation(JToken animation)
        {
            if (animation == null) return null;
            return JsonConvert.DeserializeObject<Animation>(animation.ToString(), Settings);
        }

        public static Video BuildVideo(JToken video)
        {
            if (video == null) return null;
            return JsonConvert.DeserializeObject<Video>(video.ToString(), Settings);
        }

        public static Voice BuildVoice(JToken voice)
        {
            if (voice == null) return null;
            return JsonConvert.DeserializeObject<Voice>(voice.ToString(), Settings);
        }

        public static Contact BuildContact(JToken contact)
        {
            if (contact == null) return null;
            return JsonConvert.DeserializeObject<Contact>(contact.ToString(), Settings);
        }

        public static Location BuildLocation(JToken location)
        {
            if (location == null) return null;
            return JsonConvert.DeserializeObject<Location>(location.ToString(), Settings);
        }

        public static Venue BuildVenue(JToken venue)
        {
            if (venue == null) return null;
            return JsonConvert.DeserializeObject<Venue>(venue.ToString(), Settings);
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

        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };
    }
}