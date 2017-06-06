using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;

namespace CoffeeJelly.gmailNotifyBot.Bot.DataBase
{
    public partial class GmailDbContextWorker
    {
        public UserSettingsModel FindUserSettings(int userId)
        {
            using (var db = new GmailBotDbContext())
            {
                var userSettingsModel =  db.UserSettings.FirstOrDefault(u => u.UserId == userId);
                if (userSettingsModel == null)
                    return null;
                db.Entry(userSettingsModel)
                    .Collection(c => c.IgnoreList)
                    .Load();
                return userSettingsModel;
            }
        }

        public Task<UserSettingsModel> FindUserSettingsAsync(int userId)
        {
            return Task.Run(() => FindUserSettings(userId));
        }

        public UserSettingsModel AddNewUserSettings(int userId, string access)
        {
            access.NullInspect(nameof(access));

            using (var db = new GmailBotDbContext())
            {
                var userSettings = db.UserSettings.Add(new UserSettingsModel { UserId = userId, Access = access });
                db.SaveChanges();
                return userSettings;
            }
        }

        public Task<UserSettingsModel> AddNewUserSettingsAsync(int userId, string access)
        {
            access.NullInspect(nameof(access));

            return Task.Run(() => AddNewUserSettings(userId, access));
        }

        public void RemoveUserSettingsRecord(UserSettingsModel model)
        {
            model.NullInspect(nameof(model));

            using (var db = new GmailBotDbContext())
            {
                var existModel = db.UserSettings
                                    .Where(userSettings => userSettings.Id == model.Id)
                                    .Include(userSettings => userSettings.IgnoreList)
                                    .SingleOrDefault();
                if (existModel == null)
                    return;

                db.UserSettings.Remove(existModel);
                db.SaveChanges();
            }
        }

        public Task RemoveUserSettingsRecordAsync(UserSettingsModel model)
        {
            model.NullInspect(nameof(model));
            return Task.Run(() => RemoveUserSettingsRecord(model));
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
                }
                return db.UserSettings.ToList();
            }
        }

        public Task<List<UserSettingsModel>> GetAllUsersSettingsAsync()
        {
            return Task.Run(() => GetAllUsersSettings());
        }

        public void UpdateUserSettingsRecord(UserSettingsModel newModel)
        {
            using (var db = new GmailBotDbContext())
            {
                var existModel = db.UserSettings
                                    .Where(userSettings => userSettings.Id == newModel.Id)
                                    .Include(nmStore => nmStore.IgnoreList)
                                    .SingleOrDefault();
                if (existModel == null)
                    return;
                // Update 
                db.Entry(existModel).CurrentValues.SetValues(newModel);
                UpdateIgnoreList(db, newModel.IgnoreList, existModel.IgnoreList);
                db.SaveChanges();
            }
        }

        public Task UpdateUserSettingsRecordAsync(UserSettingsModel userSettingsModel)
        {
            return Task.Run(() => UpdateUserSettingsRecord(userSettingsModel));
        }

        private void UpdateIgnoreList(DbContext dbContext, ICollection<IgnoreModel> newIgnoreListCollection,
           ICollection<IgnoreModel> existIgnoreListCollection)
        {
            foreach (var ignoreModel in newIgnoreListCollection)
            {
                var existIgnoreModel = existIgnoreListCollection
                .SingleOrDefault(i => i.Id == ignoreModel.Id);

                if (existIgnoreModel != null)
                    dbContext.Entry(existIgnoreModel).CurrentValues.SetValues(ignoreModel);
                else
                {
                    var newfile = new IgnoreModel
                    {
                        Address = ignoreModel.Address,
                        UserSettingsModel = ignoreModel.UserSettingsModel
                    };
                    existIgnoreListCollection.Add(newfile);
                }
            }
        }
    }
}