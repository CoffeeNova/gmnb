using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.DataBaseModels;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram;

namespace CoffeeJelly.gmailNotifyBot.Bot
{
    public static class DbWorker
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
               return db.Users.Add(new UserModel(user));
            }
        }

        public static Task<UserModel> AddNewUserAsync(User user)
        {
           return Task.Run(() => AddNewUser(user));
        }
    }
}