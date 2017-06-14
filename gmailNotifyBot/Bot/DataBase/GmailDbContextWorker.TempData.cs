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

        public Task<TempDataModel> FindTempDataAsync(int userId)
        {
            return Task.Run(() => FindTempData(userId));
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

        public Task<TempDataModel> AddNewTempDataAsync(TempDataModel tempData)
        {
            tempData.NullInspect(nameof(tempData));

            return Task.Run(() => AddNewTempData(tempData));
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

        public Task UpdateTempDataRecordAsync(TempDataModel tempDataModel)
        {
            return Task.Run(() => UpdateTempDataRecord(tempDataModel));
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

        public Task RemoveTempDataAsync(TempDataModel model)
        {
            model.NullInspect(nameof(model));
            return Task.Run(() => RemoveTempData(model));
        }
    }
}