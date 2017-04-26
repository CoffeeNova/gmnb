using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using Google.Apis.Util.Store;

namespace CoffeeJelly.gmailNotifyBot.Bot
{
    public class DbDataStore :IDataStore
    {
        public async Task StoreAsync<T>(string key, T value)
        {
            key.NullInspect(nameof(key));

            using (var context = new GmailBotDbContext())
            {
                
            }
                throw new NotImplementedException();
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
    }
}