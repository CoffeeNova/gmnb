﻿using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CoffeeJelly.TelegramBotApiWrapper.Attributes;
using CoffeeJelly.TelegramBotApiWrapper.Converters;
using CoffeeJelly.TelegramBotApiWrapper.Exceptions;
using CoffeeJelly.TelegramBotApiWrapper.Extensions;
using CoffeeJelly.TelegramBotApiWrapper.Helpers;
using CoffeeJelly.TelegramBotApiWrapper.JsonParsers;
using CoffeeJelly.TelegramBotApiWrapper.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;
using CoffeeJelly.TelegramBotApiWrapper.Types.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[assembly: InternalsVisibleTo("TelegramBotApiWrapperTests")]

namespace CoffeeJelly.TelegramBotApiWrapper.Methods
{
    public partial class TelegramMethods
    {

        public TelegramMethods(string token)
        {
            Token = token;
            _downloadFile = new DownloadFile();
        }

        internal TelegramMethods(string token, bool test = true)
        {
            Token = token;
            _downloadFile = new DownloadFileStub();
        }


        #region private methods
        private void SendMethodsDefaultContent(Content content, string chatId, bool disableNotification,
            int? replyToMessageId, IMarkup replyMarkup, string caption = null)
        {
            content.Add("disable_notification", disableNotification.ToString());

            if (chatId != null)
                content.Add("chat_id", chatId);
            if (caption != null)
                content.Add("caption", caption);
            if (replyToMessageId != null)
                content.Add("reply_to_message_id", replyToMessageId.ToString());
            if (replyMarkup != null)
                content.Add("reply_markup",
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
                        JsonConvert.SerializeObject(replyMarkup, Formatting.None, Settings), Encoding.UTF8, "application/json"),
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

        private async Task<T> UploadFormData<T>(HttpContent content, [CallerMemberName] string callerName = "")
        {
            var response = await UploadMultipartFormDataContent(content, callerName).ConfigureAwait(false);
            var responseObject = JsonConvert.DeserializeObject<Response<T>>(response);
            if (responseObject == null)
                throw new TelegramMethodsException("No response recieved");
            if (!responseObject.Ok)
                throw TelegramMethodsException.CreateException(responseObject);

            return responseObject.Result;
        }

        private async Task<string> UploadMultipartFormDataContent(HttpContent content, [CallerMemberName] string callerName = "")
        {
            var telegramMethodName = TelegramMethodAttribute.GetMethodNameValue(this.GetType(), callerName);
            Debug.Assert(!string.IsNullOrEmpty(telegramMethodName),
                $"Use {nameof(TelegramMethodAttribute)} to avoid error.");

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response =
                        await httpClient.PostAsync(TelegramBotUrl + Token + "/" + telegramMethodName, content).ConfigureAwait(false);
                    return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                }
            }
            catch (HttpRequestException ex)
            {
                throw new TelegramMethodsException(
                    "Bad http request, wrong parameters or something. See inner exception.", ex);
            }
        }

        private void UpdateMethodsDefaultContent(Content content, string chatId = null, string messageId = null,
            string inlineMessageId = null, IMarkup replyMarkup = null)
        {
            if (chatId != null)
                content.Add("chat_id", chatId);
            if (messageId != null)
                content.Add("message_id", messageId);
            if (inlineMessageId != null)
                content.Add("inline_message_id", inlineMessageId);
            if (replyMarkup != null)
                content.Add("reply_markup",
                    JsonConvert.SerializeObject(replyMarkup, Formatting.None, Settings));
        }

        #endregion



        public string FileStorage { get; set; } = AppDomain.CurrentDomain.BaseDirectory;

        private string Token { get; }

        private const string TelegramBotUrl = "https://api.telegram.org/bot";
        private const string TelegramFileUrl = "https://api.telegram.org/file/bot";

        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };
        private readonly IDownloadFile _downloadFile;
    }
}