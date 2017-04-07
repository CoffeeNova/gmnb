using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


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
                return MessageBuilder.BuildUser(newRequests["result"]);
            }
        }

        public Task<User> GetMeAsync()
        {
            return Task.Run(() => GetMe());
        }

        public TextMessage SendMessage(int chatId, string message, string parseMode=null, 
            bool disableNotification = false, int? replyToMessageId = null, Markup replyMarkup = null)
        {
            using (var webClient = new WebClient())
            {
                var parameters = new NameValueCollection();
                parameters.Add()
            }
        }

        private string Token { get;}

        private const string TelegramBotUrl = "https://api.telegram.org/bot";
    }
}