using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CoffeeJelly.TelegramBotApiWrapper.Attributes;
using CoffeeJelly.TelegramBotApiWrapper.Extensions;
using CoffeeJelly.TelegramBotApiWrapper.JsonParsers;
using CoffeeJelly.TelegramBotApiWrapper.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types.Messages;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Methods
{
	public partial class TelegramMethods
	{
        [TelegramMethod("sendMessage")]
        public TextMessage SendMessage(string chatId, string message, ParseMode? parseMode = null, bool disableWebPagePreview = false,
            bool disableNotification = false, int? replyToMessageId = null, IMarkup replyMarkup = null)
        {
            chatId.NullInspect(nameof(chatId));
            message.NullInspect(nameof(message));

            var parameters = new NameValueCollection();
            SendMethodsDefaultContent(parameters, chatId, disableNotification, replyToMessageId, replyMarkup);
            parameters.Add("text", message);
            parameters.Add("disable_web_page_preview", disableWebPagePreview.ToString());

            if (parseMode.HasValue)
            {
                var parse = JsonConvert.SerializeObject(parseMode, Formatting.None, Settings);
                parameters.Add("parse_mode", parse.Trim('"'));

            }
            var json = UploadUrlQuery(parameters);
            return MessageBuilder.BuildMessage<TextMessage>(json["result"]);
        }

        public Task<TextMessage> SendMessageAsync(string chatId, string message, ParseMode? parseMode = null, bool disableWebPagePreview = false,
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

            var json = UploadUrlQuery(parameters);
            return MessageBuilder.BuildMessage<TextMessage>(json["result"]);
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
                return await UploadFormData(form);
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

            var json = UploadUrlQuery(parameters);
            return MessageBuilder.BuildMessage<TextMessage>(json["result"]);
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
                return await UploadFormData(form);
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

            var json = UploadUrlQuery(parameters);
            return MessageBuilder.BuildMessage<TextMessage>(json["result"]);
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
                return await UploadFormData(form);
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

            var json = UploadUrlQuery(parameters);
            return MessageBuilder.BuildMessage<TextMessage>(json["result"]);
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
                return await UploadFormData(form);
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

            var json = UploadUrlQuery(parameters);
            return MessageBuilder.BuildMessage<TextMessage>(json["result"]);
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
                return await UploadFormData(form);
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

            var json = UploadUrlQuery(parameters);
            return MessageBuilder.BuildMessage<TextMessage>(json["result"]);
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
                return await UploadFormData(form);
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

            var json = UploadUrlQuery(parameters);
            return MessageBuilder.BuildMessage<TextMessage>(json["result"]);
        }

        public Task<TextMessage> SendVoiceByUriAsync(string chatId, Uri voiceUri, string caption = null,
            int? duration = null, bool disableNotification = false, int? replyToMessageId = null, IMarkup replyMarkup = null)
        {
            return Task.Run(() =>
                        SendVoiceByUri(chatId, voiceUri, caption, duration, disableNotification,
                            replyToMessageId, replyMarkup));
        }


        [TelegramMethod("sendLocation")]
        public TextMessage SendLocation(string chatId, float latitude, float longitude,
            bool disableNotification = false, int? replyToMessageId = null, IMarkup replyMarkup = null)
        {
            chatId.NullInspect(nameof(chatId));

            var parameters = new NameValueCollection();
            SendMethodsDefaultContent(parameters, chatId, disableNotification, replyToMessageId, replyMarkup);
            parameters.Add("latitude", latitude.ToString(CultureInfo.InvariantCulture));
            parameters.Add("longitude", longitude.ToString(CultureInfo.InvariantCulture));

            var json = UploadUrlQuery(parameters);
            return MessageBuilder.BuildMessage<TextMessage>(json["result"]);
        }

        public Task<TextMessage> SendLocationAsync(string chatId, float latitude, float longitude,
            bool disableNotification = false, int? replyToMessageId = null, IMarkup replyMarkup = null)
        {
            return Task.Run(() =>
                        SendLocation(chatId, latitude, longitude, disableNotification,
                            replyToMessageId, replyMarkup));
        }

        [TelegramMethod("sendVenue")]
        public TextMessage SendVenue(string chatId, float latitude, float longitude, string title, string address,
            string foursquareId = null, bool disableNotification = false, int? replyToMessageId = null, IMarkup replyMarkup = null)
        {
            chatId.NullInspect(nameof(chatId));
            title.NullInspect(nameof(title));
            address.NullInspect(nameof(address));

            var parameters = new NameValueCollection();
            SendMethodsDefaultContent(parameters, chatId, disableNotification, replyToMessageId, replyMarkup);
            parameters.Add("latitude", latitude.ToString(CultureInfo.InvariantCulture));
            parameters.Add("longitude", longitude.ToString(CultureInfo.InvariantCulture));
            parameters.Add("title", title);
            parameters.Add("address", address);
            if (foursquareId != null)
                parameters.Add("foursquare_id", foursquareId);

            var json = UploadUrlQuery(parameters);
            return MessageBuilder.BuildMessage<TextMessage>(json["result"]);
        }

        public Task<TextMessage> SendVenueAsync(string chatId, float latitude, float longitude, string title, string address,
            string foursquareId = null, bool disableNotification = false, int? replyToMessageId = null, IMarkup replyMarkup = null)
        {
            return Task.Run(() =>
                        SendVenue(chatId, latitude, longitude, title, address, foursquareId, disableNotification,
                            replyToMessageId, replyMarkup));
        }

        [TelegramMethod("sendContact")]
        public TextMessage SendContact(string chatId, string phoneNumber, string firstName, string lastName=null, bool disableNotification = false,
		 int? replyToMessageId = null, IMarkup replyMarkup = null)
        {
            chatId.NullInspect(nameof(chatId));
            phoneNumber.NullInspect(nameof(phoneNumber));
            firstName.NullInspect(nameof(firstName));

            var parameters = new NameValueCollection();
            SendMethodsDefaultContent(parameters, chatId, disableNotification, replyToMessageId, replyMarkup);
            parameters.Add("phone_number", phoneNumber);
            parameters.Add("first_name", firstName);
            if (lastName != null)
                parameters.Add("last_name", lastName);

            var json = UploadUrlQuery(parameters);
            return MessageBuilder.BuildMessage<TextMessage>(json["result"]);
        }

        public Task<TextMessage> SendContactAsync(string chatId, string phoneNumber, string firstName, string lastName = null, bool disableNotification = false,
         int? replyToMessageId = null, IMarkup replyMarkup = null)
        {
            return Task.Run(() =>
                        SendContact(chatId, phoneNumber, firstName, lastName, disableNotification,
                            replyToMessageId, replyMarkup));
        }

        [TelegramMethod("sendChatAction")]
        public bool SendChatAction(string chatId, ChatAction action)
	    {
            chatId.NullInspect(nameof(chatId));
	        var parameters = new NameValueCollection
	        {
	            {"chat_id", chatId},
	            {"action", ChatActionAttribute.GetActionValue(action)}
	        };

            var json = UploadUrlQuery(parameters);
            var result = (string)json["result"];
	        return result == null ? false : Convert.ToBoolean(result);
	    }

	    public Task<bool> SendChatActionAsync(string chatId, ChatAction action)
	    {
	        return Task.Run(() => SendChatAction(chatId, action));
        }
    }
}