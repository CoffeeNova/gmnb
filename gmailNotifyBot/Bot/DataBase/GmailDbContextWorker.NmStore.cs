using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels;
using System.Data.Entity;

namespace CoffeeJelly.gmailNotifyBot.Bot.DataBase
{
    public partial class GmailDbContextWorker
    {
        public NmStoreModel FindNmStore(int userId)
        {
            using (var db = new GmailBotDbContext())
            {
                var nmStoreModel = db.NmStore.FirstOrDefault(u => u.UserId == userId);
                if (nmStoreModel == null)
                    return null;
                db.Entry(nmStoreModel)
                    .Collection(c => c.To)
                    .Load();
                db.Entry(nmStoreModel)
                    .Collection(c => c.Cc)
                    .Load();
                db.Entry(nmStoreModel)
                    .Collection(c => c.Bcc)
                    .Load();
                db.Entry(nmStoreModel)
                    .Collection(c => c.File)
                    .Load();
                return nmStoreModel;
            }
        }

        public Task<NmStoreModel> FindNmStoreAsync(int userId)
        {
            return Task.Run(() => FindNmStore(userId));
        }

        public void RemoveNmStore(NmStoreModel model)
        {
            model.NullInspect(nameof(model));

            using (var dbContext = new GmailBotDbContext())
            {
                var existModel = dbContext.NmStore
                                    .Where(nmStore => nmStore.Id == model.Id)
                                    .Include(nmStore => nmStore.To)
                                    .Include(nmStore => nmStore.Cc)
                                    .Include(nmStore => nmStore.Bcc)
                                    .Include(nmStore => nmStore.File)
                                    .SingleOrDefault();
                if (existModel == null)
                    return;

                dbContext.NmStore.Remove(existModel);
                dbContext.SaveChanges();
            }
        }

        public Task RemoveNmStoreAsync(NmStoreModel model)
        {
            return Task.Run(() => RemoveNmStore(model));
        }

        public NmStoreModel AddNewNmStore(int userId)
        { 
            using (var db = new GmailBotDbContext())
            {
                var newModel = db.NmStore.Add(new NmStoreModel {UserId = userId});
                db.SaveChanges();
                return newModel;
            }
        }

        public Task<NmStoreModel> AddNewNmStoreAsync(int userId)
        {
            return Task.Run(() => AddNewNmStore(userId));
        }

        public void UpdateNmStoreRecord(NmStoreModel newModel)
        {
            using (var dbContext = new GmailBotDbContext())
            {
                var existModel = dbContext.NmStore
                                    .Where(nmStore => nmStore.Id == newModel.Id)
                                    .Include(nmStore => nmStore.To)
                                    .Include(nmStore => nmStore.Cc)
                                    .Include(nmStore => nmStore.Bcc)
                                    .Include(nmStore => nmStore.File)
                                    .SingleOrDefault();

                if (existModel == null)
                    return;
                // Update 
                dbContext.Entry(existModel).CurrentValues.SetValues(newModel);
                // Delete
                dbContext.To.RemoveRange(existModel.To.Intersect(newModel.To));
                dbContext.Cc.RemoveRange(existModel.Cc.Intersect(newModel.Cc));
                dbContext.Bcc.RemoveRange(existModel.Bcc.Intersect(newModel.Bcc));
                dbContext.File.RemoveRange(existModel.File.Intersect(newModel.File));

                UpdateAdress(dbContext, newModel.To, existModel.To);
                UpdateAdress(dbContext, newModel.Cc, existModel.Cc);
                UpdateAdress(dbContext, newModel.Bcc, existModel.Bcc);
                UpdateFile(dbContext, newModel.File, existModel.File);
                dbContext.SaveChanges();
            }
        }

        public Task UpdateNmStoreRecordAsync(NmStoreModel model)
        {
            return Task.Run(() => UpdateNmStoreRecord(model));
        }

        private void UpdateAdress<T>(DbContext dbContext, ICollection<T> newAddressCollection,
    ICollection<T> existAddressCollection) where T : class, IAddressModel, new()
        {
            foreach (var address in newAddressCollection)
            {
                var existAddressModel = existAddressCollection
                .SingleOrDefault(a => a.Id == address.Id);

                if (existAddressModel != null)
                    dbContext.Entry(existAddressModel).CurrentValues.SetValues(address);
                else
                {
                    var newAddress = new T { Address = address.Address };
                    existAddressCollection.Add(newAddress);
                }
            }
        }

        private void UpdateFile(DbContext dbContext, ICollection<FileModel> newFileCollection,
            ICollection<FileModel> existFileCollection)
        {
            foreach (var file in newFileCollection)
            {
                var existFileModel = existFileCollection
                .SingleOrDefault(f => f.Id == file.Id);

                if (existFileModel != null)
                    dbContext.Entry(existFileModel).CurrentValues.SetValues(file);
                else
                {
                    var newfile = new FileModel
                    {
                        FileId = file.FileId,
                        OriginalName = file.OriginalName,
                        NmStoreModel = file.NmStoreModel
                    };
                    existFileCollection.Add(newfile);
                }
            }
        }
    }
}