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
            messagePartHeader = message.Payload.Headers.FirstOrDefault(h => h.Name == "To");
            if (!string.IsNullOrEmpty(messagePartHeader?.Value))
            {
                if (!messagePartHeader.Value.StartsWith("undisclosed-recipients"))
                {
                    var spl = messagePartHeader.Value.Split(Separator1, StringSplitOptions.RemoveEmptyEntries).ToList();
                    To = new List<Recipient>(spl.Select(r =>
                    {
                        var recipient = new Recipient
                        {
                            Email = r.GetBetweenFirst('<', '>'),
                            Name = r.Split(Separator2, StringSplitOptions.RemoveEmptyEntries).First()
                        };
                        return recipient;
                    }));
                }
            }
            messagePartHeader = message.Payload.Headers.FirstOrDefault(h => h.Name == "Bcc");
            if (messagePartHeader != null)
            {
                //var value = messagePartHeader.Value.GetBetweenFirst('<', '>');
                Bcc = new Recipient
                {sad
                    Email = messagePartHeader.Value.GetBetweenFirst('<', '>'),
                    Name = messagePartHeader.Value.Split(Separator2, StringSplitOptions.RemoveEmptyEntries).First()
                };
            }
            messagePartHeader = message.Payload.Headers.FirstOrDefault(h => h.Name == "Cc");
            if (!string.IsNullOrEmpty(messagePartHeader?.Value))
            {
                var spl = messagePartHeader.Value.Split(Separator1, StringSplitOptions.RemoveEmptyEntries).ToList();
                Cc = new List<Recipient>(spl.Select(r =>
                {
                    var recipient = new Recipient
                    {
                        Email = r.GetBetweenFirst('<', '>'),
                        Name = r.Split(Separator2, StringSplitOptions.RemoveEmptyEntries).First()
                    };
                    return recipient;
                }));
            }
            messagePartHeader = message.Payload.Headers.FirstOrDefault(h => h.Name == "Subject");
            if (messagePartHeader != null)
                Subject = messagePartHeader.Value;
            messagePartHeader = message.Payload.Headers.FirstOrDefault(h => h.Name == "Date");
            if (messagePartHeader != null)
                Date = DateTime.Parse(Regex.Replace(messagePartHeader.Value, @"\s\(\D{3}\)", string.Empty));
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

        private string HtmlStyledMessageHeader()
        {
            return
                $"<b>{SenderName}</b>    {SenderEmail}   <i>{Date}</i> \r\n\r\n<b>{Subject}</b>";
        }

        private static int SymbolsCounter(int lines)
        {
            return SymbolsPerLine * lines;
        }

        private int CalculatePages()
        {
            int? pages;
            if (IgnoreHtmlParts)
                pages = TextBody?.Count;
            else
                pages = HaveHtmlBody ? HtmlBody?.Count : TextBody?.Count;
            return pages ?? 0;
        }

        private List<string> GetDesirableBody()
        {
            if (IgnoreHtmlParts)
                return TextBody;
            return HtmlBody ?? TextBody;
        }

        private bool IsSnippetEqualsBody()
        {
            if (IgnoreHtmlParts)
            {
                if (TextBody.Count > 1) return false;
                return Equals(TextBody.First(), Snippet);
            }
            if (TextBody?.Count > 1 || HtmlBody?.Count > 1)
                return false;
            return Equals(TextBody?.First(), Snippet) || Equals(HtmlBody?.First(), Snippet);
        }

        private const int SymbolsPerLine = 58;
        private int MinSymbolsToBreakPage => SymbolsCounter(MinLinePerPage);
        private int MaxSymbolsToBreakPage => SymbolsCounter(MaxLinePerPage);
        private string _senderName;
        private string _senderEmail;
        private static readonly string[] Separator1 = { ", " };
        private static readonly string[] Separator2 = { " <" };

        public string Id { get; set; }

        public string ThreadId { get; set; }

        public string Snippet { get; set; }

        public string SenderName
        {
            get { return _senderName; }
            set
            {
                _senderName = !string.IsNullOrEmpty(value)
                    ? value
                    : "noname";
            }
        }

        public string SenderEmail
        {
            get { return _senderEmail; }
            set
            {
                _senderEmail = !string.IsNullOrEmpty(value)
                    ? value
                    : "unknown";
            }
        }

        public List<Recipient> To { get; set; }

        public List<Recipient> Cc { get; set; }

        public Recipient Bcc { get; set; }

        public string Subject { get; set; }

        public DateTime DateUtc => Date.ToUniversalTime();

        public DateTime Date { get; set; }

        public string ETag { get; set; }

        public int MaxLinePerPage { get; set; } = 35;
        public int MinLinePerPage { get; set; } = 25;

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
                TextBody = FormattedMessageHelper
                    .FormatTextBody(value, MinSymbolsToBreakPage, MaxSymbolsToBreakPage)?
                    .ToList();
                if (!IgnoreHtmlParts)
                    HtmlBody = FormattedMessageHelper
                        .FormatHtmlBody(value, MinSymbolsToBreakPage, MaxSymbolsToBreakPage)?
                        .ToList();
            }
        }

        public List<string> TextBody { get; private set; }

        public List<string> HtmlBody { get; private set; }

        public List<string> DesirableBody => GetDesirableBody();

        public string Header => HtmlStyledMessageHeader();

        public bool HaveHtmlBody => HtmlBody?.Count > 0;

        public bool MultiPageBody => HaveHtmlBody ? HtmlBody?.Count > 1 : TextBody?.Count > 1;

        public int Pages => CalculatePages();

        private bool _ignoreHtmlParts = false;

        public bool IgnoreHtmlParts
        {
            get { return _ignoreHtmlParts; }
            set
            {
                _ignoreHtmlParts = value;
                if (value)
                    HtmlBody = null;
            }

        }
        public bool HtmlConvertToPlain { get; set; } = true;

        private static IList<string> _mimeTypes = new List<string>
        {
            "text/plain",
            "text/html"
        };

        public ReadOnlyCollection<string> MimeTypes { get; } = new ReadOnlyCollection<string>(_mimeTypes);

        public bool SnippetEqualsBody => IsSnippetEqualsBody();
    }

    internal class BodyForm
    {
        public BodyForm(string mimeType, string value)
        {
            MimeType = mimeType;
            Value = value;
        }

        public string MimeType { get; }

        public string Value { get; }
    }

    internal class Recipient
    {
        public string Email { get; set; }
        public string Name { get; set; }
    }
}