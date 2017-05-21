using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.Exceptions;
using CoffeeJelly.gmailNotifyBot.Bot.Types;
using Newtonsoft.Json;
using Google.Apis.Gmail.v1;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls
{
    public partial class CommandHandler
    {
        public bool HandleGoogleNotifyMessage(GoogleNotifyMessage message)
        {
            bool result = true;
            if (message?.Message == null)
            {
                result = false;
                LogMaker.Log(Logger, "message?.Message==null", false);
            }
            else if (message.Subscription != BotInitializer.Instance.BotSettings.Subscription)
            {
                result = false;
                LogMaker.Log(Logger, "message.Subscription", false);
            }

            if (result)
            {
                var decodedData = DecodeMessageData(message.Message.Data);
                if (decodedData == null)
                {
                    result = false;
                    LogMaker.Log(Logger, $"encodedData, data:{message.Message.Data}", false);
                }
                else
                {
                    LogMaker.Log(Logger, $"Recieved push notification from account {decodedData.Email} with historyId={decodedData.HistoryId}.", false);
                    try
                    {
                        var userModel = _dbWorker.FindUserByEmail(decodedData.Email);
                        var userSettings = _dbWorker.FindUserSettings(userModel.UserId);
                        var service = SearchServiceByUserId(userModel.UserId.ToString());
                        var query = service.GmailService.Users.History.List("me");
                        query.HistoryTypes = UsersResource.HistoryResource.ListRequest.HistoryTypesEnum.MessageAdded;
                        query.LabelId = "INBOX";
                        query.StartHistoryId = userSettings.HistoryId;
                        var listRequest = query.Execute();
                        if (listRequest.HistoryId != null)
                        {
                            userSettings.HistoryId = ulong.Parse(decodedData.HistoryId);
                            _dbWorker.UpdateUserSettingsRecord(userSettings);
                        }
                        var historyList = listRequest.History;
                        if (historyList == null) 
                            LogMaker.Log(Logger, "historyList==null", false);
                        var newMessages = historyList.First().MessagesAdded;
                        if (newMessages == null)
                            LogMaker.Log(Logger, "newMessages==null", false);
                        var newMessage = newMessages.First()?.Message;
                        if (newMessage == null)
                            LogMaker.Log(Logger, "newMessage==null", false);
                        var formattedMessage = new FormattedMessage(newMessage);
                        var isIgnored = _dbWorker.IsPresentInIgnoreList(userModel.UserId, formattedMessage.From.Email);
                        _botActions.ShowChosenShortMessage(userModel.UserId.ToString(), formattedMessage, isIgnored).Wait();
                    }
                    catch (Exception ex)
                    {
                        LogMaker.Log(Logger, ex);
                    }
                }
            }

            if (!result)
                LogMaker.Log(Logger, $"Unauthorized attempt to notify.", false);
            return result;
        }

        private EncodedMessageData DecodeMessageData(string text)
        {
            try
            {
                text = text.Replace('-', '+');
                text = text.Replace('_', '/');
                var encodedJson = Base64.Decode(text);
                var encodedData = JsonConvert.DeserializeObject<EncodedMessageData>(encodedJson);
                return encodedData;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}