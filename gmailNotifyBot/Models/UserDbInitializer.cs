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
                Username = "testUsername",
                IssuedTimeUtc = DateTime.UtcNow,
                Email = "test@gmail.com",
                AccessToken = "testaccess",
                EmailVerified = true,
                ExpiresIn =100,
                GoogleUserId = "1",
                IdToken = "1",
                RefreshToken ="testrefresh",
                TokenType = "test"
            });
            db.PendingUser.Add(new PendingUserModel
            {
                UserId = 1,
                JoinTimeUtc = DateTime.UtcNow
            });
            db.UserSettings.Add(new UserSettingsModel
            {
                MailNotification = false,
                UserId = 1,
                Access = UserAccess.Full,
                IgnoreList = new List<string>
                {
                 "testadr1@gmail.com",   
                 "testadr2@gmail.com"
                },
                Expiration = 100,
                HistoryId = 1
            });
            base.Seed(db);
        }
    }
}