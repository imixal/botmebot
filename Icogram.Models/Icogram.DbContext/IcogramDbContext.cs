using System.Data.Entity;
using Icogram.Models.BotModels;
using Icogram.Models.CompanyModels;
using Icogram.Models.EmailModels;
using Icogram.Models.ModuleModels.CommandModule;
using Icogram.Models.ModuleModels.CustomMessageModule;
using Icogram.Models.ModuleModels.WelcomeMessageModule;
using Icogram.Models.ResourcesModels;
using Icogram.Models.UserModels;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Icogram.DbContext
{
    public class IcogramDbContext : IdentityDbContext<AplicationUser>
    {
        public DbSet<CustomerBot> CustomerBots { get; set; }

        public DbSet<IcogramBot> IcogramBots { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<CompanyChat> CompanyChats { get; set; }

        public DbSet<CompanyTarif> CompanyTarifs { get; set; }

        public DbSet<EmailMessage> EmailMessages { get; set; }

        public DbSet<EmailTemplate> EmailTemplates { get; set; }

        public DbSet<EmailVariable> EmailVariables { get; set; }

        public DbSet<Resource> Resources { get; set; }

        public DbSet<Command> Commands { get; set; }

        public DbSet<CommandType> CommandTypes { get; set; }

        public DbSet<CustomMessage> CustomMessages { get; set; }

        public DbSet<WelcomeMessage> WelcomeMessages { get; set; }


        public IcogramDbContext() : base("Connection")
        {
            
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CustomerBot>()
                .HasRequired(cb => cb.Company)
                .WithMany(c => c.TelegramBots)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<CustomerBot>()
                .HasOptional(cb => cb.WelcomeMessage)
                .WithRequired(wm => wm.Bot)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CompanyChat>()
                .HasOptional(cb => cb.Company)
                .WithMany(c => c.Chats)
                .WillCascadeOnDelete();

            modelBuilder.Entity<CompanyTarif>()
                .HasRequired(cb => cb.Company)
                .WithOptional(c => c.Tarif)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EmailMessage>()
                .HasOptional(em => em.Sender);
            modelBuilder.Entity<EmailMessage>()
                .HasRequired(em => em.Template);

            modelBuilder.Entity<EmailTemplate>()
                .HasMany(et => et.Variables);
            modelBuilder.Entity<EmailTemplate>()
                .HasRequired(et => et.Creator);

            modelBuilder.Entity<Command>()
                .HasRequired(c=>c.Bot)
                .WithMany(cb=>cb.Commands)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Command>()
                .HasRequired(c => c.Type);

            modelBuilder.Entity<CustomMessage>()
                .HasRequired(c => c.Bot);


        }
    }
}