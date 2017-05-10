using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CoffeeJelly.gmailNotifyBot.Bot.Exceptions;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using Google.Apis.Gmail.v1.Data;
using HtmlAgilityPack;

[assembly: InternalsVisibleTo("gmailNotifyBotTests")]
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
            ETag = message.ETag;
            LabelIds = message.LabelIds == null ? null : new List<string>(message.LabelIds);
            var messagePartHeader = message.Payload.Headers.FirstOrDefault(h => h.Name == "From");
            if (messagePartHeader != null)
            {
                SenderEmail = messagePartHeader.Value?.GetBetweenFirst('<', '>');
                SenderName = messagePartHeader.Value?.ReplaceFirst($" <{SenderEmail}>", "");
            }

            messagePartHeader = message.Payload.Headers.FirstOrDefault(h => h.Name == "Subject");
            if (messagePartHeader != null)
                Subject = messagePartHeader.Value;
            messagePartHeader = message.Payload.Headers.FirstOrDefault(h => h.Name == "Date");
            if (messagePartHeader != null)
                Date = messagePartHeader.Value;
            _body = new List<string>();
            if (message.Payload.Parts != null)
                DecodeDevidedBody(message.Payload.Parts, _body);
            else if (message.Payload.Body?.Data != null)
                Body.Add(DecodeBody(message.Payload.Body.Data));
        }

        private static string DecodeBody(string base64EncodedData)
        {
            base64EncodedData = base64EncodedData.Replace('-', '+');
            base64EncodedData = base64EncodedData.Replace('_', '/');
            return Base64.Decode(base64EncodedData);
        }

        private void DecodeDevidedBody(IList<MessagePart> parts, List<string> decodedBody)
        {
            parts.NullInspect(nameof(parts));

            foreach (var part in parts)
            {
                if (part.Parts != null)
                    DecodeDevidedBody(part.Parts, decodedBody);
                else if (part.Body?.Data != null)
                    decodedBody.Add(DecodeBody(part.Body.Data));
            }
        }

        private static List<string> FormatBody(IEnumerable<string> body)
        {
            return body?.Select(FormatText).ToList();
        }

        private static string FormatText(string text)
        {
            AddUrlTags(ref text);
            ParseInnerText(ref text);
            ReplaceSymbolsWithHtmlEntities(ref text);
            return text;
        }

        private static void AddUrlTags(ref string text)
        {
        }

        private static void ParseInnerText(ref string text)
        {
            //var htmlDoc = new HtmlDocument();
            //htmlDoc.LoadHtml(text);
            //text = htmlDoc.DocumentNode.InnerText;
        }

        private static void ReplaceSymbolsWithHtmlEntities(ref string text)
        {
            text = text.Replace("&", "&amp;");
            text = text.Replace("<", "&lt;");
            text = text.Replace(">", "&gt;");
            text = text.Replace("\"", "&quot;");
        }



        public string Id { get; set; }

        public string ThreadId { get; set; }

        public string Snippet { get; set; }

        public string SenderName { get; set; }

        public string SenderEmail { get; set; }

        public string Subject { get; set; }

        public string Date { get; set; }

        public string ETag { get; set; }

        public List<string> LabelIds;

        private List<string> _body;

        public List<string> Body
        {
            get { return _body; }
            set
            {
                _body = value;
            }
        }

        private List<string> _formattedBody;

        public List<string> FormattedBody => FormatBody(_body);

        public bool MultiPartBody => Body?.Count > 1;

        //public string FormattedBody => FormateBody();
    }
}