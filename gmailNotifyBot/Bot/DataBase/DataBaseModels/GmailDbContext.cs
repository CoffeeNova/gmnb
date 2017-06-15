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

        public DbSet<BlacklistModel> Blacklist { get; set; }

        public DbSet<WhitelistModel> Whitelist { get; set; }

        public DbSet<LogEntryModel> LogEntry { get; set; }

        public DbSet<TempDataModel> TempData { get; set; }

    }

}