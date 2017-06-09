using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using CoffeeJelly.gmailNotifyBot.Bot.Exceptions;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.gmailNotifyBot.Bot.Types;
using Google.Apis.Gmail.v1.Data;

[assembly: InternalsVisibleTo("gmailNotifyBotTests")]
namespace CoffeeJelly.gmailNotifyBot.Bot
{
    using Helper = FormattedMessageHelper;
    internal sealed class FormattedMessage
    {
        public FormattedMessage()
        {

        }

        public FormattedMessage(Draft draft)
        {
            draft.NullInspect(nameof(draft));
            DraftId = draft.Id;
            ParseMessage(draft.Message);
        }

        public FormattedMessage(Message message)
        {
            ParseMessage(message);
        }

        private void ParseMessage(Message message)
        {
            message.NullInspect(nameof(message));
            if (message.Payload == null)
                throw new FormattedGmailMessageException($"{nameof(message.Payload)} must be not null.");

            Id = message.Id;
            ThreadId = message.ThreadId;
            Snippet = Helper.FormatTextToHtmlParseMode(message.Snippet);
            ETag = message.ETag;
            LabelIds = message.LabelIds == null ? null : new List<string>(message.LabelIds);
            var messagePartHeader = message.Payload.Headers.FirstOrDefault(h => h.Name == "From");
            if (!string.IsNullOrEmpty(messagePartHeader?.Value))
                From = Helper.ParseUserInfo(messagePartHeader.Value);

            messagePartHeader = message.Payload.Headers.FirstOrDefault(h => h.Name == "To");
            if (!string.IsNullOrEmpty(messagePartHeader?.Value))
            {
                if (!messagePartHeader.Value.StartsWith("undisclosed-recipients"))
                {
                    var userInfoList = messagePartHeader.Value.Split(Separator, StringSplitOptions.RemoveEmptyEntries).ToList();
                    To = new List<UserInfo>(userInfoList.Select(Helper.ParseUserInfo));
                }
            }
            messagePartHeader = message.Payload.Headers.FirstOrDefault(h => h.Name == "Bcc");
            if (!string.IsNullOrEmpty(messagePartHeader?.Value))
            {
                var userInfoList = messagePartHeader.Value.Split(Separator, StringSplitOptions.RemoveEmptyEntries).ToList();
                Bcc = new List<UserInfo>(userInfoList.Select(Helper.ParseUserInfo));
            }

            messagePartHeader = message.Payload.Headers.FirstOrDefault(h => h.Name == "Cc");
            if (!string.IsNullOrEmpty(messagePartHeader?.Value))
            {
                var userInfoList = messagePartHeader.Value.Split(Separator, StringSplitOptions.RemoveEmptyEntries).ToList();
                Cc = new List<UserInfo>(userInfoList.Select(Helper.ParseUserInfo));
            }
            messagePartHeader = message.Payload.Headers.FirstOrDefault(h => h.Name == "Subject");
            if (messagePartHeader != null)
                Subject = messagePartHeader.Value;
            messagePartHeader = message.Payload.Headers.FirstOrDefault(h => h.Name == "Date");
            if (messagePartHeader != null)
                Date = DateTime.Parse(Regex.Replace(messagePartHeader.Value, @"\s\(\D{3}\)", string.Empty));

            var body = new List<BodyForm>();
            if (message.Payload.Parts != null)
            {
                DecodeDevidedBody(message.Payload.Parts, body);
                Attachments = GetAttachments(message.Payload.Parts);
            }
            else if (message.Payload.Body?.Data != null)
                body.Add(new BodyForm(message.Payload.MimeType, Base64.DecodeUrlSafe(message.Payload.Body.Data)));

            Body = body;
        }

        private void DecodeDevidedBody(IList<MessagePart> parts, List<BodyForm> decodedBody)
        {
            parts.NullInspect(nameof(parts));

            foreach (var part in parts)
            {
                if (part.Parts != null)
                    DecodeDevidedBody(part.Parts, decodedBody);
                else if (part.Body?.Data != null)
                    decodedBody.Add(new BodyForm(part.MimeType, Base64.DecodeUrlSafe(part.Body.Data)));
            }
        }

        private IReadOnlyList<AttachmentInfo> GetAttachments(IList<MessagePart> parts)
        {
            var ids = new List<AttachmentInfo>();
            foreach (var part in parts)
            {
                if (!string.IsNullOrEmpty(part.Filename))
                    ids.Add(new AttachmentInfo
                    {
                        Id = part.Body.AttachmentId,
                        FileName = part.Filename,
                        MimeType = part.MimeType
                    });
            }
            return ids;
        }

        private string HtmlStyledMessageHeader()
        {
            var date = Date.Date == DateTime.Now.Date ? Date.ToString("HH:mm") : Date.ToString("dd.MM.yy");
                
            var header = string.IsNullOrEmpty(From.Name) 
                ? $"*lt;b*gt;{From.Email}*lt;/b*gt;    *lt;i*gt;{date}*lt;/i*gt; {Environment.NewLine}{Environment.NewLine}*lt;b*gt{Subject}*lt;/b*gt;" 
                : $"*lt;b*gt;{From.Name}*lt;/b*gt;    '{From.Email}'  *lt;i*gt;{date}*lt;/i*gt; {Environment.NewLine}{Environment.NewLine}*lt;b*gt;{Subject}*lt;/b*gt;";
            return Helper.FormatTextToHtmlParseMode(header);
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

        private IReadOnlyList<string> GetDesirableBody()
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
        private static readonly string[] Separator = { ", " };

        public string Id { get; set; }

        public string ThreadId { get; set; }

        public string Snippet { get; set; }

        public UserInfo From { get; set; }

        public List<UserInfo> To { get; set; }

        public List<UserInfo> Cc { get; set; }

        public List<UserInfo> Bcc { get; set; }

        public string Subject { get; set; }

        public DateTime DateUtc => Date.ToUniversalTime();

        public DateTime Date { get; set; }

        public string ETag { get; set; }

        public int MaxLinePerPage { get; set; } = 35;
        public int MinLinePerPage { get; set; } = 25;

        public List<string> LabelIds;

        private IReadOnlyList<BodyForm> _body;
        public IReadOnlyList<BodyForm> Body 
        {
            get
            {
                return _body;
            }
            set
            {
                _body = value;
                TextBody = Helper
                    .FormatTextBody(value, MinSymbolsToBreakPage, MaxSymbolsToBreakPage)?
                    .ToList();
                if (!IgnoreHtmlParts)
                    HtmlBody = Helper
                        .FormatHtmlBody(value, MinSymbolsToBreakPage, MaxSymbolsToBreakPage)?.ToList();

            }
        }

        public IReadOnlyList<string> TextBody { get; private set; }

        public IReadOnlyList<string> HtmlBody { get; private set; }

        public IReadOnlyList<string> DesirableBody => GetDesirableBody();

        public IReadOnlyList<AttachmentInfo> Attachments { get; private set; }

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

        public bool HasAttachments => Attachments?.Count > 0;

        private static IList<string> _mimeTypes = new List<string>
        {
            "text/plain",
            "text/html"
        };

        public ReadOnlyCollection<string> MimeTypes { get; } = new ReadOnlyCollection<string>(_mimeTypes);

        public bool SnippetEqualsBody => IsSnippetEqualsBody();

        public string DraftId { get; }

        public bool IsDraft => !string.IsNullOrEmpty(DraftId);
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
}