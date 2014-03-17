namespace QOAM.Console.DataImporter
{
    using System;

    using Autofac;

    using QOAM.Core.Import;
    using QOAM.Core.Repositories;

    internal static class DependencyInjectionConfig
    {
        internal static IContainer RegisterComponents()
        {
            var builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(typeof(JournalRepository).Assembly)
                .Where(IsRepositoryType)
                .AsImplementedInterfaces()
                .SingleInstance();
            
            builder.Register(_ => ImportSettings.Current).SingleInstance();
            builder.Register(c => c.Resolve<ImportSettings>().General).SingleInstance();
            builder.Register(c => c.Resolve<ImportSettings>().Doaj).SingleInstance();
            builder.Register(c => c.Resolve<ImportSettings>().Ulrichs).SingleInstance();
            builder.RegisterType<ApplicationDbContext>().SingleInstance();
            builder.RegisterType<JournalsImport>().SingleInstance();
            builder.RegisterType<DoajImport>().SingleInstance();
            builder.RegisterType<UlrichsClient>().SingleInstance();
            builder.RegisterType<UlrichsImport>().SingleInstance();
            builder.RegisterType<UlrichsCache>().SingleInstance();

            return builder.Build();
        }

        private static bool IsRepositoryType(Type t)
        {
            return t.Name.EndsWith("Repository");
        }
    }
}
