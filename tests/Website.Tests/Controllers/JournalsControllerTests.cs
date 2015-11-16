using Moq;
using QOAM.Core.Import.Licences;
using QOAM.Core.Repositories;
using QOAM.Website.Controllers;
using QOAM.Website.Helpers;
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

        #endregion

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

            _controller = new JournalsController(_journalRepository.Object, _baseJournalPriceRepository.Object, _valuationJournalPriceRepository.Object, _valuationScoreCardRepository.Object, 
                _languageRepository.Object, _subjectRepository.Object, _institutionJournalRepository.Object, _baseScoreCardRepository.Object, _userProfileRepository.Object, 
                _authentication.Object, _institutionRepository.Object, _bulkImporter.Object);
        }

        //[Fact]
        //public void
    }
}