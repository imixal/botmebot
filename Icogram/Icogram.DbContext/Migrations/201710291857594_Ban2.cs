namespace Icogram.DbContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Ban2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SuspiciousUsers", "TelegramUserId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SuspiciousUsers", "TelegramUserId", c => c.String());
        }
    }
}
