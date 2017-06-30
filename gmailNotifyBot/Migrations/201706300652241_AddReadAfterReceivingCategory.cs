namespace CoffeeJelly.gmailNotifyBot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReadAfterReceivingCategory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserSettingsModels", "ReadAfterReceiving", c => c.Boolean(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserSettingsModels", "ReadAfterReceiving");
        }
    }
}
