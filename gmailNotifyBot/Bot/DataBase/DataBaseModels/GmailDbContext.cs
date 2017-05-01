using System;
using System.Data.Entity;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram;
using CoffeeJelly.gmailNotifyBot.Models;
using Newtonsoft.Json;

namespace CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels
{
    public class GmailBotDbContext : DbContext
    {

        public DbSet<UserModel> Users { get; set; }

        public DbSet<PendingUserModel> PendingUser { get; set; }

        public DbSet<UserSettingsModel> UserSettings { get; set; }

    }

}