using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CoffeeJelly.TelegramBotApiWrapper.Types;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.Handler.InlineQuery
{
	using Query = TelegramBotApiWrapper.Types.InlineQuery;

    public partial class InlineQueryHandler
	{
        private async Task HandleShowMessagesInlineQueryCommand(Query query, string labelId, int page = 1, string searchExpression = null)
        {
            var resultsPerPage = 50;
            var messagesInOneResponse = 10;

            int offset;
            Int32.TryParse(query.Offset, out offset);
            if (offset == -1)
                return;
            if (offset >= resultsPerPage)
            {
                page++;
                offset = offset - resultsPerPage;
            }
            var formatedMessages = await GetMessages(query, offset, labelId, page, searchExpression, resultsPerPage, messagesInOneResponse);
            if (formatedMessages.Count == 0) return;
            if (formatedMessages.Count == messagesInOneResponse)
                await _botActions.ShowShortMessageAnswerInlineQuery(query.Id, formatedMessages, offset + messagesInOneResponse);
            else
                await _botActions.ShowShortMessageAnswerInlineQuery(query.Id, formatedMessages); //last response
        }

        private async Task HandleShowContactsInlineQueryCommand(Query query, int page = 1, string searchExpression = null)
        {
            var resultsPerPage = 50;
            var messagesInOneResponse = 10;
            int offset;
            Int32.TryParse(query.Offset, out offset);
            if (offset == -1)
                return;
            if (offset >= resultsPerPage)
            {
                page++;
                offset = offset - resultsPerPage;
            }
            var formatedMessages = await GetMessages(query, offset, Types.Label.Sent, page, searchExpression, resultsPerPage, messagesInOneResponse);
            var uniqueContacts = Methods.GetUniqueContactsFromMessageList(formatedMessages);
            if (uniqueContacts.Count == messagesInOneResponse)
                await _botActions.ShowContactsAnswerInlineQuery(query.Id, uniqueContacts, offset + messagesInOneResponse);
            else
                await _botActions.ShowContactsAnswerInlineQuery(query.Id, uniqueContacts); //last response
        }



        private async Task<List<FormattedMessage>> GetMessages(ISender sender, int offset, string labelId = null, int page = 1,
		string searchExpression = null, int resultsPerPage = 50, int messagesInOneResponse = 10)
        {
            if (page < 1)
                throw new ArgumentOutOfRangeException(nameof(page), "Must be not lower then 1");

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
            string pageToken = null;
            int tempPage = page;
            while (tempPage >= 1)
            {
                query.PageToken = pageToken;
                listMessagesResponse = await query.ExecuteAsync();
                if (string.IsNullOrEmpty(listMessagesResponse.NextPageToken))
                    break;
                pageToken = listMessagesResponse.NextPageToken;
                tempPage--;
            }
            if (listMessagesResponse?.Messages == null || listMessagesResponse.Messages.Count == 0)
            {
                if (string.IsNullOrEmpty(labelId))
                    await _botActions.EmptyAllMessage(sender.From, page);
                else
                    await _botActions.EmptyLabelMessage(sender.From, labelId, page);
                return formattedMessages;
            }
            foreach (var message in listMessagesResponse.Messages.Skip(offset).Take(messagesInOneResponse))
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