using AutoMapper;
using Icogram.Models.UserModels;
using Icogram.ViewModels.User;
using Ninject.Modules;

namespace Icogram.AutoMapper
{
    public class AutoMapperModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IMapper>().ToMethod(AutoMapper).InSingletonScope();
        }

        public static IMapper AutoMapper(Ninject.Activation.IContext context)
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<ApplicationUser, UserProfileViewModel>()
                .ForMember(up => up.Company, d => d.MapFrom(au => au.Company));
            });

            return Mapper.Instance;
        }
    }
}