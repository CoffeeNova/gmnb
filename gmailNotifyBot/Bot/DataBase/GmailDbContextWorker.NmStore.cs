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
                var nmStoreModel = db.NmStore.FirstOrDefault(u => u.UserId == userId);
                if (nmStoreModel == null)
                    return null;
                var files = db.File.Include(f => f.NmStoreModelId == nmStoreModel.Id).ToList();
                nmStoreModel.File = files;
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

        public void UpdateNmStoreRecord(NmStoreModel model)
        {
            //using (var db = new GmailBotDbContext())
            //{
            //    db.NmStore.Attach(model);
            //    db.Entry(model).State = EntityState.Modified;
            //    db.SaveChanges();
            //}

            using (var dbContext = new GmailBotDbContext())
            {
                var nmStoreModel = dbContext.NmStore
                                    .Where(nmStore => nmStore.Id == model.Id)
                                    .Include(nmStore => nmStore.File)
                                    .SingleOrDefault();

                if (nmStoreModel == null)
                    return;
                // Update 
                dbContext.Entry(nmStoreModel).CurrentValues.SetValues(model);

                // Delete file
                foreach (var file in nmStoreModel.File)
                {
                    if (model.File.All(f => f.Id != file.Id))
                        dbContext.File.Remove(file);
                }

                foreach (var file in model.File)
                {
                    var fileModel = nmStoreModel.File
                        .SingleOrDefault(f => f.Id == file.Id);

                    if (fileModel != null)
                        // Update
                        dbContext.Entry(fileModel).CurrentValues.SetValues(file);
                    else
                    {
                        // Insert child
                        var newfile = new FileModel
                        {
                            //FilePath = file.FilePath,
                            //FileSize = file.FileSize,
                            FileId = file.FileId,
                            OriginalName = file.OriginalName,
                            NmStoreModel = file.NmStoreModel
                        };
                        nmStoreModel.File.Add(newfile);
                    }
                }

                dbContext.SaveChanges();
            }
        }

        public Task UpdateNmStoreRecordAsync(NmStoreModel model)
        {
            return Task.Run(() => UpdateNmStoreRecord(model));
        }
    }
}