using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CoffeeJelly.TelegramBotApiWrapper.Attributes;
using CoffeeJelly.TelegramBotApiWrapper.Extensions;
using CoffeeJelly.TelegramBotApiWrapper.Helpers;
using CoffeeJelly.TelegramBotApiWrapper.JsonParsers;
using CoffeeJelly.TelegramBotApiWrapper.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types.Messages;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Methods
{
    public partial class TelegramMethods
    {
        [TelegramMethod("sendMessage")]
        public async Task<TextMessage> SendMessage(string chatId, string message, ParseMode? parseMode = null, bool disableWebPagePreview = false,
            bool disableNotification = false, int? replyToMessageId = null, IMarkup replyMarkup = null)
        {
            chatId.NullInspect(nameof(chatId));
            message.NullInspect(nameof(message));
            if (!message.Length.InRange(Constants.MESSAGE_TEXT_MIN_LENGTH, Constants.MESSAGE_TEXT_MAX_LENGTH))
                throw new ArgumentOutOfRangeException(nameof(message),
                    $"Must be in range from {Constants.MESSAGE_TEXT_MIN_LENGTH} to {Constants.MESSAGE_TEXT_MAX_LENGTH}");

            var content = new Content { Json = true };
            SendMethodsDefaultContent(content, chatId, disableNotification, replyToMessageId, replyMarkup);
            content.Add("text", message);
            content.Add("disable_web_page_preview", disableWebPagePreview.ToString());
            if (parseMode.HasValue)
            {
                var parse = JsonConvert.SerializeObject(parseMode, Formatting.None, Settings);
                content.Add("parse_mode", parse.Trim('"'));
            }

            var json = JsonConvert.SerializeObject(content.JsonData);
            using (var httpContent = new StringContent(json, Encoding.UTF8, "application/json"))
            {
                return await UploadFormData<TextMessage>(httpContent).ConfigureAwait(false);
            }
        }

        [TelegramMethod("forwardMessage")]
        public async Task<T> ForwardMessage<T>(string chatId, string fromChatId, int messageId, bool disableNotification = false) where T : Message
        {
            chatId.NullInspect(nameof(chatId));
            fromChatId.NullInspect(nameof(fromChatId));

            var content = new Content();
            content.Add("from_chat_id", fromChatId);
            content.Add("message_id", messageId.ToString());
            SendMethodsDefaultContent(content, chatId, disableNotification, null, null);

            using (var form = new FormUrlEncodedContent(content.Data))
            {
                return await UploadFormData<T>(form).ConfigureAwait(false);
            }
        }

        [TelegramMethod("sendPhoto", "photo")]
        public async Task<PhotoMessage> SendPhoto(string chatId, string photo, string caption = null,
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
                return await UploadFormData<PhotoMessage>(form).ConfigureAwait(false);
            }
        }

        [TelegramMethod("sendPhoto")]
        public async Task<PhotoMessage> SendPhotoUri(string chatId, Uri photoUri, string caption = null,
            bool disableNotification = false, int? replyToMessageId = null, IMarkup replyMarkup = null)
        {
            chatId.NullInspect(nameof(chatId));
            photoUri.NullInspect(nameof(photoUri));

            var content = new Content { Json = true };
            SendMethodsDefaultContent(content, chatId, disableNotification, replyToMessageId, replyMarkup, caption);
            content.Add("photo", photoUri.OriginalString);

            var json = JsonConvert.SerializeObject(content.JsonData);
            using (var httpContent = new StringContent(json, Encoding.UTF8, "application/json"))
            {
                return await UploadFormData<PhotoMessage>(httpContent).ConfigureAwait(false);
            }
        }

        [TelegramMethod("sendAudio", "audio")]
        public async Task<AudioMessage> SendAudio(string chatId, string audio, string caption = null,
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
                return await UploadFormData<AudioMessage>(form).ConfigureAwait(false);
            }
        }

        [TelegramMethod("sendAudio")]
        public async Task<AudioMessage> SendAudioUri(string chatId, Uri audioUri, string caption = null,
           int? duration = null, string performer = null, string title = null, bool disableNotification = false, int? replyToMessageId = null, IMarkup replyMarkup = null)
        {
            chatId.NullInspect(nameof(chatId));
            audioUri.NullInspect(nameof(audioUri));

            var content = new Content { Json = true };
            SendMethodsDefaultContent(content, chatId, disableNotification, replyToMessageId, replyMarkup, caption);
            content.Add("audio", audioUri.OriginalString);
            if (duration != null)
                content.Add("duration", duration.ToString());
            if (performer != null)
                content.Add("performer", performer);
            if (title != null)
                content.Add("title", title);

            var json = JsonConvert.SerializeObject(content.JsonData);
            using (var httpContent = new StringContent(json, Encoding.UTF8, "application/json"))
            {
                return await UploadFormData<AudioMessage>(httpContent).ConfigureAwait(false);
            }
        }

        [TelegramMethod("sendDocument", "document")]
        public async Task<DocumentMessage> SendDocument(string chatId, string document, string caption = null,
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
                return await UploadFormData<DocumentMessage>(form).ConfigureAwait(false);
            }
        }

        [TelegramMethod("sendDocument")]
        public async Task<DocumentMessage> SendDocumentUri(string chatId, Uri documentUri, string caption = null,
            bool disableNotification = false, int? replyToMessageId = null, IMarkup replyMarkup = null)
        {
            chatId.NullInspect(nameof(chatId));
            documentUri.NullInspect(nameof(documentUri));

            var content = new Content { Json = true };
            SendMethodsDefaultContent(content, chatId, disableNotification, replyToMessageId, replyMarkup, caption);
            content.Add("document", documentUri.OriginalString);

            var json = JsonConvert.SerializeObject(content.JsonData);
            using (var httpContent = new StringContent(json, Encoding.UTF8, "application/json"))
            {
                return await UploadFormData<DocumentMessage>(httpContent).ConfigureAwait(false);
            }
        }

        [TelegramMethod("sendSticker", "sticker")]
        public async Task<StickerMessage> SendSticker(string chatId, string sticker,
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
                return await UploadFormData<StickerMessage>(form).ConfigureAwait(false);
            }
        }

        [TelegramMethod("sendSticker")]
        public async Task<StickerMessage> SendStickerUri(string chatId, Uri stickerUri,
            bool disableNotification = false, int? replyToMessageId = null, IMarkup replyMarkup = null)
        {
            chatId.NullInspect(nameof(chatId));
            stickerUri.NullInspect(nameof(stickerUri));

            var content = new Content { Json = true };
            SendMethodsDefaultContent(content, chatId, disableNotification, replyToMessageId, replyMarkup);
            content.Add("sticker", stickerUri.OriginalString);

            var json = JsonConvert.SerializeObject(content.JsonData);
            using (var httpContent = new StringContent(json, Encoding.UTF8, "application/json"))
            {
                return await UploadFormData<StickerMessage>(httpContent).ConfigureAwait(false);
            }
        }

        [TelegramMethod("sendVideo", "video")]
        public async Task<VideoMessage> SendVideo(string chatId, string video, string caption = null, int? duration = null,
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
                return await UploadFormData<VideoMessage>(form).ConfigureAwait(false);
            }
        }

        [TelegramMethod("sendVideo")]
        public async Task<VideoMessage> SendVideoUri(string chatId, Uri videoUri, string caption = null, int? duration = null,
            int? width = null, int? height = null, bool disableNotification = false, int? replyToMessageId = null, IMarkup replyMarkup = null)
        {
            chatId.NullInspect(nameof(chatId));
            videoUri.NullInspect(nameof(videoUri));

            var content = new Content { Json = true };
            SendMethodsDefaultContent(content, chatId, disableNotification, replyToMessageId, replyMarkup, caption);
            content.Add("video", videoUri.OriginalString);
            if (duration != null)
                content.Add("duration", duration.ToString());
            if (width != null)
                content.Add("width", width.ToString());
            if (height != null)
                content.Add("height", height.ToString());

            var json = JsonConvert.SerializeObject(content.JsonData);
            using (var httpContent = new StringContent(json, Encoding.UTF8, "application/json"))
            {
                return await UploadFormData<VideoMessage>(httpContent).ConfigureAwait(false);
            }
        }

        [TelegramMethod("sendVoice", "voice")]
        public async Task<VoiceMessage> SendVoice(string chatId, string voice, string caption = null, int? duration = null,
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
                return await UploadFormData<VoiceMessage>(form).ConfigureAwait(false);
            }
        }

        [TelegramMethod("sendVoice")]
        public async Task<VoiceMessage> SendVoiceUri(string chatId, Uri voiceUri, string caption = null, int? duration = null,
            bool disableNotification = false, int? replyToMessageId = null, IMarkup replyMarkup = null)
        {
            chatId.NullInspect(nameof(chatId));
            voiceUri.NullInspect(nameof(voiceUri));

            var content = new Content { Json = true };
            SendMethodsDefaultContent(content, chatId, disableNotification, replyToMessageId, replyMarkup, caption);
            content.Add("voice", voiceUri.OriginalString);
            if (duration != null)
                content.Add("duration", duration.ToString());

            var json = JsonConvert.SerializeObject(content.JsonData);
            using (var httpContent = new StringContent(json, Encoding.UTF8, "application/json"))
            {
                return await UploadFormData<VoiceMessage>(httpContent).ConfigureAwait(false);
            }
        }

        [TelegramMethod("sendLocation")]
        public async Task<LocationMessage> SendLocation(string chatId, float latitude, float longitude,
            bool disableNotification = false, int? replyToMessageId = null, IMarkup replyMarkup = null)
        {
            chatId.NullInspect(nameof(chatId));

            var content = new Content { Json = true };
            SendMethodsDefaultContent(content, chatId, disableNotification, replyToMessageId, replyMarkup);
            content.Add("latitude", latitude.ToString(CultureInfo.InvariantCulture));
            content.Add("longitude", longitude.ToString(CultureInfo.InvariantCulture));

            var json = JsonConvert.SerializeObject(content.JsonData);
            using (var httpContent = new StringContent(json, Encoding.UTF8, "application/json"))
            {
                return await UploadFormData<LocationMessage>(httpContent).ConfigureAwait(false);
            }
        }

        [TelegramMethod("sendVenue")]
        public async Task<VenueMessage> SendVenue(string chatId, float latitude, float longitude, string title, string address,
            string foursquareId = null, bool disableNotification = false, int? replyToMessageId = null, IMarkup replyMarkup = null)
        {
            chatId.NullInspect(nameof(chatId));
            title.NullInspect(nameof(title));
            address.NullInspect(nameof(address));

            var content = new Content { Json = true };
            SendMethodsDefaultContent(content, chatId, disableNotification, replyToMessageId, replyMarkup);
            content.Add("latitude", latitude.ToString(CultureInfo.InvariantCulture));
            content.Add("longitude", longitude.ToString(CultureInfo.InvariantCulture));
            content.Add("title", title);
            content.Add("address", address);
            if (foursquareId != null)
                content.Add("foursquare_id", foursquareId);

            var json = JsonConvert.SerializeObject(content.JsonData);
            using (var httpContent = new StringContent(json, Encoding.UTF8, "application/json"))
            {
                return await UploadFormData<VenueMessage>(httpContent).ConfigureAwait(false);
            }
        }

        [TelegramMethod("sendContact")]
        public async Task<ContactMessage> SendContact(string chatId, string phoneNumber, string firstName, string lastName = null, bool disableNotification = false,
         int? replyToMessageId = null, IMarkup replyMarkup = null)
        {
            chatId.NullInspect(nameof(chatId));
            phoneNumber.NullInspect(nameof(phoneNumber));
            firstName.NullInspect(nameof(firstName));

            var content = new Content { Json = true };
            SendMethodsDefaultContent(content, chatId, disableNotification, replyToMessageId, replyMarkup);
            content.Add("phone_number", phoneNumber);
            content.Add("first_name", firstName);
            if (lastName != null)
                content.Add("last_name", lastName);

            var json = JsonConvert.SerializeObject(content.JsonData);
            using (var httpContent = new StringContent(json, Encoding.UTF8, "application/json"))
            {
                return await UploadFormData<ContactMessage>(httpContent).ConfigureAwait(false);
            }
        }

        [TelegramMethod("sendChatAction")]
        public async Task<bool> SendChatAction(string chatId, ChatAction action)
        {
            chatId.NullInspect(nameof(chatId));
            var content = new Content();
            content.Add("chat_id", chatId);
            content.Add("action", ChatActionAttribute.GetActionValue(action));

            using (var form = new FormUrlEncodedContent(content.Data))
            {
                return await UploadFormData<bool>(form).ConfigureAwait(false);
            }
        }

        [TelegramMethod("sendVideoNote", "video_note")]
        public async Task<VideoNoteMessage> SendVideoNote(string chatId, string videoNote, string caption = null, int? duration = null,
            int? length = null, bool disableNotification = false, int? replyToMessageId = null, IMarkup replyMarkup = null)
        {
            chatId.NullInspect(nameof(chatId));
            videoNote.NullInspect(nameof(videoNote));

            if (chatId == string.Empty)
                throw new ArgumentException($"{nameof(chatId)} should not be empty");

            using (var form = new MultipartFormDataContent())
            {
                SendMethodsDefaultContent(form, chatId, disableNotification, replyToMessageId, replyMarkup, caption);
                if (duration != null)
                    form.Add(new StringContent(duration.ToString(), Encoding.UTF8), "duration");
                if (length != null)
                    form.Add(new StringContent(length.ToString(), Encoding.UTF8), "length");

                AddFileDataContent(form, videoNote);
                return await UploadFormData<VideoNoteMessage>(form).ConfigureAwait(false);
            }
        }

        [TelegramMethod("sendVideoNote")]
        public async Task<VideoNoteMessage> SendVideoNoteUri(string chatId, Uri videoNoteUri, string caption = null, int? duration = null,
            int? length = null, bool disableNotification = false, int? replyToMessageId = null, IMarkup replyMarkup = null)
        {
            chatId.NullInspect(nameof(chatId));
            videoNoteUri.NullInspect(nameof(videoNoteUri));

            var content = new Content { Json = true };
            SendMethodsDefaultContent(content, chatId, disableNotification, replyToMessageId, replyMarkup, caption);
            content.Add("voice", videoNoteUri.OriginalString);
            if (duration != null)
                content.Add("duration", duration.ToString());
            if (length != null)
                content.Add("length", length.ToString());

            var json = JsonConvert.SerializeObject(content.JsonData);
            using (var httpContent = new StringContent(json, Encoding.UTF8, "application/json"))
            {
                return await UploadFormData<VideoNoteMessage>(httpContent).ConfigureAwait(false);
            }
        }
    }
}