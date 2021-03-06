﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels;
using System.Data.Entity;
using CoffeeJelly.gmailNotifyBot.Bot.Types;

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

        public async Task<NmStoreModel> FindNmStoreAsync(int userId)
        {
            using (var db = new GmailBotDbContext())
            {
                var nmStoreModel = await db.NmStore.FirstOrDefaultAsync(u => u.UserId == userId);
                if (nmStoreModel == null)
                    return null;
                await db.Entry(nmStoreModel)
                    .Collection(c => c.To)
                    .LoadAsync();
                await db.Entry(nmStoreModel)
                    .Collection(c => c.Cc)
                    .LoadAsync();
                await db.Entry(nmStoreModel)
                    .Collection(c => c.Bcc)
                    .LoadAsync();
                await db.Entry(nmStoreModel)
                    .Collection(c => c.File)
                    .LoadAsync();
                return nmStoreModel;
            }
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

        public async Task RemoveNmStoreAsync(NmStoreModel model)
        {
            model.NullInspect(nameof(model));

            using (var dbContext = new GmailBotDbContext())
            {
                var existModel = await dbContext.NmStore
                    .Where(nmStore => nmStore.Id == model.Id)
                    .Include(nmStore => nmStore.To)
                    .Include(nmStore => nmStore.Cc)
                    .Include(nmStore => nmStore.Bcc)
                    .Include(nmStore => nmStore.File)
                    .SingleOrDefaultAsync();
                if (existModel == null)
                    return;

                dbContext.NmStore.Remove(existModel);
                await dbContext.SaveChangesAsync();
            }
        }

        public NmStoreModel AddNewNmStore(int userId)
        {
            using (var db = new GmailBotDbContext())
            {
                var newModel = db.NmStore.Add(new NmStoreModel { UserId = userId });
                db.SaveChanges();
                return newModel;
            }
        }

        public async Task<NmStoreModel> AddNewNmStoreAsync(int userId)
        {
            using (var db = new GmailBotDbContext())
            {
                var newModel = db.NmStore.Add(new NmStoreModel { UserId = userId });
                await db.SaveChangesAsync();
                return newModel;
            }
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
                dbContext.To.RemoveRange(existModel.To.Except(newModel.To, new IdEqualityComparer<ToModel>()));
                dbContext.Cc.RemoveRange(existModel.Cc.Except(newModel.Cc, new IdEqualityComparer<CcModel>()));
                dbContext.Bcc.RemoveRange(existModel.Bcc.Except(newModel.Bcc, new IdEqualityComparer<BccModel>()));
                dbContext.File.RemoveRange(existModel.File.Except(newModel.File, new IdEqualityComparer<FileModel>()));
                //dbContext.SaveChanges();
                UpdateAdress(dbContext, newModel.To, existModel.To);
                UpdateAdress(dbContext, newModel.Cc, existModel.Cc);
                UpdateAdress(dbContext, newModel.Bcc, existModel.Bcc);
                UpdateFile(dbContext, newModel.File, existModel.File);
                dbContext.SaveChanges();
            }
        }

        public async Task UpdateNmStoreRecordAsync(NmStoreModel newModel)
        {
            using (var dbContext = new GmailBotDbContext())
            {
                var existModel = await dbContext.NmStore
                                    .Where(nmStore => nmStore.Id == newModel.Id)
                                    .Include(nmStore => nmStore.To)
                                    .Include(nmStore => nmStore.Cc)
                                    .Include(nmStore => nmStore.Bcc)
                                    .Include(nmStore => nmStore.File)
                                    .SingleOrDefaultAsync();

                if (existModel == null)
                    return;
                // Update 
                dbContext.Entry(existModel).CurrentValues.SetValues(newModel);
                // Delete
                dbContext.To.RemoveRange(existModel.To.Except(newModel.To, new IdEqualityComparer<ToModel>()));
                dbContext.Cc.RemoveRange(existModel.Cc.Except(newModel.Cc, new IdEqualityComparer<CcModel>()));
                dbContext.Bcc.RemoveRange(existModel.Bcc.Except(newModel.Bcc, new IdEqualityComparer<BccModel>()));
                dbContext.File.RemoveRange(existModel.File.Except(newModel.File, new IdEqualityComparer<FileModel>()));

                UpdateAdress(dbContext, newModel.To, existModel.To);
                UpdateAdress(dbContext, newModel.Cc, existModel.Cc);
                UpdateAdress(dbContext, newModel.Bcc, existModel.Bcc);
                UpdateFile(dbContext, newModel.File, existModel.File);
                await dbContext.SaveChangesAsync();
            }
        }

        private void UpdateAdress<T>(DbContext dbContext, ICollection<T> newAddressCollection,
    ICollection<T> existAddressCollection) where T : class, INmStoreModelRelation, IUserInfo, new()
        {
            var tempCollection = existAddressCollection.Select(i => i).ToList();

            foreach (var address in newAddressCollection)
            {
                var existAddressModel = tempCollection
                .SingleOrDefault(a => a.Id == address.Id);

                if (existAddressModel != null)
                    dbContext.Entry(existAddressModel).CurrentValues.SetValues(address);
                else
                {
                    var newAddress = new T { Email = address.Email, Name = address.Name };
                    existAddressCollection.Add(newAddress);
                }
            }
        }

        private void UpdateFile(DbContext dbContext, ICollection<FileModel> newFileCollection,
            ICollection<FileModel> existFileCollection)
        {
            var tempCollection = existFileCollection.Select(i => i).ToList();
            foreach (var file in newFileCollection)
            {
                var existFileModel = tempCollection
                .SingleOrDefault(f => f.Id == file.Id);

                if (existFileModel != null)
                    dbContext.Entry(existFileModel).CurrentValues.SetValues(file);
                else
                {
                    var newfile = new FileModel
                    {
                        FileId = file.FileId,
                        AttachId = file.AttachId,
                        OriginalName = file.OriginalName
                    };
                    existFileCollection.Add(newfile);
                }
            }
        }
    }
}