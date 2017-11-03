namespace Icogram.DbContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommandDescription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Commands", "CommandDescription", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Commands", "CommandDescription");
        }
    }
}
