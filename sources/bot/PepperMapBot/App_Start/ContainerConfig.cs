using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using PepperMap.Infrastructure.Interfaces;
using PepperMap.Infrastructure.Services;
using PepperMapBot.Services;
using RouteService = PepperMap.Infrastructure.Services.RouteService;

namespace PepperMapBot
{
    public static class ContainerConfig
    {
        public static void Configure()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(WebApiApplication).Assembly);

            ConfigureSettings(builder);
            ConfigureInfrastructure(builder);

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        private static void ConfigureSettings(ContainerBuilder builder)
        {
            builder.RegisterType<SettingService>().As<ISettingService>();
        }

        private static void ConfigureInfrastructure(ContainerBuilder builder)
        {
            builder.RegisterType<RouteService>().As<IRouteService>();
            builder.RegisterType<UrlService>().As<IUrlService>().InstancePerLifetimeScope();

        }
    }
}