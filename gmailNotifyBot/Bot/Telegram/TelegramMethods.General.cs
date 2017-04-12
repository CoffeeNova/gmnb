using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
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
using CoffeeJelly.gmailNotifyBot.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;


namespace CoffeeJelly.gmailNotifyBot.Bot.Telegram
{
    public partial class TelegramMethods
    {

        public TelegramMethods(string token)
        {
            Token = token;
        }

        public void DownloadFileAsync(File file, string path = null)
        {
            if (path == null) path = FileStorage;

            if (DateTime.Now.Subtract(file.FilePathCreated).Hours > 1)
                throw new TelegramFileDownloadException(
                    $"The link's lifetime is expired. It is probably not valid now. Try to call {nameof(GetFile)} method again");

            using (var webClient = new WebClient())
            {
                try
                {
                    webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;
                    webClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;
                    webClient.DownloadFileAsync(new Uri(TelegramFileUrl + Token + "/" + file.FilePath),
                        path.PathFormatter() + file.FileId);
                }
                catch (WebException ex)
                {
                    throw new TelegramFileDownloadException("Some arguments are not correct.", ex);
                }
            }
        }



        public event DownloadProgressChangedEventHandler DownloadFileProgressChanged;
        public event AsyncCompletedEventHandler DownloadFileCompleted;

        private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            DownloadFileProgressChanged?.Invoke(sender, e);
        }

        private void WebClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            DownloadFileCompleted?.Invoke(sender, e);
        }

        #region private methods
        private void SendMethodsDefaultContent(NameValueCollection collection, string chatId, bool disableNotification,
            int? replyToMessageId, IMarkup replyMarkup, string caption = null)
        {
            collection.Add("disable_notification", disableNotification.ToString());

            if (chatId != null)
                collection.Add("chat_id", chatId);
            if (caption != null)
                collection.Add("caption", caption);
            if (replyToMessageId != null)
                collection.Add("reply_to_message_id", replyToMessageId.ToString());
            if (replyMarkup != null)
                collection.Add("reply_markup",
                    JsonConvert.SerializeObject(replyMarkup, Formatting.None, Settings));
        }

        private void SendMethodsDefaultContent(MultipartFormDataContent form, string chatId, bool disableNotification,
            int? replyToMessageId, IMarkup replyMarkup, string caption = null)
        {
            form.Add(new StringContent(chatId, Encoding.UTF8), "chat_id");
            form.Add(new StringContent(disableNotification.ToString(), Encoding.UTF8), "disable_notification");
            if (caption != null)
                form.Add(new StringContent(caption, Encoding.UTF8), "caption");
            if (replyToMessageId != null)
                form.Add(new StringContent(replyToMessageId.ToString(), Encoding.UTF8), "reply_to_message_id");
            if (replyMarkup != null)
                form.Add(new StringContent(
                        JsonConvert.SerializeObject(replyMarkup, Formatting.None, Settings), Encoding.UTF8),
                    "reply_markup");
        }

        private void AddFileDataContent(MultipartFormDataContent form, string fullFileName, [CallerMemberName] string callerName = "")
        {
            fullFileName.NullInspect(nameof(fullFileName));

            var fileType = TelegramMethodAttribute.GetFileTypeValue(this.GetType(), callerName);
            Debug.Assert(!string.IsNullOrEmpty(fileType),
                $"Use {nameof(TelegramMethodAttribute)} to avoid error.");

            if (!Path.HasExtension(fullFileName))
            {
                form.Add(new StringContent(fullFileName, Encoding.UTF8), fileType);
                return;
            }

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

        private JToken UploadUrlQuery(NameValueCollection collection, [CallerMemberName] string callerName = "")
        {
            var telegramMethodName = TelegramMethodAttribute.GetMethodNameValue(this.GetType(), callerName);
            Debug.Assert(!string.IsNullOrEmpty(telegramMethodName),
                $"Use {nameof(TelegramMethodAttribute)} to avoid error.");

            using (var webClient = new WebClient())
            {
                try
                {
                    var byteResult = webClient.UploadValues(TelegramBotUrl + Token + "/" + telegramMethodName, "POST", collection);
                    var strResult = webClient.Encoding.GetString(byteResult);

                    return JsonConvert.DeserializeObject<JToken>(strResult);
                }
                catch (WebException ex)
                {
                    throw new TelegramMethodsException("Some arguments are not correct.", ex);
                }
            }
        }

        private async Task<TextMessage> UploadFormData(MultipartFormDataContent form, [CallerMemberName] string callerName = "")
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
        #endregion

        public string FileStorage { get; set; } = AppDomain.CurrentDomain.BaseDirectory;

        private string Token { get; }

        private const string TelegramBotUrl = "https://api.telegram.org/bot";
        private const string  TelegramFileUrl = "https://api.telegram.org/file/bot";

        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };
    }
}