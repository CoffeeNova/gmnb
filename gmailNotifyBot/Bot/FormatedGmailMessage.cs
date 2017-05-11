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
            _body = new List<BodyForm>();
            if (message.Payload.Parts != null)
                DecodeDevidedBody(message.Payload.Parts, _body);
            else if (message.Payload.Body?.Data != null)
                _body.Add(new BodyForm(message.Payload.MimeType, DecodeBody(message.Payload.Body.Data)));
        }

        private static string DecodeBody(string base64EncodedData)
        {
            base64EncodedData = base64EncodedData.Replace('-', '+');
            base64EncodedData = base64EncodedData.Replace('_', '/');
            return Base64.Decode(base64EncodedData);
        }

        private void DecodeDevidedBody(IList<MessagePart> parts, List<BodyForm> decodedBody)
        {
            parts.NullInspect(nameof(parts));

            foreach (var part in parts)
            {
                if (part.Parts != null)
                    DecodeDevidedBody(part.Parts, decodedBody);
                else if (part.Body?.Data != null)
                    decodedBody.Add(new BodyForm(part.MimeType, DecodeBody(part.Body.Data)));
            }
        }

        private List<string> FormatBody(List<BodyForm> body)
        {
            var stringedBody = "";
            foreach (var bodyForm in body)
            {
                if (!bodyForm.MimeType.EqualsAny(MimeTypes?.ToArray())) continue;

                if (bodyForm.MimeType == "text/html" && HtmlConvertToPlain)
                    stringedBody += HtmlConvertToPlainText(bodyForm.Value);
                else
                    stringedBody += bodyForm.Value;
            }
            if (String.IsNullOrEmpty(stringedBody))
                return new List<string>();

            var formatted = FormatText(stringedBody);
            return DivideIntoPages(formatted, 2000, 3000);
        }

        private static string HtmlConvertToPlainText(string html)
        {
            return HtmlToText.ConvertHtml(html);
            //return HtmlFilter.ConvertToPlainText(html);
        }

        private string FormatText(string text)
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

        private static List<string> DivideIntoPages(string text, int minChunkLength, int maxChunkLength)
        {
            if (minChunkLength < 1)
                throw new ArgumentOutOfRangeException(nameof(maxChunkLength), "Must equals at least 1");

            int floatingLength = 0;
            int chunks = (int)Math.Ceiling(text.Length / (double)minChunkLength);
            var devided = Enumerable.Range(0, chunks).Select(i =>
            {
                if (floatingLength >= text.Length)
                    return null;
                if (floatingLength + maxChunkLength > text.Length)
                {
                    floatingLength = text.Length;
                    return text.Substring(floatingLength);
                }
                var temp = text.Substring(floatingLength, maxChunkLength);
                var newLineIndex = temp.LastIndexOf(Environment.NewLine, StringComparison.Ordinal);
                if (newLineIndex < minChunkLength)
                {
                    floatingLength += maxChunkLength;
                    return temp;
                }
                floatingLength += newLineIndex + 1;
                return temp.Substring(0, newLineIndex);
            }).ToList();
            devided.RemoveAll(i => string.IsNullOrEmpty(i));
            return devided;
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

        public int LinesPerPage { get; set; } = 30;

        private List<BodyForm> _body;

        public List<BodyForm> Body
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

        public List<string> MimeTypes { get; set; } = new List<string> { "text/plain" };

        public bool HtmlConvertToPlain { get; set; } = true;
    }

    public class BodyForm
    {
        public BodyForm(string mimeType, string value)
        {
            MimeType = mimeType;
            Value = value;
        }

        public string MimeType { get; }

        public string Value { get; }
    }
}