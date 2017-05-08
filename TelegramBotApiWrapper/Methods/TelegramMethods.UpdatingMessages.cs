using System;
using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;
using CoffeeJelly.TelegramBotApiWrapper.Attributes;
using CoffeeJelly.TelegramBotApiWrapper.Extensions;
using CoffeeJelly.TelegramBotApiWrapper.JsonParsers;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;
using CoffeeJelly.TelegramBotApiWrapper.Types.Messages;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Methods
{
    public partial class TelegramMethods
    {
        [TelegramMethod("editMessageText")]
        public TextMessage EditMessageText(string newText, string chatId = null, string messageId = null, string inlineMessageId = null,
                                                    string parseMode = null, bool? disabeleWebPagePreview = null, InlineKeyboardMarkup replyMarkup = null)
        {
            newText.NullInspect(nameof(newText));

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
            var parameters = new NameValueCollection { { "text", newText } };

            UpdateMethodsDefaultContent(parameters, chatId, messageId, inlineMessageId, replyMarkup);
           
            if (parseMode != null)
                parameters.Add("parse_mode", parseMode);
            if (disabeleWebPagePreview != null)
                parameters.Add("disable_web_page_preview", disabeleWebPagePreview.ToString());

            var json = UploadUrlQuery(parameters);
            try
            {
                return MessageBuilder.BuildMessage<TextMessage>(json["result"]);
            }
            catch (JsonException)
            {
                return null;
            }
        }

        [TelegramMethod("editMessageText")]
        public Task<TextMessage> EditMessageTextAsync(string newText, string chatId = null, string messageId = null,
                        string inlineMessageId = null, string parseMode = null, bool? disabeleWebPagePreview = null,
                        InlineKeyboardMarkup replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))

        {
            return
                Task.Run(
                    () =>
                        EditMessageText(newText, chatId, messageId, inlineMessageId, parseMode,
                            disabeleWebPagePreview, replyMarkup));
        }

        public TextMessage EditMessageCaption()
        {
            throw new NotImplementedException();
        }

        public TextMessage EditMessageReplyMarkup()
        {
            throw new NotImplementedException();
        }
    }
}