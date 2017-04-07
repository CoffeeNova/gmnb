using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram;

namespace CoffeeJelly.gmailNotifyBot.Bot.DataBase
{
    public static class UserContextWorker
    {
        public static UserModel FindUser(User user)
        {
            using (var db = new UserContext())
            {
                return db.Users.Find(user.Id);
            }
        }

        public static async Task<UserModel> FindUserAsync(User user)
        {
            using (var db = new UserContext())
            {
                return await db.Users.FindAsync(user.Id);
            }
        }

        public static UserModel AddNewUser(User user)
        {
            using (var db = new UserContext())
            {
                var newModel = db.Users.Add(new UserModel(user));
                db.SaveChanges();
                return newModel;
            }
        }

        public static Task<UserModel> AddNewUserAsync(User user)
        {
            return Task.Run(() => AddNewUser(user));
        }

        public static PendingUserModel Queue(int userId, string state)
        {
            using (var db = new UserContext())
            {
                var newModel = db.PendingUser.Add(new PendingUserModel
                {
                    Id = userId,
                    State = state,
                    JoinTime = DateTime.Now
                });
                db.SaveChanges();
                return newModel;
            }
        }

        public static Task<PendingUserModel> QueueAsync(int userId, string state)
        {
            return Task.Run(() => Queue(userId, state));
        }

        public static void RemoveFromQueue(PendingUserModel model)
        {
            using (var db = new UserContext())
            {
                db.PendingUser.Remove(model);
                db.SaveChanges();
            }
        }

        public static void RemoveFromQueueAsync(PendingUserModel model)
        {
            Task.Run(() => RemoveFromQueue(model));
        }
    }
}