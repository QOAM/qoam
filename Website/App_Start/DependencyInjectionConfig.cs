namespace RU.Uci.OAMarket.Website.App_Start
{
    using System.Web.Mvc;

    using Autofac;
    using Autofac.Integration.Mvc;

    using RU.Uci.OAMarket.Data;
    using RU.Uci.OAMarket.Domain.Import;
    using RU.Uci.OAMarket.Domain.Services;
    using RU.Uci.OAMarket.Website.Helpers;
    using RU.Uci.OAMarket.Website.Models;

    public static class DependencyInjectionConfig
    {
        public static void RegisterComponents()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterAssemblyTypes(typeof(Repository).Assembly).AssignableTo<Repository>().AsImplementedInterfaces().InstancePerHttpRequest();
            builder.RegisterType<ApplicationDbContext>().InstancePerHttpRequest();
            builder.RegisterType<Authentication>().As<IAuthentication>().InstancePerHttpRequest();
            builder.RegisterType<Roles>().As<IRoles>().InstancePerHttpRequest();
            builder.Register(c => OAMarketSettings.Current).AsSelf().InstancePerHttpRequest();
            builder.Register(c => c.Resolve<OAMarketSettings>().Contact).InstancePerHttpRequest();
            builder.Register(c => c.Resolve<OAMarketSettings>().General).InstancePerHttpRequest();
            builder.Register(c => c.Resolve<OAMarketSettings>().GeneralImport).InstancePerHttpRequest();
            builder.Register(c => c.Resolve<OAMarketSettings>().Doaj).InstancePerHttpRequest();
            builder.Register(c => c.Resolve<OAMarketSettings>().Ulrichs).InstancePerHttpRequest();
            builder.Register(c => new MailSender(c.Resolve<OAMarketSettings>().Contact.SmtpHost)).As<IMailSender>().InstancePerHttpRequest();
            builder.RegisterType<JournalsImport>().InstancePerHttpRequest();
            builder.RegisterType<DoajImport>().InstancePerHttpRequest();
            builder.RegisterType<UlrichsClient>().InstancePerHttpRequest();
            builder.RegisterType<UlrichsImport>().InstancePerHttpRequest();
            builder.RegisterType<UlrichsCache>().InstancePerHttpRequest();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}