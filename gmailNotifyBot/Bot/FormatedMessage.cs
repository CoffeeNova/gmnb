using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using CoffeeJelly.gmailNotifyBot.Bot.Exceptions;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using Google.Apis.Gmail.v1.Data;
using HtmlAgilityPack;

[assembly: InternalsVisibleTo("gmailNotifyBotTests")]
namespace CoffeeJelly.gmailNotifyBot.Bot
{
    internal sealed class FormattedMessage
    {
        public FormattedMessage()
        {

        }

        public FormattedMessage(Message message)
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
            var body = new List<BodyForm>();
            if (message.Payload.Parts != null)
                DecodeDevidedBody(message.Payload.Parts, body);
            else if (message.Payload.Body?.Data != null)
                body.Add(new BodyForm(message.Payload.MimeType, DecodeBody(message.Payload.Body.Data)));

            Body = body;
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

        private List<string> FormatTextBody(IEnumerable<BodyForm> body)
        {
            if (body == null)
                return null;
            var textBody = "";
            foreach (var bodyForm in body)
                if (bodyForm.MimeType == "text/plain")
                    textBody += bodyForm.Value;

            if (String.IsNullOrEmpty(textBody))
                return null;

            var formatted = FormatText(textBody);
            return DivideIntoPages(formatted, MinSymbolsToBreakPage, MaxSymbolsToBreakPage);
        }

        private List<string> FormatHtmlBody(IEnumerable<BodyForm> body)
        {
            if (body == null)
                return null;
            var htmlBody = "";
            foreach (var bodyForm in body)
                if (bodyForm.MimeType == "text/html")
                    htmlBody += HtmlConvertToPlainText(bodyForm.Value);

            if (String.IsNullOrEmpty(htmlBody))
                return null;

            var formatted = FormatText(htmlBody);
            return DivideIntoPages(formatted, MinSymbolsToBreakPage, MaxSymbolsToBreakPage);
        }

        private static string HtmlConvertToPlainText(string text)
        {
            return HtmlToText.ConvertHtml(text);
        }

        private string FormatText(string text)
        {
            ParseInnerText(ref text);
            ReplaceSymbolsWithHtmlEntities(ref text);
            AddUrlTags(ref text);
            return text;
        }

        //private string FormatHtml(string text)
        //{
        //    AddUrlTags(ref text);
        //    ParseInnerText(ref text);
        //    HtmlConvertToPlainText(ref text);
        //    ReplaceSymbolsWithHtmlEntities(ref text);
        //    return text;
        //}

        private static void AddUrlTags(ref string text)
        {
            text = text.Replace("*lt;", "<");
            text = text.Replace("*gt;", ">");
        }

        private static void ParseInnerText(ref string text)
        {
            //var htmlDoc = new HtmlDocument();
            //htmlDoc.LoadHtml(text);
            //text = htmlDoc.DocumentNode.InnerText;
        }

