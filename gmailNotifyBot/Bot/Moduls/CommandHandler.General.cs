using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase;
using CoffeeJelly.gmailNotifyBot.Bot.Exceptions;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.TelegramBotApiWrapper.Types;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using NLog;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls
{
    public sealed partial class CommandHandler
    {
        private CommandHandler(string token, UpdatesHandler updatesHandler, Secrets clientSecret, string topicName)
        {
            updatesHandler.NullInspect(nameof(updatesHandler));
            clientSecret.NullInspect(nameof(clientSecret));

            _botActions = new BotActions(token);

            _authorizer = Authorizer.GetInstance(token, updatesHandler, clientSecret);
            _updatesHandler = updatesHandler;
            ClientSecret = clientSecret;
            TopicName = topicName;
            _dbWorker = new GmailDbContextWorker();
            _updatesHandler.TelegramTextMessageEvent += _updatesHandler_TelegramTextMessageEvent;
            _updatesHandler.TelegramCallbackQueryEvent += _updatesHandler_TelegramCallbackQueryEvent;
            _updatesHandler.TelegramInlineQueryEvent += _updatesHandler_TelegramInlineQueryEvent;
            _updatesHandler.TelegramChosenInlineEvent += _updatesHandler_TelegramChosenInlineEvent;
        }

        public static CommandHandler GetInstance(string token, UpdatesHandler updatesHandler, Secrets clientSecret, string topicName)
        {
            if (Instance == null)
            {
                lock (_locker)
                {
                    if (Instance == null)
                        Instance = new CommandHandler(token, updatesHandler, clientSecret, topicName);
                }
            }
            return Instance;
        }

        private async Task<List<FormattedMessage>> GetMessages(ISender sender, int offset, string labelId = null, int page = 1,
                                                    string searchExpression = null, int resultsPerPage = 50, int messagesInOneResponse = 10)
        {
            if (page < 1)
                throw new ArgumentOutOfRangeException(nameof(page), "Must be not lower then 1");

            var formatedMessages = new List<FormattedMessage>();
            var service = SearchServiceByUserId(sender.From);
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
                return formatedMessages;
            }
            foreach (var message in listMessagesResponse.Messages.Skip(offset).Take(messagesInOneResponse))
            {
                var getMailRequest = service.GmailService.Users.Messages.Get("me", message.Id);
                getMailRequest.Format = UsersResource.MessagesResource.GetRequest.FormatEnum.Metadata;
                var mailInfoResponse = await getMailRequest.ExecuteAsync();
                if (mailInfoResponse == null) continue;
                var fMessage = new FormattedMessage(mailInfoResponse);
                formatedMessages.Add(fMessage);
            }
            return formatedMessages;
        }

        private Service SearchServiceByUserId(string userId)
        {
            var gmailServiceFactory = ServiceFactory.GetInstanse(ClientSecret);
            var service = gmailServiceFactory.ServiceCollection.FirstOrDefault(s => s.UserCredential.UserId == userId);
            if (service == null)
                throw new ServiceNotFoundException($"Service with credentials from user with id={userId} is not created. User, probably, is not authorized");
            return service;
        }


        private async Task<FormattedMessage> GetMessage(string userId, string messageId)
        {
            var service = SearchServiceByUserId(userId);
            var query = service.GmailService.Users.Messages.Get("me", messageId);
            var messageResponse = await query.ExecuteAsync();
            return new FormattedMessage(messageResponse);
        }

        private async Task<FormattedMessage> ModifyMessageLabels(ModifyLabelsAction action, string userId, string messageId, string eTag = null, params string[] labels)
        {
            var labelsList = labels.ToList();
            if (action == ModifyLabelsAction.Add)
                return await ModifyMessageLabels(userId, messageId, labelsList, null, eTag);
            return await ModifyMessageLabels(userId, messageId, null, labelsList, eTag);
        }

        private async Task<FormattedMessage> ModifyMessageLabels(string userId, string messageId, List<string> addedLabels = null, List<string> removedLabels = null, string eTag = null)
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
        private List<Recipient> GetUniqueContactsFromMessageList(List<FormattedMessage> messages)
        {
            var recipients = new List<Recipient>();
            messages.ForEach(message =>
            {
                message.To.ForEach(t => recipients.Add(t));
                message.Cc.ForEach(t => recipients.Add(t));
                recipients.Add(message.Bcc);
            });
            return recipients.Unique(r => r.Email).ToList();
        }


        private UpdatesHandler _updatesHandler;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static readonly object _locker = new object();
        private BotActions _botActions;
        private Authorizer _authorizer;
        private GmailDbContextWorker _dbWorker;

        public static CommandHandler Instance { get; private set; }

        public Secrets ClientSecret { get; set; }

        public string TopicName { get; set; }

    }

    public enum ModifyLabelsAction
    {
        Add,
        Remove
    }
}