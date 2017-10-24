using System;
using System.Collections.Generic;
using DataAccessLayer.Async;
using Icogram.DataAccessLayer.EntityFramework.Identity;
using Icogram.DataAccessLayer.Interfaces;
using Icogram.DataAccessLayer.Repository;
using Icogram.DbContext;
using Icogram.Models.BotModels;
using Icogram.Models.CompanyModels;
using Icogram.Models.EmailModels;
using Icogram.Models.ModuleModels.CommandModule;
using Icogram.Models.ModuleModels.CustomMessageModule;
using Icogram.Models.ModuleModels.WelcomeMessageModule;
using Icogram.Models.ResourcesModels;
using Icogram.Models.UserModels;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Icogram.DataAccessLayer.UnitOfWork
{
    public class IcogramUnitOfWork : global::DataAccessLayer.Async.UnitOfWork, IIcogramUnitOfWork
    {
        private IDictionary<Type, Type> _entityTypeToRepositoryType;

        public ApplicationUserManager UserManager { get; }

        public ApplicationRoleManager RoleManager { get; }
        

        public IcogramUnitOfWork(IcogramDbContext dbContext) : base(dbContext)
        {
            UserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(dbContext));
            RoleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(dbContext));
            InitializeRepositoryMapping();
        }


        protected override IRepository<TEntity> CreateRepository<TEntity>()
        {
            Type repositoryType;
            var repository = _entityTypeToRepositoryType.TryGetValue(typeof(TEntity), out repositoryType)
                ? (Repository<TEntity>)Activator.CreateInstance(repositoryType, typeof(System.Data.Entity.DbContext))
                : base.CreateRepository<TEntity>();

            return repository;
        }


        private void InitializeRepositoryMapping()
        {
            _entityTypeToRepositoryType = new Dictionary<Type, Type>
            {
                { typeof(Resource), typeof(Repository<Resource>) },
                { typeof(CustomerBot), typeof(CustomerBotRepository) },
                { typeof(IcogramBot), typeof(Repository<IcogramBot>) },
                { typeof(Company), typeof(CompanyRepository) },
                { typeof(CompanyChat), typeof(CompanyChatRepository) },
                { typeof(CompanyTarif), typeof(CompanyTarifRepository) },
                { typeof(EmailMessage), typeof(EmailMessageRepository) },
                { typeof(EmailTemplate), typeof(EmailTemplateRepository) },
                { typeof(EmailVariable), typeof(Repository<EmailVariable>) },
                { typeof(Command), typeof(CommandRepository) },
                { typeof(CommandType), typeof(Repository<CommandType>) },
                { typeof(CustomMessage), typeof(CustomMessageRepository) },
                { typeof(WelcomeMessage), typeof(WelcomeMessageRepository) },
                { typeof(Resource), typeof(Repository<Resource>) }
            };
        }
    }
}