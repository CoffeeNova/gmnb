using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeJelly.gmailNotifyBot.Bot.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.InlineQueryUpdates
{
    public partial class InlineQueryHandler
    {
        public async Task HandleShowMessagesInlineQueryCommand(InlineQuery query, string labelId = null, int skipMessages = 0, string searchExpression = null)
        {
            int offset;
            Int32.TryParse(query.Offset, out offset);
            if (offset == -1)
                return;
            if (offset == 0)
                offset += skipMessages;
            var messagesInOneResponse = 10;
            var resultsPerPage = offset + messagesInOneResponse;
            if (resultsPerPage > 500)
                resultsPerPage = 500;

            var formatedMessages = await GetMessages(query, offset, labelId, searchExpression, resultsPerPage, messagesInOneResponse);
            if (formatedMessages.Count == 0)
                await _botActions.ShowShortEmptyAnswerInlineQuery(query.Id);
            else if (formatedMessages.Count == messagesInOneResponse)
                await _botActions.ShowShortMessageAnswerInlineQuery(query.Id, formatedMessages, offset + messagesInOneResponse);
            else
                await _botActions.ShowShortMessageAnswerInlineQuery(query.Id, formatedMessages); //last response
        }

        //label should be "SEND" to get user's contacts
        public async Task HandleShowContactsInlineQueryCommand(InlineQuery query, string labelId = null, string userContact = null)
        {
            var nmModel = await _dbWorker.FindNmStoreAsync(query.From);
            if (nmModel == null)
                return;

            if (!string.IsNullOrEmpty(userContact))
            {
                var userInfo = new UserInfo { Email = userContact };
                await _botActions.ShowContactsAnswerInlineQuery(query.Id, new List<UserInfo> { userInfo });
                return;
            }
            int offset;
            Int32.TryParse(query.Offset, out offset);
            if (offset == -1)
                return;
            var messagesInOneResponse = 10;
            var resultsPerPage = offset + messagesInOneResponse;
            if (resultsPerPage > 500)
                resultsPerPage = 500;

            var formatedMessages = await GetMessages(query, offset, labelId, null, resultsPerPage, messagesInOneResponse);
            var uniqueContacts = Methods.GetUniqueContactsFromMessageList(formatedMessages);
            if (uniqueContacts.Count == messagesInOneResponse)
                await _botActions.ShowContactsAnswerInlineQuery(query.Id, uniqueContacts, offset + messagesInOneResponse);
            else
                await _botActions.ShowContactsAnswerInlineQuery(query.Id, uniqueContacts); //last response
        }

        public async Task HandleShowEditDraftsInlineQueryCommand(InlineQuery query, int skipDrafts = 0, string searchExpression = null)
        {
            int offset;
            Int32.TryParse(query.Offset, out offset);
            if (offset == -1)
                return;
            if (offset == 0)
                offset += skipDrafts;
            var messagesInOneResponse = 10;
            var resultsPerPage = offset + messagesInOneResponse;
            if (resultsPerPage > 500)
                resultsPerPage = 500;

            var formatedMessages = await GetDrafts(query, offset, searchExpression, resultsPerPage, messagesInOneResponse);
            if (formatedMessages.Count == 0)
                await _botActions.ShowShortEmptyAnswerInlineQuery(query.Id);
            else if (formatedMessages.Count == messagesInOneResponse)
                await _botActions.ShowShortMessageAnswerInlineQuery(query.Id, formatedMessages, offset + messagesInOneResponse);
            else
                await _botActions.ShowShortMessageAnswerInlineQuery(query.Id, formatedMessages); //last response
        }

        private async Task<List<FormattedMessage>> GetDrafts(ISender sender, int offset = 0,
            string searchExpression = null, int resultsPerPage = 50, int draftsInOneResponse = 10)
        {
            if (resultsPerPage < 1 || resultsPerPage > 500)
                throw new ArgumentOutOfRangeException(nameof(resultsPerPage), "must be from 1 to 500.");
            if (draftsInOneResponse < 1 || draftsInOneResponse > 50)
                throw new ArgumentOutOfRangeException(nameof(draftsInOneResponse), "must be from 1 to 50.");

            var formattedMessages = new List<FormattedMessage>();
            var service = Methods.SearchServiceByUserId(sender.From);
            var query = service.GmailService.Users.Drafts.List("me");
            query.MaxResults = resultsPerPage;
            query.Q = searchExpression;

            ListDraftsResponse listDraftsResponse = null;
            var totalDrafts = new List<Draft>();
            string pageToken = null;
            var tempOffset = offset;
            while (tempOffset >= 0)
            {
                query.PageToken = pageToken;
                listDraftsResponse = await query.ExecuteAsync();
                if (listDraftsResponse.Drafts != null)
                    totalDrafts.AddRange(listDraftsResponse.Drafts);
                if (string.IsNullOrEmpty(listDraftsResponse.NextPageToken))
                    break;
                pageToken = listDraftsResponse.NextPageToken;
                tempOffset -= resultsPerPage;
            }

            if (listDraftsResponse?.Drafts == null)
                return formattedMessages;

            foreach (var draft in totalDrafts.Skip(offset).Take(draftsInOneResponse))
            {
                var getDraftRequest = service.GmailService.Users.Drafts.Get("me", draft.Id);
                getDraftRequest.Format = UsersResource.DraftsResource.GetRequest.FormatEnum.Metadata;
                var draftInfoResponse = await getDraftRequest.ExecuteAsync();
                if (draftInfoResponse == null) continue;
                var fMessage = new FormattedMessage(draftInfoResponse);
                formattedMessages.Add(fMessage);
            }
            return formattedMessages;
        }

        private async Task<List<FormattedMessage>> GetMessages(ISender sender, int offset = 0, string labelId = null,
        string searchExpression = null, int resultsPerPage = 50, int messagesInOneResponse = 10)
        {
            if (resultsPerPage < 1 || resultsPerPage > 500)
                throw new ArgumentOutOfRangeException(nameof(resultsPerPage), "must be from 1 to 500.");
            if (messagesInOneResponse < 1 || messagesInOneResponse > 50)
                throw new ArgumentOutOfRangeException(nameof(messagesInOneResponse), "must be from 1 to 50.");

            var formattedMessages = new List<FormattedMessage>();
            var service = Methods.SearchServiceByUserId(sender.From);
            var query = service.GmailService.Users.Messages.List("me");
            if (string.IsNullOrEmpty(labelId))
                query.IncludeSpamTrash = false;
            else
                query.LabelIds = labelId;

            query.MaxResults = resultsPerPage;
            query.Q = searchExpression;

            ListMessagesResponse listMessagesResponse = null;
            var totalMessages = new List<Message>();
            string pageToken = null;

            var tempOffset = offset;
            while (tempOffset >= 0)
            {
                query.PageToken = pageToken;
                listMessagesResponse = await query.ExecuteAsync();
                if (listMessagesResponse.Messages != null)
                    totalMessages.AddRange(listMessagesResponse.Messages);
                if (string.IsNullOrEmpty(listMessagesResponse.NextPageToken))
                    break;
                pageToken = listMessagesResponse.NextPageToken;
                tempOffset -= resultsPerPage;
            }

            if (listMessagesResponse?.Messages == null)
                return formattedMessages;

            foreach (var message in totalMessages.Skip(offset).Take(messagesInOneResponse))
            {
                var getMailRequest = service.GmailService.Users.Messages.Get("me", message.Id);
                getMailRequest.Format = UsersResource.MessagesResource.GetRequest.FormatEnum.Metadata;
                var mailInfoResponse = await getMailRequest.ExecuteAsync();
                if (mailInfoResponse == null) continue;
                var fMessage = new FormattedMessage(mailInfoResponse);
                formattedMessages.Add(fMessage);
            }
            return formattedMessages;
        }
    }
}