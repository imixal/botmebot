namespace Icogram.DbContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FileandCommand : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        ContentType = c.String(),
                        Content = c.Binary(),
                        Type = c.Int(nullable: false),
                        ChatId = c.Int(nullable: false),
                        CreationDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Chats", t => t.ChatId, cascadeDelete: true)
                .Index(t => t.ChatId);
            
            AddColumn("dbo.Commands", "CommandParams", c => c.String());
            AddColumn("dbo.Commands", "Type", c => c.Int(nullable: false));
            AddColumn("dbo.Commands", "FileId", c => c.Int());
            CreateIndex("dbo.Commands", "FileId");
            AddForeignKey("dbo.Commands", "FileId", "dbo.Files", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Commands", "FileId", "dbo.Files");
            DropForeignKey("dbo.Files", "ChatId", "dbo.Chats");
            DropIndex("dbo.Files", new[] { "ChatId" });
            DropIndex("dbo.Commands", new[] { "FileId" });
            DropColumn("dbo.Commands", "FileId");
            DropColumn("dbo.Commands", "Type");
            DropColumn("dbo.Commands", "CommandParams");
            DropTable("dbo.Files");
        }
    }
}
