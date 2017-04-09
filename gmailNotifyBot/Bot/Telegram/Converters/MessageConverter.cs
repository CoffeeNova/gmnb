using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram.JsonParsers;
using CoffeeJelly.gmailNotifyBot.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace CoffeeJelly.gmailNotifyBot.Bot.Telegram.Converters
{
    public class MessageConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);

            switch (MessageBuilder.DefineMessage(jObject))
            {
                #region cases
                case "text":
                    return JsonConvert.DeserializeObject<TextMessage>(jObject.ToString(), _specifiedSubclassConversion);
                case "audio":
                    return JsonConvert.DeserializeObject<AudioMessage>(jObject.ToString(), _specifiedSubclassConversion);
                case "document":
                    return JsonConvert.DeserializeObject<DocumentMessage>(jObject.ToString(), _specifiedSubclassConversion);
                case "sticker":
                    return JsonConvert.DeserializeObject<StickerMessage>(jObject.ToString(), _specifiedSubclassConversion);
                case "photo":
                    return JsonConvert.DeserializeObject<PhotoMessage>(jObject.ToString(), _specifiedSubclassConversion);
                case "game":
                    return JsonConvert.DeserializeObject<GameMessage>(jObject.ToString(), _specifiedSubclassConversion);
                case "video":
                    return JsonConvert.DeserializeObject<VideoMessage>(jObject.ToString(), _specifiedSubclassConversion);
                case "voice":
                    return JsonConvert.DeserializeObject<VoiceMessage>(jObject.ToString(), _specifiedSubclassConversion);
                case "contact":
                    return JsonConvert.DeserializeObject<ContactMessage>(jObject.ToString(), _specifiedSubclassConversion);
                case "location":
                    return JsonConvert.DeserializeObject<LocationMessage>(jObject.ToString(), _specifiedSubclassConversion);
                case "venue":
                    return JsonConvert.DeserializeObject<VenueMessage>(jObject.ToString(), _specifiedSubclassConversion);
                case "new_chat_member":
                    return JsonConvert.DeserializeObject<NewChatMemberMessage>(jObject.ToString(), _specifiedSubclassConversion);
                case "left_chat_member":
                    return JsonConvert.DeserializeObject<LeftChatMemberMessage>(jObject.ToString(), _specifiedSubclassConversion);
                case "new_chat_title":
                    return JsonConvert.DeserializeObject<NewChatTitleMessage>(jObject.ToString(), _specifiedSubclassConversion);
                case "new_chat_photo":
                    return JsonConvert.DeserializeObject<NewChatPhotoMessage>(jObject.ToString(), _specifiedSubclassConversion);
                case "migrate_to_chat_id":
                    return JsonConvert.DeserializeObject<MigrateToChatIdMessage>(jObject.ToString(), _specifiedSubclassConversion);
                case "migrate_from_chat_id":
                    return JsonConvert.DeserializeObject<MigrateFromChatIdMessage>(jObject.ToString(), _specifiedSubclassConversion);
                case "pinned_message":
                    return JsonConvert.DeserializeObject<PinnedMessage>(jObject.ToString(), _specifiedSubclassConversion);
                default:
                    return JsonConvert.DeserializeObject<UnknownMessage>(jObject.ToString(), _specifiedSubclassConversion);
                    #endregion
            }
            
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Message);
        }

        static JsonSerializerSettings _specifiedSubclassConversion = 
            new JsonSerializerSettings() { ContractResolver = new MessageSpecifiedConcreteClassConverter() };
    }

    public class MessageSpecifiedConcreteClassConverter : DefaultContractResolver
    {
        protected override JsonConverter ResolveContractConverter(Type objectType)
        {
            if (typeof(Message).IsAssignableFrom(objectType) && !objectType.IsAbstract)
                return null; // pretend TableSortRuleConvert is not specified (thus avoiding a stack overflow)
            return base.ResolveContractConverter(objectType);
        }
    }
}