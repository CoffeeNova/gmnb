using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;

namespace CoffeeJelly.gmailNotifyBot.Bot.DataBase
{
    public partial class GmailDbContextWorker
    {
        public TempDataModel FindTempData(int userId)
        {
            using (var db = new GmailBotDbContext())
            {
                return db.TempData.FirstOrDefault(t => t.UserId == userId);
            }
        }

        public async Task<TempDataModel> FindTempDataAsync(int userId)
        {
            using (var db = new GmailBotDbContext())
            {
                return await db.TempData.FirstOrDefaultAsync(t => t.UserId == userId);
            }
        }

        public TempDataModel AddNewTempData(TempDataModel tempData)
        {
            tempData.NullInspect(nameof(tempData));

            using (var db = new GmailBotDbContext())
            {
                var newModel = db.TempData.Add(tempData);
                db.SaveChanges();
                return newModel;
            }
        }

        public async Task<TempDataModel> AddNewTempDataAsync(TempDataModel tempData)
        {
            tempData.NullInspect(nameof(tempData));

            using (var db = new GmailBotDbContext())
            {
                var newModel = db.TempData.Add(tempData);
                await db.SaveChangesAsync();
                return newModel;
            }
        }

        public void UpdateTempDataRecord(TempDataModel tempDataModel)
        {
            using (var db = new GmailBotDbContext())
            {
                db.TempData.Attach(tempDataModel);
                db.Entry(tempDataModel).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public async Task UpdateTempDataRecordAsync(TempDataModel tempDataModel)
        {
            using (var db = new GmailBotDbContext())
            {
                db.TempData.Attach(tempDataModel);
                db.Entry(tempDataModel).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
        }

        public void RemoveTempData(TempDataModel model)
        {
            model.NullInspect(nameof(model));

            using (var db = new GmailBotDbContext())
            {
                var entry = db.Entry(model);
                if (entry.State == EntityState.Detached)
                    db.TempData.Attach(model);
                db.TempData.Remove(model);
                db.SaveChanges();
            }
        }

        public async Task RemoveTempDataAsync(TempDataModel model)
        {
            model.NullInspect(nameof(model));

            using (var db = new GmailBotDbContext())
            {
                var entry = db.Entry(model);
                if (entry.State == EntityState.Detached)
                    db.TempData.Attach(model);
                db.TempData.Remove(model);
                await db.SaveChangesAsync();
            }
        }
    }
}