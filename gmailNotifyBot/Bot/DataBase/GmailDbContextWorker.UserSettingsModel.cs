using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
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
                var entry = db.Entry(model);
                if (entry.State == EntityState.Detached)
                    db.UserSettings.Attach(model);
                db.UserSettings.Remove(model);
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
                return db.UserSettings.ToList();
            }
        }

        public Task<List<UserSettingsModel>> GetAllUsersSettingsAsync()
        {
            return Task.Run(() => GetAllUsersSettings());
        }

        public void UpdateUserSettingsRecord(UserSettingsModel userSettingsModel)
        {
            using (var db = new GmailBotDbContext())
            {
                db.UserSettings.Attach(userSettingsModel);
                db.Entry(userSettingsModel).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public Task UpdateUserSettingsRecordAsync(UserSettingsModel userSettingsModel)
        {
            return Task.Run(() => UpdateUserSettingsRecord(userSettingsModel));
        }
    }
}