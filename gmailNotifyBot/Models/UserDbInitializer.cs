using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels;

namespace CoffeeJelly.gmailNotifyBot.Models
{
    public class UserDbInitializer : DropCreateDatabaseAlways<UserContext>
    {
        protected override void Seed(UserContext db)
        {

            db.Users.Add(new UserModel
            {
                UserId = 1,
                FirstName = "testFirst",
                LastName = "testLast",
                Username = "testUsername",
                ReceivingTime = DateTime.Now
                
            });
            db.PendingUser.Add(new PendingUserModel
            {
                UserId = 1,
                JoinTime = DateTime.Now,
                State = "testState"
            });
            base.Seed(db);
        }
    }
}