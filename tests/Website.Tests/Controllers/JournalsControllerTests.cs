using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Castle.Components.DictionaryAdapter;
using Moq;
using QOAM.Core;
using QOAM.Core.Import;
using QOAM.Core.Import.Licences;
using QOAM.Core.Import.SubmissionLinks;
using QOAM.Core.Repositories;
using QOAM.Website.Controllers;
using QOAM.Website.Helpers;
using QOAM.Website.Tests.Controllers.Helpers;
using QOAM.Website.Tests.Controllers.Stubs;
using QOAM.Website.ViewModels.Journals;
using Xunit;

namespace QOAM.Website.Tests.Controllers
{
    public class JournalsControllerTests
    {
        #region Fields

        JournalsController _controller;

        Mock<IJournalRepository> _journalRepository;
        Mock<IBaseJournalPriceRepository> _baseJournalPriceRepository;
        Mock<ILanguageRepository> _languageRepository;
        Mock<ISubjectRepository> _subjectRepository;
        Mock<IInstitutionJournalRepository> _institutionJournalRepository;
        Mock<IValuationJournalPriceRepository> _valuationJournalPriceRepository;
        Mock<IValuationScoreCardRepository> _valuationScoreCardRepository;
        Mock<IInstitutionRepository> _institutionRepository;
        Mock<IBaseScoreCardRepository> _baseScoreCardRepository;
        Mock<IUserProfileRepository> _userProfileRepository;
        Mock<IAuthentication> _authentication;
        Mock<IBulkImporter<UniversityLicense>> _bulkImporter;

        Mock<HttpPostedFileBase> _uploadFile;
        InstitutionalPricesViewModel _viewModel;
        Mock<IBulkImporter<JournalRelatedLink>> _journalRelatedLinkBulkImporter;

        #endregion

        #region Setup()

        void Initialize()
        {
            _journalRepository = new Mock<IJournalRepository>();
            _baseJournalPriceRepository = new Mock<IBaseJournalPriceRepository>();
            _languageRepository = new Mock<ILanguageRepository>();
            _subjectRepository = new Mock<ISubjectRepository>();
            _institutionJournalRepository = new Mock<IInstitutionJournalRepository>();
            _valuationJournalPriceRepository = new Mock<IValuationJournalPriceRepository>();
            _valuationScoreCardRepository = new Mock<IValuationScoreCardRepository>();
            _institutionRepository = new Mock<IInstitutionRepository>();
            _baseScoreCardRepository = new Mock<IBaseScoreCardRepository>();
            _userProfileRepository = new Mock<IUserProfileRepository>();
            _authentication = new Mock<IAuthentication>();
            _bulkImporter = new Mock<IBulkImporter<UniversityLicense>>();
            _journalRelatedLinkBulkImporter = new Mock<IBulkImporter<JournalRelatedLink>>();
            
            _controller = new JournalsController(_journalRepository.Object, _baseJournalPriceRepository.Object, _valuationJournalPriceRepository.Object, _valuationScoreCardRepository.Object, _languageRepository.Object, _institutionJournalRepository.Object, _baseScoreCardRepository.Object, _userProfileRepository.Object, _authentication.Object, _institutionRepository.Object, _bulkImporter.Object, _subjectRepository.Object, _journalRelatedLinkBulkImporter.Object);

            _uploadFile = new Mock<HttpPostedFileBase>();
            _viewModel = new InstitutionalPricesViewModel
            {
                File = _uploadFile.Object
            };
        }

        #endregion

        [Fact]
        public void BulkLicenseImportRejectsInvalidImportFilesAndShowsAnErrorToTheUser()
        {
            Initialize();

            _bulkImporter.Setup(x => x.Execute(_uploadFile.Object.InputStream))
                .Throws<ArgumentException>();

            _controller.BulkImportInstitutionalPrices(_viewModel);

            Assert.Equal(1, _controller.ModelState["generalError"].Errors.Count);
        }

