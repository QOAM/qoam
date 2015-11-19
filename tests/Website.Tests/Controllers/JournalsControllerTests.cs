namespace QOAM.Website.Tests.Controllers
{
    using System;
    using System.Web;
    using Core;
    using Core.Import.Licences;
    using Core.Repositories;
    using Helpers;
    using Moq;
    using Stubs;
    using Website.Controllers;
    using Website.Helpers;
    using Website.ViewModels.Journals;
    using Xunit;

    public class JournalsControllerTests
    {
        #region Fields

        private JournalsController _controller;

        private Mock<IJournalRepository> _journalRepository;
        private Mock<IBaseJournalPriceRepository> _baseJournalPriceRepository;
        private Mock<ILanguageRepository> _languageRepository;
        private Mock<ISubjectRepository> _subjectRepository;
        private Mock<IInstitutionJournalRepository> _institutionJournalRepository;
        private Mock<IValuationJournalPriceRepository> _valuationJournalPriceRepository;
        private Mock<IValuationScoreCardRepository> _valuationScoreCardRepository;
        private Mock<IInstitutionRepository> _institutionRepository;
        private Mock<IBaseScoreCardRepository> _baseScoreCardRepository;
        private Mock<IUserProfileRepository> _userProfileRepository;
        private Mock<IAuthentication> _authentication;
        private Mock<IBulkImporter> _bulkImporter;

        private Mock<HttpPostedFileBase> _uploadFile;
        private InstitutionalPricesViewModel _viewModel;

        #endregion

        #region Setup()

        private void Initialize()
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

            _controller = new JournalsController(_journalRepository.Object, _baseJournalPriceRepository.Object, _valuationJournalPriceRepository.Object, _valuationScoreCardRepository.Object,
                _languageRepository.Object, _subjectRepository.Object, _institutionJournalRepository.Object, _baseScoreCardRepository.Object, _userProfileRepository.Object, _authentication.Object,
                _institutionRepository.Object, _bulkImporter.Object);

            _uploadFile = new Mock<HttpPostedFileBase>();
            _viewModel = new InstitutionalPricesViewModel { File = _uploadFile.Object };
        }

        #endregion

        [Fact]
        public void BulkLicenseImportRejectsInvalidImportFilesAndShowsAnErrorToTheUser()
        {
            Initialize();

            _bulkImporter.Setup(x => x.Execute(_uploadFile.Object.InputStream)).Throws<ArgumentException>();

            _controller.BulkImportInstitutionalPrices(_viewModel);

            Assert.Equal(1, _controller.ModelState["generalError"].Errors.Count);
        }

        [Fact]
        public void BulkLicenseImportConvertsImportResultsToInstitutionJournals()
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
    }
}