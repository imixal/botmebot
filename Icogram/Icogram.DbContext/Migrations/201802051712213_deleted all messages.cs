namespace Icogram.DbContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deletedallmessages : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Chats", "IsNeededToDeleteAllMessages", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Chats", "IsNeededToDeleteAllMessages");
        }
    }
}
