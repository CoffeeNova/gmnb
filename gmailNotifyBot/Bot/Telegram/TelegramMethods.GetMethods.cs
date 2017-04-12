using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram.Attributes;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram.Exceptions;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram.JsonParsers;
using CoffeeJelly.gmailNotifyBot.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CoffeeJelly.gmailNotifyBot.Bot.Telegram
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

    }
}