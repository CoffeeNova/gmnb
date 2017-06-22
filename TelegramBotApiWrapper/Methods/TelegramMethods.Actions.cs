using System;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;
using CoffeeJelly.TelegramBotApiWrapper.Attributes;
using CoffeeJelly.TelegramBotApiWrapper.Extensions;
using CoffeeJelly.TelegramBotApiWrapper.Helpers;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Methods
{
    public partial class TelegramMethods
    {
        [TelegramMethod("kickChatMember")]
        public async Task<bool> KickChatMember(string chatId, int userId)
        {
            chatId.NullInspect(nameof(chatId));
            var content = new Content();
            content.Add("chat_id", chatId);
            content.Add("userId", userId.ToString());

            using (var form = new FormUrlEncodedContent(content.Data))
            {
                return await UploadFormData<bool>(form).ConfigureAwait(false);
            }
        }

        [TelegramMethod("leaveChat")]
        public async Task<bool> LeaveChat(string chatId)
        {
            chatId.NullInspect(nameof(chatId));
            var content = new Content();
            content.Add("chat_id", chatId);

            using (var form = new FormUrlEncodedContent(content.Data))
            {
                return await UploadFormData<bool>(form).ConfigureAwait(false);
            }
        }

        [TelegramMethod("unbanChatMember")]
        public async Task<bool> UnbanChatMember(string chatId, int userId)
        {
            chatId.NullInspect(nameof(chatId));
            var content = new Content();
            content.Add("chat_id", chatId);
            content.Add("userId", userId.ToString());

            using (var form = new FormUrlEncodedContent(content.Data))
            {
                return await UploadFormData<bool>(form).ConfigureAwait(false);
            }
        }

        [TelegramMethod("answerCallbackQuery")]
        public async Task<bool> AnswerCallbackQuery(string callbackQueryId, string text = null, bool? showAlert = null, string url = null, int? cacheTime = null)
        {
            callbackQueryId.NullInspect(nameof(callbackQueryId));
            var content = new Content();
            content.Add("callback_query_id", callbackQueryId);
            if(text!=null)
                content.Add("text", text);
            if (showAlert != null)
                content.Add("show_alert", showAlert.ToString());
            if (url != null)
                content.Add("url", url);
            if (cacheTime != null)
                content.Add("cache_time", cacheTime.ToString());

            using (var form = new FormUrlEncodedContent(content.Data))
            {
                return await UploadFormData<bool>(form).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Use this method to delete a message. A message can only be deleted if it was sent less than 48 hours ago. 
        /// Any such recently sent outgoing message may be deleted. Additionally, if the bot is an administrator in a group chat, it can delete any message. 
        /// If the bot is an administrator in a supergroup, it can delete messages from any other user and service messages about people joining 
        /// or leaving the group (other types of service messages may only be removed by the group creator). In channels, bots can only remove their own messages.
        /// </summary>
        /// <param name="chatId">Unique identifier for the target chat or username of the target channel (in the format @channelusername).</param>
        /// <param name="messageId">Identifier of the message to delete.</param>
        /// <returns> Returns <see langword="true"/> on success.</returns>
        [TelegramMethod("deleteMessage")]
        public async Task<bool> DeleteMessage(string chatId, int messageId)
        {
            chatId.NullInspect(nameof(chatId));
            var content = new Content();
            content.Add("chat_id", chatId);
            content.Add("message_id", messageId.ToString());

            using (var form = new FormUrlEncodedContent(content.Data))
            {
                return await UploadFormData<bool>(form).ConfigureAwait(false);
            }
        }
    }

}