namespace Icogram.DbContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Ban3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SuspiciousUsers", "BannedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SuspiciousUsers", "BannedDate", c => c.DateTime(nullable: false));
        }
    }
}
