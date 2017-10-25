using AutoMapper;
using Ninject.Modules;

namespace Icogram.Utility
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

            });
            return Mapper.Instance;
        }
    }
}