using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeJelly.TelegramBotApiWrapper.Types;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.InlineQueryUpdates
{
    using Query = TelegramBotApiWrapper.Types.InlineQuery;

    public partial class InlineQueryHandler
    {
        public async Task HandleShowMessagesInlineQueryCommand(Query query, string labelId = null, int skipMessages = 0, string searchExpression = null)
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
            if (formatedMessages.Count == 0) return;
            if (formatedMessages.Count == messagesInOneResponse)
                await _botActions.ShowShortMessageAnswerInlineQuery(query.Id, formatedMessages, offset + messagesInOneResponse);
            else
                await _botActions.ShowShortMessageAnswerInlineQuery(query.Id, formatedMessages); //last response
        }

        //label should be "SEND" to get user's contacts
        public async Task HandleShowContactsInlineQueryCommand(Query query, string labelId = null)
        {
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
                await _botActions.ShowContactsAnswerInlineQuery(query.Id,  uniqueContacts, offset + messagesInOneResponse);
            else
                await _botActions.ShowContactsAnswerInlineQuery(query.Id, uniqueContacts); //last response
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
            var totalMessages = new List<Google.Apis.Gmail.v1.Data.Message>();
            string pageToken = null;
            var tempOffset = offset;
            while (tempOffset >= 0)
            {
                query.PageToken = pageToken;
                listMessagesResponse = await query.ExecuteAsync();
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