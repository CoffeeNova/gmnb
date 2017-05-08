using System;
using System.Linq;
using CoffeeJelly.TelegramBotApiWrapper.Exceptions;
using CoffeeJelly.TelegramBotApiWrapper.Extensions;
using CoffeeJelly.TelegramBotApiWrapper.Types.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CoffeeJelly.TelegramBotApiWrapper.JsonParsers
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
            #region 19
            else if (typeof(T) == typeof(ServiceMessage))
                message = BuildServiceMessage(messageToken);
            #endregion
            #region 20. as UnknownMessage

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
            message.Chat = GeneralBuilder.BuildChat(messageToken["chat"]);
            message.ForwardFrom = GeneralBuilder.BuildUser(messageToken["forward_from"]);
            message.ForwardFromChat = GeneralBuilder.BuildChat(messageToken["forward_from_chat"]);
            message.ForwardFromMessageId = messageToken["forward_from_message_id"] == null
                ? null
                : new int?(Convert.ToInt32(messageToken["forward_from_message_id"]));

            message.ForwardDate = messageToken["forward_date"] == null
                ? null
                : new DateTime?(
                    DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64((messageToken["forward_date"] as JValue)?.Value))
                        .UtcDateTime);
        }



        #region builders

        public static dynamic BuildUnspecifiedMessage(JToken messageToken)
        {
            if (messageToken == null)
                throw new TelegramMessageIsUnidentifiedException();

            Message message;
            switch (DefineMessage(messageToken))
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
                case "delete_chat_photo":
                case "group_chat_created":
                case "supergroup_chat_created":
                case "channel_chat_created":
                    message = BuildMessage<ServiceMessage>(messageToken);
                    break;
                default:
                    message = BuildMessage<UnknownMessage>(messageToken);
                    break;
                    #endregion
            }

            return message;
        }

        public static string DefineMessage(JToken jToken)
        {
            var children = jToken.Children();
            var messageBody = children.FirstOrDefault(j =>
            {
                var jProperty = j as JProperty;
                return jProperty != null && jProperty.Name.EqualsAny(MessagesType);
            });

            if (messageBody == null || (messageBody as JProperty).Name == null)
                throw new TelegramMessageIsUnidentifiedException();
            return (messageBody as JProperty).Name;
        }

        private static TextMessage BuildTextMessage(JToken messageToken)
        {
            return JsonConvert.DeserializeObject<TextMessage>(messageToken.ToString(), Settings);
        }

        private static AudioMessage BuildAudioMessage(JToken messageToken)
        {
            return JsonConvert.DeserializeObject<AudioMessage>(messageToken.ToString(), Settings);
        }

        private static StickerMessage BuildStickerMessage(JToken messageToken)
        {
            return JsonConvert.DeserializeObject<StickerMessage>(messageToken.ToString(), Settings);
        }

        private static DocumentMessage BuildDocumentMessage(JToken messageToken)
        {
            return JsonConvert.DeserializeObject<DocumentMessage>(messageToken.ToString(), Settings);
        }


        private static PhotoMessage BuildPhotoMessage(JToken messageToken)
        {
            return JsonConvert.DeserializeObject<PhotoMessage>(messageToken.ToString(), Settings);
        }

        private static GameMessage BuildGameMessage(JToken messageToken)
        {
            return JsonConvert.DeserializeObject<GameMessage>(messageToken.ToString(), Settings);
        }

        private static VideoMessage BuildVideoMessage(JToken messageToken)
        {
            return JsonConvert.DeserializeObject<VideoMessage>(messageToken.ToString(), Settings);
        }

        private static VoiceMessage BuildVoiceMessage(JToken messageToken)
        {
            return JsonConvert.DeserializeObject<VoiceMessage>(messageToken.ToString(), Settings);
        }

        private static ContactMessage BuildContactMessage(JToken messageToken)
        {
            return JsonConvert.DeserializeObject<ContactMessage>(messageToken.ToString(), Settings);
        }

        private static LocationMessage BuildLocationMessage(JToken messageToken)
        {
            return JsonConvert.DeserializeObject<LocationMessage>(messageToken.ToString(), Settings);
        }

        private static VenueMessage BuildVenueMessage(JToken messageToken)
        {
            return JsonConvert.DeserializeObject<VenueMessage>(messageToken.ToString(), Settings);
        }

        private static NewChatMemberMessage BuildNewChatMemberMessage(JToken messageToken)
        {
            return JsonConvert.DeserializeObject<NewChatMemberMessage>(messageToken.ToString(), Settings);
        }

        private static LeftChatMemberMessage BuildLeftChatMemberMessage(JToken messageToken)
        {
            return JsonConvert.DeserializeObject<LeftChatMemberMessage>(messageToken.ToString(), Settings);
        }

        private static NewChatTitleMessage BuildNewChatTitleMessage(JToken messageToken)
        {
            return JsonConvert.DeserializeObject<NewChatTitleMessage>(messageToken.ToString(), Settings);
        }

        private static NewChatPhotoMessage BuildNewChatPhotoMessage(JToken messageToken)
        {
            return JsonConvert.DeserializeObject<NewChatPhotoMessage>(messageToken.ToString(), Settings);
        }

        private static MigrateToChatIdMessage BuildMigrateToChatIdMessage(JToken messageToken)
        {
            return JsonConvert.DeserializeObject<MigrateToChatIdMessage>(messageToken.ToString(), Settings);
        }

        private static MigrateFromChatIdMessage BuildMigrateFromChatIdMessage(JToken messageToken)
        {

            return JsonConvert.DeserializeObject<MigrateFromChatIdMessage>(messageToken.ToString(), Settings);
        }

        private static PinnedMessage BuildPinnedMessage(JToken messageToken)
        {
            return JsonConvert.DeserializeObject<PinnedMessage>(messageToken.ToString(), Settings);
        }

        private static ServiceMessage BuildServiceMessage(JToken messageToken)
        {
            return JsonConvert.DeserializeObject<ServiceMessage>(messageToken.ToString(), Settings);
        }

        private static UnknownMessage BuildUnknownMessage(JToken messageToken)
        {
            var message = new UnknownMessage();
            AttachGeneralProperties(message, messageToken);
            return message;
        }

        #endregion

        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        public static readonly string[] MessagesType =
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
                    "pinned_message",
                    "delete_chat_photo",
                    "group_chat_created",
                    "supergroup_chat_created",
                    "channel_chat_created"
                    #endregion
                };

    }
}