using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CoffeeJelly.TelegramBotApiWrapper.Attributes;
using CoffeeJelly.TelegramBotApiWrapper.Converters;
using CoffeeJelly.TelegramBotApiWrapper.Exceptions;
using CoffeeJelly.TelegramBotApiWrapper.Extensions;
using CoffeeJelly.TelegramBotApiWrapper.Helpers;
using CoffeeJelly.TelegramBotApiWrapper.JsonParsers;
using CoffeeJelly.TelegramBotApiWrapper.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CoffeeJelly.TelegramBotApiWrapper.Methods
{
    public partial class TelegramMethods
    {
        /// <summary>
        /// A simple method for testing your bot's auth token. Requires no parameters. Returns basic information about the bot in form of a User object.
        /// </summary>
        /// <returns></returns>
        public async Task<User> GetMe()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var content = await client.GetStringAsync(TelegramBotUrl + Token + "/getMe").ConfigureAwait(false);
                    var newRequests = JsonConvert.DeserializeObject<JObject>(content);
                    return GeneralBuilder.BuildUser(newRequests["result"]);
                }
                catch (WebException ex)
                {
                    throw new TelegramMethodsException($"Wrong {nameof(Token)} value.", ex);
                }
            }
        }

        [TelegramMethod("getUpdates")]
        public async Task<List<Update>> GetUpdates(long? offset = null, int? limit = null, int? timeout = null, List<UpdateType> allowedUpdates = null)
        {
            var content = new Content();
            if (offset != null)
                content.Add("offset", offset.ToString());
            if (limit != null)
                content.Add("limit", limit.ToString());
            if (timeout != null)
                content.Add("timeout", timeout.ToString());
            if (allowedUpdates != null)
            {
                var allowedUpdatesString = string.Join(",", allowedUpdates.Select(i => $"\"{UpdateAttribute.GetUpdateType(i)}\""));
                content.Add("allowed_updates", $"[{allowedUpdatesString}]");
            }
            using (var form = new FormUrlEncodedContent(content.Data))
            {
                return await UploadFormData<List<Update>>(form).ConfigureAwait(false);
            }
        }

        [TelegramMethod("getUserProfilePhotos")]
        public async  Task<UserProfilePhotos> GetUserProfilePhotos(int userId, int? offset = null, int? limit = null)
        {
            var content = new Content();
            content.Add("user_id", userId.ToString());
            if (offset != null)
                content.Add("offset", offset.ToString());
            if (limit != null)
                content.Add("limit", limit.ToString());

            using (var form = new FormUrlEncodedContent(content.Data))
            {
                return await UploadFormData<UserProfilePhotos>(form).ConfigureAwait(false);
            }
        }

        [TelegramMethod("getFile")]
        public async Task<File> GetFile(string fileId)
        {
            var content = new Content();
            content.Add("file_id", fileId);

            using (var form = new FormUrlEncodedContent(content.Data))
            {
                return await UploadFormData<File>(form).ConfigureAwait(false);
            }
        }

        [TelegramMethod("getChat")]
        public async Task<Chat> GetChat(string chatId)
        {
            chatId.NullInspect(nameof(chatId));
            var content = new Content();
            content.Add("chat_id", chatId);

            using (var form = new FormUrlEncodedContent(content.Data))
            {
                return await UploadFormData<Chat>(form).ConfigureAwait(false);
            }
        }

        [TelegramMethod("getChatAdministrators")]
        public async Task<List<ChatMember>> GetChatAdministrators(string chatId)
        {
            chatId.NullInspect(nameof(chatId));
            var content = new Content();
            content.Add("chat_id", chatId);

            using (var form = new FormUrlEncodedContent(content.Data))
            {
                return await UploadFormData<List<ChatMember>>(form).ConfigureAwait(false);
            }
        }

        [TelegramMethod("getChatMembersCount")]
        public async Task<int> GetChatMembersCount(string chatId)
        {
            chatId.NullInspect(nameof(chatId));
            var content = new Content();
            content.Add("chat_id", chatId);

            using (var form = new FormUrlEncodedContent(content.Data))
            {
                return await UploadFormData<int>(form).ConfigureAwait(false);
            }
        }

        [TelegramMethod("getChatMember")]
        public async Task<ChatMember> GetChatMember(string chatId, int userId)
        {
            chatId.NullInspect(nameof(chatId));
            var content = new Content();
            content.Add("chat_id", chatId);
            content.Add("user_id", userId.ToString());

            using (var form = new FormUrlEncodedContent(content.Data))
            {
                return await UploadFormData<ChatMember>(form).ConfigureAwait(false);
            }
        }
    }
}