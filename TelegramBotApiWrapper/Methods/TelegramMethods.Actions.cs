using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using CoffeeJelly.TelegramBotApiWrapper.Attributes;
using CoffeeJelly.TelegramBotApiWrapper.Extensions;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Methods
{
    public partial class TelegramMethods
    {
        [TelegramMethod("kickChatMember")]
        public bool KickChatMember(string chatId, int userId)
        {
            chatId.NullInspect(nameof(chatId));
            var parameters = new NameValueCollection
            {
                {"chat_id", chatId},
                {"user_id", userId.ToString()}
            };

            var json = UploadUrlQuery(parameters);
            var result = (string) json["result"];
            return result == null ? false : Convert.ToBoolean(result);
        }

        public Task<bool> KickChatMemberAsync(string chatId, int userId)
        {
            chatId.NullInspect(nameof(chatId));
            return Task.Run(() => KickChatMember(chatId, userId));
        }

        [TelegramMethod("leaveChat")]
        public bool LeaveChat(string chatId)
        {
            chatId.NullInspect(nameof(chatId));
            var parameters = new NameValueCollection {{"chat_id", chatId}};

            var json = UploadUrlQuery(parameters);
            var result = (string)json["result"];
            return result == null ? false : Convert.ToBoolean(result);
        }

        public Task<bool> LeaveChatAsync(string chatId)
        {
            chatId.NullInspect(nameof(chatId));
            return Task.Run(() => LeaveChat(chatId));
        }

        [TelegramMethod("unbanChatMember")]
        public bool UnbanChatMember(string chatId, int userId)
        {
            chatId.NullInspect(nameof(chatId));
            var parameters = new NameValueCollection
            {
                {"chat_id", chatId},
                {"user_id", userId.ToString()}
            };

            var json = UploadUrlQuery(parameters);
            var result = (string)json["result"];
            return result == null ? false : Convert.ToBoolean(result);
        }

        public Task<bool> UnbanChatMemberAsync(string chatId, int userId)
        {
            chatId.NullInspect(nameof(chatId));
            return Task.Run(() => UnbanChatMember(chatId, userId));
        }


        [TelegramMethod("answerCallbackQuery")]
        public bool AnswerCallbackQuery(string callbackQueryId, string text = null, bool? showAlert = null, string url = null, int? cacheTime = null)
        {
            callbackQueryId.NullInspect(nameof(callbackQueryId));
            var parameters = new NameValueCollection {{"callback_query_id", callbackQueryId}};
            if(text!=null)
                parameters.Add("text", text);
            if (showAlert != null)
                parameters.Add("show_alert", showAlert.ToString());
            if (url != null)
                parameters.Add("url", url);
            if (cacheTime != null)
                parameters.Add("cache_time", cacheTime.ToString());

            var json = UploadUrlQuery(parameters);
            var result = (string)json["result"];
            return JsonConvert.DeserializeObject<bool>(result);
        }

        public Task<bool> AnswerCallbackQueryAsync(string callbackQueryId, string text = null, bool? showAlert = null, string url = null, int? cacheTime = null)
        {
            callbackQueryId.NullInspect(nameof(callbackQueryId));
            return Task.Run(() => AnswerCallbackQuery(callbackQueryId, text, showAlert, url, cacheTime));
        }
    }

}