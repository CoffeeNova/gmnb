using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CoffeeJelly.TelegramApiWrapper.Attributes;
using CoffeeJelly.TelegramApiWrapper.Converters;
using CoffeeJelly.TelegramApiWrapper.Exceptions;
using CoffeeJelly.TelegramApiWrapper.Extensions;
using CoffeeJelly.TelegramApiWrapper.JsonParsers;
using CoffeeJelly.TelegramApiWrapper.Types;
using CoffeeJelly.TelegramApiWrapper.Types.General;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CoffeeJelly.TelegramApiWrapper.Methods
{
    public partial class TelegramMethods
    {
        /// <summary>
        /// A simple method for testing your bot's auth token. Requires no parameters. Returns basic information about the bot in form of a User object.
        /// </summary>
        /// <returns></returns>
        public User GetMe()
        {
            using (var webClient = new WebClient())
            {
                try
                {
                    var content = webClient.DownloadString(TelegramBotUrl + Token + "/getMe");
                    var newRequests = JsonConvert.DeserializeObject<JObject>(content);
                    return GeneralBuilder.BuildUser(newRequests["result"]);
                }
                catch (WebException ex)
                {
                    throw new TelegramMethodsException($"Wrong {nameof(Token)} value.", ex);
                }
            }
        }

        public Task<User> GetMeAsync()
        {
            return Task.Run(() => GetMe());
        }

        [TelegramMethod("getUpdates")]
        public List<Update> GetUpdates(long? offset = null, int? limit = null, int? timeout = null, List<UpdateType> allowedUpdates = null)
        {
            var parameters = new NameValueCollection();

            if (offset != null)
                parameters.Add("offset", offset.ToString());
            if (limit != null)
                parameters.Add("limit", limit.ToString());
            if (timeout != null)
                parameters.Add("timeout", timeout.ToString());
            if (allowedUpdates != null)
            {
                var allowedUpdatesString = string.Join(",", allowedUpdates.Select(i => $"\"{UpdateAttribute.GetUpdateType(i)}\""));
                parameters.Add("allowed_updates", $"[{allowedUpdatesString}]");
            }
            var json = UploadUrlQuery(parameters);
            var result = json["result"];

            return JsonConvert.DeserializeObject<List<Update>>(result.ToString());
        }

        public Task<List<Update>> GetUpdatesAsync(int? offset = null, int? limit = null, int? timeout = null, List<UpdateType> allowedUpdates = null)
        {
            return Task.Run(() => GetUpdates(offset, limit, timeout, allowedUpdates));
        }

        [TelegramMethod("getUserProfilePhotos")]
        public UserProfilePhotos GetUserProfilePhotos(int userId, int? offset = null, int? limit = null)
        {
            var parameters = new NameValueCollection { { "user_id", userId.ToString() } };
            if (offset != null)
                parameters.Add("offset", offset.ToString());
            if (limit != null)
                parameters.Add("limit", limit.ToString());

            var json = UploadUrlQuery(parameters);
            var result = json["result"];

            return JsonConvert.DeserializeObject<UserProfilePhotos>(result.ToString());

        }

        public Task<UserProfilePhotos> GetUserProfilePhotosAsync(int userId, int? offset = null, int? limit = null)
        {
            return Task.Run(() => GetUserProfilePhotos(userId, offset, limit));
        }

        [TelegramMethod("getFile")]
        public File GetFile(string fileId)
        {
            var parameters = new NameValueCollection { { "file_id", fileId } };

            var json = UploadUrlQuery(parameters);
            var result = json["result"];

            return JsonConvert.DeserializeObject<File>(result.ToString());
        }

        [TelegramMethod("getChat")]
        public Chat GetChat(string chatId)
        {
            chatId.NullInspect(nameof(chatId));
            var parameters = new NameValueCollection { { "chat_id", chatId } };

            var json = UploadUrlQuery(parameters);
            var result = json["result"].ToString();
            return JsonConvert.DeserializeObject<Chat>(result);
        }

        public Task<Chat> GetChatAsync(string chatId)
        {
            chatId.NullInspect(nameof(chatId));
            return Task.Run(() => GetChat(chatId));
        }

        [TelegramMethod("getChatAdministrators")]
        public List<ChatMember> GetChatAdministrators(string chatId)
        {
            chatId.NullInspect(nameof(chatId));
            var parameters = new NameValueCollection { { "chat_id", chatId } };

            var json = UploadUrlQuery(parameters);
            var result = json["result"].ToString();
            return JsonConvert.DeserializeObject<List<ChatMember>>(result);
        }

        public Task<List<ChatMember>> GetChatAdministratorsAsync(string chatId)
        {
            chatId.NullInspect(nameof(chatId));
            return Task.Run(() => GetChatAdministrators(chatId));
        }

        [TelegramMethod("getChatMembersCount")]
        public int GetChatMembersCount(string chatId)
        {
            chatId.NullInspect(nameof(chatId));
            var parameters = new NameValueCollection { { "chat_id", chatId } };

            var json = UploadUrlQuery(parameters);
            var result = json["result"].ToString();
            return JsonConvert.DeserializeObject<int>(result);
        }

        public Task<int> GetChatMembersCountAsync(string chatId)
        {
            chatId.NullInspect(nameof(chatId));
            return Task.Run(() => GetChatMembersCount(chatId));
        }

        [TelegramMethod("getChatMember")]
        public ChatMember GetChatMember(string chatId, int userId)
        {
            chatId.NullInspect(nameof(chatId));
            var parameters = new NameValueCollection
            {
                {"chat_id", chatId},
                {"user_id", userId.ToString()}
            };

            var json = UploadUrlQuery(parameters);
            var result = json["result"].ToString();
            return JsonConvert.DeserializeObject<ChatMember>(result);
        }

        public Task<ChatMember> GetChatMemberAsync(string chatId, int userId)
        {
            chatId.NullInspect(nameof(chatId));
            return Task.Run(() => GetChatMember(chatId, userId));
        }
    }
}