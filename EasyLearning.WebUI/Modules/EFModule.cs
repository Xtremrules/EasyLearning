using Autofac;
using EasyLearning.Domain.Concrete;

namespace EasyLearning.WebUI.Modules
{
    public class EFModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(EasyLearningDB)).AsSelf();
        }
    }
}