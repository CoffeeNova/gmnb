﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;


namespace CoffeeJelly.gmailNotifyBot.Bot.DataBase
{
    public partial class GmailDbContextWorker
    {
        public UserModel FindUser(int userId)
        {
            using (var db = new GmailBotDbContext())
            {
                return db.Users.FirstOrDefault(u => u.UserId == userId);
            }
        }

        public Task<UserModel> FindUserAsync(int userId)
        {
            return Task.Run(() => FindUser(userId));
        }

        public UserModel FindUserByEmail(string email)
        {
            using (var db = new GmailBotDbContext())
            {
                return db.Users.FirstOrDefault(u => u.Email == email);
            }
        }

        public Task<UserModel> FindUserByEmailAsync(string email)
        {
            return Task.Run(() => FindUserByEmail(email));
        }

        public UserModel AddNewUser(User user)
        {
            user.NullInspect(nameof(user));

            using (var db = new GmailBotDbContext())
            {
                var newModel = db.Users.Add(new UserModel(user));
                db.SaveChanges();
                return newModel;
            }
        }

        public Task<UserModel> AddNewUserAsync(User user)
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

        public void RemoveUserRecord(UserModel model)
        {
            model.NullInspect(nameof(model));

            using (var db = new GmailBotDbContext())
            {
                var entry = db.Entry(model);
                if (entry.State == EntityState.Detached)
                    db.Users.Attach(model);
                db.Users.Remove(model);
                db.SaveChanges();
            }
        }

        public Task RemoveUserRecordAsync(UserModel model)
        {
            model.NullInspect(nameof(model));
            return Task.Run(() => RemoveUserRecord(model));
        }

        public PendingUserModel Queue(int userId)
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

        public Task<PendingUserModel> QueueAsync(int userId)
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

        public PendingUserModel UpdateRecordJoinTime(int id, DateTime time)
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

        public Task<PendingUserModel> UpdateRecordJoinTimeAsync(int id, DateTime time)
        {
            return Task.Run(() => UpdateRecordJoinTime(id, time));
        }

        public PendingUserModel FindPendingUser(int userId)
        {
            using (var db = new GmailBotDbContext())
            {
                return db.PendingUser.FirstOrDefault(p => p.UserId == userId);
            }
        }

        public Task<PendingUserModel> FindPendingUserAsync(int userId)
        {
            return Task.Run(() => FindPendingUser(userId));
        }


        public UserSettingsModel FindUserSettings(int userId)
        {
            using (var db = new GmailBotDbContext())
            {
                return db.UserSettings.FirstOrDefault(u => u.UserId == userId);
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

        public List<UserModel> GetAllUsers()
        {
            using (var db = new GmailBotDbContext())
            {
                return db.Users.ToList();
            }
        }

        public Task<List<UserModel>> GetAllUsersAsync()
        {
            return Task.Run(() => GetAllUsers());
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

        public void AddToIgnoreList(int userId, string address) //marked
        {
            address.NullInspect(nameof(address));

            using (var db = new GmailBotDbContext())
            {
                var query = db.UserSettings.FirstOrDefault(a => a.UserId == userId);
                if (query == null) return;

                query.IgnoreList.Add(new IgnoreModel {Address = address});
                db.SaveChanges();
            }
        }

        public async Task AddToIgnoreListAsync(int userId, string address)
        {
            address.NullInspect(nameof(address));

            using (var db = new GmailBotDbContext())
            {
                var query = await db.UserSettings.FirstOrDefaultAsync(a => a.UserId == userId);
                if (query == null) return;

                query.IgnoreList.Add(new IgnoreModel { Address = address});
                await db.SaveChangesAsync();
            }
        }

        public Task AddToIgnoreListAsyncTest(int userId, string address)
        {
            return Task.Run(() => AddToIgnoreList(userId, address));
        }

        public void RemoveFromIgnoreList(int userId, string address)
        {
            address.NullInspect(nameof(address));

            using (var db = new GmailBotDbContext())
            {
                var query = db.UserSettings.FirstOrDefault(a => a.UserId == userId);
                var ignoreModel = query?.IgnoreList.FirstOrDefault(i => i.Address == address);
                if (ignoreModel == null)
                    return;

                var removed = query.IgnoreList.Remove(ignoreModel);
                if (removed)
                    db.SaveChanges();
            }
        }

        public async Task RemoveFromIgnoreListAsync(int userId, string address)
        {
            address.NullInspect(nameof(address));

            using (var db = new GmailBotDbContext())
            {
                var query = await db.UserSettings.FirstOrDefaultAsync(a => a.UserId == userId);
                var ignoreModel = query?.IgnoreList.FirstOrDefault(i => i.Address == address);
                if (ignoreModel == null)
                    return;
                var removed = query.IgnoreList.Remove(ignoreModel);
                if (removed)
                    await db.SaveChangesAsync();
            }
        }

        public bool IsPresentInIgnoreList(int userId, string address)
        {
            address.NullInspect(nameof(address));

            using (var db = new GmailBotDbContext())
            {
                var query = db.UserSettings.FirstOrDefault(a => a.UserId == userId);

                var ignoreModel = query?.IgnoreList.FirstOrDefault(i => i.Address == address);
                if (ignoreModel == null)
                    return false;
                return query.IgnoreList.Any(a => a == ignoreModel);
            }
        }

        public async Task<bool> IsPresentInIgnoreListAsync(int userId, string address)
        {
            address.NullInspect(nameof(address));

            using (var db = new GmailBotDbContext())
            {
                var query = await db.UserSettings.FirstOrDefaultAsync(a => a.UserId == userId);
                var ignoreModel = query?.IgnoreList.FirstOrDefault(i => i.Address == address);
                if (ignoreModel == null)
                    return false;
                return query.IgnoreList.Any(a => a == ignoreModel);
            }
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