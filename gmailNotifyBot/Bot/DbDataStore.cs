using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels;
using CoffeeJelly.gmailNotifyBot.Bot.Exceptions;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.gmailNotifyBot.Bot.Moduls;
using Google.Apis.Util.Store;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;

namespace CoffeeJelly.gmailNotifyBot.Bot
{
    public class DbDataStore : IDataStore
    {
        public async Task StoreAsync<T>(string key, T value)
        {
            key.NullInspect(nameof(key));

            var dbWorker = new GmailDbContextWorker();

            var json = JsonConvert.SerializeObject(value);

            int userId;
            if (!int.TryParse(key, out userId))
                throw new ArgumentException("Wrong key, it must be an User Id number.", key);

            var userModel = await dbWorker.FindUserAsync(userId);
            if (userModel == null)
                throw new DbDataStoreException(
                    $"Can't store refreshed data in database. {nameof(UserModel)} record with id {userId} is absent in the database.");

            JsonConvert.PopulateObject(json, userModel);
            await dbWorker.UpdateUserRecordAsync(userModel);
            LogMaker.Log(Logger, $"{nameof(UserModel)} record with userId={userId} updated with new access token.", false);

           // Debug.Assert(false, "?? should i revoke service from ServiceFactory.Instance.ServiceCollection ??");
        }

        public async Task DeleteAsync<T>(string key)
        {
            key.NullInspect(nameof(key));

            var dbWorker = new GmailDbContextWorker();
            int userId;
            if (!int.TryParse(key, out userId))
                throw new ArgumentException("Wrong key, it must be an User Id number.", key);

            var userModel = await dbWorker.FindUserAsync(userId);
            if (userModel == null)
                LogMaker.Log(Logger,
                    $"Error while removing {nameof(UserModel)} record from database. {nameof(UserModel)} record with id {userId} is absent in the database.",
                    false);
            else
            {
                await dbWorker.RemoveUserRecordAsync(userModel);
                LogMaker.Log(Logger, $"{nameof(UserModel)} record with userId={userId} deleted.", false);
            }

            var userSettingsModel = await dbWorker.FindUserSettingsAsync(userId);
            if (userSettingsModel == null)
                LogMaker.Log(Logger,
                    $"Error while removing {nameof(UserSettingsModel)} record from database. {nameof(UserSettingsModel)} record with id {userId} is absent in the database.",
                    false);
            else
            {
                await dbWorker.RemoveUserSettingsRecordAsync(userSettingsModel);
                LogMaker.Log(Logger, $"{nameof(UserSettingsModel)} record with userId={userId} deleted.", false);
            }

            var nmStoreModel = await dbWorker.FindNmStoreAsync(userId);
            if (nmStoreModel != null)
            {
                await dbWorker.RemoveNmStoreAsync(nmStoreModel);
                LogMaker.Log(Logger, $"{nameof(NmStoreModel)} record with userId={userId} deleted.", false);
            }
            var tempData = await dbWorker.FindTempDataAsync(userId);
            if (tempData != null)
            {
                await dbWorker.RemoveTempDataAsync(tempData);
                LogMaker.Log(Logger, $"{nameof(TempDataModel)} record with userId={userId} deleted.", false);
            }

            var i = ServiceFactory.Instance?.ServiceCollection.RemoveAll(s => s.From == userId);
            if (i > 0)
                LogMaker.Log(Logger, $"Removed service from {nameof(ServiceFactory.ServiceCollection)} with userId={userId}.", false);
        }

        public Task<T> GetAsync<T>(string key)
        {
            throw new NotImplementedException();
        }

        public Task ClearAsync()
        {
            throw new NotImplementedException();
        }

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    }
}