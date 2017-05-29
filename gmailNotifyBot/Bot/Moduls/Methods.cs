using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CoffeeJelly.gmailNotifyBot.Bot.Exceptions;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.gmailNotifyBot.Bot.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types;
using Google.Apis.Gmail.v1.Data;
using MimeKit;
using System.Net.Mail;
using System.Threading;
using Google.Apis.Gmail.v1;
using MimeKit.Text;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls
{
    using Format = UsersResource.DraftsResource.GetRequest.FormatEnum;
    internal static class Methods
    {
        public static Service SearchServiceByUserId(string userId)
        {
            var gmailServiceFactory = ServiceFactory.Instance;
            if (gmailServiceFactory == null)
                throw new InvalidOperationException($"{nameof(ServiceFactory)} must be initialized!");

            var service = gmailServiceFactory.ServiceCollection.FirstOrDefault(s => s.From == userId);
            if (service == null)
                throw new ServiceNotFoundException($"Service with credentials from user with id={userId} is not created. User, probably, is not authorized");
            return service;
        }

        public static async Task<FormattedMessage> GetMessage(string userId, string messageId)
        {
            var service = SearchServiceByUserId(userId);
            var getRequest = service.GmailService.Users.Messages.Get("me", messageId);
            var messageResponse = await getRequest.ExecuteAsync();
            return new FormattedMessage(messageResponse);
        }

        public static async Task<FormattedMessage> ModifyMessageLabels(ModifyLabelsAction action, string userId, string messageId, string eTag = null, params string[] labels)
        {
            var labelsList = labels.ToList();
            if (action == ModifyLabelsAction.Add)
                return await ModifyMessageLabels(userId, messageId, labelsList, null, eTag);
            return await ModifyMessageLabels(userId, messageId, null, labelsList, eTag);
        }

        public static async Task<FormattedMessage> ModifyMessageLabels(string userId, string messageId, List<string> addedLabels = null, List<string> removedLabels = null, string eTag = null)
        {
            var service = SearchServiceByUserId(userId);
            var modifyMessageRequest = new ModifyMessageRequest
            {
                ETag = eTag,
                AddLabelIds = addedLabels,
                RemoveLabelIds = removedLabels
            };
            var modifyRequest = service.GmailService.Users.Messages.Modify(modifyMessageRequest, "me", messageId);
            var messageResponse = await modifyRequest.ExecuteAsync();
            var getRequest = service.GmailService.Users.Messages.Get("me", messageResponse.Id);
            messageResponse = await getRequest.ExecuteAsync();
            return new FormattedMessage(messageResponse);
        }

        public static List<UserInfo> GetUniqueContactsFromMessageList(List<FormattedMessage> messages)
        {
            var recipients = new List<UserInfo>();
            messages.ForEach(message =>
            {
                message.To?.ForEach(t => recipients.Add(t));
                message.Cc?.ForEach(t => recipients.Add(t));
                if (message.Bcc != null)
                    recipients.Add(message.Bcc);
            });
            return recipients.Unique(r => r.Email).ToList();
        }

        public static string CutArguments(TelegramBotApiWrapper.Types.InlineQuery query)
        {
            var splittedQuery = query.Query.Split(" ".ToCharArray(), 2);
            var queryArguments = splittedQuery.Length > 1 ? splittedQuery[1] : "";
            return queryArguments;
        }

        /// <summary>
        /// This method is used to define if user send 's:' parameter as argument.
        /// If true - returns the number of messages to skip, otherwise it means that <paramref name="queryArguments"/> is a search expression,
        /// and returned as is (returned skip number equals 0 for this situation).
        /// </summary>
        /// <param name="queryArguments">Some string, defined as <see cref="InlineQuery"/> cutted part (can be carried out by <see cref="CutArguments"/> method)</param>
        /// <returns></returns>
        public static int ArgumentAssigment(ref string queryArguments)
        {
            int skipMessages = 0;
            if (queryArguments.StartsWith("s:"))
            {
                skipMessages = Int32.TryParse(queryArguments.Remove(0, 2), out skipMessages) == false ? 0 : skipMessages;
                queryArguments = null;
            }
            return skipMessages;
        }

        public static async Task<byte[]> GetAttachment(Service service, string messageId, AttachmentInfo info)
        {
            var query = service.GmailService.Users.Messages.Attachments.Get("me", messageId, info.Id);
            var attachPart = await query.ExecuteAsync();
            return Base64.DecodeUrlSafeToBytes(attachPart.Data);
        }

        public static async Task WriteAttachmentToTemp(string fullname, byte[] buffer)
        {
            using (var fs = new FileStream(fullname, FileMode.Create, FileAccess.Write))
            {
                await fs.WriteAsync(buffer, 0, buffer.Length);
                await fs.FlushAsync();
            }
        }

        public static void CreateDirectory(string dirName)
        {
            var dirInfo = new DirectoryInfo(dirName);
            dirInfo.Create();
        }

        public static async Task<Message> GetDraft(string userId, string draftId, Format format = Format.Raw)
        {
            var service = SearchServiceByUserId(userId);
            var getRequest = service.GmailService.Users.Drafts.Get("me", draftId);
            getRequest.Format = format;
            var draftResponce = await getRequest.ExecuteAsync();
            return draftResponce?.Message;
        }

        public static async Task<FormattedMessage> CreateDraft(Draft body, string userId)
        {
            var service = SearchServiceByUserId(userId);
            var getRequest = service.GmailService.Users.Drafts.Create(body, "me");
            var draftResponce = await getRequest.ExecuteAsync();
            if (draftResponce == null) return null;
            return new FormattedMessage(draftResponce.Message);
        }

        //i use mimekit here :/
        public static Draft CreateNewDraftBody(List<string> to = null, string subject = null, string text = null, List<string> cc = null, string fullFileName = null, List<string> bcc = null)
        {
            //var msg = new MailMessage
            //{
            //    IsBodyHtml = true,
            //    Subject = subject,
            //    Body = text ?? ""
            //};
            //if (to != null)
            //    msg.To.Add(new MailAddress(to));
            //if (cc != null)
            //    msg.CC.Add(new MailAddress(cc));
            //if (bcc != null)
            //    msg.Bcc.Add(new MailAddress(bcc));

            //var mimeMessage = MimeMessage.CreateFromMailMessage(msg);
            var mimeMessage = new MimeMessage();
            FillMimeMessage(mimeMessage, to, subject, text, cc, bcc, fullFileName);

            var message = TransformMimeMessageToMessage(mimeMessage);
            return new Draft { Message = message };
        }

        public static Draft AddToDraftBody(Message draft, List<string> to = null, string subject = null, string text = null,
    List<string> cc = null, List<string> bcc = null, string fullFileName = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            draft.NullInspect(nameof(draft));
            if (draft.Raw == null)
                throw new InvalidOperationException($"The {nameof(draft)} must contain a non-null Raw property.");

            var decodedRaw = Base64.DecodeUrlSafeToBytes(draft.Raw);
            using (var stream = new MemoryStream(decodedRaw))
            {
                var mimeMessage = MimeMessage.Load(stream, cancellationToken);
                FillMimeMessage(mimeMessage, to, subject, text, cc, bcc, fullFileName);
                var message = TransformMimeMessageToMessage(mimeMessage);
                return new Draft { Message = message };
            }
        }

        private static Message TransformMimeMessageToMessage(MimeMessage mimeMsg)
        {
            return new Message
            {
                Raw = Base64.EncodeUrlSafe(mimeMsg)
            };
        }

        private static void FillMimeMessage(MimeMessage mimeMessage, List<string> to, string subject,
            string text, List<string> cc, List<string> bcc, string fullFileName = null)
        {
            to?.ForEach(recipient => mimeMessage.To.Add(new MailboxAddress(recipient)));
            cc?.ForEach(recipient => mimeMessage.Cc.Add(new MailboxAddress(recipient)));
            bcc?.ForEach(recipient => mimeMessage.Bcc.Add(new MailboxAddress(recipient)));
            if (subject != null)
                mimeMessage.Subject = subject;

            if (text != null || fullFileName != null)
            {
                TextPart plainPart = null;
                TextPart htmlPart = null;
                MimePart attachment = null;

                if (text != null)
                {
                    plainPart = new TextPart(TextFormat.Plain) { Text = text };
                    htmlPart = new TextPart(TextFormat.Html) { Text = text };
                }
                if (fullFileName != null)
                    attachment = new MimePart
                    {
                        ContentObject = new ContentObject(File.OpenRead(fullFileName)),
                        ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                        ContentTransferEncoding = ContentEncoding.Base64,
                        FileName = Path.GetFileName(fullFileName)
                    };

                var multipart = new Multipart("mixed");
                var mimeEntities = mimeMessage.BodyParts.ToList();
                mimeEntities.ForEach(mimeEntity =>
                {
                    multipart.Add(mimeEntity);
                });
                if (plainPart != null)
                    mimeEntities.Add(plainPart);
                if (htmlPart != null)
                    mimeEntities.Add(htmlPart);
                if (attachment != null)
                    mimeEntities.Add(attachment);

                mimeMessage.Body = multipart;
            }
        }
    }
}