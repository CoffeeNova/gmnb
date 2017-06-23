using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.gmailNotifyBot.Bot.Types;

namespace CoffeeJelly.gmailNotifyBot.Bot.DataBase
{
    public partial class GmailDbContextWorker
    {
        public UserSettingsModel FindUserSettings(int userId)
        {
            using (var db = new GmailBotDbContext())
            {
                var userSettingsModel = db.UserSettings.FirstOrDefault(u => u.UserId == userId);
                if (userSettingsModel == null)
                    return null;

                db.Entry(userSettingsModel)
                    .Collection(c => c.IgnoreList)
                    .Load();
                db.Entry(userSettingsModel)
                    .Collection(c => c.Blacklist)
                    .Load();
                db.Entry(userSettingsModel)
                    .Collection(c => c.Whitelist)
                    .Load();
                return userSettingsModel;
            }
        }

        public async Task<UserSettingsModel> FindUserSettingsAsync(int userId)
        {
            using (var db = new GmailBotDbContext())
            {
                var userSettingsModel = db.UserSettings.FirstOrDefault(u => u.UserId == userId);
                if (userSettingsModel == null)
                    return null;

                await db.Entry(userSettingsModel)
                    .Collection(c => c.IgnoreList)
                    .LoadAsync();
                await db.Entry(userSettingsModel)
                    .Collection(c => c.Blacklist)
                    .LoadAsync();
                await db.Entry(userSettingsModel)
                    .Collection(c => c.Whitelist)
                    .LoadAsync();
                return userSettingsModel;
            }
        }

        public UserSettingsModel AddNewUserSettings(int userId)
        {
            using (var db = new GmailBotDbContext())
            {
                var userSettings = db.UserSettings.Add(new UserSettingsModel { UserId = userId });
                db.SaveChanges();
                return userSettings;
            }
        }

        public async Task<UserSettingsModel> AddNewUserSettingsAsync(int userId)
        {
            using (var db = new GmailBotDbContext())
            {
                var userSettings = db.UserSettings.Add(new UserSettingsModel { UserId = userId });
                await db.SaveChangesAsync();
                return userSettings;
            }
        }

        public void RemoveUserSettingsRecord(UserSettingsModel model)
        {
            model.NullInspect(nameof(model));

            using (var db = new GmailBotDbContext())
            {
                var existModel = db.UserSettings
                                    .Where(userSettings => userSettings.Id == model.Id)
                                    .Include(userSettings => userSettings.IgnoreList)
                                    .Include(userSettings => userSettings.Blacklist)
                                    .Include(userSettings => userSettings.Whitelist)
                                    .SingleOrDefault();
                if (existModel == null)
                    return;

                db.UserSettings.Remove(existModel);
                db.SaveChanges();
            }
        }

        public async Task RemoveUserSettingsRecordAsync(UserSettingsModel model)
        {
            model.NullInspect(nameof(model));

            using (var db = new GmailBotDbContext())
            {
                var existModel = await db.UserSettings
                    .Where(userSettings => userSettings.Id == model.Id)
                    .Include(userSettings => userSettings.IgnoreList)
                    .Include(userSettings => userSettings.Blacklist)
                    .Include(userSettings => userSettings.Whitelist)
                    .SingleOrDefaultAsync();
                if (existModel == null)
                    return;

                db.UserSettings.Remove(existModel);
                await db.SaveChangesAsync();
            }
        }

        public List<UserSettingsModel> GetAllUsersSettings()
        {
            using (var db = new GmailBotDbContext())
            {
                foreach (var us in db.UserSettings)
                {
                    db.Entry(us)
                    .Collection(c => c.IgnoreList)
                    .Load();
                    db.Entry(us)
                    .Collection(c => c.Blacklist)
                    .Load();
                    db.Entry(us)
                    .Collection(c => c.Whitelist)
                    .Load();
                }
                return db.UserSettings.ToList();
            }
        }

        public async Task<List<UserSettingsModel>> GetAllUsersSettingsAsync()
        {
            using (var db = new GmailBotDbContext())
            {
                foreach (var us in db.UserSettings)
                {
                    await db.Entry(us)
                        .Collection(c => c.IgnoreList)
                        .LoadAsync();
                    await db.Entry(us)
                        .Collection(c => c.Blacklist)
                        .LoadAsync();
                    await db.Entry(us)
                        .Collection(c => c.Whitelist)
                        .LoadAsync();
                }
                return db.UserSettings.ToList();
            }
        }

        public void UpdateUserSettingsRecord(UserSettingsModel newModel)
        {
            using (var db = new GmailBotDbContext())
            {
                var existModel = db.UserSettings
                                    .Where(userSettings => userSettings.Id == newModel.Id)
                                    .Include(userSettings => userSettings.IgnoreList)
                                    .Include(userSettings => userSettings.Blacklist)
                                    .Include(userSettings => userSettings.Whitelist)
                                    .SingleOrDefault();
                if (existModel == null)
                    return;
                // Update 
                db.Entry(existModel).CurrentValues.SetValues(newModel);
                //Delete
                db.Ignore.RemoveRange(existModel.IgnoreList.Except(newModel.IgnoreList, new IdEqualityComparer<IgnoreModel>()));
                db.Blacklist.RemoveRange(existModel.Blacklist.Except(newModel.Blacklist, new IdEqualityComparer<BlacklistModel>()));
                db.Whitelist.RemoveRange(existModel.Whitelist.Except(newModel.Whitelist, new IdEqualityComparer<WhitelistModel>()));

                UpdateIgnoreList(db, newModel.IgnoreList, existModel.IgnoreList);
                UpdateLabelsList(db, newModel.Blacklist, existModel.Blacklist);
                UpdateLabelsList(db, newModel.Whitelist, existModel.Whitelist);
                db.SaveChanges();
            }
        }

