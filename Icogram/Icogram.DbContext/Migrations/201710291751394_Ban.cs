namespace Icogram.DbContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Ban : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SuspiciousUsers", "UserName", c => c.String());
            AddColumn("dbo.SuspiciousUsers", "FirstName", c => c.String());
            AddColumn("dbo.SuspiciousUsers", "LastName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SuspiciousUsers", "LastName");
            DropColumn("dbo.SuspiciousUsers", "FirstName");
            DropColumn("dbo.SuspiciousUsers", "UserName");
        }
    }
}
