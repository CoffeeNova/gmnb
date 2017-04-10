using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram.Exceptions;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram.JsonParsers;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;


namespace CoffeeJelly.gmailNotifyBot.Bot.Telegram
{
    public class TelegramMethods
    {

        public TelegramMethods(string token)
        {
            Token = token;
        }

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

        public TextMessage SendMessage(string chatId, string message, string parseMode = null, bool disableWebPagePreview = false,
            bool disableNotification = false, int? replyToMessageId = null, IMarkup replyMarkup = null)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentNullException(nameof(message));

            using (var webClient = new WebClient())
            {
                try
                {
                    var test = disableNotification.ToString();
                    var parameters = new NameValueCollection
                    {
                        {"chat_id", chatId},
                        {"text", message},
                        {"disable_notification", disableNotification.ToString()},
                        {"disable_web_page_preview", disableWebPagePreview.ToString()}
                    };
                    if (parseMode != null)
                        parameters.Add("parse_mode", parseMode);
                    if (replyToMessageId != null)
                        parameters.Add("reply_to_message_id", replyToMessageId.ToString());
                    if (replyMarkup != null)
                        parameters.Add("reply_markup",
                            JsonConvert.SerializeObject(replyMarkup, Formatting.None, Settings));

                    var byteResult = webClient.UploadValues(TelegramBotUrl + Token + "/sendMessage", "POST", parameters);
                    var strResult = webClient.Encoding.GetString(byteResult);
                    var json = JsonConvert.DeserializeObject<JToken>(strResult);
                    return MessageBuilder.BuildMessage<TextMessage>(json["result"]);
                }
                catch (WebException ex)
                {
                    throw new TelegramMethodsException("Some arguments are not correct.", ex);
                }
            }
        }

        public Task<TextMessage> SendMessageAsync(string chatId, string message, string parseMode = null, bool disableWebPagePreview = false,
            bool disableNotification = false, int? replyToMessageId = null, IMarkup replyMarkup = null)
        {

            return Task.Run(
                    () => SendMessage(chatId, message, parseMode, disableWebPagePreview, disableNotification,
                        replyToMessageId, replyMarkup));
        }


        public TextMessage ForwardMessage(string chatId, long fromChatId, int messageId, bool disableNotification = false)
        {
            using (var webClient = new WebClient())
            {
                try
                {
                    var parameters = new NameValueCollection
                    {
                        {"chat_id", chatId},
                        {"from_chat_id", fromChatId.ToString()},
                        {"message_id", messageId.ToString()},
                        {"disable_notification", disableNotification.ToString()}
                    };

                    var byteResult = webClient.UploadValues(TelegramBotUrl + Token + "/forwardMessage", "POST",
                        parameters);
                    var strResult = webClient.Encoding.GetString(byteResult);
                    var json = JsonConvert.DeserializeObject<JToken>(strResult);
                    return MessageBuilder.BuildMessage<TextMessage>(json["result"]);
                }
                catch (WebException ex)
                {
                    throw new TelegramMethodsException("Wrong method arguments, probably chat ides does not exist or wrong message id", ex);
                }
            }
        }

        public Task<TextMessage> ForwardMessageAsync(string chatId, long fromChatId, int messageId,
            bool disableNotification = false)
        {
            return Task.Run(() => ForwardMessage(chatId, fromChatId, messageId, disableNotification));
        }

        [TelegramMethod("sendPhoto", "photo")]
        public async Task<TextMessage> SendPhoto(string chatId, string fullFileName, string caption = "",
            bool disableNotification = false, int? replyToMessageId = null, IMarkup replyMarkup = null)
        {
            chatId.NullInspect(nameof(chatId));
            caption.NullInspect(nameof(caption));

            if (chatId == string.Empty)
                throw new ArgumentException($"{nameof(chatId)} should not be empty");

            using (var form = new MultipartFormDataContent())
            {
                form.Add(new StringContent(chatId, Encoding.UTF8), "chat_id");
                form.Add(new StringContent(caption, Encoding.UTF8), "caption");
                form.Add(new StringContent(disableNotification.ToString(), Encoding.UTF8), "disable_notification");
                if (replyToMessageId != null)
                    form.Add(new StringContent(replyToMessageId.ToString(), Encoding.UTF8), "reply_to_message_id");
                if (replyMarkup != null)
                    form.Add(new StringContent(
                            JsonConvert.SerializeObject(replyMarkup, Formatting.None, Settings), Encoding.UTF8),
                        "reply_markup");

                AddFileDataContent(form, fullFileName);
                return await UploadFile(form);
            }
        }

        private async Task<TextMessage> UploadFile(MultipartFormDataContent form, [CallerMemberName] string callerName = "")
        {
            var telegramMethodName = TelegramMethodAttribute.GetMethodNameValue(this.GetType(), callerName);
            Debug.Assert(!string.IsNullOrEmpty(telegramMethodName),
                $"Use {nameof(TelegramMethodAttribute)} to avoid error.");

            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    var responce = await httpClient.PostAsync(TelegramBotUrl + Token + "/" + telegramMethodName, form);
                    var strResult = await responce.Content.ReadAsStringAsync();
                    var json = JsonConvert.DeserializeObject<JToken>(strResult);
                    return MessageBuilder.BuildMessage<TextMessage>(json["result"]);
                }
            }
            catch (HttpRequestException ex)
            {
                throw new TelegramMethodsException(
                    "Bad http request, wrong parameters or something. See inner exception.", ex);
            }
        }

        private void AddFileDataContent(MultipartFormDataContent form, string fullFileName, [CallerMemberName] string callerName = "")
        {
            fullFileName.NullInspect(nameof(fullFileName));

            var fileType = TelegramMethodAttribute.GetFileTypeValue(this.GetType(), callerName);
            Debug.Assert(!string.IsNullOrEmpty(fileType),
                $"Use {nameof(TelegramMethodAttribute)} to avoid error.");

            try
            { //can't use using here, because these streams are necessary open
                var fileStream = new FileStream(fullFileName, FileMode.Open, FileAccess.Read);
                form.Add(new StreamContent(fileStream), fileType, Path.GetFileName(fullFileName));
            }
            catch (Exception ex)
            {
                throw new TelegramMethodsException($"Something wrong with the file {fullFileName}", ex);
            }

        }


        private string Token { get; }

        private const string TelegramBotUrl = "https://api.telegram.org/bot";

        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };
    }
}