namespace Icogram.DbContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Stat : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChatStatistics", "NumberOfNewUsers", c => c.Int(nullable: false));
            AddColumn("dbo.ChatStatistics", "NumberOfLeavedUsers", c => c.Int(nullable: false));
            AddColumn("dbo.ChatStatistics", "NumberOfSymbolsInMessage", c => c.Long(nullable: false));
            DropColumn("dbo.ChatStatistics", "NumberOfUsers");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ChatStatistics", "NumberOfUsers", c => c.Int(nullable: false));
            DropColumn("dbo.ChatStatistics", "NumberOfSymbolsInMessage");
            DropColumn("dbo.ChatStatistics", "NumberOfLeavedUsers");
            DropColumn("dbo.ChatStatistics", "NumberOfNewUsers");
        }
    }
}
