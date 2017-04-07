using QOAM.Core.Import.JournalTOCs;

namespace QOAM.Console.DataImporter
{
    using Autofac;

    using QOAM.Core.Import;
    using QOAM.Core.Repositories;

    public static class DependencyInjectionConfig
    {
        public static IContainer RegisterComponents()
        {
            var builder = new ContainerBuilder();
            
            RegisterRepositories(builder);
            RegisterImportAndExportComponents(builder);
            RegisterConfigurationSections(builder);
            RegisterMiscellaneousComponents(builder);
            
            return builder.Build();
        }

        private static void RegisterRepositories(ContainerBuilder builder)
        {
            builder.RegisterType<BlockedISSNRepository>().As<IBlockedISSNRepository>().SingleInstance();
            builder.RegisterType<CountryRepository>().As<ICountryRepository>().SingleInstance();
            builder.RegisterType<JournalRepository>().As<IJournalRepository>().SingleInstance();
            builder.RegisterType<LanguageRepository>().As<ILanguageRepository>().SingleInstance();
            builder.RegisterType<PublisherRepository>().As<IPublisherRepository>().SingleInstance();
            builder.RegisterType<SubjectRepository>().As<ISubjectRepository>().SingleInstance();
        }

        private static void RegisterImportAndExportComponents(ContainerBuilder builder)
        {
            builder.RegisterType<JournalsImport>().SingleInstance();
            builder.RegisterType<DoajImport>().SingleInstance();
            builder.RegisterType<UlrichsClient>().SingleInstance();
            builder.RegisterType<UlrichsImport>().SingleInstance();
            builder.RegisterType<UlrichsCache>().SingleInstance();
            builder.RegisterType<JournalTocsClient>().As<IJournalTocsClient>().SingleInstance();
            builder.RegisterType<SystemWebClientFactory>().As<IWebClientFactory>().SingleInstance();
            builder.RegisterType<JournalTocsImport>().SingleInstance();
            builder.RegisterType<JournalTocsXmlParser>().As<IJournalTocsParser>().SingleInstance();
        }

        private static void RegisterConfigurationSections(ContainerBuilder builder)
        {
            builder.Register(_ => ImportSettings.Current).SingleInstance();
            builder.Register(c => c.Resolve<ImportSettings>().General).SingleInstance();
            builder.Register(c => c.Resolve<ImportSettings>().Doaj).SingleInstance();
            builder.Register(c => c.Resolve<ImportSettings>().Ulrichs).SingleInstance();
            builder.Register(c => c.Resolve<ImportSettings>().JournalTocs).SingleInstance();
        }

        private static void RegisterMiscellaneousComponents(ContainerBuilder builder)
        {
            builder.RegisterType<ApplicationDbContext>().SingleInstance();
        }
    }
}
