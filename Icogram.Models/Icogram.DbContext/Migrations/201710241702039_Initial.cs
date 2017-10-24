namespace Icogram.DbContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Commands",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CommandName = c.String(),
                        CommandDescription = c.String(),
                        TypeId = c.Int(nullable: false),
                        BotId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomerBots", t => t.BotId)
                .ForeignKey("dbo.CommandTypes", t => t.TypeId, cascadeDelete: true)
                .Index(t => t.TypeId)
                .Index(t => t.BotId);
            
            CreateTable(
                "dbo.CustomerBots",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BotId = c.Int(nullable: false),
                        Token = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        UserName = c.String(),
                        CompanyId = c.Int(nullable: false),
                        WelcomeMessageId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Type = c.Int(nullable: false),
                        TarifId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CompanyChats",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ChatId = c.String(),
                        Type = c.String(),
                        Title = c.String(),
                        CompanyId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.CompanyTarifs",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        IsWelcomeMessageModuleActivated = c.Boolean(nullable: false),
                        IsCommandModuleActivated = c.Boolean(nullable: false),
                        IsNotificationModuleActivated = c.Boolean(nullable: false),
                        IsCustomMessageModuleActivated = c.Boolean(nullable: false),
                        Begin = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                        CompanyId = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        CompanyId = c.Int(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .Index(t => t.CompanyId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.WelcomeMessages",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Message = c.String(),
                        BotId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomerBots", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.CommandTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Rule = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CustomMessages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        BotId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomerBots", t => t.BotId, cascadeDelete: true)
                .Index(t => t.BotId);
            
            CreateTable(
                "dbo.EmailMessages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Text = c.String(),
                        From = c.String(),
                        To = c.String(),
                        SenderId = c.String(maxLength: 128),
                        TemplateId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.SenderId)
                .ForeignKey("dbo.EmailTemplates", t => t.TemplateId, cascadeDelete: true)
                .Index(t => t.SenderId)
                .Index(t => t.TemplateId);
            
            CreateTable(
                "dbo.EmailTemplates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TemplateName = c.String(),
                        TemaplateDescription = c.String(),
                        TemplateHtml = c.String(),
                        PreviewTitle = c.String(),
                        PreviewText = c.String(),
                        CreatorId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatorId, cascadeDelete: true)
                .Index(t => t.CreatorId);
            
            CreateTable(
                "dbo.EmailVariables",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Variable = c.String(),
                        Description = c.String(),
                        EmailTemplate_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EmailTemplates", t => t.EmailTemplate_Id)
                .Index(t => t.EmailTemplate_Id);
            
            CreateTable(
                "dbo.IcogramBots",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BotId = c.Int(nullable: false),
                        Token = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        UserName = c.String(),
                        IsPrivacyModeActivated = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Resources",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DefaultValue = c.String(),
                        EnglishValue = c.String(),
                        RussianValue = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.EmailMessages", "TemplateId", "dbo.EmailTemplates");
            DropForeignKey("dbo.EmailVariables", "EmailTemplate_Id", "dbo.EmailTemplates");
            DropForeignKey("dbo.EmailTemplates", "CreatorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.EmailMessages", "SenderId", "dbo.AspNetUsers");
            DropForeignKey("dbo.CustomMessages", "BotId", "dbo.CustomerBots");
            DropForeignKey("dbo.Commands", "TypeId", "dbo.CommandTypes");
            DropForeignKey("dbo.Commands", "BotId", "dbo.CustomerBots");
            DropForeignKey("dbo.WelcomeMessages", "Id", "dbo.CustomerBots");
            DropForeignKey("dbo.CustomerBots", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.CompanyTarifs", "Id", "dbo.Companies");
            DropForeignKey("dbo.CompanyChats", "CompanyId", "dbo.Companies");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.EmailVariables", new[] { "EmailTemplate_Id" });
            DropIndex("dbo.EmailTemplates", new[] { "CreatorId" });
            DropIndex("dbo.EmailMessages", new[] { "TemplateId" });
            DropIndex("dbo.EmailMessages", new[] { "SenderId" });
            DropIndex("dbo.CustomMessages", new[] { "BotId" });
            DropIndex("dbo.WelcomeMessages", new[] { "Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "CompanyId" });
            DropIndex("dbo.CompanyTarifs", new[] { "Id" });
            DropIndex("dbo.CompanyChats", new[] { "CompanyId" });
            DropIndex("dbo.CustomerBots", new[] { "CompanyId" });
            DropIndex("dbo.Commands", new[] { "BotId" });
            DropIndex("dbo.Commands", new[] { "TypeId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Resources");
            DropTable("dbo.IcogramBots");
            DropTable("dbo.EmailVariables");
            DropTable("dbo.EmailTemplates");
            DropTable("dbo.EmailMessages");
            DropTable("dbo.CustomMessages");
            DropTable("dbo.CommandTypes");
            DropTable("dbo.WelcomeMessages");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.CompanyTarifs");
            DropTable("dbo.CompanyChats");
            DropTable("dbo.Companies");
            DropTable("dbo.CustomerBots");
            DropTable("dbo.Commands");
        }
    }
}
