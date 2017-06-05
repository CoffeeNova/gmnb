using System.Data.Entity;

namespace CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels
{
    public class GmailBotDbContext : DbContext
    {
        public GmailBotDbContext()
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<UserModel> Users { get; set; }

        public DbSet<PendingUserModel> PendingUser { get; set; }

        public DbSet<UserSettingsModel> UserSettings { get; set; }

        public DbSet<NmStoreModel> NmStore { get; set; }

        public DbSet<FileModel> File { get; set; }

        public DbSet<ToModel> To { get; set; }

        public DbSet<CcModel> Cc { get; set; }

        public DbSet<BccModel> Bcc { get; set; }

        public DbSet<IgnoreModel> Ignore { get; set; }

    }

}