        public async Task UpdateUserSettingsRecordAsync(UserSettingsModel newModel)
        {
            using (var db = new GmailBotDbContext())
            {
                var existModel = await db.UserSettings
                    .Where(userSettings => userSettings.Id == newModel.Id)
                    .Include(userSettings => userSettings.IgnoreList)
                    .Include(userSettings => userSettings.Blacklist)
                    .Include(userSettings => userSettings.Whitelist)
                    .SingleOrDefaultAsync();
                if (existModel == null)
                    return;
                // Update 
                db.Entry(existModel).CurrentValues.SetValues(newModel);
                //Delete
                db.Ignore.RemoveRange(existModel.IgnoreList.Except(newModel.IgnoreList, new IdEqualityComparer<IgnoreModel>()));
                db.Blacklist.RemoveRange(existModel.Blacklist.Except(newModel.Blacklist, new IdEqualityComparer<BlacklistModel>()));
                db.Whitelist.RemoveRange(existModel.Whitelist.Except(newModel.Whitelist, new IdEqualityComparer<WhitelistModel>()));

                UpdateIgnoreList(db, newModel.IgnoreList, existModel.IgnoreList);
                UpdateLabelsList(db, newModel.Blacklist, existModel.Blacklist);
                UpdateLabelsList(db, newModel.Whitelist, existModel.Whitelist);
                await db.SaveChangesAsync();
            }
        }

        private void UpdateIgnoreList(DbContext dbContext, ICollection<IgnoreModel> newIgnoreListCollection,
           ICollection<IgnoreModel> existIgnoreListCollection)
        {
            var tempCollection = existIgnoreListCollection.Select(i => i).ToList();
            foreach (var ignoreModel in newIgnoreListCollection)
            {
                var existIgnoreModel = tempCollection
                .SingleOrDefault(i => i.Id == ignoreModel.Id);

                if (existIgnoreModel != null)
                    dbContext.Entry(existIgnoreModel).CurrentValues.SetValues(ignoreModel);
                else
                {
                    var newIgnoreModel = new IgnoreModel
                    {
                        Address = ignoreModel.Address,
                        UserSettingsModel = ignoreModel.UserSettingsModel
                    };
                    existIgnoreListCollection.Add(newIgnoreModel);
                }
            }
        }

        private void UpdateLabelsList<T>(DbContext dbContext, ICollection<T> newLabelListCollection,
          ICollection<T> existLabelListCollection) where T : class, IUserSettingsModelRelation, ILabelInfo, new()
        {
            var tempCollection = existLabelListCollection.Select(i => i).ToList();
            foreach (var labelInfoModel in newLabelListCollection)
            {
                var existLabelInfoModel = tempCollection
                .SingleOrDefault(l => l.Id == labelInfoModel.Id);

                if (existLabelInfoModel != null)
                    dbContext.Entry(existLabelInfoModel).CurrentValues.SetValues(labelInfoModel);
                else
                {
                    var newLabel = new T
                    {
                        Name = labelInfoModel.Name,
                        LabelId = labelInfoModel.LabelId
                    };
                    existLabelListCollection.Add(newLabel);
                }
            }
        }

        #region ignore part

        public void AddToIgnoreList(int userId, string address) //marked
        {
            address.NullInspect(nameof(address));

            var userSettings = FindUserSettings(userId);
            if (userSettings == null) return;

            userSettings.IgnoreList.Add(new IgnoreModel { Address = address });
            UpdateUserSettingsRecord(userSettings);
        }

        public async Task AddToIgnoreListAsync(int userId, string address)
        {
            address.NullInspect(nameof(address));

            var userSettings = await FindUserSettingsAsync(userId);
            if (userSettings == null) return;

            userSettings.IgnoreList.Add(new IgnoreModel { Address = address });
            await UpdateUserSettingsRecordAsync(userSettings);
        }

        public void RemoveFromIgnoreList(int userId, string address)
        {
            address.NullInspect(nameof(address));

            var userSettings = FindUserSettings(userId);
            if (userSettings == null) return;

            userSettings.IgnoreList.RemoveAll(i => i.Address == address);
            UpdateUserSettingsRecord(userSettings);
        }

        public async Task RemoveFromIgnoreListAsync(int userId, string address)
        {
            address.NullInspect(nameof(address));

            var userSettings = await FindUserSettingsAsync(userId);
            if (userSettings == null) return;

            userSettings.IgnoreList.RemoveAll(i => i.Address == address);
            await UpdateUserSettingsRecordAsync(userSettings);
        }

        public bool IsPresentInIgnoreList(int userId, string address)
        {
            address.NullInspect(nameof(address));

            var userSettings = FindUserSettings(userId);
            return userSettings != null && userSettings.IgnoreList.Any(a => a.Address == address);
        }

        public async Task<bool> IsPresentInIgnoreListAsync(int userId, string address)
        {
            address.NullInspect(nameof(address));

            var userSettings = await FindUserSettingsAsync(userId);
            return userSettings != null && userSettings.IgnoreList.Any(a => a.Address == address);
        }
        #endregion
    }
}