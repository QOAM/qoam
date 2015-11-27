using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Moq;
using QOAM.Core;
using QOAM.Core.Import.Licences;
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
        Mock<IBulkImporter> _bulkImporter;

        Mock<HttpPostedFileBase> _uploadFile;
        InstitutionalPricesViewModel _viewModel;

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
            _bulkImporter = new Mock<IBulkImporter>();

            _controller = new JournalsController(_journalRepository.Object, _baseJournalPriceRepository.Object, _valuationJournalPriceRepository.Object, _valuationScoreCardRepository.Object, _languageRepository.Object, _subjectRepository.Object, _institutionJournalRepository.Object, _baseScoreCardRepository.Object, _userProfileRepository.Object, _authentication.Object, _institutionRepository.Object, _bulkImporter.Object);

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

            var data = ConvertedEntityStubs.Licenses();
            var institution = new Institution { Id = 1, Name = "Test Institution" };
            var journal = new Journal { Id = 1, ISSN = "Some ISSN" };

            //var institutions = 1.To(10).Select(i => new Institution { Id = i, Name = $"Test Institution #{i}", ShortName = $"www.{i}.nl"});

            _bulkImporter.Setup(x => x.Execute(_uploadFile.Object.InputStream)).Returns(data);
            _institutionRepository.Setup(x => x.Find(It.IsAny<string>())).Returns(institution);
            _journalRepository.Setup(x => x.FindByIssn(It.IsAny<string>())).Returns(journal);

            _controller.BulkImportInstitutionalPrices(_viewModel);

            _institutionJournalRepository.Verify(x => x.InsertOrUpdate(It.IsAny<InstitutionJournal>()), Times.Exactly(16));
        }

        [Fact]
        public void BulkLicenseImportUpdatesExisting()
        {
            Initialize();
            _controller.Url = HttpContextHelper.CreateUrlHelper();

            var data = ConvertedEntityStubs.Licenses();

            var journalCount = 0;
            var institutionCount = 0;

            var journals = new List<Journal>();
            var institutions = new List<Institution>();

            _bulkImporter.Setup(x => x.Execute(_uploadFile.Object.InputStream)).Returns(data);
            _institutionRepository.Setup(x => x.Find(It.IsAny<string>())).Returns<string>(s =>
            {
                var institution = new Institution { Id = ++institutionCount, ShortName = s };
                institutions.Add(institution);

                return institution;
            });
            _journalRepository.Setup(x => x.FindByIssn(It.IsAny<string>())).Returns<string>(s =>
            {
                var journal = new Journal { Id = ++journalCount, ISSN = s };
                journals.Add(journal);

                return journal;
            });

            _institutionJournalRepository.Setup(x => x.Find(It.IsAny<int>(), It.IsAny<int>())).Returns<int, int>((i, j) => new InstitutionJournal
            {
                Journal = journals.SingleOrDefault(journal => journal.Id == j),
                Institution = institutions.SingleOrDefault(insitution => insitution.Id == i),
            });

            _controller.BulkImportInstitutionalPrices(_viewModel);

            _institutionJournalRepository.Verify(x => x.InsertOrUpdate(It.IsAny<InstitutionJournal>()), Times.Exactly(16));
        }

        [Fact]
        public void BulkLicenseImportDeletesInstitutionJournalsWithEmptyLicenseTexts()
        {
            Initialize();
            _controller.Url = HttpContextHelper.CreateUrlHelper();

            var data = ConvertedEntityStubs.SomeLicensesToDelete();

            var journalCount = 0;
            var institutionCount = 0;

            var journals = new List<Journal>();
            var institutions = new List<Institution>();

            _bulkImporter.Setup(x => x.Execute(_uploadFile.Object.InputStream)).Returns(data);
            _institutionRepository.Setup(x => x.Find(It.IsAny<string>())).Returns<string>(s =>
            {
                var institution = new Institution { Id = ++institutionCount, ShortName = s };
                institutions.Add(institution);

                return institution;
            });
            _journalRepository.Setup(x => x.FindByIssn(It.IsAny<string>())).Returns<string>(s =>
            {
                var journal = new Journal { Id = ++journalCount, ISSN = s };
                journals.Add(journal);

                return journal;
            });

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