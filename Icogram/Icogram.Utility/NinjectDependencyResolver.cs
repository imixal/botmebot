using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using Icogram.DbContext;
using Icogram.Models.UserModels;
using Microsoft.AspNet.Identity.EntityFramework;
using Ninject;
using Ninject.Web.Common;
using DataAccessLayer.Async;
using Icogram.AutoMapper;
using Icogram.DataAccessLayer.Interfaces;
using Icogram.DataAccessLayer.UnitOfWork;
using Icogram.Service.Login;
using Icogram.Service.User;
using Icogram.ViewModelBuilder;
using Service;

namespace Icogram.Utility
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private static IKernel _kernel;


        public NinjectDependencyResolver(IKernel kernelParam)
        {
            _kernel = kernelParam;
            AddBindings();
        }


        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }

        private static void AddBindings()
        {
            _kernel.Bind<IdentityDbContext<ApplicationUser>>().To<IcogramDbContext>().InRequestScope();
            _kernel.Bind<IUnitOfWork>().To<IcogramUnitOfWork>().InRequestScope();
            _kernel.Bind<IIcogramUnitOfWork>().To<IcogramUnitOfWork>().InRequestScope();
            _kernel.Bind<IMapper>().ToMethod(AutoMapperModule.AutoMapper).InRequestScope();

            _kernel.Bind<IViewModelBuilder>().To<ViewModelBuilder.ViewModelBuilder>().InRequestScope();
            _kernel.Bind<ILoginService>().To<LoginService>().InRequestScope();
            _kernel.Bind<IUserService>().To<UserService>().InRequestScope();
            _kernel.Bind(typeof(ICrudService<>)).To(typeof(CrudService<>));
        }
    }
}