        [Fact]
        public void BulkLicenseImportConvertsInsertsNonExistingInstitutionJournals()
        {
            Initialize();
            _controller.Url = HttpContextHelper.CreateUrlHelper();

            var data = UniversityLicenseStubs.Licenses();

            var journalCount = 0;
            var institutionCount = 0;

            var journals = (from ul in data
                            from l in ul.Licenses
                            select new Journal { Id = ++journalCount, ISSN = l.ISSN}).ToList();

            var institutions = (from ul in data
                                select new Institution { Id = ++institutionCount, ShortName = ul.Domain }).ToList();

            _bulkImporter.Setup(x => x.Execute(_uploadFile.Object.InputStream)).Returns(data);
            _institutionRepository.Setup(x => x.FindWhere(It.IsAny<Expression<Func<Institution, bool>>>()))
                .Returns((Expression<Func<Institution, bool>> query) => institutions.AsQueryable().Where(query).ToList());
            
            _journalRepository.Setup(x => x.AllWhereIncluding(It.IsAny<Expression<Func<Journal, bool>>>(), It.IsAny<Expression<Func<Journal, object>>[]>()))
                .Returns((Expression<Func<Journal, bool>> query, Expression<Func<Journal, object>>[] _) => journals.AsQueryable().Where(query).ToList());

            _controller.BulkImportInstitutionalPrices(_viewModel);

            _institutionJournalRepository.Verify(x => x.InsertOrUpdate(It.IsAny<InstitutionJournal>()), Times.Exactly(12));
        }

        [Fact]
        public void BulkLicenseImportUpdatesExisting()
        {
            Initialize();
            _controller.Url = HttpContextHelper.CreateUrlHelper();

            var data = UniversityLicenseStubs.Licenses();

            var journalCount = 0;
            var institutionCount = 0;

            var journals = (from ul in data
                            from l in ul.Licenses
                            select new Journal { Id = ++journalCount, ISSN = l.ISSN}).ToList();

            var institutions = (from ul in data
                                select new Institution { Id = ++institutionCount, ShortName = ul.Domain }).ToList();

            _bulkImporter.Setup(x => x.Execute(_uploadFile.Object.InputStream)).Returns(data);
            _institutionRepository.Setup(x => x.FindWhere(It.IsAny<Expression<Func<Institution, bool>>>()))
                .Returns((Expression<Func<Institution, bool>> query) => institutions.AsQueryable().Where(query).ToList());
            
            _journalRepository.Setup(x => x.AllWhereIncluding(It.IsAny<Expression<Func<Journal, bool>>>(), It.IsAny<Expression<Func<Journal, object>>[]>()))
                .Returns((Expression<Func<Journal, bool>> query, Expression<Func<Journal, object>>[] _) => journals.AsQueryable().Where(query).ToList());

            _institutionJournalRepository.Setup(x => x.Find(It.IsAny<int>(), It.IsAny<int>())).Returns<int, int>((i, j) => new InstitutionJournal
            {
                Journal = journals.SingleOrDefault(journal => journal.Id == j),
                Institution = institutions.SingleOrDefault(insitution => insitution.Id == i),
            });

            _controller.BulkImportInstitutionalPrices(_viewModel);

            _institutionJournalRepository.Verify(x => x.InsertOrUpdate(It.IsAny<InstitutionJournal>()), Times.Exactly(12));
        }

        [Fact]
        public void BulkLicenseImportDeletesInstitutionJournalsWithEmptyLicenseTexts()
        {
            Initialize();
            _controller.Url = HttpContextHelper.CreateUrlHelper();

            var data = UniversityLicenseStubs.SomeLicensesToDelete();

            var journalCount = 0;
            var institutionCount = 0;

            var journals = (from ul in data
                            from l in ul.Licenses
                            select new Journal { Id = ++journalCount, ISSN = l.ISSN}).ToList();

            var institutions = (from ul in data
                                select new Institution { Id = ++institutionCount, ShortName = ul.Domain }).ToList();

            _bulkImporter.Setup(x => x.Execute(_uploadFile.Object.InputStream)).Returns(data);
            _institutionRepository.Setup(x => x.FindWhere(It.IsAny<Expression<Func<Institution, bool>>>()))
                .Returns((Expression<Func<Institution, bool>> query) => institutions.AsQueryable().Where(query).ToList());
            
            _journalRepository.Setup(x => x.AllWhereIncluding(It.IsAny<Expression<Func<Journal, bool>>>(), It.IsAny<Expression<Func<Journal, object>>[]>()))
                .Returns((Expression<Func<Journal, bool>> query, Expression<Func<Journal, object>>[] _) => journals.AsQueryable().Where(query).ToList());

            _institutionJournalRepository.Setup(x => x.Find(It.IsAny<int>(), It.IsAny<int>())).Returns<int, int>((i, j) => new InstitutionJournal
            {
                Journal = journals.SingleOrDefault(journal => journal.Id == j),
                Institution = institutions.SingleOrDefault(insitution => insitution.Id == i),
            });

            _controller.BulkImportInstitutionalPrices(_viewModel);

            _institutionJournalRepository.Verify(x => x.InsertOrUpdate(It.IsAny<InstitutionJournal>()), Times.Exactly(3));
            _institutionJournalRepository.Verify(x => x.Delete(It.Is<InstitutionJournal>(y => y.Journal.ISSN == "0219-3094")), Times.Exactly(1));
        }
    }
}