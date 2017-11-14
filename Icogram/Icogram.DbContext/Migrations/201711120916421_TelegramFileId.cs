namespace Icogram.DbContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TelegramFileId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Commands", "TelegramFileId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Commands", "TelegramFileId");
        }
    }
}
