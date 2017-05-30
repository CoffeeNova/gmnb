using System.Data.Entity;

namespace CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels
{
    public class GmailBotDbContext : DbContext
    {

        public DbSet<UserModel> Users { get; set; }

        public DbSet<PendingUserModel> PendingUser { get; set; }

        public DbSet<UserSettingsModel> UserSettings { get; set; }

        public DbSet<NmStoreModel> NmStore { get; set; }

    }

}