using System;
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

        public async Task<UserModel> FindUserAsync(int userId)
        {
            using (var db = new GmailBotDbContext())
            {
                return await db.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            }
        }

        public UserModel FindUserByEmail(string email)
        {
            using (var db = new GmailBotDbContext())
            {
                return db.Users.FirstOrDefault(u => u.Email == email);
            }
        }

        public async Task<UserModel> FindUserByEmailAsync(string email)
        {
            using (var db = new GmailBotDbContext())
            {
                return await db.Users.FirstOrDefaultAsync(u => u.Email == email);
            }
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

        public async Task<UserModel> AddNewUserAsync(User user)
        {
            user.NullInspect(nameof(user));

            using (var db = new GmailBotDbContext())
            {
                var newModel = db.Users.Add(new UserModel(user));
                await db.SaveChangesAsync();
                return newModel;
            }
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

        public async Task UpdateUserRecordAsync(UserModel userModel)
        {
            using (var db = new GmailBotDbContext())
            {
                db.Users.Attach(userModel);
                db.Entry(userModel).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
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

        public async Task RemoveUserRecordAsync(UserModel model)
        {
            model.NullInspect(nameof(model));

            using (var db = new GmailBotDbContext())
            {
                var entry = db.Entry(model);
                if (entry.State == EntityState.Detached)
                    db.Users.Attach(model);
                db.Users.Remove(model);
                await db.SaveChangesAsync();
            }
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

        public async Task<PendingUserModel> QueueAsync(int userId)
        {
            using (var db = new GmailBotDbContext())
            {
                var newModel = db.PendingUser.Add(new PendingUserModel
                {
                    UserId = userId,
                    JoinTimeUtc = DateTime.Now
                });
                await db.SaveChangesAsync();
                return newModel;
            }
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

        public async Task RemoveFromQueueAsync(PendingUserModel model)
        {
            model.NullInspect(nameof(model));

            using (var db = new GmailBotDbContext())
            {
                var entry = db.Entry(model);
                if (entry.State == EntityState.Detached)
                    db.PendingUser.Attach(model);
                db.PendingUser.Remove(model);
                await db.SaveChangesAsync();
            }
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

        public async Task<PendingUserModel> UpdateRecordJoinTimeAsync(int id, DateTime time)
        {
            using (var db = new GmailBotDbContext())
            {
                var query = await db.PendingUser.FindAsync(id);
                if (query != null)
                {
                    query.JoinTimeUtc = time;
                    await db.SaveChangesAsync();
                }
                return query;
            }
        }

        public PendingUserModel FindPendingUser(int userId)
        {
            using (var db = new GmailBotDbContext())
            {
                return db.PendingUser.FirstOrDefault(p => p.UserId == userId);
            }
        }

        public async Task<PendingUserModel> FindPendingUserAsync(int userId)
        {
            using (var db = new GmailBotDbContext())
            {
                return await db.PendingUser.FirstOrDefaultAsync(p => p.UserId == userId);
            }
        }

        public List<UserModel> GetAllUsers()
        {
            using (var db = new GmailBotDbContext())
            {
                return db.Users.ToList();
            }
        }

        public async Task<List<UserModel>> GetAllUsersAsync()
        {
            using (var db = new GmailBotDbContext())
            {
                return await db.Users.ToListAsync();
            }
        }
    }
}