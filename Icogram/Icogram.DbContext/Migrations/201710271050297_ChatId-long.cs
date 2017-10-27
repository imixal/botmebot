namespace Icogram.DbContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChatIdlong : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Chats", "TelegramChatId", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Chats", "TelegramChatId", c => c.String());
        }
    }
}
