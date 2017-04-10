using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram.Exceptions;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram.JsonParsers;
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

        public TextMessage SendMessage(long chatId, string message, string parseMode = null, bool disableWebPagePreview = false,
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
                        {"chat_id", chatId.ToString()},
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

        public Task<TextMessage> SendMessageAsync(long chatId, string message, string parseMode = null, bool disableWebPagePreview = false,
            bool disableNotification = false, int? replyToMessageId = null, IMarkup replyMarkup = null)
        {

            return Task.Run(
                    () => SendMessage(chatId, message, parseMode, disableWebPagePreview, disableNotification,
                        replyToMessageId, replyMarkup));
        }


        public TextMessage ForwardMessage(long chatId, long fromChatId, int messageId, bool disableNotification = false)
        {
            using (var webClient = new WebClient())
            {
                try
                {
                    var parameters = new NameValueCollection
                    {
                        {"chat_id", chatId.ToString()},
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

        public Task<TextMessage> ForwardMessageAsync(long chatId, long fromChatId, int messageId,
            bool disableNotification = false)
        {
            return Task.Run(() => ForwardMessage(chatId, fromChatId, messageId, disableNotification));
        }

        public TextMessage SendPhoto(long chatId, string fullFileName, string caption = "",
            bool disableNotification = false, int? replyToMEssageId = null, IMarkup replyMarkup = null)
        {
            using (var form = new MultipartFormDataContent())

        }



        private string Token { get; }

        private const string TelegramBotUrl = "https://api.telegram.org/bot";

        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };
    }
}