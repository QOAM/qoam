using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Moq;
using QOAM.Core;
using QOAM.Core.Import.Licences;
using QOAM.Core.Repositories;
using QOAM.Website.Controllers;
using QOAM.Website.Helpers;
using QOAM.Website.Tests.Controllers.Helpers;
using QOAM.Website.Tests.Controllers.Stubs;
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
        }

        #endregion


        [Fact]
        public void BulkLicenseImportRejectsInvalidImportFilesAndShowsAnErrorToTheUser()
        {
            Initialize();

            _bulkImporter.Setup(x => x.Execute(_uploadFile.Object.InputStream)).Throws<ArgumentException>();

            _controller.BulkLicenseImport(_uploadFile.Object);

            Assert.Equal(1, _controller.ModelState["invalidFile"].Errors.Count);
            Assert.IsType<ArgumentException>(_controller.ModelState["invalidFile"].Errors[0].Exception);
        }

        [Fact]
        public void BulkLicenseImportConvertsImportResultsToInstitutionJournals()
        {
            Initialize();
            _controller.Url = HttpContextHelper.CreateUrlHelper();

            var data = ConvertedEntitiyStubs.Licenses();
            var institution = new Institution { Id = 1, Name = "Test Institution" };
            var journal = new Journal { Id = 1, ISSN = "Some ISSN" };

            //var institutions = 1.To(10).Select(i => new Institution { Id = i, Name = $"Test Institution #{i}", ShortName = $"www.{i}.nl"});

            _bulkImporter.Setup(x => x.Execute(_uploadFile.Object.InputStream)).Returns(data);
            _institutionRepository.Setup(x => x.Find(It.IsAny<string>())).Returns(institution);
            _journalRepository.Setup(x => x.FindByIssn(It.IsAny<string>())).Returns(journal);

            _controller.BulkLicenseImport(_uploadFile.Object);

            _institutionJournalRepository.Verify(x => x.InsertOrUpdate(It.IsAny<InstitutionJournal>()), Times.Exactly(16));
        }
    }
}