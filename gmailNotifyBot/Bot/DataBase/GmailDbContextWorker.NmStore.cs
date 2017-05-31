using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels;
using System.Data.Entity;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;

namespace CoffeeJelly.gmailNotifyBot.Bot.DataBase
{
    public partial class GmailDbContextWorker
    {
        public NmStoreModel FindNmStore(int userId)
        {
            using (var db = new GmailBotDbContext())
            {
                return db.NmStore.FirstOrDefault(u => u.UserId == userId);
            }
        }

        public Task<NmStoreModel> FindNmStoreAsync(int userId)
        {
            return Task.Run(() => FindNmStore(userId));
        }

        public void RemoveNmStore(NmStoreModel model)
        {
            model.NullInspect(nameof(model));

            using (var db = new GmailBotDbContext())
            {
                var entry = db.Entry(model);
                if (entry.State == EntityState.Detached)
                    db.NmStore.Attach(model);
                db.NmStore.Remove(model);
                db.SaveChanges();
            }
        }

        public Task RemoveNmStoreAsync(NmStoreModel model)
        {
            return Task.Run(() => RemoveNmStore(model));
        }

        public void AddNewNmStore(NmStoreModel model)
        {
            model.NullInspect(nameof(model));

            using (var db = new GmailBotDbContext())
            {
                var newModel = db.NmStore.Add(model);
                db.SaveChanges();
            }
        }

        public Task AddNewNmStoreAsync(NmStoreModel model)
        {
            model.NullInspect(nameof(model));

            return Task.Run(() => AddNewNmStore(model));
        }

        public void UpdateUserRecord(NmStoreModel model)
        {
            using (var db = new GmailBotDbContext())
            {
                db.NmStore.Attach(model);
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public Task UpdateNmStoreRecordAsync(NmStoreModel model)
        {
            return Task.Run(() => UpdateUserRecord(model));
        }
    }
}