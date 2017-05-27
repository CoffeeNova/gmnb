using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeJelly.gmailNotifyBot.Bot.Exceptions;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.gmailNotifyBot.Bot.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types;
using Google.Apis.Gmail.v1.Data;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls
{
    internal  static class Methods
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

        public static async Task<FormattedMessage> GetDraft(string userId, string draftId)
        {
            var service = SearchServiceByUserId(userId);
            var getRequest = service.GmailService.Users.Drafts.Get("me", draftId);
            var draftResponce = await getRequest.ExecuteAsync();
            if (draftResponce == null) return null;
            return new FormattedMessage(draftResponce.Message);
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
    }
}