using System;
using System.Collections.Specialized;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CoffeeJelly.TelegramBotApiWrapper.Attributes;
using CoffeeJelly.TelegramBotApiWrapper.Extensions;
using CoffeeJelly.TelegramBotApiWrapper.Helpers;
using CoffeeJelly.TelegramBotApiWrapper.JsonParsers;
using CoffeeJelly.TelegramBotApiWrapper.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;
using CoffeeJelly.TelegramBotApiWrapper.Types.Messages;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Methods
{
    public partial class TelegramMethods
    {
        [TelegramMethod("editMessageText")]
        public async Task<TextMessage> EditMessageText(string newText, string chatId = null, string messageId = null, string inlineMessageId = null,
                                                    ParseMode? parseMode = null, bool? disableWebPagePreview = null, IMarkup replyMarkup = null)
        {
            newText.NullInspect(nameof(newText));
            if (!newText.Length.InRange(Constants.MESSAGE_TEXT_MIN_LENGTH, Constants.MESSAGE_TEXT_MAX_LENGTH))
                throw new ArgumentOutOfRangeException(nameof(newText),
                    $"Must be in range from {Constants.MESSAGE_TEXT_MIN_LENGTH} to {Constants.MESSAGE_TEXT_MAX_LENGTH}");
            if (inlineMessageId == null)
            {
                if (chatId == null)
                    throw new ArgumentException(
                        $"If {nameof(inlineMessageId)} is not specified {nameof(chatId)} must be non null.");
                if (messageId == null)
                    throw new ArgumentException(
                        $"If {nameof(inlineMessageId)} is not specified {nameof(messageId)} must be non null.");
            }
            else
            {
                if (chatId != null)
                    throw new ArgumentException(
                        $"If {nameof(inlineMessageId)} is specified {nameof(chatId)} must be a null value.");
                if (messageId != null)
                    throw new ArgumentException(
                        $"If {nameof(inlineMessageId)} is specified {nameof(messageId)} must be a null value.");
            }

            var content = new Content { Json = true };
            content.Add("text", newText);
            UpdateMethodsDefaultContent(content, chatId, messageId, inlineMessageId, replyMarkup);

            if (parseMode.HasValue)
            {
                var parse = JsonConvert.SerializeObject(parseMode, Formatting.None, Settings);
                content.Add("parse_mode", parse.Trim('"'));
            }
            if (disableWebPagePreview.HasValue)
                content.Add("disable_web_page_preview", disableWebPagePreview.ToString());

            var json = JsonConvert.SerializeObject(content.JsonData);
            using (var httpContent = new StringContent(json, Encoding.UTF8, "application/json"))
            {
                return await UploadFormData<TextMessage>(httpContent).ConfigureAwait(false);
            }
        }

        public async Task<TextMessage> EditMessageCaption()
        {
            throw new NotImplementedException();
        }

        public async Task<TextMessage> EditMessageReplyMarkup()
        {
            throw new NotImplementedException();
        }
    }
}