namespace QOAM.Console.ExpirationChecker
{
    using Autofac;

    using QOAM.Core.Repositories;
    using QOAM.Core.Services;

    public static class DependencyInjectionConfig
    {
        public static IContainer RegisterComponents()
        {
            var builder = new ContainerBuilder();

            RegisterRepositories(builder);
            RegisterExpirationCheckerComponents(builder);
            RegisterConfigurationSections(builder);
            RegisterMiscellaneousComponents(builder);

            return builder.Build();
        }

        private static void RegisterRepositories(ContainerBuilder builder)
        {
            builder.RegisterType<BaseScoreCardRepository>().As<IBaseScoreCardRepository>().SingleInstance();
            builder.RegisterType<ValuationScoreCardRepository>().As<IValuationScoreCardRepository>().SingleInstance();
        }

        private static void RegisterExpirationCheckerComponents(ContainerBuilder builder)
        {
            builder.RegisterType<ExpirationChecker>().SingleInstance();
            builder.RegisterType<ExpirationCheckerNotification>().SingleInstance();
        }

        private static void RegisterConfigurationSections(ContainerBuilder builder)
        {
            builder.Register(_ => ExpirationCheckerSettings.Current).As<ExpirationCheckerSettings>().SingleInstance();
        }

        private static void RegisterMiscellaneousComponents(ContainerBuilder builder)
        {
            builder.RegisterType<ApplicationDbContext>().SingleInstance();
            builder.Register(c => new MailSender(c.Resolve<ExpirationCheckerSettings>().SmtpHost)).As<IMailSender>().SingleInstance();
        }
    }
}