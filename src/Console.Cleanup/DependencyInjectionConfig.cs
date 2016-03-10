namespace QOAM.Console.Cleanup
{
    using Autofac;
    using Core.Cleanup;
    using Core.Repositories;

    public static class DependencyInjectionConfig
    {
        public static IContainer RegisterComponents()
        {
            var builder = new ContainerBuilder();
            
            RegisterRepositories(builder);
            RegisterCleanupComponents(builder);
            RegisterConfigurationSections(builder);
            RegisterMiscellaneousComponents(builder);
            
            return builder.Build();
        }

        private static void RegisterRepositories(ContainerBuilder builder)
        {
            builder.RegisterType<BaseScoreCardRepository>().As<IBaseScoreCardRepository>().SingleInstance();
            builder.RegisterType<UserProfileRepository>().As<IUserProfileRepository>().SingleInstance();
            builder.RegisterType<ValuationScoreCardRepository>().As<IValuationScoreCardRepository>().SingleInstance();
        }

        private static void RegisterCleanupComponents(ContainerBuilder builder)
        {
            builder.RegisterType<UnpublishedScoreCardsCleanup>().SingleInstance();
            builder.RegisterType<DuplicateScoreCardsCleanup>().SingleInstance();
            builder.RegisterType<InactiveProfilesCleanup>().SingleInstance();
        }

        private static void RegisterConfigurationSections(ContainerBuilder builder)
        {
            builder.Register(_ => CleanupSettings.Current).SingleInstance();
        }

        private static void RegisterMiscellaneousComponents(ContainerBuilder builder)
        {
            builder.RegisterType<ApplicationDbContext>().SingleInstance();
        }
    }
}
