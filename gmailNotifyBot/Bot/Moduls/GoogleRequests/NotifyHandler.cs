using System;
using System.Collections.Generic;
using System.Linq;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels;
using CoffeeJelly.gmailNotifyBot.Bot.Exceptions;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity;
using CoffeeJelly.gmailNotifyBot.Bot.Types;
using Google.Apis.Gmail.v1;
using Newtonsoft.Json;
using NLog;
using MessageHandler = CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.MessageUpdates.MessageHandler;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.GoogleRequests
{
    //this modul should be initialized after MessageHandler and Authorizer
    public class NotifyHandler
    {
        public NotifyHandler()
        {
            try
            {
                _botSettings = BotInitializer.Instance.BotSettings;
                _botActions = new BotActions(_botSettings.Token);
                _messageHandler = BotInitializer.Instance.MessageHandler;
                _dbWorker = new GmailDbContextWorker();
                var authorizer = BotInitializer.Instance.Authorizer;
                authorizer.AuthorizationRegistredEvent += Instance_AuthorizationRegistredEvent;
            }
            catch (Exception ex)
            {
                throw new TypeInitializationException(nameof(NotifyHandler), ex);
            }
        }

        public bool HandleGoogleNotifyMessage(GoogleNotifyMessage message)
        {
            bool result = true;
            if (message?.Message == null)
                result = false;
            else if (message.Subscription != BotInitializer.Instance.BotSettings.Subscription)
                result = false;

            if (result)
            {
                var decodedData = JsonConvert.DeserializeObject<EncodedMessageData>(Base64.DecodeUrlSafe(message.Message.Data));
                if (decodedData == null)
                    result = false;
                else
                {
                    LogMaker.Log(Logger, $"Received push notification from account {decodedData.Email} with historyId={decodedData.HistoryId}.", false);
                    UserSettingsModel userSettings = null;
                    try
                    {
                        var userModel = _dbWorker.FindUserByEmail(decodedData.Email);
                        if (userModel == null)
                            throw new DbDataStoreException(
                                $"Can't find user data in database. User record with email {decodedData.Email} is absent in the database.");
                        userSettings = _dbWorker.FindUserSettings(userModel.UserId);
                        if (userSettings == null)
                            throw new DbDataStoreException(
                                $"Can't find user settings data in database. User record with id {userModel.UserId} is absent in the database.");

                        var service = Methods.SearchServiceByUserId(userModel.UserId.ToString());
                        var query = service.GmailService.Users.History.List("me");
                        query.HistoryTypes = UsersResource.HistoryResource.ListRequest.HistoryTypesEnum.MessageAdded;
                        query.LabelId = "INBOX";
                        query.StartHistoryId = Convert.ToUInt64(userSettings.HistoryId);
                        var listRequest = query.Execute();
                        var historyList = listRequest.History?.ToList();
                        if (historyList != null)
                        {
                            var addedMessageCollection =
                                historyList.FindAll(h => h.MessagesAdded != null)
                                    .SelectMany(m => m.MessagesAdded)
                                    .ToList();

                            foreach (var addedMessage in addedMessageCollection)
                            {
                                var getRequest = service.GmailService.Users.Messages.Get("me", addedMessage.Message.Id);
                                var messageResponse = getRequest.Execute();
                                var formattedMessage = new FormattedMessage(messageResponse);

                                if (userSettings.IgnoreList.Any(ignoreModel => ignoreModel.Address == formattedMessage.From.Email))
                                    continue;

                                if (userSettings.UseWhitelist)
                                    if (!userSettings.Whitelist.Any(label => formattedMessage.LabelIds.Contains(label.LabelId)))
                                        continue;

                                if (userSettings.Blacklist.Any(label => formattedMessage.LabelIds.Contains(label.LabelId)))
                                    continue;

                                if (userSettings.ReadAfterReceiving)
                                    formattedMessage = Methods.ModifyMessageLabels(service, formattedMessage.MessageId, new List<string> { "UNREAD" });

                                _botActions.ShowShortMessage(userModel.UserId.ToString(), formattedMessage);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogMaker.Log(Logger, ex);
                    }
                    finally
                    {
                        if (userSettings != null)
                        {
                            userSettings.HistoryId = long.Parse(decodedData.HistoryId);
                            _dbWorker.UpdateUserSettingsRecord(userSettings);
                        }
                    }
                }
            }

            if (!result)
                LogMaker.Log(Logger, $"Unauthorized attempt to notify.", false);
            return result;
        }

        private void Instance_AuthorizationRegistredEvent(UserModel userModel, UserSettingsModel userSettingsModel)
        {
            try
            {
                var service = Methods.SearchServiceByUserId(userModel.UserId.ToString());
                if (service == null) return;

                _messageHandler.HandleStartWatchCommand(service);
            }
            catch (Exception ex)
            {
                LogMaker.Log(Logger, ex);
            }
        }

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly GmailDbContextWorker _dbWorker;
        private readonly BotActions _botActions;
        private readonly MessageHandler _messageHandler;
        private readonly BotSettings _botSettings;
    }
}