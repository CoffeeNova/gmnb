using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.Exceptions;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.TelegramBotApiWrapper.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types.Messages;
using Google.Apis.Gmail.v1.Data;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.Handler.Message
{
    public partial class MessageHandler
    {
        public async Task HandleTestMessageCommand(ISender sender)
        {
            sender.NullInspect(nameof(sender));

            string messageId = "";
            if (sender.GetType() == typeof(TextMessage))
            {
                // ReSharper disable once PossibleNullReferenceException
                var splittedtext = (sender as TextMessage).Text.Split(' ');
                messageId = splittedtext.Length > 1 ? splittedtext[1] : "";
            }

            var service = Methods.SearchServiceByUserId(sender.From);
            var query = service.GmailService.Users.Messages.List("me");
            //query.LabelIds = "INBOX";
            var listMessagesResponse = await query.ExecuteAsync();
            if (listMessagesResponse?.Messages == null) return;

            var message = listMessagesResponse.Messages.First();
            if (!string.IsNullOrEmpty(messageId))
                message = listMessagesResponse.Messages.SingleOrDefault(m => m.Id == messageId);
            if (message == null)
                return;

            var getMailRequest = service.GmailService.Users.Messages.Get("me", message.Id);
            var mailInfoResponse = await getMailRequest.ExecuteAsync();
            if (mailInfoResponse == null) return;
            var formattedMessage = new FormattedMessage(mailInfoResponse);
            await _botActions.ShowShortMessageAsync(sender.From, formattedMessage);
        }

        public async Task HandleTestNameCommand(ISender sender)
        {
            var service = Methods.SearchServiceByUserId(sender.From);
            var query = service.Oauth2Service.Userinfo.Get();
            var userinfo = await query.ExecuteAsync();
            await _botActions.EmailAddressMessage(sender.From, userinfo.Name);
        }

        public async Task HandleTestThreadCommand(ISender sender)
        {
            var service = Methods.SearchServiceByUserId(sender.From);
            var query = service.GmailService.Users.Threads.List("me");
            query.LabelIds = "INBOX";
            var listThreadsResponse = await query.ExecuteAsync();
            if (listThreadsResponse?.Threads == null) return;

            var thread = listThreadsResponse.Threads.First();
            var getMailRequest = service.GmailService.Users.Messages.Get("me", thread.Id);
            var mailInfoResponse = await getMailRequest.ExecuteAsync();
            if (mailInfoResponse == null) return;
            var formattedMessage = new FormattedMessage(mailInfoResponse);
            await _botActions.ShowShortMessageAsync(sender.From, formattedMessage);
        }

        public async Task HandleStartNotifyCommand(ISender sender)
        {
            var userSettings = await _dbWorker.FindUserSettingsAsync(sender.From);
            if (userSettings == null)
                throw new DbDataStoreException(
                    $"Can't find user settings data in database. User record with id {sender.From} is absent in the database.");
            userSettings.MailNotification = true;
            await _dbWorker.UpdateUserSettingsRecordAsync(userSettings);
            await HandleStartWatchCommandAsync(sender);
            //message to chat about start notification
        }

        public async Task HandleStopNotifyCommand(ISender sender)
        {
            var userSettings = await _dbWorker.FindUserSettingsAsync(sender.From);
            if (userSettings == null)
                throw new DbDataStoreException(
                    $"Can't find user settings data in database. User record with id {sender.From} is absent in the database.");
            await _dbWorker.UpdateUserSettingsRecordAsync(userSettings);
            await HandleStopWatchCommandAsync(sender);
            userSettings.MailNotification = false;
            //message to chat about stop notification
        }

        public async Task HandleStartWatchCommandAsync(ISender sender)
        {
            var service = Methods.SearchServiceByUserId(sender.From);
            var userSettings = await _dbWorker.FindUserSettingsAsync(sender.From);
            if (userSettings == null)
                throw new DbDataStoreException(
                    $"Can't find user settings data in database. User record with id {sender.From} is absent in the database.");
            if (!userSettings.MailNotification) return;

            var watchRequest = new WatchRequest
            {
                LabelIds = new List<string> { "INBOX" },
                TopicName = BotInitializer.Instance.BotSettings.Topic
            };
            var query = service.GmailService.Users.Watch(watchRequest, "me");
            var watchResponse = await query.ExecuteAsync();
            if (watchResponse.Expiration != null)
                userSettings.Expiration = watchResponse.Expiration.Value;
            if (watchResponse.HistoryId != null)
                userSettings.HistoryId = Convert.ToInt64(watchResponse.HistoryId.Value);

            await _dbWorker.UpdateUserSettingsRecordAsync(userSettings);
        }

        public void HandleStartWatchCommand(ISender sender)
        {
            var service = Methods.SearchServiceByUserId(sender.From);
            var userSettings = _dbWorker.FindUserSettings(sender.From);
            if (userSettings == null)
                throw new DbDataStoreException(
                    $"Can't find user settings data in database. User record with id {sender.From} is absent in the database.");
            if (!userSettings.MailNotification) return;

            var watchRequest = new WatchRequest
            {
                LabelIds = new List<string> { "INBOX" },
                TopicName = BotInitializer.Instance.BotSettings.Topic
            };
            var query = service.GmailService.Users.Watch(watchRequest, "me");
            var watchResponse = query.Execute();
            if (watchResponse.Expiration != null)
                userSettings.Expiration = watchResponse.Expiration.Value;
            if (watchResponse.HistoryId != null)
                userSettings.HistoryId = Convert.ToInt64(watchResponse.HistoryId.Value);

            _dbWorker.UpdateUserSettingsRecord(userSettings);
        }


        public async Task HandleStopWatchCommandAsync(ISender sender)
        {
            var service = Methods.SearchServiceByUserId(sender.From);
            var userSettings = await _dbWorker.FindUserSettingsAsync(sender.From);
            if (userSettings == null)
                throw new DbDataStoreException(
                    $"Can't find user settings data in database. User record with id {sender.From} is absent in the database.");
            if (!userSettings.MailNotification) return;

            var query = service.GmailService.Users.Stop("me");
            var stopResponse = await query.ExecuteAsync();
        }

        public async Task HandleNewMessageCommand(ISender sender)
        {
            await _botActions.SpecifyNewMailMessage(sender.From);
        }

        public async Task HandleGetInboxMessagesCommand(ISender sender)
        {
            await _botActions.GmailInlineCommandMessage(sender.From);
        }
    }
}