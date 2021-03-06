﻿using System;
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
using System.Text.RegularExpressions;
using System.Threading;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels;
using Google.Apis.Gmail.v1;
using MimeKit.Text;
using GmailLabel = Google.Apis.Gmail.v1.Data.Label;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls
{
    using DraftFormat = UsersResource.DraftsResource.GetRequest.FormatEnum;
    using MessageFormat = UsersResource.MessagesResource.GetRequest.FormatEnum;
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

        //public static async Task<FormattedMessage> GetMessage(string userId, string messageId)
        //{
        //    var service = SearchServiceByUserId(userId);
        //    return await GetMessage(service, messageId);
        //}

        public static async Task<FormattedMessage> GetMessage(Service service, string messageId)
        {
            var getRequest = service.GmailService.Users.Messages.Get("me", messageId);
            getRequest.Format = service.FullUserAccess
                ? MessageFormat.Full
                : MessageFormat.Metadata;
            var messageResponse = await getRequest.ExecuteAsync();
            return new FormattedMessage(messageResponse, service.FullUserAccess);
        }

        public static async Task<FormattedMessage> ModifyMessageLabelsAsync(ModifyLabelsAction action, string userId, string messageId, string eTag = null, params string[] labels)
        {
            var labelsList = labels.ToList();
            if (action == ModifyLabelsAction.Add)
                return await ModifyMessageLabelsAsync(userId, messageId, labelsList, null, eTag);
            return await ModifyMessageLabelsAsync(userId, messageId, null, labelsList, eTag);
        }

        public static async Task<FormattedMessage> ModifyMessageLabelsAsync(string userId, string messageId, List<string> addedLabels = null, List<string> removedLabels = null, string eTag = null)
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

        public static FormattedMessage ModifyMessageLabels(string userId, string messageId, List<string> addedLabels = null, List<string> removedLabels = null, string eTag = null)
        {
            var service = SearchServiceByUserId(userId);
            var modifyMessageRequest = new ModifyMessageRequest
            {
                ETag = eTag,
                AddLabelIds = addedLabels,
                RemoveLabelIds = removedLabels
            };
            var modifyRequest = service.GmailService.Users.Messages.Modify(modifyMessageRequest, "me", messageId);
            var messageResponse = modifyRequest.Execute();
            var getRequest = service.GmailService.Users.Messages.Get("me", messageResponse.Id);
            messageResponse = getRequest.Execute();
            return new FormattedMessage(messageResponse);
        }

        public static FormattedMessage ModifyMessageLabels(Service service, string messageId, List<string> addedLabels = null, List<string> removedLabels = null, string eTag = null)
        {
            var modifyMessageRequest = new ModifyMessageRequest
            {
                ETag = eTag,
                AddLabelIds = addedLabels,
                RemoveLabelIds = removedLabels
            };
            var modifyRequest = service.GmailService.Users.Messages.Modify(modifyMessageRequest, "me", messageId);
            var messageResponse = modifyRequest.Execute();
            var getRequest = service.GmailService.Users.Messages.Get("me", messageResponse.Id);
            messageResponse = getRequest.Execute();
            return new FormattedMessage(messageResponse);
        }

        public static List<UserInfo> GetUniqueContactsFromMessageList(List<FormattedMessage> messages, bool includeCc = false, bool includeBcc = false)
        {
            var recipients = new List<UserInfo>();
            messages.ForEach(message =>
            {
                message.To?.ForEach(t => recipients.Add(t));
                if (includeCc)
                    message.Cc?.ForEach(t => recipients.Add(t));
                if (includeBcc)
                    message.Bcc?.ForEach(t => recipients.Add(t));
            });
            return recipients.Unique(r => r.Email).ToList();
        }

        public static string CutArguments(string query)
        {
            var splittedQuery = query.Split(" ".ToCharArray(), 2);
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

        public static async Task<byte[]> GetAttachment(Service service, string messageId, string attachId)
        {
            var query = service.GmailService.Users.Messages.Attachments.Get("me", messageId, attachId);
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

        public static async Task<Draft> GetDraft(string userId, string draftId, DraftFormat format = DraftFormat.Raw)
        {
            var service = SearchServiceByUserId(userId);
            var getRequest = service.GmailService.Users.Drafts.Get("me", draftId);
            getRequest.Format = format;
            var draftResponce = await getRequest.ExecuteAsync();
            return draftResponce;
        }

        public static async Task<Draft> CreateDraft(Draft body, string userId)
        {
            var service = SearchServiceByUserId(userId);
            var getRequest = service.GmailService.Users.Drafts.Create(body, "me");
            var draftResponce = await getRequest.ExecuteAsync();
            return draftResponce;
        }

        public static async Task<Draft> UpdateDraft(Draft body, string userId, string draftId)
        {
            var service = SearchServiceByUserId(userId);
            var getRequest = service.GmailService.Users.Drafts.Update(body, "me", draftId);
            var draftResponce = await getRequest.ExecuteAsync();
            return draftResponce;
        }

        //i use mimekit here :/
        public static Draft CreateNewDraftBody(string subject = null, string text = null, List<IUserInfo> to = null, List<IUserInfo> cc = null,
                                                List<IUserInfo> bcc = null, List<IUserInfo> from = null, List<FileStream> fileStream = null)
        {
            var mimeMessage = new MimeMessage();
            FillMimeMessage(mimeMessage, subject, text, to, cc, bcc, from, fileStream);
            var message = TransformMimeMessageToMessage(mimeMessage);
            return new Draft { Message = message };
        }

        public static Message CreateNewMessageBody(string subject, string text, List<IUserInfo> to)
        {
            var mimeMessage = new MimeMessage();
            FillMimeMessage(mimeMessage, subject, text, to);
            return TransformMimeMessageToMessage(mimeMessage);
        }

        public static Draft AddToDraftBody(Draft draft, string subject = null, string text = null, List<IUserInfo> to = null,
                                List<IUserInfo> cc = null, List<IUserInfo> bcc = null, List<IUserInfo> from = null,
                                List<FileStream> fileStream = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            draft.NullInspect(nameof(draft));
            if (draft.Message?.Raw == null)
                throw new InvalidOperationException($"The {nameof(Draft.Message)} must contain a non-null Raw property.");

            var decodedRaw = Base64.DecodeUrlSafeToBytes(draft.Message.Raw);
            using (var stream = new MemoryStream(decodedRaw))
            {
                var mimeMessage = MimeMessage.Load(stream, cancellationToken);
                FillMimeMessage(mimeMessage, subject, text, to, cc, bcc, from, fileStream);
                var message = TransformMimeMessageToMessage(mimeMessage);
                return new Draft { Message = message };
            }
        }

        public static bool EmailAddressValidation(string email)
        {
            return Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
                RegexOptions.IgnoreCase);
        }

        public static void ComposeNmStateModel(NmStoreModel model, FormattedMessage formattedMessage)
        {
            model.NullInspect(nameof(model));
            formattedMessage.NullInspect(nameof(formattedMessage));

            formattedMessage.To?.ForEach(to => { model.To.Add(new ToModel { Email = to.Email, Name = to.Name }); });
            formattedMessage.Cc?.ForEach(cc => { model.Cc.Add(new CcModel { Email = cc.Email, Name = cc.Name }); });
            formattedMessage.Bcc?.ForEach(bcc => { model.Bcc.Add(new BccModel { Email = bcc.Email, Name = bcc.Name }); });
            if (formattedMessage.HasAttachments)
                foreach (var attach in formattedMessage.Attachments)
                {
                    model.File.Add(new FileModel
                    {
                        AttachId = attach.Id,
                        OriginalName = attach.FileName
                    });
                }
            model.Subject = formattedMessage.Subject;
            if (formattedMessage.Body != null)
                model.Message = GetTextPlainMessage(formattedMessage.Body);
        }

        public static async Task<GmailLabel> CreateLabelAsync(string labelName, Service service)
        {
            var label = new GmailLabel { Name = labelName };
            var request = service.GmailService.Users.Labels.Create(label, "me");
            return await request.ExecuteAsync();
        }

        public static async Task<GmailLabel> GetLabelByNameAsync(string labelName, Service service)
        {
            var requestList = service.GmailService.Users.Labels.List("me");
            var labelsListResponse = await requestList.ExecuteAsync();
            foreach (var label in labelsListResponse.Labels)
            {
                if (label.Name == labelName)
                    return label;
            }
            return null;
        }

        public static async Task<GmailLabel> GetLabelAsync(string labelId, Service service)
        {
            var requestList = service.GmailService.Users.Labels.Get("me", labelId);
            return await requestList.ExecuteAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <param name="service"></param>
        /// <param name="labelId">The id of the label. Use if <paramref name="label"/> is new instance of <see cref="GmailLabel"/> or to change an another label.</param>
        /// <returns></returns>
        public static async Task<GmailLabel> EditLabelAsync(GmailLabel label, Service service, string labelId = null)
        {
            var id = labelId ?? label.Id;
            var requestList = service.GmailService.Users.Labels.Update(label, "me", id);
            return await requestList.ExecuteAsync();
        }

        public static async Task<IList<GmailLabel>> GetLabelsList(Service service)
        {
            var requestList = service.GmailService.Users.Labels.List("me");
            var labels = await requestList.ExecuteAsync();
            return labels?.Labels;
        }

        public static async Task<string> DeleteLabelAsync(string labelId, Service service)
        {
            var requestList = service.GmailService.Users.Labels.Delete("me", labelId);
            return await requestList.ExecuteAsync();
        }

        private static string GetTextPlainMessage(IEnumerable<BodyForm> collection)
        {
            string message = "";
            foreach (var body in collection)
            {
                if (body.MimeType == "text/plain")
                    message += body.Value;
            }
            return message;
        }

        private static Message TransformMimeMessageToMessage(MimeMessage mimeMsg)
        {
            return new Message
            {
                Raw = Base64.EncodeUrlSafe(mimeMsg)
            };
        }

        private static void FillMimeMessage(MimeMessage mimeMessage, string subject = null,
            string text = null, List<IUserInfo> to = null, List<IUserInfo> cc = null, List<IUserInfo> bcc = null, List<IUserInfo> from = null, List<FileStream> contentList = null)
        {
            to?.ForEach(recipient => mimeMessage.To.Add(new MailboxAddress(recipient.Name, recipient.Email)));
            cc?.ForEach(recipient => mimeMessage.Cc.Add(new MailboxAddress(recipient.Name, recipient.Email)));
            bcc?.ForEach(recipient => mimeMessage.Bcc.Add(new MailboxAddress(recipient.Name, recipient.Email)));
            from?.ForEach(sender => mimeMessage.From.Add(new MailboxAddress(sender.Name, sender.Email)));
            if (subject != null)
                mimeMessage.Subject = subject;

            if (text != null || contentList != null)
            {
                TextPart plainPart = null;
                List<MimePart> attachments = null;

                if (text != null)
                    plainPart = new TextPart(TextFormat.Plain) { Text = text };
                if (contentList != null)
                {
                    attachments = new List<MimePart>();
                    contentList.ForEach(content =>
                    {
                        attachments.Add(new MimePart
                        {
                            ContentObject = new ContentObject(content),
                            ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                            ContentTransferEncoding = ContentEncoding.Base64,
                            FileName = Path.GetFileName(content.Name)
                        });
                    });
                }
                var multipart = new Multipart("mixed");
                if (plainPart != null)
                    multipart.Add(plainPart);

                attachments?.ForEach(attach => { multipart.Add(attach); });
                mimeMessage.Body = multipart;
            }
        }
    }
}