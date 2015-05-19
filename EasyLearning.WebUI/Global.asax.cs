using Autofac;
using Autofac.Integration.Mvc;
using EasyLearning.WebUI.Modules;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace EasyLearning.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var builder = new Autofac.ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired();
            builder.RegisterModule(new ServiceModule());
            builder.RegisterModule(new EFModule());

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            //Database.SetInitializer<EasyLearningDB>(new DbInitializer());
        }
    }
}
