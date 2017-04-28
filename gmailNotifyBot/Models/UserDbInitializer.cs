using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels;

namespace CoffeeJelly.gmailNotifyBot.Models
{
    public class UserDbInitializer : CreateDatabaseIfNotExists<GmailBotDbContext>
    {
        protected override void Seed(GmailBotDbContext db)
        {
            db.Users.Add(new UserModel
            {
                UserId = 1,
                FirstName = "testFirst",
                LastName = "testLast",
                Username = "testUsername"
            });
            db.PendingUser.Add(new PendingUserModel
            {
                UserId = 1,
                JoinTimeUtc = DateTime.UtcNow,
            });
            db.UserSettings.Add(new UserSettingsModel
            {
                MailNotification = false,
                UserId = 1,
                Access = UserAccess.Full
            });
            base.Seed(db);
        }
    }
}