        /// <summary>
        /// Divides the text into pages adjusted to telegram message.
        /// Tries to finish pages by detecting new line or space char in range of <paramref name="minChunkLength"/> to <paramref name="maxChunkLength"/>
        /// if possible. Also checks if each left bracket (&lt;) has a pair (&gt;). The brackets used in this method indicate html tags.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="minChunkLength"></param>
        /// <param name="maxChunkLength"></param>
        /// <returns></returns>
        public static List<string> DivideIntoPages(string text, int minChunkLength, int maxChunkLength)
        {
            if (minChunkLength < 1)
                throw new ArgumentOutOfRangeException(nameof(maxChunkLength), "Must equals at least 1");

            int floatingLength = 0;
            int chunks = (int)Math.Ceiling(text.Length / (double)minChunkLength);
            var devided = Enumerable.Range(0, chunks).Select(i =>
            {
                //this inner method checks an integrity of last html tag (<sometag> </sometag>) in content and returns content if last tag is holistic
                //and increase floatingLength by floatingGain or by index last left bracket
                var htmlIntegrity = new Func<string, int, string>((content, floatingGain) =>
                {
                    int indexAtag;
                    var indexLeft = content.LastIndexOf('<');
                    if (indexLeft == -1)
                    {
                        floatingLength += floatingGain;
                        return content;
                    }
                    var indexRight = content.LastIndexOf('>');
                    if (indexRight > indexLeft)
                    {
                        if (content[indexLeft + 1] == '/')
                        {
                            floatingLength += floatingGain;
                            return content;
                        }
                    }

                    floatingLength += indexLeft;
                    return content.Substring(0, indexLeft);
                });
                if (floatingLength >= text.Length)
                    return null;
                if (floatingLength + maxChunkLength > text.Length)
                {
                    var subText = text.Substring(floatingLength);
                    floatingLength = text.Length;
                    return subText;
                }
                var temp = text.Substring(floatingLength, maxChunkLength);
                string subTemp;
                var newLineIndex = temp.LastIndexOf(Environment.NewLine, StringComparison.Ordinal);//try to find last line break symbolin pagebreaker range (minChunkLength, maxChunkLength)
                if (newLineIndex < minChunkLength)                                                 //if we can't
                {
                    var emptySymbolIndex = temp.LastIndexOf(' ');//then try to find last space char
                    if (emptySymbolIndex < minChunkLength) //we couldn't find it
                    {
                        return htmlIntegrity(temp, maxChunkLength);
                    }
                    subTemp = temp.Substring(0, emptySymbolIndex);
                    return htmlIntegrity(subTemp, emptySymbolIndex + 1);
                }
                subTemp = temp.Substring(0, newLineIndex);
                return htmlIntegrity(subTemp, newLineIndex + Environment.NewLine.Length);
            }).ToList();
            devided.RemoveAll(string.IsNullOrEmpty);
            return devided;
        }

        private static void ReplaceSymbolsWithHtmlEntities(ref string text)
        {
            text = text.Replace("&", "&amp;");
            text = text.Replace("<", "&lt;");
            text = text.Replace(">", "&gt;");
            //text = text.Replace("\"", "&quot;");
        }

        private string HtmlStyledMessageHeader(string senderName, string senderEmail, string subject)
        {
            if (senderName == null || senderEmail == null || subject == null)
                return string.Empty;
            return
                $"From: <b>{senderName}</b>    <i>{senderEmail}</i> \r\n<b>{subject}</b>";
        }

        private const int SymbolsPerLine = 58;
        private const int MaxLinePerPage = 25;
        private const int MinLinePerPage = 25;
        private const int MinSymbolsToBreakPage = SymbolsPerLine * MinLinePerPage;
        private const int MaxSymbolsToBreakPage = SymbolsPerLine * MaxLinePerPage;

        public string Id { get; set; }

        public string ThreadId { get; set; }

        public string Snippet { get; set; }

        public string SenderName { get; set; }

        public string SenderEmail { get; set; }

        public string Subject { get; set; }

        public string Date { get; set; }

        public string ETag { get; set; }


        public List<string> LabelIds;

        private List<BodyForm> _body;
        public List<BodyForm> Body
        {
            get
            {
                return _body;
            }
            set
            {
                _body = value;
                TextBody = FormatTextBody(value);
                HtmlBody = FormatHtmlBody(value);
            }
        }

        public List<string> TextBody { get; private set; }

        public List<string> HtmlBody { get; private set; }

        public string Header => HtmlStyledMessageHeader(SenderName, SenderEmail, Subject);

        public bool MultiPartBody => Body?.Count > 1;

        public bool HtmlConvertToPlain { get; set; } = true;

        private static IList<string> _mimeTypes = new List<string>
        {
            "text/plain",
            "text/html"
        };

        public ReadOnlyCollection<string> MimeTypes { get; } = new ReadOnlyCollection<string>(_mimeTypes);

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