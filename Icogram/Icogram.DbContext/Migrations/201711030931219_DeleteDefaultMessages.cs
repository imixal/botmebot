namespace Icogram.DbContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteDefaultMessages : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Chats", "IsNeededToDeleteNewUserMessage", c => c.Boolean(nullable: false));
            AddColumn("dbo.Chats", "IsNeededToDeleteLeaveUserMessage", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Chats", "IsNeededToDeleteLeaveUserMessage");
            DropColumn("dbo.Chats", "IsNeededToDeleteNewUserMessage");
        }
    }
}
