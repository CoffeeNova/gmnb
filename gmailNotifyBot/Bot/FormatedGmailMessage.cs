using System;
using System.Collections.Generic;
using System.Linq;
using CoffeeJelly.gmailNotifyBot.Bot.Exceptions;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using Google.Apis.Gmail.v1.Data;
using HtmlAgilityPack;

namespace CoffeeJelly.gmailNotifyBot.Bot
{
    internal sealed class FormattedGmailMessage
    {
        public FormattedGmailMessage()
        {

        }

        public FormattedGmailMessage(Message message)
        {
            if (message?.Payload == null)
                throw new FormattedGmailMessageException($"{nameof(message.Payload)}");

            Id = message.Id;
            ThreadId = message.ThreadId;
            Snippet = message.Snippet;
            var messagePartHeader = message.Payload.Headers.FirstOrDefault(h => h.Name == "From");
            if (messagePartHeader != null)
                Sender = messagePartHeader.Value;
            messagePartHeader = message.Payload.Headers.FirstOrDefault(h => h.Name == "Subject");
            if (messagePartHeader != null)
                Subject = messagePartHeader.Value;
            messagePartHeader = message.Payload.Headers.FirstOrDefault(h => h.Name == "Date");
            if (messagePartHeader != null)
                Date = messagePartHeader.Value;
            if (message.Payload.Parts != null)
                DecodeDevidedBody(message.Payload.Parts, out _body);
            else if (message.Payload.Body?.Data != null)
                Body = DecodeBody(message.Payload.Body.Data);
        }

        private static string DecodeBody(string base64EncodedData)
        {
            base64EncodedData = base64EncodedData.Replace('-', '+');
            base64EncodedData = base64EncodedData.Replace('_', '/');
            return Base64.Decode(base64EncodedData);
        }

        private void DecodeDevidedBody(IList<MessagePart> parts, out string decodedBody)
        {
            parts.NullInspect(nameof(parts));

            decodedBody = "";
            foreach (var part in parts)
            {
                if (part.Parts != null)
                    DecodeDevidedBody(part.Parts, out decodedBody);
                else if (part.Body?.Data != null)
                    decodedBody += DecodeBody(part.Body.Data);
            }
        }

        private string FormateBody()
        {
            if(_body == null) return null;

            throw new NotImplementedException();
        }

        private string ParseInnerText(string html)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            return htmlDoc.DocumentNode.InnerText;
        }

        public string Id { get; set; }

        public string ThreadId { get; set; }

        public string Snippet { get; private set; }

        public string Sender { get; private set; }

        public string Subject { get; private set; }

        public string Date { get; private set; }

        private string _body;

        public string Body
        {
            get { return _body; }
            set { _body = value; }
        }

        public string FormattedBody => FormateBody();
    }
}