namespace CoffeeJelly.gmailNotifyBot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BccModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NmStoreModelId = c.Int(nullable: false),
                        Email = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => new { t.Id, t.NmStoreModelId })
                .ForeignKey("dbo.NmStoreModels", t => t.NmStoreModelId, cascadeDelete: true)
                .Index(t => t.NmStoreModelId);
            
            CreateTable(
                "dbo.NmStoreModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        MessageId = c.Int(nullable: false),
                        Subject = c.String(),
                        Message = c.String(),
                        DraftId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CcModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NmStoreModelId = c.Int(nullable: false),
                        Email = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => new { t.Id, t.NmStoreModelId })
                .ForeignKey("dbo.NmStoreModels", t => t.NmStoreModelId, cascadeDelete: true)
                .Index(t => t.NmStoreModelId);
            
            CreateTable(
                "dbo.FileModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NmStoreModelId = c.Int(nullable: false),
                        FileId = c.String(),
                        AttachId = c.String(),
                        OriginalName = c.String(),
                    })
                .PrimaryKey(t => new { t.Id, t.NmStoreModelId })
                .ForeignKey("dbo.NmStoreModels", t => t.NmStoreModelId, cascadeDelete: true)
                .Index(t => t.NmStoreModelId);
            
            CreateTable(
                "dbo.ToModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NmStoreModelId = c.Int(nullable: false),
                        Email = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => new { t.Id, t.NmStoreModelId })
                .ForeignKey("dbo.NmStoreModels", t => t.NmStoreModelId, cascadeDelete: true)
                .Index(t => t.NmStoreModelId);
            
            CreateTable(
                "dbo.BlacklistModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserSettingsModelId = c.Int(nullable: false),
                        Name = c.String(),
                        LabelId = c.String(),
                    })
                .PrimaryKey(t => new { t.Id, t.UserSettingsModelId })
                .ForeignKey("dbo.UserSettingsModels", t => t.UserSettingsModelId, cascadeDelete: true)
                .Index(t => t.UserSettingsModelId);
            
            CreateTable(
                "dbo.UserSettingsModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        MailNotification = c.Boolean(nullable: false),
                        HistoryId = c.Long(nullable: false),
                        Expiration = c.Long(nullable: false),
                        Access = c.String(),
                        UseWhitelist = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IgnoreModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserSettingsModelId = c.Int(nullable: false),
                        Address = c.String(),
                    })
                .PrimaryKey(t => new { t.Id, t.UserSettingsModelId })
                .ForeignKey("dbo.UserSettingsModels", t => t.UserSettingsModelId, cascadeDelete: true)
                .Index(t => t.UserSettingsModelId);
            
            CreateTable(
                "dbo.WhitelistModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserSettingsModelId = c.Int(nullable: false),
                        Name = c.String(),
                        LabelId = c.String(),
                    })
                .PrimaryKey(t => new { t.Id, t.UserSettingsModelId })
                .ForeignKey("dbo.UserSettingsModels", t => t.UserSettingsModelId, cascadeDelete: true)
                .Index(t => t.UserSettingsModelId);
            
            CreateTable(
                "dbo.LogEntryModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CallSite = c.String(),
                        Date = c.String(),
                        Exception = c.String(),
                        Level = c.String(),
                        Logger = c.String(),
                        MachineName = c.String(),
                        Message = c.String(),
                        StackTrace = c.String(),
                        Thread = c.String(),
                        Username = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PendingUserModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        JoinTimeUtc = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TempDataModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        LabelId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        GoogleUserId = c.String(),
                        Email = c.String(),
                        EmailVerified = c.Boolean(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Username = c.String(),
                        IdToken = c.String(),
                        AccessToken = c.String(),
                        IssuedTimeUtc = c.DateTime(nullable: false),
                        ExpiresIn = c.Int(nullable: false),
                        TokenType = c.String(),
                        RefreshToken = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WhitelistModels", "UserSettingsModelId", "dbo.UserSettingsModels");
            DropForeignKey("dbo.IgnoreModels", "UserSettingsModelId", "dbo.UserSettingsModels");
            DropForeignKey("dbo.BlacklistModels", "UserSettingsModelId", "dbo.UserSettingsModels");
            DropForeignKey("dbo.ToModels", "NmStoreModelId", "dbo.NmStoreModels");
            DropForeignKey("dbo.FileModels", "NmStoreModelId", "dbo.NmStoreModels");
            DropForeignKey("dbo.CcModels", "NmStoreModelId", "dbo.NmStoreModels");
            DropForeignKey("dbo.BccModels", "NmStoreModelId", "dbo.NmStoreModels");
            DropIndex("dbo.WhitelistModels", new[] { "UserSettingsModelId" });
            DropIndex("dbo.IgnoreModels", new[] { "UserSettingsModelId" });
            DropIndex("dbo.BlacklistModels", new[] { "UserSettingsModelId" });
            DropIndex("dbo.ToModels", new[] { "NmStoreModelId" });
            DropIndex("dbo.FileModels", new[] { "NmStoreModelId" });
            DropIndex("dbo.CcModels", new[] { "NmStoreModelId" });
            DropIndex("dbo.BccModels", new[] { "NmStoreModelId" });
            DropTable("dbo.UserModels");
            DropTable("dbo.TempDataModels");
            DropTable("dbo.PendingUserModels");
            DropTable("dbo.LogEntryModels");
            DropTable("dbo.WhitelistModels");
            DropTable("dbo.IgnoreModels");
            DropTable("dbo.UserSettingsModels");
            DropTable("dbo.BlacklistModels");
            DropTable("dbo.ToModels");
            DropTable("dbo.FileModels");
            DropTable("dbo.CcModels");
            DropTable("dbo.NmStoreModels");
            DropTable("dbo.BccModels");
        }
    }
}
