using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram.Attributes;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram.JsonParsers;
using Newtonsoft.Json;

namespace CoffeeJelly.gmailNotifyBot.Bot.Telegram
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