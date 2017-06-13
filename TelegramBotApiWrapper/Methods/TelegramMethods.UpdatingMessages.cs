using System;
using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;
using CoffeeJelly.TelegramBotApiWrapper.Attributes;
using CoffeeJelly.TelegramBotApiWrapper.Extensions;
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
        public TextMessage EditMessageText(string newText, string chatId = null, string messageId = null, string inlineMessageId = null,
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
               
            var parameters = new NameValueCollection { { "text", newText } };

            UpdateMethodsDefaultContent(parameters, chatId, messageId, inlineMessageId, replyMarkup);

            if (parseMode.HasValue)
            {
                var parse = JsonConvert.SerializeObject(parseMode, Formatting.None, Settings);
                parameters.Add("parse_mode", parse.Trim('"'));
            }
            if (disableWebPagePreview.HasValue)
                parameters.Add("disable_web_page_preview", disableWebPagePreview.ToString());

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
                        string inlineMessageId = null, ParseMode? parseMode = null, bool? disableWebPagePreview = null,
                        IMarkup replyMarkup = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return
                Task.Run(
                    () =>
                        EditMessageText(newText, chatId, messageId, inlineMessageId, parseMode,
                            disableWebPagePreview, replyMarkup), cancellationToken);
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