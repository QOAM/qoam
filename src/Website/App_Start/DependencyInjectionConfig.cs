using System.Web.Http;
using Autofac.Core;
using Autofac.Integration.WebApi;
using QOAM.Core;
using QOAM.Core.Import.Institutions;
using QOAM.Core.Import.Invitations;
using QOAM.Core.Import.JournalTOCs;
using QOAM.Core.Import.QOAMcorners;
using QOAM.Core.Import.SubmissionLinks;

namespace QOAM.Website
{
    using System.Web.Mvc;
    using Autofac;
    using Autofac.Integration.Mvc;
    using Core.Export;
    using Core.Import;
    using Core.Import.Licences;
    using Core.Repositories;
    using Core.Services;
    using Helpers;
    using Models;

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
            // Set the dependency resolver for Web API.
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static void RegisterControllers(ContainerBuilder builder)
        {
            var assembly = typeof(MvcApplication).Assembly;
            builder.RegisterControllers(assembly);
            builder.RegisterApiControllers(assembly);
        }

        private static void RegisterRepositories(ContainerBuilder builder)
        {
            builder.RegisterType<BaseJournalPriceRepository>().As<IBaseJournalPriceRepository>().InstancePerRequest();
            builder.RegisterType<BaseScoreCardRepository>().As<IBaseScoreCardRepository>().InstancePerRequest();
            builder.RegisterType<CountryRepository>().As<ICountryRepository>().InstancePerRequest();
            builder.RegisterType<InstitutionJournalRepository>().As<IInstitutionJournalRepository>().InstancePerRequest();
            builder.RegisterType<UserJournalRepository>().As<IUserJournalRepository>().InstancePerRequest();
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
            builder.RegisterType<CornerRepository>().As<ICornerRepository>().InstancePerRequest();
        }

        private static void RegisterConfigurationSections(ContainerBuilder builder)
        {
            builder.Register(c => OAMarketSettings.Current).AsSelf().InstancePerRequest();
            builder.Register(c => c.Resolve<OAMarketSettings>().Contact).AsSelf().InstancePerRequest();
            builder.Register(c => c.Resolve<OAMarketSettings>().General).AsSelf().InstancePerRequest();
            builder.Register(c => c.Resolve<OAMarketSettings>().GeneralImport).AsSelf().InstancePerRequest();
            builder.Register(c => c.Resolve<OAMarketSettings>().Doaj).AsSelf().InstancePerRequest();
            builder.Register(c => c.Resolve<OAMarketSettings>().Ulrichs).AsSelf().InstancePerRequest();
            builder.Register(c => c.Resolve<OAMarketSettings>().JournalTocs).AsSelf().InstancePerRequest();
        }

        private static void RegisterImportAndExportComponents(ContainerBuilder builder)
        {
            builder.RegisterType<JournalsExport>().AsSelf().InstancePerRequest();
            builder.RegisterType<JournalsImport>().AsSelf().InstancePerRequest();
            builder.RegisterType<DoajImport>().AsSelf().InstancePerRequest();
            builder.RegisterType<UlrichsClient>().AsSelf().InstancePerRequest();
            builder.RegisterType<UlrichsImport>().AsSelf().InstancePerRequest();
            builder.RegisterType<UlrichsCache>().AsSelf().InstancePerRequest();
            builder.RegisterType<JournalTocsClient>().As<IJournalTocsClient>().InstancePerRequest();
            builder.RegisterType<SystemWebClientFactory>().As<IWebClientFactory>().InstancePerRequest();
            builder.RegisterType<JournalTocsImport>().AsSelf().InstancePerRequest();
            builder.RegisterType<JournalTocsJsonParser>().As<IJournalTocsParser>().SingleInstance();

            builder.RegisterType<LicenseFileImporter>().As<IFileImporter>().InstancePerRequest();
            builder.RegisterType<InvitationFileImporter>().Named<IFileImporter>("invitation").InstancePerRequest();
            builder.RegisterType<SubmissionLinksFileImporter>().Named<IFileImporter>("submission-link").InstancePerRequest();
            builder.RegisterType<InstitutionFileImporter>().Named<IFileImporter>("institution").InstancePerRequest();
            builder.RegisterType<CornerFileImporter>().Named<IFileImporter>("corner").InstancePerRequest();

            builder.RegisterType<ImportLicenseEntityConverter>().As<IImportEntityConverter<UniversityLicense>>().InstancePerRequest();
            builder.RegisterType<ImportAuthorEntityConverter>().As<IImportEntityConverter<AuthorToInvite>>().InstancePerRequest();
            builder.RegisterType<SubmissionPageLinkEntityConverter>().As<IImportEntityConverter<SubmissionPageLink>>().InstancePerRequest();
            builder.RegisterType<InstitutionEntityConverter>().As<IImportEntityConverter<Institution>>().InstancePerRequest();
            builder.RegisterType<CornerEntityConverter>().As<IImportEntityConverter<CornerToImport>>().InstancePerRequest();

            builder.RegisterType<BulkImporter<UniversityLicense>>().As<IBulkImporter<UniversityLicense>>().InstancePerRequest();
            builder.RegisterType<BulkImporter<AuthorToInvite>>().As<IBulkImporter<AuthorToInvite>>().WithParameter(ResolvedParameter.ForNamed<IFileImporter>("invitation")).InstancePerRequest();
            builder.RegisterType<BulkImporter<SubmissionPageLink>>().As<IBulkImporter<SubmissionPageLink>>().WithParameter(ResolvedParameter.ForNamed<IFileImporter>("submission-link")).InstancePerRequest();
            builder.RegisterType<BulkImporter<Institution>>().As<IBulkImporter<Institution>>().WithParameter(ResolvedParameter.ForNamed<IFileImporter>("institution")).InstancePerRequest();
            builder.RegisterType<BulkImporter<CornerToImport>>().As<IBulkImporter<CornerToImport>>().WithParameter(ResolvedParameter.ForNamed<IFileImporter>("corner")).InstancePerRequest();
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