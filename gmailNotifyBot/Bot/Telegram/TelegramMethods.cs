using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
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

        public User GetMe()
        {
            using (var webClient = new WebClient())
            {
                var content = webClient.DownloadString(TelegramBotUrl + Token + "/getMe");
                var newRequests = JsonConvert.DeserializeObject<JObject>(content);
                return GeneralBuilder.BuildUser(newRequests["result"]);
            }
        }

        public Task<User> GetMeAsync()
        {
            return Task.Run(() => GetMe());
        }

        public TextMessage SendMessage(long chatId, string message, string parseMode = null, bool disableWebPagePreview = false,
            bool disableNotification = false, int? replyToMessageId = null, IMarkup replyMarkup = null)
        {
            using (var webClient = new WebClient())
            {
                webClient.Headers.Add(HttpRequestHeader.ContentType, "application/json");
               
                var parameters = new NameValueCollection
                {
                    {"chat_id", chatId.ToString()},
                    {"text", message},
                    {"disable_notification", disableNotification.ToString()},
                    {"disable_web_page_preview", disableWebPagePreview.ToString() }
                };
                if (parseMode != null)
                    parameters.Add("parse_mode", parseMode);
                if (replyToMessageId != null)
                    parameters.Add("reply_to_message_id", replyToMessageId.ToString());
                if (replyMarkup != null)
                {
                    var test = JsonConvert.SerializeObject(replyMarkup, Formatting.None, Settings);
                   parameters.Add("reply_markup", JsonConvert.SerializeObject(replyMarkup, Formatting.None));


                    //var testJson =
                    //{"inline_keyboard":[[[{"text":"URL Button","url":"https://www.twitch.tv"},{"text":"Callback Button","callback_data":"Test callback data"}]]]}
                    //parameters.Add("reply_markup", testJson);
                }

                var byteResult = webClient.UploadValues(TelegramBotUrl + Token + "/sendMessage","POST" , parameters);
                //webClient.UploadData("",,)
                var strResult = webClient.Encoding.GetString(byteResult);
                var jsonObject = JsonConvert.DeserializeObject<JObject>(strResult);
                return MessageBuilder.BuildMessage<TextMessage>(jsonObject["result"]);
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