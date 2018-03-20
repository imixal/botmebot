namespace Icogram.DbContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdminCommands : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Chats", "IsCommandForAdminOnly", c => c.Boolean(nullable: false));
            AddColumn("dbo.Chats", "IsNeedToDeleteUserCommands", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Chats", "IsNeedToDeleteUserCommands");
            DropColumn("dbo.Chats", "IsCommandForAdminOnly");
        }
    }
}
