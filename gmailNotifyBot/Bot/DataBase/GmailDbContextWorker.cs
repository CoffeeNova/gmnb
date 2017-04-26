using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram;


namespace CoffeeJelly.gmailNotifyBot.Bot.DataBase
{
    public class GmailDbContextWorker
    {
        public UserModel FindUser(long userId)
        {
            userId.NullInspect(nameof(userId));

            using (var db = new GmailBotDbContext())
            {
                return db.Users.FirstOrDefault(u => u.UserId == userId);
            }
        }

        public Task<UserModel> FindUserAsync(long userId)
        {
            userId.NullInspect(nameof(userId));

            return Task.Run(() => FindUser(userId));
        }

        public UserModel AddNewUser(Chat user)
        {
            user.NullInspect(nameof(user));

            using (var db = new GmailBotDbContext())
            {
                var newModel = db.Users.Add(new UserModel(user));
                db.SaveChanges();
                return newModel;
            }
        }

        public Task<UserModel> AddNewUserAsync(Chat user)
        {
            user.NullInspect(nameof(user));

            return Task.Run(() => AddNewUser(user));
        }

        public void UpdateUserRecord(UserModel userModel)
        {
            using (var db = new GmailBotDbContext())
            {
                db.Users.Attach(userModel);
                db.Entry(userModel).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public Task UpdateUserRecordAsync(UserModel userModel)
        {
            return Task.Run(() => UpdateUserRecord(userModel));
        }

        public PendingUserModel Queue(long userId)
        {
            using (var db = new GmailBotDbContext())
            {
                var newModel = db.PendingUser.Add(new PendingUserModel
                {
                    UserId = userId,
                    JoinTimeUtc = DateTime.Now
                });
                db.SaveChanges();
                return newModel;
            }
        }

        public Task<PendingUserModel> QueueAsync(long userId)
        {
            return Task.Run(() => Queue(userId));
        }

        public void RemoveFromQueue(PendingUserModel model)
        {
            model.NullInspect(nameof(model));

            using (var db = new GmailBotDbContext())
            {
                var entry = db.Entry(model);
                if (entry.State == EntityState.Detached)
                    db.PendingUser.Attach(model);
                db.PendingUser.Remove(model);
                db.SaveChanges();
            }
        }

        public Task RemoveFromQueueAsync(PendingUserModel model)
        {
            model.NullInspect(nameof(model));

            return Task.Run(() => RemoveFromQueue(model));
        }

        public PendingUserModel UpdateRecordJoinTime(long id, DateTime time)
        {
            using (var db = new GmailBotDbContext())
            {
                var query = db.PendingUser.Find(id);
                if (query != null)
                {
                    query.JoinTimeUtc = time;
                    db.SaveChanges();
                }
                return query;
            }
        }

        public Task<PendingUserModel> UpdateRecordJoinTimeAsync(long id, DateTime time)
        {
            return Task.Run(() => UpdateRecordJoinTime(id, time));
        }

        public PendingUserModel FindPendingUser(long userId)
        {
            using (var db = new GmailBotDbContext())
            {
                return db.PendingUser.FirstOrDefault(p => p.UserId == userId);
            }
        }

        public Task<PendingUserModel> FindPendingUserAsync(long userId)
        {
            return Task.Run(() => FindPendingUser(userId));
        }

        public UserSettingsModel FindUserSettings(long userId)
        {
            userId.NullInspect(nameof(userId));

            using (var db = new GmailBotDbContext())
            {
                return db.UserSettings.FirstOrDefault(u => u.UserId == userId);
            }
        }

        public Task<UserSettingsModel> FindUserSettingsrAsync(long userId)
        {
            userId.NullInspect(nameof(userId));

            return Task.Run(() => FindUserSettings(userId));
        }
    }
}