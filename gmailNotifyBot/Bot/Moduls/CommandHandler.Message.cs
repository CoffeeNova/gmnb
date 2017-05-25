using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CoffeeJelly.gmailNotifyBot.Bot.Exceptions;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.TelegramBotApiWrapper.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types.Messages;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Message = CoffeeJelly.TelegramBotApiWrapper.Types.Messages.Message;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls
{
    public sealed partial class CommandHandler
    {
        private async void _updatesHandler_TelegramTextMessageEvent(TextMessage message)
        {
            if (message?.Text == null)
                throw new ArgumentNullException(nameof(message));
            if (!message.Text.StartsWithAny(StringComparison.CurrentCultureIgnoreCase,
                    Commands.TESTNAME_COMMAND, Commands.TESTMESSAGE_COMMAND,
                    Commands.CONNECT_COMMAND, Commands.INBOX_COMMAND, Commands.TESTTHREAD_COMMAND,
                    Commands.START_NOTIFY_COMMAND, Commands.STOP_NOTIFY_COMMAND, Commands.START_WATCH_COMMAND,
                    Commands.STOP_WATCH_COMMAND, Commands.NEW_MESSAGE_COMMAND)) return;
            Exception exception = null;

            LogMaker.Log(Logger, $"{message.Text} command received from user with id {(string)message.From}", false);
            try
            {
                if (message.Text.StartsWith(Commands.TESTNAME_COMMAND))
                    await HandleTestNameCommand(message);
                else if (message.Text.StartsWith(Commands.TESTMESSAGE_COMMAND))
                {
                    var splittedtext = message.Text.Split(' ');
                    var messageId = splittedtext.Length > 1 ? splittedtext[1] : "";
                    await HandleTestMessageCommand(message, messageId);
                }
                else if (message.Text.StartsWith(Commands.CONNECT_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                    await _authorizer.SendAuthorizeLink(message);
                else if (message.Text.StartsWith(Commands.INBOX_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                    await HandleGetInboxMessagesCommand(message);
                else if (message.Text.StartsWith(Commands.TESTTHREAD_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                    await HandleTestThreadCommand(message);
                else if (message.Text.StartsWith(Commands.START_NOTIFY_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                    await HandleStartNotifyCommand(message);
                else if (message.Text.StartsWith(Commands.STOP_NOTIFY_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                    await HandleStopNotifyCommand(message);
                else if (message.Text.StartsWith(Commands.START_WATCH_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                    await HandleStartWatchCommandAsync(message);
                else if (message.Text.StartsWith(Commands.STOP_WATCH_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                    await HandleStopWatchCommandAsync(message);
                else if (message.Text.StartsWith(Commands.NEW_MESSAGE_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                    await HandleNewMessageCommand(message);
            }
            catch (ServiceNotFoundException ex)
            {
                exception = ex;
                await _botActions.WrongCredentialsMessage(message.From);
            }
            catch (DbDataStoreException ex)
            {
                exception = ex;
                await _botActions.WrongCredentialsMessage(message.From);
            }
            catch (AuthorizeException ex)
            {
                exception = ex;
                await _botActions.AuthorizationErrorMessage(message.From);
            }
            catch (Exception ex)
            {
                exception = ex;
#if DEBUG
                Debug.Assert(false, "operation error show to telegram chat as answerCallbackQuery");
#endif
            }
            finally
            {
                if (exception != null)
                    LogMaker.Log(Logger, exception,
                    $"An exception has been thrown in processing TextMessage with command {message.Text}");
            }
        }

        private async Task HandleTestNameCommand(ISender sender)
        {
            var service = SearchServiceByUserId(sender.From);
            var query = service.Oauth2Service.Userinfo.Get();
            var userinfo = await query.ExecuteAsync();
            await _botActions.EmailAddressMessage(sender.From, userinfo.Name);
        }

        private async Task HandleTestMessageCommand(ISender sender, string messageId)
        {
            var service = SearchServiceByUserId(sender.From);
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

        private async Task HandleTestThreadCommand(ISender sender)
        {
            var service = SearchServiceByUserId(sender.From);
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

        private async Task HandleStartNotifyCommand(ISender sender)
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
        private async Task HandleStopNotifyCommand(ISender sender)
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
            var service = SearchServiceByUserId(sender.From);
            var userSettings = await _dbWorker.FindUserSettingsAsync(sender.From);
            if (userSettings == null)
                throw new DbDataStoreException(
                    $"Can't find user settings data in database. User record with id {sender.From} is absent in the database.");
            if (!userSettings.MailNotification) return;

            var watchRequest = new WatchRequest
            {
                LabelIds = new List<string> { "INBOX" },
                TopicName = TopicName
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
            var service = SearchServiceByUserId(sender.From);
            var userSettings = _dbWorker.FindUserSettings(sender.From);
            if (userSettings == null)
                throw new DbDataStoreException(
                    $"Can't find user settings data in database. User record with id {sender.From} is absent in the database.");
            if (!userSettings.MailNotification) return;

            var watchRequest = new WatchRequest
            {
                LabelIds = new List<string> { "INBOX" },
                TopicName = TopicName
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
            var service = SearchServiceByUserId(sender.From);
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

        private async Task HandleGetInboxMessagesCommand(Message sender)
        {
            await _botActions.GmailInlineCommandMessage(sender.From);
        }

    }

}