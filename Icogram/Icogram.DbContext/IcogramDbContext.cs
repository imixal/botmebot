using System.Data.Entity;
using Icogram.Models.ChatModels;
using Icogram.Models.CompanyModels;
using Icogram.Models.EmailModels;
using Icogram.Models.FileModel;
using Icogram.Models.ModuleModels.AntiSpamModule;
using Icogram.Models.ModuleModels.CommandModule;
using Icogram.Models.ModuleModels.CustomMessageModule;
using Icogram.Models.ModuleModels.StatisticsModule;
using Icogram.Models.Payments;
using Icogram.Models.ResourcesModels;
using Icogram.Models.UserModels;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Icogram.DbContext
{
    public class IcogramDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Company> Companies { get; set; }

        public DbSet<EmailMessage> EmailMessages { get; set; }

        public DbSet<EmailTemplate> EmailTemplates { get; set; }

        public DbSet<EmailVariable> EmailVariables { get; set; }

        public DbSet<Resource> Resources { get; set; }

        public DbSet<Command> Commands { get; set; }

        public DbSet<CustomMessage> CustomMessages { get; set; }

        public DbSet<AntiSpamSetting> AntiSpamSettings { get; set; }

        public DbSet<SuspiciousUser> SuspiciousUsers { get; set; }

        public DbSet<WhiteLink> WhiteLinks { get; set; }

        public DbSet<Chat> Chats { get; set; }

        public DbSet<ChatStatistic> ChatStatistics { get; set; }

        public DbSet<PaymentType> PaymentTypes { get; set; }

        public DbSet<Payment> Payments { get; set; }

        public DbSet<File> Files { get; set; }

        public IcogramDbContext() : base("Connection")
        {
            
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Chat>()
                .HasOptional(cb => cb.Company)
                .WithMany(c => c.Chats)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<EmailMessage>()
                .HasOptional(em => em.Sender);
            modelBuilder.Entity<EmailMessage>()
                .HasRequired(em => em.Template);

            modelBuilder.Entity<EmailTemplate>()
                .HasMany(et => et.Variables);
            modelBuilder.Entity<EmailTemplate>()
                .HasRequired(et => et.Creator);

            modelBuilder.Entity<AntiSpamSetting>()
                .HasRequired(ass => ass.Chat)
                .WithMany()
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<SuspiciousUser>()
                .HasRequired(su=>su.Chat)
                .WithMany()
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<WhiteLink>()
                .HasRequired(wl => wl.Chat)
                .WithMany()
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<CustomMessage>()
                .HasRequired(cm => cm.Chat);

            modelBuilder.Entity<ChatStatistic>()
                .HasRequired(cs => cs.Chat);

            modelBuilder.Entity<ApplicationUser>()
                .HasOptional(au => au.Company);

            modelBuilder.Entity<Command>()
                .HasRequired(c => c.Chat)
                .WithMany(chat => chat.Commands)
                .WillCascadeOnDelete(true);
            modelBuilder.Entity<Command>()
                .HasOptional(c => c.File);

            modelBuilder.Entity<Payment>()
                .HasRequired(p => p.Company)
                .WithMany(c => c.Payments)
                .WillCascadeOnDelete(true);
            modelBuilder.Entity<Payment>()
                .HasRequired(p => p.PaymentType)
                .WithMany()
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<File>()
                .HasRequired(f => f.Chat);
        }
    }
}