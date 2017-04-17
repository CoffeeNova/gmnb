using System;
using System.Data.Entity;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram;

namespace CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels
{
    public class UserModel
    {
        public UserModel(Chat chat = null)
        {
            if (chat == null) return;
            Id = chat.Id;
            FirstName = chat.FirstName;
            LastName = chat.LastName;
            Username = chat.Username;
        }

        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }


        public string Username { get; set; }

    }

    public class PendingUserModel
    {
        public long Id { get; set; }

        public string State { get; set; }

        public DateTime JoinTime { get; set; }
    }

    public class UserContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; }

        public DbSet<PendingUserModel> PendingUser { get; set; }

    }

}