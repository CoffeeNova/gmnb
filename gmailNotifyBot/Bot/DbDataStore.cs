using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels;
using CoffeeJelly.gmailNotifyBot.Bot.Exceptions;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using Google.Apis.Util.Store;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;

namespace CoffeeJelly.gmailNotifyBot.Bot
{
    public class DbDataStore :IDataStore
    {
        public async Task StoreAsync<T>(string key, T value)
        {
            key.NullInspect(nameof(key));

            var dbWorker = new GmailDbContextWorker();

            var json = JsonConvert.SerializeObject(value);

            long userId;
            if (!long.TryParse(key, out userId))
                throw new ArgumentException("Wrong key, it must be an User Id number.", key);

            var userModel = await dbWorker.FindUserAsync(userId);
            if (userModel == null)
                throw new DbDataStroreException(
                    $"Can't store refreshed data in database. User record with id {userId} is absent in the database.");

            JsonConvert.PopulateObject(json, userModel);
            await dbWorker.UpdateUserRecordAsync(userModel);
            LogMaker.Log(Logger, $"User record with userId={userId} updated with new access token.", false);
        }

        public Task DeleteAsync<T>(string key)
        {
            throw new NotImplementedException();
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