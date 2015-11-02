﻿namespace QOAM.Website
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
            builder.RegisterType<BaseJournalPriceRepository>().As<IBaseJournalPriceRepository>().InstancePerRequest();
            builder.RegisterType<BaseScoreCardRepository>().As<IBaseScoreCardRepository>().InstancePerRequest();
            builder.RegisterType<CountryRepository>().As<ICountryRepository>().InstancePerRequest();
            builder.RegisterType<InstitutionJournalRepository>().As<IInstitutionJournalRepository>().InstancePerRequest();
            builder.RegisterType<InstitutionRepository>().As<IInstitutionRepository>().InstancePerRequest();
            builder.RegisterType<JournalRepository>().As<IJournalRepository>().InstancePerRequest();
            builder.RegisterType<LanguageRepository>().As<ILanguageRepository>().InstancePerRequest();
            builder.RegisterType<PublisherRepository>().As<IPublisherRepository>().InstancePerRequest();
            builder.RegisterType<QuestionRepository>().As<IQuestionRepository>().InstancePerRequest();
            builder.RegisterType<ScoreCardVersionRepository>().As<IScoreCardVersionRepository>().InstancePerRequest();
            builder.RegisterType<SubjectRepository>().As<ISubjectRepository>().InstancePerRequest();
            builder.RegisterType<UserProfileRepository>().As<IUserProfileRepository>().InstancePerRequest();
            builder.RegisterType<ValuationJournalPriceRepository>().As<IValuationJournalPriceRepository>().InstancePerRequest();
            builder.RegisterType<ValuationScoreCardRepository>().As<IValuationScoreCardRepository>().InstancePerRequest();
            builder.RegisterType<BlockedISSNRepository>().As<IBlockedISSNRepository>().InstancePerRequest();
        }

        private static void RegisterConfigurationSections(ContainerBuilder builder)
        {
            builder.Register(c => OAMarketSettings.Current).AsSelf().InstancePerRequest();
            builder.Register(c => c.Resolve<OAMarketSettings>().Contact).AsSelf().InstancePerRequest();
            builder.Register(c => c.Resolve<OAMarketSettings>().General).AsSelf().InstancePerRequest();
            builder.Register(c => c.Resolve<OAMarketSettings>().GeneralImport).AsSelf().InstancePerRequest();
            builder.Register(c => c.Resolve<OAMarketSettings>().Doaj).AsSelf().InstancePerRequest();
            builder.Register(c => c.Resolve<OAMarketSettings>().Ulrichs).AsSelf().InstancePerRequest();
        }

        private static void RegisterImportAndExportComponents(ContainerBuilder builder)
        {
            builder.RegisterType<JournalsExport>().AsSelf().InstancePerRequest();
            builder.RegisterType<JournalsImport>().AsSelf().InstancePerRequest();
            builder.RegisterType<DoajImport>().AsSelf().InstancePerRequest();
            builder.RegisterType<UlrichsClient>().AsSelf().InstancePerRequest();
            builder.RegisterType<UlrichsImport>().AsSelf().InstancePerRequest();
            builder.RegisterType<UlrichsCache>().AsSelf().InstancePerRequest();
        }

        private static void RegisterMiscellaneousComponents(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacWebTypesModule());
            builder.RegisterType<ApplicationDbContext>().AsSelf().InstancePerRequest();
            builder.RegisterType<Authentication>().As<IAuthentication>().InstancePerRequest();
            builder.RegisterType<Roles>().As<IRoles>().InstancePerRequest();
            builder.Register(c => new MailSender(c.Resolve<OAMarketSettings>().Contact.SmtpHost)).As<IMailSender>().InstancePerRequest();
        }
    }
}