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

        [TelegramMethod("sendMessage")]
        public TextMessage SendMessage(string chatId, string message, string parseMode = null, bool disableWebPagePreview = false,
            bool disableNotification = false, int? replyToMessageId = null, IMarkup replyMarkup = null)
        {
            chatId.NullInspect(nameof(chatId));
            message.NullInspect(nameof(message));

            var parameters = new NameValueCollection();
            SendMethodsDefaultContent(parameters, chatId, disableNotification, replyToMessageId, replyMarkup);
            parameters.Add("text", message);
            parameters.Add("disable_web_page_preview", disableWebPagePreview.ToString());
            if (parseMode != null) parameters.Add("parse_mode", parseMode);

            return UploadValues(parameters);
        }


        public Task<TextMessage> SendMessageAsync(string chatId, string message, string parseMode = null, bool disableWebPagePreview = false,
            bool disableNotification = false, int? replyToMessageId = null, IMarkup replyMarkup = null)
        {
            return Task.Run(
                    () => SendMessage(chatId, message, parseMode, disableWebPagePreview, disableNotification,
                        replyToMessageId, replyMarkup));
        }

        [TelegramMethod("forwardMessage")]
        public TextMessage ForwardMessage(string chatId, string fromChatId, int messageId, bool disableNotification = false)
        {
            chatId.NullInspect(nameof(chatId));
            fromChatId.NullInspect(nameof(fromChatId));

            var parameters = new NameValueCollection
            {
                {"from_chat_id", fromChatId},
                {"message_id", messageId.ToString()}
            };
            SendMethodsDefaultContent(parameters, chatId, disableNotification, null, null);
            return UploadValues(parameters);
        }

        public Task<TextMessage> ForwardMessageAsync(string chatId, string fromChatId, int messageId,
            bool disableNotification = false)
        {
            return Task.Run(() => ForwardMessage(chatId, fromChatId, messageId, disableNotification));
        }

        [TelegramMethod("sendPhoto", "photo")]
        public async Task<TextMessage> SendPhoto(string chatId, string photo, string caption = null,
            bool disableNotification = false, int? replyToMessageId = null, IMarkup replyMarkup = null)
        {
            chatId.NullInspect(nameof(chatId));
            photo.NullInspect(nameof(photo));

            if (chatId == string.Empty)
                throw new ArgumentException($"{nameof(chatId)} should not be empty");

            using (var form = new MultipartFormDataContent())
            {
                SendMethodsDefaultContent(form, chatId, disableNotification, replyToMessageId, replyMarkup, caption);
                AddFileDataContent(form, photo);
                return await UploadFile(form);
            }
        }

        [TelegramMethod("sendPhoto")]
        public TextMessage SendPhotoByUri(string chatId, Uri photoUri, string caption = null,
            bool disableNotification = false, int? replyToMessageId = null, IMarkup replyMarkup = null)
        {
            chatId.NullInspect(nameof(chatId));
            photoUri.NullInspect(nameof(photoUri));

            var parameters = new NameValueCollection();
            SendMethodsDefaultContent(parameters, chatId, disableNotification, replyToMessageId, replyMarkup, caption);
            parameters.Add("photo", photoUri.OriginalString);

            return UploadValues(parameters);
        }

        public Task<TextMessage> SendPhotoByUriAsync(string chatId, Uri photoUri, string caption = null,
            bool disableNotification = false, int? replyToMessageId = null, IMarkup replyMarkup = null)
        {
            return
                Task.Run(
                    () => SendPhotoByUri(chatId, photoUri, caption, disableNotification, replyToMessageId, replyMarkup));
        }

        [TelegramMethod("sendAudio", "audio")]
        public async Task<TextMessage> SendAudio(string chatId, string audio, string caption = null,
           int? duration = null, string performer = null, string title = null, bool disableNotification = false, int? replyToMessageId = null, IMarkup replyMarkup = null)
        {
            chatId.NullInspect(nameof(chatId));
            audio.NullInspect(nameof(audio));

            if (chatId == string.Empty)
                throw new ArgumentException($"{nameof(chatId)} should not be empty");

            using (var form = new MultipartFormDataContent())
            {
                SendMethodsDefaultContent(form, chatId, disableNotification, replyToMessageId, replyMarkup, caption);
                if (duration != null)
                    form.Add(new StringContent(duration.ToString(), Encoding.UTF8), "duration");
                if (performer != null)
                    form.Add(new StringContent(performer, Encoding.UTF8), "performer");
                if (title != null)
                    form.Add(new StringContent(title, Encoding.UTF8), "title");

                AddFileDataContent(form, audio);
                return await UploadFile(form);
            }
        }

        [TelegramMethod("sendAudio")]
        public TextMessage SendAudioByUri(string chatId, Uri audioUri, string caption = null,
           int? duration = null, string performer = null, string title = null, bool disableNotification = false, int? replyToMessageId = null, IMarkup replyMarkup = null)
        {
            chatId.NullInspect(nameof(chatId));
            audioUri.NullInspect(nameof(audioUri));

            var parameters = new NameValueCollection();
            SendMethodsDefaultContent(parameters, chatId, disableNotification, replyToMessageId, replyMarkup, caption);
            parameters.Add("audio", audioUri.OriginalString);
            if (duration != null)
                parameters.Add("duration", duration.ToString());
            if (performer != null)
                parameters.Add("performer", performer);
            if (title != null)
                parameters.Add("title", title);

            return UploadValues(parameters);
        }

        public Task<TextMessage> SendAudioByUriAsync(string chatId, Uri audioUri, string caption = null,
            int? duration = null, string performer = null, string title = null, bool disableNotification = false,
            int? replyToMessageId = null, IMarkup replyMarkup = null)
        {
            return Task.Run(() =>
                        SendAudioByUri(chatId, audioUri, caption, duration, performer, title, disableNotification,
                            replyToMessageId, replyMarkup));
        }

        [TelegramMethod("sendDocument", "document")]
        public async Task<TextMessage> SendDocument(string chatId, string document, string caption = null,
             bool disableNotification = false, int? replyToMessageId = null, IMarkup replyMarkup = null)
        {
            chatId.NullInspect(nameof(chatId));
            document.NullInspect(nameof(document));

            if (chatId == string.Empty)
                throw new ArgumentException($"{nameof(chatId)} should not be empty");

            using (var form = new MultipartFormDataContent())
            {
                SendMethodsDefaultContent(form, chatId, disableNotification, replyToMessageId, replyMarkup, caption);

                AddFileDataContent(form, document);
                return await UploadFile(form);
            }
        }

        [TelegramMethod("sendDocument")]
        public TextMessage SendDocumentByUri(string chatId, Uri documentUri, string caption = null,
            bool disableNotification = false, int? replyToMessageId = null, IMarkup replyMarkup = null)
        {
            chatId.NullInspect(nameof(chatId));
            documentUri.NullInspect(nameof(documentUri));

            var parameters = new NameValueCollection();
            SendMethodsDefaultContent(parameters, chatId, disableNotification, replyToMessageId, replyMarkup, caption);
            parameters.Add("document", documentUri.OriginalString);

            return UploadValues(parameters);
        }

        public Task<TextMessage> SendDocumentByUriAsync(string chatId, Uri documentUri, string caption = null,
            bool disableNotification = false, int? replyToMessageId = null, IMarkup replyMarkup = null)
        {
            return Task.Run(() =>
                    SendDocumentByUri(chatId, documentUri, caption, disableNotification, replyToMessageId,
                        replyMarkup));
        }

        [TelegramMethod("sendSticker", "sticker")]
        public async Task<TextMessage> SendSticker(string chatId, string sticker,
             bool disableNotification = false, int? replyToMessageId = null, IMarkup replyMarkup = null)
        {
            chatId.NullInspect(nameof(chatId));
            sticker.NullInspect(nameof(sticker));

            if (chatId == string.Empty)
                throw new ArgumentException($"{nameof(chatId)} should not be empty");

            using (var form = new MultipartFormDataContent())
            {
                SendMethodsDefaultContent(form, chatId, disableNotification, replyToMessageId, replyMarkup);
                AddFileDataContent(form, sticker);
                return await UploadFile(form);
            }
        }

        [TelegramMethod("sendSticker")]
        public TextMessage SendStickerByUri(string chatId, Uri stickerUri,
            bool disableNotification = false, int? replyToMessageId = null, IMarkup replyMarkup = null)
        {
            chatId.NullInspect(nameof(chatId));
            stickerUri.NullInspect(nameof(stickerUri));

            var parameters = new NameValueCollection();
            SendMethodsDefaultContent(parameters, chatId, disableNotification, replyToMessageId, replyMarkup);
            parameters.Add("sticker", stickerUri.OriginalString);

            return UploadValues(parameters);
        }

        public Task<TextMessage> SendStickerByUriAsync(string chatId, Uri stickerUri,
            bool disableNotification = false, int? replyToMessageId = null, IMarkup replyMarkup = null)
        {
            return Task.Run(() =>
                    SendStickerByUri(chatId, stickerUri, disableNotification, replyToMessageId,
                        replyMarkup));
        }

        [TelegramMethod("sendVideo", "video")]
        public async Task<TextMessage> SendVideo(string chatId, string video, string caption = null, int? duration = null,
            int? width = null, int? height = null, bool disableNotification = false, int? replyToMessageId = null, IMarkup replyMarkup = null)
        {
            chatId.NullInspect(nameof(chatId));
            video.NullInspect(nameof(video));

            if (chatId == string.Empty)
                throw new ArgumentException($"{nameof(chatId)} should not be empty");

            using (var form = new MultipartFormDataContent())
            {
                SendMethodsDefaultContent(form, chatId, disableNotification, replyToMessageId, replyMarkup, caption);
                if (duration != null)
                    form.Add(new StringContent(duration.ToString(), Encoding.UTF8), "duration");
                if (width != null)
                    form.Add(new StringContent(width.ToString(), Encoding.UTF8), "width");
                if (height != null)
                    form.Add(new StringContent(height.ToString(), Encoding.UTF8), "height");

                AddFileDataContent(form, video);
                return await UploadFile(form);
            }
        }

        [TelegramMethod("sendVideo")]
        public TextMessage SendVideoByUri(string chatId, Uri videoUri, string caption = null, int? duration = null,
            int? width = null, int? height = null, bool disableNotification = false, int? replyToMessageId = null, IMarkup replyMarkup = null)
        {
            chatId.NullInspect(nameof(chatId));
            videoUri.NullInspect(nameof(videoUri));

            var parameters = new NameValueCollection();
            SendMethodsDefaultContent(parameters, chatId, disableNotification, replyToMessageId, replyMarkup, caption);
            parameters.Add("video", videoUri.OriginalString);
            if (duration != null)
                parameters.Add("duration", duration.ToString());
            if (width != null)
                parameters.Add("width", width.ToString());
            if (height != null)
                parameters.Add("height", height.ToString());

            return UploadValues(parameters);
        }

        public Task<TextMessage> SendVideoByUriAsync(string chatId, Uri videoUri, string caption = null, int? duration = null,
            int? width = null, int? height = null, bool disableNotification = false, int? replyToMessageId = null, IMarkup replyMarkup = null)
        {
            return Task.Run(() =>
                        SendVideoByUri(chatId, videoUri, caption, duration, width, height, disableNotification,
                            replyToMessageId, replyMarkup));
        }

        [TelegramMethod("sendVoice", "voice")]
        public async Task<TextMessage> SendVoice(string chatId, string voice, string caption = null, int? duration = null,
            bool disableNotification = false, int? replyToMessageId = null, IMarkup replyMarkup = null)
        {
            chatId.NullInspect(nameof(chatId));
            voice.NullInspect(nameof(voice));

            if (chatId == string.Empty)
                throw new ArgumentException($"{nameof(chatId)} should not be empty");

            using (var form = new MultipartFormDataContent())
            {
                SendMethodsDefaultContent(form, chatId, disableNotification, replyToMessageId, replyMarkup, caption);
                if (duration != null)
                    form.Add(new StringContent(duration.ToString(), Encoding.UTF8), "duration");

                AddFileDataContent(form, voice);
                return await UploadFile(form);
            }
        }

        [TelegramMethod("sendVoice")]
        public TextMessage SendVoiceByUri(string chatId, Uri voiceUri, string caption = null, int? duration = null,
            bool disableNotification = false, int? replyToMessageId = null, IMarkup replyMarkup = null)
        {
            chatId.NullInspect(nameof(chatId));
            voiceUri.NullInspect(nameof(voiceUri));

            var parameters = new NameValueCollection();
            SendMethodsDefaultContent(parameters, chatId, disableNotification, replyToMessageId, replyMarkup, caption);
            parameters.Add("voice", voiceUri.OriginalString);
            if (duration != null)
                parameters.Add("duration", duration.ToString());

            return UploadValues(parameters);
        }

        public Task<TextMessage> SendVoiceByUriAsync(string chatId, Uri voiceUri, string caption = null,
            int? duration = null,  bool disableNotification = false, int? replyToMessageId = null, IMarkup replyMarkup = null)
        {
            return Task.Run(() =>
                        SendVoiceByUri(chatId, voiceUri, caption, duration,  disableNotification,
                            replyToMessageId, replyMarkup));
        }


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

        private TextMessage UploadValues(NameValueCollection collection, [CallerMemberName] string callerName = "")
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
                    var json = JsonConvert.DeserializeObject<JToken>(strResult);
                    return MessageBuilder.BuildMessage<TextMessage>(json["result"]);
                }
                catch (WebException ex)
                {
                    throw new TelegramMethodsException("Some arguments are not correct.", ex);
                }
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



        private string Token { get; }

        private const string TelegramBotUrl = "https://api.telegram.org/bot";

        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };
    }
}