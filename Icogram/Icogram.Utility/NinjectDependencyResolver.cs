using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using Icogram.DbContext;
using Icogram.Models.UserModels;
using Microsoft.AspNet.Identity.EntityFramework;
using Ninject;
using Ninject.Web.Common;

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
            _kernel.Bind<IMapper>().ToMethod(AutoMapperModule.AutoMapper).InRequestScope();

        }
    }
}