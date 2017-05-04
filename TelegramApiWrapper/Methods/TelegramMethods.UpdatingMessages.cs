using System;
using System.Collections.Specialized;
using CoffeeJelly.TelegramApiWrapper.Attributes;
using CoffeeJelly.TelegramApiWrapper.Extensions;
using CoffeeJelly.TelegramApiWrapper.JsonParsers;
using CoffeeJelly.TelegramApiWrapper.Types.General;
using CoffeeJelly.TelegramApiWrapper.Types.Message;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramApiWrapper.Methods
{
    public partial class TelegramMethods
    {
        [TelegramMethod("editMessageText")]
        public TextMessage EditMessageText(string newText, string chatId = null, int? messageId = null, string inlineMessageId = null,
                                                    string parseMode = null, bool? disabeleWebPagePreview = null, InlineKeyboardMarkup replyMarkup = null)
        {
            newText.NullInspect(nameof(newText));

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