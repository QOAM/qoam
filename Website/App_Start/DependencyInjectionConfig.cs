namespace QOAM.Website
{
    using System.Web.Mvc;

    using Autofac;
    using Autofac.Integration.Mvc;

    using QOAM.Core.Export;
    using QOAM.Core.Import;
    using QOAM.Core.Repositories;
    using QOAM.Core.Services;
    using QOAM.Website.Helpers;
    using QOAM.Website.Models;

    public static class DependencyInjectionConfig
    {
        public static void RegisterComponents()
        {
            var builder = new ContainerBuilder();

            RegisterControllers(builder);
            RegisterRepositories(builder);
            RegisterConfigurationSections(builder);
            RegisterImportAndExportComponents(builder);
            RegisterMiscellaneousComponents(builder);

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        private static void RegisterControllers(ContainerBuilder builder)
        {
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
        }

        private static void RegisterRepositories(ContainerBuilder builder)
        {
            builder.RegisterType<BaseJournalPriceRepository>().As<IBaseJournalPriceRepository>().InstancePerHttpRequest();
            builder.RegisterType<BaseScoreCardRepository>().As<IBaseScoreCardRepository>().InstancePerHttpRequest();
            builder.RegisterType<CountryRepository>().As<ICountryRepository>().InstancePerHttpRequest();
            builder.RegisterType<InstitutionJournalRepository>().As<IInstitutionJournalRepository>().InstancePerHttpRequest();
            builder.RegisterType<InstitutionRepository>().As<IInstitutionRepository>().InstancePerHttpRequest();
            builder.RegisterType<JournalRepository>().As<IJournalRepository>().InstancePerHttpRequest();
            builder.RegisterType<LanguageRepository>().As<ILanguageRepository>().InstancePerHttpRequest();
            builder.RegisterType<PublisherRepository>().As<IPublisherRepository>().InstancePerHttpRequest();
            builder.RegisterType<QuestionRepository>().As<IQuestionRepository>().InstancePerHttpRequest();
            builder.RegisterType<ScoreCardVersionRepository>().As<IScoreCardVersionRepository>().InstancePerHttpRequest();
            builder.RegisterType<SubjectRepository>().As<ISubjectRepository>().InstancePerHttpRequest();
            builder.RegisterType<UserProfileRepository>().As<IUserProfileRepository>().InstancePerHttpRequest();
            builder.RegisterType<ValuationJournalPriceRepository>().As<IValuationJournalPriceRepository>().InstancePerHttpRequest();
            builder.RegisterType<ValuationScoreCardRepository>().As<IValuationScoreCardRepository>().InstancePerHttpRequest();
            builder.RegisterType<BlockedISSNRepository>().As<IBlockedISSNRepository>().InstancePerHttpRequest();
        }

        private static void RegisterConfigurationSections(ContainerBuilder builder)
        {
            builder.Register(c => OAMarketSettings.Current).AsSelf().InstancePerHttpRequest();
            builder.Register(c => c.Resolve<OAMarketSettings>().Contact).AsSelf().InstancePerHttpRequest();
            builder.Register(c => c.Resolve<OAMarketSettings>().General).AsSelf().InstancePerHttpRequest();
            builder.Register(c => c.Resolve<OAMarketSettings>().GeneralImport).AsSelf().InstancePerHttpRequest();
            builder.Register(c => c.Resolve<OAMarketSettings>().Doaj).AsSelf().InstancePerHttpRequest();
            builder.Register(c => c.Resolve<OAMarketSettings>().Ulrichs).AsSelf().InstancePerHttpRequest();
        }

        private static void RegisterImportAndExportComponents(ContainerBuilder builder)
        {
            builder.RegisterType<JournalsExport>().AsSelf().InstancePerHttpRequest();
            builder.RegisterType<JournalsImport>().AsSelf().InstancePerHttpRequest();
            builder.RegisterType<DoajImport>().AsSelf().InstancePerHttpRequest();
            builder.RegisterType<UlrichsClient>().AsSelf().InstancePerHttpRequest();
            builder.RegisterType<UlrichsImport>().AsSelf().InstancePerHttpRequest();
            builder.RegisterType<UlrichsCache>().AsSelf().InstancePerHttpRequest();
        }

        private static void RegisterMiscellaneousComponents(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacWebTypesModule());
            builder.RegisterType<ApplicationDbContext>().AsSelf().InstancePerHttpRequest();
            builder.RegisterType<Authentication>().As<IAuthentication>().InstancePerHttpRequest();
            builder.RegisterType<Roles>().As<IRoles>().InstancePerHttpRequest();
            builder.Register(c => new MailSender(c.Resolve<OAMarketSettings>().Contact.SmtpHost)).As<IMailSender>().InstancePerHttpRequest();
        }
    }
}