namespace Icogram.DbContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AntiSpamSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ChatId = c.Int(nullable: false),
                        IsModuleIncluded = c.Boolean(nullable: false),
                        IsInvertMode = c.Boolean(nullable: false),
                        IsNeededToBanUser = c.Boolean(nullable: false),
                        NumberOfAttempts = c.Int(nullable: false),
                        WarningMessage = c.String(),
                        CreationDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Chats", t => t.ChatId, cascadeDelete: true)
                .Index(t => t.ChatId);
            
            CreateTable(
                "dbo.Chats",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TelegramChatId = c.Long(nullable: false),
                        Type = c.String(),
                        Title = c.String(),
                        CompanyId = c.Int(),
                        IsApproved = c.Boolean(nullable: false),
                        WelcomeUserMessage = c.String(),
                        CommandTimeOut = c.Int(nullable: false),
                        CreationDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.Commands",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CommandName = c.String(),
                        ActionMessage = c.String(),
                        ChatId = c.Int(nullable: false),
                        IsCommandShowInList = c.Boolean(nullable: false),
                        LastUsage = c.DateTime(),
                        CreationDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Chats", t => t.ChatId, cascadeDelete: true)
                .Index(t => t.ChatId);
            
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Type = c.Int(nullable: false),
                        IsWelcomeMessageModuleActivated = c.Boolean(nullable: false),
                        IsCommandModuleActivated = c.Boolean(nullable: false),
                        IsAntiSpamModuleActivated = c.Boolean(nullable: false),
                        IsCustomMessageModuleActivated = c.Boolean(nullable: false),
                        End = c.DateTime(),
                        Price = c.Double(nullable: false),
                        CreationDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        TxTransaction = c.String(),
                        Comment = c.String(),
                        Email = c.String(),
                        PaymentDate = c.DateTime(nullable: false),
                        TelegramContact = c.String(),
                        ChatCount = c.Int(nullable: false),
                        IsAproved = c.Boolean(nullable: false),
                        PaymentTypeId = c.Int(nullable: false),
                        CreationDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .ForeignKey("dbo.PaymentTypes", t => t.PaymentTypeId, cascadeDelete: true)
                .Index(t => t.CompanyId)
                .Index(t => t.PaymentTypeId);
            
            CreateTable(
                "dbo.PaymentTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        NumberOfMonth = c.Int(nullable: false),
                        Eth = c.Double(nullable: false),
                        CreationDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        CompanyId = c.Int(),
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
                .ForeignKey("dbo.Companies", t => t.CompanyId)
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
                "dbo.ChatStatistics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ChatId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        NumberOfMessages = c.Int(nullable: false),
                        NumberOfDeletedMessages = c.Int(nullable: false),
                        NumberOfUsers = c.Int(nullable: false),
                        NumberOfBannedUsers = c.Int(nullable: false),
                        NumberOfCommands = c.Int(nullable: false),
                        CreationDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Chats", t => t.ChatId, cascadeDelete: true)
                .Index(t => t.ChatId);
            
            CreateTable(
                "dbo.CustomMessages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        ChatId = c.Int(nullable: false),
                        CreationDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Chats", t => t.ChatId, cascadeDelete: true)
                .Index(t => t.ChatId);
            
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
                        CreationDate = c.DateTime(),
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
                        CreationDate = c.DateTime(),
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
                        CreationDate = c.DateTime(),
                        EmailTemplate_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EmailTemplates", t => t.EmailTemplate_Id)
                .Index(t => t.EmailTemplate_Id);
            
            CreateTable(
                "dbo.Resources",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DefaultValue = c.String(),
                        EnglishValue = c.String(),
                        RussianValue = c.String(),
                        CreationDate = c.DateTime(),
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
            
            CreateTable(
                "dbo.SuspiciousUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ChatId = c.Int(nullable: false),
                        TelegramUserId = c.Int(nullable: false),
                        UserName = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        NumberOfAttempts = c.Int(nullable: false),
                        IsUserBanned = c.Boolean(nullable: false),
                        BannedDate = c.DateTime(),
                        CreationDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Chats", t => t.ChatId, cascadeDelete: true)
                .Index(t => t.ChatId);
            
            CreateTable(
                "dbo.WhiteLinks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ChatId = c.Int(nullable: false),
                        Link = c.String(),
                        CreationDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Chats", t => t.ChatId, cascadeDelete: true)
                .Index(t => t.ChatId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WhiteLinks", "ChatId", "dbo.Chats");
            DropForeignKey("dbo.SuspiciousUsers", "ChatId", "dbo.Chats");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.EmailMessages", "TemplateId", "dbo.EmailTemplates");
            DropForeignKey("dbo.EmailVariables", "EmailTemplate_Id", "dbo.EmailTemplates");
            DropForeignKey("dbo.EmailTemplates", "CreatorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.EmailMessages", "SenderId", "dbo.AspNetUsers");
            DropForeignKey("dbo.CustomMessages", "ChatId", "dbo.Chats");
            DropForeignKey("dbo.ChatStatistics", "ChatId", "dbo.Chats");
            DropForeignKey("dbo.AntiSpamSettings", "ChatId", "dbo.Chats");
            DropForeignKey("dbo.Chats", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Payments", "PaymentTypeId", "dbo.PaymentTypes");
            DropForeignKey("dbo.Payments", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.Commands", "ChatId", "dbo.Chats");
            DropIndex("dbo.WhiteLinks", new[] { "ChatId" });
            DropIndex("dbo.SuspiciousUsers", new[] { "ChatId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.EmailVariables", new[] { "EmailTemplate_Id" });
            DropIndex("dbo.EmailTemplates", new[] { "CreatorId" });
            DropIndex("dbo.EmailMessages", new[] { "TemplateId" });
            DropIndex("dbo.EmailMessages", new[] { "SenderId" });
            DropIndex("dbo.CustomMessages", new[] { "ChatId" });
            DropIndex("dbo.ChatStatistics", new[] { "ChatId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "CompanyId" });
            DropIndex("dbo.Payments", new[] { "PaymentTypeId" });
            DropIndex("dbo.Payments", new[] { "CompanyId" });
            DropIndex("dbo.Commands", new[] { "ChatId" });
            DropIndex("dbo.Chats", new[] { "CompanyId" });
            DropIndex("dbo.AntiSpamSettings", new[] { "ChatId" });
            DropTable("dbo.WhiteLinks");
            DropTable("dbo.SuspiciousUsers");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Resources");
            DropTable("dbo.EmailVariables");
            DropTable("dbo.EmailTemplates");
            DropTable("dbo.EmailMessages");
            DropTable("dbo.CustomMessages");
            DropTable("dbo.ChatStatistics");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.PaymentTypes");
            DropTable("dbo.Payments");
            DropTable("dbo.Companies");
            DropTable("dbo.Commands");
            DropTable("dbo.Chats");
            DropTable("dbo.AntiSpamSettings");
        }
    }
}
