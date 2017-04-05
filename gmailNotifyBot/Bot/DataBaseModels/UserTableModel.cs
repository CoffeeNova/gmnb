using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram;

namespace CoffeeJelly.gmailNotifyBot.Bot.DataBaseModels
{
    public class UserModel
    {
        public UserModel(User user = null)
        {
            if (user == null) return;
            UserId = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Username = user.Username;
        }

        public int UserId { get; set; }

        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        
        public string Username { get; set; }

    }

    public class UserContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; }

    }

}