using System.Web;
using FizzWare.NBuilder;
using QOAM.Core.Import.SubmissionLinks;
using QOAM.Website.Tests.Controllers.Stubs;
using QOAM.Website.ViewModels;
using QOAM.Website.ViewModels.Admin;
using QOAM.Website.ViewModels.Institutions;

namespace QOAM.Website.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Web.Mvc;
    using Moq;
    using MvcContrib.TestHelper;
    using Core;
    using Core.Export;
    using Core.Import;
    using Core.Repositories;
    using TestHelpers;
    using Website.Controllers;
    using Website.Helpers;
    using Website.ViewModels.Import;
    using Xunit;

    public class AdminControllerTests
    {

        private const string ExpectedJournalsCsv = "Title;ISSN;Link;DateAdded;Country;Publisher;Languages;Subjects\r\n027.7 : Zeitschrift fuer Bibliothekskultur;2296-0597;http://www.0277.ch/ojs/index.php/cdrs_0277;2/10/2013 9:52:51 AM;Switzerland;<none indicated>;English,German;library and information sciences\r\n16:9;1603-5194;http://www.16-9.dk;2/10/2013 9:52:51 AM;Denmark;Springer;English,Danish;motion pictures,films\r\nACIMED;1024-9435;http://scielo.sld.cu/scielo.php?script=sci_serial&pid=1024-9435&lng=en&nrm=iso;2/10/2013 9:52:51 AM;Cuba;Centro Nacional de Información de Ciencias Médicas;<none indicated>;health sciences\r\n";
        private const string ExpectedOpenAccessJournalsCsv = "Title;ISSN;Link;DateAdded;Country;Publisher;Languages;Subjects\r\n027.7 : Zeitschrift fuer Bibliothekskultur;2296-0597;http://www.0277.ch/ojs/index.php/cdrs_0277;2/10/2013 9:52:51 AM;Switzerland;<none indicated>;English,German;library and information sciences\r\n16:9;1603-5194;http://www.16-9.dk;2/10/2013 9:52:51 AM;Denmark;Springer;English,Danish;motion pictures,films\r\n";
        private const string OldIssn = "2296-0597";
        private const string NewIssn = "1603-5194";

        Mock<IBulkImporter<SubmissionPageLink>> _bulkImporter;
        Mock<IBulkImporter<Institution>> _institutionImporter;
        Mock<HttpPostedFileBase> _uploadFile;

        [Fact]
        public void ConstructorWithNullJournalsImportThrowsArgumentNullException()
        {
            // Arrange
            JournalsImport nullJournalsImport = null;

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new AdminController(nullJournalsImport, CreateUlrichsImport(), CreateDoajImport(), CreateJournalsExport(), Mock.Of<IJournalRepository>(), Mock.Of<IUserProfileRepository>(), Mock.Of<IAuthentication>(), Mock.Of<IInstitutionRepository>(), Mock.Of<IBlockedISSNRepository>(), Mock.Of<IBaseScoreCardRepository>(), Mock.Of<IValuationScoreCardRepository>(), Mock.Of<IBulkImporter<SubmissionPageLink>>(), Mock.Of<IBulkImporter<Institution>>()));
        }

        [Fact]
        public void ConstructorWithNullUlrichsImportThrowsArgumentNullException()
        {
            // Arrange
            UlrichsImport nullUlrichsImport = null;

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new AdminController(CreateJournalsImport(), nullUlrichsImport, CreateDoajImport(), CreateJournalsExport(), Mock.Of<IJournalRepository>(), Mock.Of<IUserProfileRepository>(), Mock.Of<IAuthentication>(), Mock.Of<IInstitutionRepository>(), Mock.Of<IBlockedISSNRepository>(), Mock.Of<IBaseScoreCardRepository>(), Mock.Of<IValuationScoreCardRepository>(), Mock.Of<IBulkImporter<SubmissionPageLink>>(), Mock.Of<IBulkImporter<Institution>>()));
        }

        [Fact]
        public void ConstructorWithNullDoajImportThrowsArgumentNullException()
        {
            // Arrange
            DoajImport nullDoajImport = null;

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new AdminController(CreateJournalsImport(), CreateUlrichsImport(), nullDoajImport, CreateJournalsExport(), Mock.Of<IJournalRepository>(), Mock.Of<IUserProfileRepository>(), Mock.Of<IAuthentication>(), Mock.Of<IInstitutionRepository>(), Mock.Of<IBlockedISSNRepository>(), Mock.Of<IBaseScoreCardRepository>(), Mock.Of<IValuationScoreCardRepository>(), Mock.Of<IBulkImporter<SubmissionPageLink>>(), Mock.Of<IBulkImporter<Institution>>()));}

        [Fact]
        public void ConstructorWithNullJournalsExportThrowsArgumentNullException()
        {
            // Arrange
            JournalsExport nullJournalsExport = null;

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new AdminController(CreateJournalsImport(), CreateUlrichsImport(), CreateDoajImport(), nullJournalsExport, Mock.Of<IJournalRepository>(), Mock.Of<IUserProfileRepository>(), Mock.Of<IAuthentication>(), Mock.Of<IInstitutionRepository>(), Mock.Of<IBlockedISSNRepository>(), Mock.Of<IBaseScoreCardRepository>(), Mock.Of<IValuationScoreCardRepository>(), Mock.Of<IBulkImporter<SubmissionPageLink>>(), Mock.Of<IBulkImporter<Institution>>()));
        }

        [Fact]
        public void ConstructorWithNullJournalRepositoryThrowsArgumentNullException()
        {
            // Arrange
            IJournalRepository nullJournalRepository = null;

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new AdminController(CreateJournalsImport(), CreateUlrichsImport(), CreateDoajImport(), CreateJournalsExport(), nullJournalRepository, Mock.Of<IUserProfileRepository>(), Mock.Of<IAuthentication>(), Mock.Of<IInstitutionRepository>(), Mock.Of<IBlockedISSNRepository>(), Mock.Of<IBaseScoreCardRepository>(), Mock.Of<IValuationScoreCardRepository>(), Mock.Of<IBulkImporter<SubmissionPageLink>>(), Mock.Of<IBulkImporter<Institution>>()));
        }

        [Fact]
        public void ConstructorWithNullUserProfileRepositoryThrowsArgumentNullException()
        {
            // Arrange
            IUserProfileRepository nullUserProfileRepository = null;

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new AdminController(CreateJournalsImport(), CreateUlrichsImport(), CreateDoajImport(), CreateJournalsExport(), Mock.Of<IJournalRepository>(), nullUserProfileRepository, Mock.Of<IAuthentication>(), Mock.Of<IInstitutionRepository>(), Mock.Of<IBlockedISSNRepository>(), Mock.Of<IBaseScoreCardRepository>(), Mock.Of<IValuationScoreCardRepository>(), Mock.Of<IBulkImporter<SubmissionPageLink>>(), Mock.Of<IBulkImporter<Institution>>()));
        }

        [Fact]
        public void ConstructorWithNullAuthenticationThrowsArgumentNullException()
        {
            // Arrange
            IAuthentication nullAuthentication = null;

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new AdminController(CreateJournalsImport(), CreateUlrichsImport(), CreateDoajImport(), CreateJournalsExport(), Mock.Of<IJournalRepository>(), Mock.Of<IUserProfileRepository>(), nullAuthentication, Mock.Of<IInstitutionRepository>(), Mock.Of<IBlockedISSNRepository>(), Mock.Of<IBaseScoreCardRepository>(), Mock.Of<IValuationScoreCardRepository>(), Mock.Of<IBulkImporter<SubmissionPageLink>>(), Mock.Of<IBulkImporter<Institution>>()));
        }

        [Fact]
        public void ConstructorWithNullBaseScoreCardRepositoryThrowsArgumentNullException()
        {
            // Arrange
            IBaseScoreCardRepository nullBaseScoreCardRepository = null;

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new AdminController(CreateJournalsImport(), CreateUlrichsImport(), CreateDoajImport(), CreateJournalsExport(), Mock.Of<IJournalRepository>(), Mock.Of<IUserProfileRepository>(), Mock.Of<IAuthentication>(), Mock.Of<IInstitutionRepository>(), Mock.Of<IBlockedISSNRepository>(), nullBaseScoreCardRepository, Mock.Of<IValuationScoreCardRepository>(), Mock.Of<IBulkImporter<SubmissionPageLink>>(), Mock.Of<IBulkImporter<Institution>>()));
        }

        [Fact]
        public void ConstructorWithNullValuationScoreCardRepositoryThrowsArgumentNullException()
        {
            // Arrange
            IValuationScoreCardRepository nullValuationScoreCardRepository = null;

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new AdminController(CreateJournalsImport(), CreateUlrichsImport(), CreateDoajImport(), CreateJournalsExport(), Mock.Of<IJournalRepository>(), Mock.Of<IUserProfileRepository>(), Mock.Of<IAuthentication>(), Mock.Of<IInstitutionRepository>(), Mock.Of<IBlockedISSNRepository>(), Mock.Of<IBaseScoreCardRepository>(), nullValuationScoreCardRepository, Mock.Of<IBulkImporter<SubmissionPageLink>>(), Mock.Of<IBulkImporter<Institution>>()));
        }

        [Fact]
        public void ConstructorWithNullBulkImporterThrowsArgumentNullException()
        {
            // Arrange
            IBulkImporter<SubmissionPageLink> nullBulkImporter = null;

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new AdminController(CreateJournalsImport(), CreateUlrichsImport(), CreateDoajImport(), CreateJournalsExport(), Mock.Of<IJournalRepository>(), Mock.Of<IUserProfileRepository>(), Mock.Of<IAuthentication>(), Mock.Of<IInstitutionRepository>(), Mock.Of<IBlockedISSNRepository>(), Mock.Of<IBaseScoreCardRepository>(), Mock.Of<IValuationScoreCardRepository>(), nullBulkImporter, Mock.Of<IBulkImporter<Institution>>()));
        }

        [Fact]
        public void ConstructorWithNullInstitutionImporterThrowsArgumentNullException()
        {
            // Arrange
            IBulkImporter<Institution> nullInstitutionImporter = null;

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new AdminController(CreateJournalsImport(), CreateUlrichsImport(), CreateDoajImport(), CreateJournalsExport(), Mock.Of<IJournalRepository>(), Mock.Of<IUserProfileRepository>(), Mock.Of<IAuthentication>(), Mock.Of<IInstitutionRepository>(), Mock.Of<IBlockedISSNRepository>(), Mock.Of<IBaseScoreCardRepository>(), Mock.Of<IValuationScoreCardRepository>(), Mock.Of<IBulkImporter<SubmissionPageLink>>(), nullInstitutionImporter));
        }

        [Fact]
        [UseCulture("en-US")]
        public void DownloadReturnsFileContentResultWithCorrectCsvFileForAllJournals()
        {
            // Arrange
            var adminController = this.CreateAdminController(journalsExport: CreateJournalsExport(CreateJournalRepository(CreateJournals())));

            // Act
            var fileContentResult = adminController.Download("all");

            // Assert
            var fileContentResultAsString = Encoding.UTF8.GetString(fileContentResult.FileContents);
            Assert.Equal(ExpectedJournalsCsv, fileContentResultAsString);
        }

        [Fact]
        [UseCulture("en-US")]
        public void DownloadReturnsFileContentResultWithOnlyOpenAccessJournals()
        {
            // Arrange
            var adminController = CreateAdminController(journalsExport: CreateJournalsExport(CreateJournalRepository(CreateJournals())));

            // Act
            var fileContentResult = adminController.Download("open-access");

            // Assert
            var fileContentResultAsString = Encoding.UTF8.GetString(fileContentResult.FileContents);
            Assert.Equal(ExpectedOpenAccessJournalsCsv, fileContentResultAsString);
        }

        [Theory]
        [InlineData("all")]
        public void DownloadReturnsFileContentResultWithCsvContentType(string downloadType)
        {
            // Arrange
            var adminController = this.CreateAdminController(journalsExport: CreateJournalsExport(CreateJournalRepository(CreateJournals())));

            // Act
            var fileContentResult = adminController.Download(downloadType);

            // Assert
            Assert.Equal("application/csv", fileContentResult.ContentType);
        }

        [Theory]
        [InlineData("all")]
        public void DownloadReturnsFileContentResultWithFileDownloadNameSet(string downloadType)
        {
            // Arrange
            var adminController = this.CreateAdminController(journalsExport: CreateJournalsExport(CreateJournalRepository(CreateJournals())));

            // Act
            var fileContentResult = adminController.Download(downloadType);

            // Assert
            Assert.Equal("journals.csv", fileContentResult.FileDownloadName);
        }

        [Fact]
        public void CheckWithValidModelWillSetFoundISSNsToISSNsThatAreInJournalRepository()
        {
            // Arrange
            var adminController = this.CreateAdminController(journalRepository: CreateJournalRepository(CreateJournals()));

            // Act
            var viewResult = adminController.Check(new CheckViewModel { ISSNs = "2296-0597\n1603-5194\n1443-8675\n8872-3754" });

            // Assert
            Assert.Equal(new[] { "2296-0597", "1603-5194" }, ((CheckViewModel)viewResult.Model).FoundISSNs);
        }

        [Fact]
        public void CheckWithValidModelWillSetNotFoundISSNsToISSNsThatAreNotInJournalRepository()
        {
            // Arrange
            var adminController = this.CreateAdminController(journalRepository: CreateJournalRepository(CreateJournals()));

            // Act
            var viewResult = adminController.Check(new CheckViewModel { ISSNs = "2296-0597\n1603-5194\n1443-8675\n8872-3754" });

            // Assert
            Assert.Equal(new[] { "1443-8675", "8872-3754" }, ((CheckViewModel)viewResult.Model).NotFoundISSNs);
        }

        [Fact]
        public void CheckWithValidModelWillReturnUniqueISSNs()
        {
            // Arrange
            var adminController = this.CreateAdminController(journalRepository: CreateJournalRepository(CreateJournals()));

            // Act
            var viewResult = adminController.Check(new CheckViewModel { ISSNs = "2296-0597\n2296-0597\n1603-5194\n1443-8675\n8872-3754\n8872-3754" });

            // Assert
            Assert.Equal(new[] { "2296-0597", "1603-5194" }, ((CheckViewModel)viewResult.Model).FoundISSNs);
            Assert.Equal(new[] { "1443-8675", "8872-3754" }, ((CheckViewModel)viewResult.Model).NotFoundISSNs);
        }

        [Fact]
        public void CheckWithInvalidModelWillNotSetFoundISSNOrNotFoundISSNsProperties()
        {
            // Arrange
            var adminController = this.CreateAdminController();
            adminController.ModelState.AddModelError("ISSNs", "Empty");

            // Act
            var viewResult = adminController.Check(new CheckViewModel { ISSNs = "2296-0597\n1603-5194\n1443-8675\n8872-3754" });

            // Assert
            Assert.Null(((CheckViewModel)viewResult.Model).FoundISSNs);
            Assert.Null(((CheckViewModel)viewResult.Model).NotFoundISSNs);
        }

        [Fact]
        public void MoveScoreCardsRendersView()
        {
            // Arrange
            var adminController = this.CreateAdminController();

            // Act
            var viewResult = adminController.MoveScoreCards();

            // Assert
            Assert.IsType<ViewResult>(viewResult);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(false)]
        [InlineData(true)]
        public void MoveScoreCardsStoresSaveSuccessfulStatusInViewBag(bool? saveSuccessful)
        {
            // Arrange
            var adminController = this.CreateAdminController();

            // Act
            var viewResult = adminController.MoveScoreCards(saveSuccessful);

            // Assert
            Assert.Equal(saveSuccessful, viewResult.ViewBag.SaveSuccessful);
        }

        [Fact]
        public void MoveScoreCardsWithInvalidModelRendersView()
        {
            // Arrange
            var adminController = this.CreateAdminController();
            adminController.ModelState.AddModelError("ISSNs", "Empty");

            // Act
            var actionResult = adminController.MoveScoreCards(CreateMoveScoreCardsViewModel());

            // Assert
            actionResult.AssertViewRendered();
        }

        [Fact]
        public void MoveScoreCardsWithValidModelRedirectsToMoveScoreCardsAction()
        {
            // Arrange
            var adminController = this.CreateAdminController();

            // Act
            var actionResult = adminController.MoveScoreCards(CreateMoveScoreCardsViewModel());

            // Assert
            actionResult.AssertActionRedirect().ToAction("MoveScoreCards");
        }

        [Fact]
        public void MoveScoreCardsWithValidModelRedirectsWithSaveSuccessfulParameterSetToTrue()
        {
            // Arrange
            var adminController = this.CreateAdminController();

            // Act
            var actionResult = adminController.MoveScoreCards(CreateMoveScoreCardsViewModel());

            // Assert
            actionResult.AssertActionRedirect().WithParameter("saveSuccessful", true);
        }

        [Fact]
        public void MoveScoreCardsWithUnknownNewIssnAddsModelError()
        {
            // Arrange
            var moveScoreCardsViewModel = CreateMoveScoreCardsViewModel();

            var journalRepository = new Mock<IJournalRepository>();
            journalRepository.Setup(j => j.FindByIssn(moveScoreCardsViewModel.OldIssn)).Returns(new Journal());
            journalRepository.Setup(j => j.FindByIssn(moveScoreCardsViewModel.NewIssn)).Returns((Journal)null);

            var adminController = this.CreateAdminController(journalRepository: journalRepository.Object);

            // Act
            adminController.MoveScoreCards(moveScoreCardsViewModel);

            // Assert
            Assert.True(adminController.ModelState.ContainsKey(nameof(moveScoreCardsViewModel.NewIssn)));
        }

        [Fact]
        public void MoveScoreCardsWithUnknownNewIssnDoesNotMoveScoreCards()
        {
            // Arrange
            var moveScoreCardsViewModel = new MoveScoreCardsViewModel { NewIssn = NewIssn, OldIssn = OldIssn };

            var valuationScoreCardRepository = new Mock<IValuationScoreCardRepository>();
            var baseScoreCardRepository = new Mock<IBaseScoreCardRepository>();

            var journalRepository = new Mock<IJournalRepository>();
            journalRepository.Setup(j => j.FindByIssn(moveScoreCardsViewModel.OldIssn)).Returns(new Journal());
            journalRepository.Setup(j => j.FindByIssn(moveScoreCardsViewModel.NewIssn)).Returns((Journal)null);

            var adminController = this.CreateAdminController(journalRepository: journalRepository.Object, baseScoreCardRepository: baseScoreCardRepository.Object, valuationScoreCardRepository: valuationScoreCardRepository.Object);

            // Act
            adminController.MoveScoreCards(moveScoreCardsViewModel);

            // Assert
            baseScoreCardRepository.Verify(b => b.MoveScoreCards(It.IsAny<Journal>(), It.IsAny<Journal>()), Times.Never);
            valuationScoreCardRepository.Verify(v => v.MoveScoreCards(It.IsAny<Journal>(), It.IsAny<Journal>()), Times.Never);
        }

        [Fact]
        public void MoveScoreCardsWithUnknownOldIssnAddsModelError()
        {
            // Arrange
            var moveScoreCardsViewModel = new MoveScoreCardsViewModel { NewIssn = NewIssn, OldIssn = OldIssn };

            var journalRepository = new Mock<IJournalRepository>();
            journalRepository.Setup(j => j.FindByIssn(moveScoreCardsViewModel.OldIssn)).Returns((Journal)null);
            journalRepository.Setup(j => j.FindByIssn(moveScoreCardsViewModel.NewIssn)).Returns(new Journal());

            var adminController = this.CreateAdminController(journalRepository: journalRepository.Object);

            // Act
            adminController.MoveScoreCards(moveScoreCardsViewModel);

            // Assert
            Assert.True(adminController.ModelState.ContainsKey(nameof(moveScoreCardsViewModel.OldIssn)));
        }

        [Fact]
        public void MoveScoreCardsWithUnknownOldIssnDoesNotMoveScoreCards()
        {
            // Arrange
            var moveScoreCardsViewModel = new MoveScoreCardsViewModel { NewIssn = NewIssn, OldIssn = OldIssn };

            var valuationScoreCardRepository = new Mock<IValuationScoreCardRepository>();
            var baseScoreCardRepository = new Mock<IBaseScoreCardRepository>();

            var journalRepository = new Mock<IJournalRepository>();
            journalRepository.Setup(j => j.FindByIssn(moveScoreCardsViewModel.OldIssn)).Returns((Journal)null);
            journalRepository.Setup(j => j.FindByIssn(moveScoreCardsViewModel.NewIssn)).Returns(new Journal());

            var adminController = this.CreateAdminController(journalRepository: journalRepository.Object, baseScoreCardRepository: baseScoreCardRepository.Object, valuationScoreCardRepository: valuationScoreCardRepository.Object);

            // Act
            adminController.MoveScoreCards(moveScoreCardsViewModel);

            // Assert
            baseScoreCardRepository.Verify(b => b.MoveScoreCards(It.IsAny<Journal>(), It.IsAny<Journal>()), Times.Never);
            valuationScoreCardRepository.Verify(v => v.MoveScoreCards(It.IsAny<Journal>(), It.IsAny<Journal>()), Times.Never);
        }

        [Fact]
        public void MoveScoreCardsWithValidModelMovesBaseScoreCardsUsingSpecifiedIssns()
        {
            // Arrange
            var journals = CreateJournals();
            var journalRepository = CreateJournalRepository(journals);

            var baseScoreCardRepository = new Mock<IBaseScoreCardRepository>();
            var adminController = this.CreateAdminController(baseScoreCardRepository: baseScoreCardRepository.Object, journalRepository: journalRepository);

            var moveScoreCardsViewModel = CreateMoveScoreCardsViewModel();

            // Act
            adminController.MoveScoreCards(moveScoreCardsViewModel);

            // Assert
            var oldJournal = journals.First(j => j.ISSN == OldIssn);
            var newJournal = journals.First(j => j.ISSN == NewIssn);
            baseScoreCardRepository.Verify(j => j.MoveScoreCards(oldJournal, newJournal), Times.Once);
        }

        [Fact]
        public void MoveScoreCardsWithValidModelMovesValuationScoreCardsUsingSpecifiedIssns()
        {
            // Arrange
            var journals = CreateJournals();
            var journalRepository = CreateJournalRepository(journals);

            var valuationScoreCardRepository = new Mock<IValuationScoreCardRepository>();
            var adminController = this.CreateAdminController(valuationScoreCardRepository: valuationScoreCardRepository.Object, journalRepository: journalRepository);

            var moveScoreCardsViewModel = CreateMoveScoreCardsViewModel();

            // Act
            adminController.MoveScoreCards(moveScoreCardsViewModel);

            // Assert
            var oldJournal = journals.First(j => j.ISSN == OldIssn);
            var newJournal = journals.First(j => j.ISSN == NewIssn);
            valuationScoreCardRepository.Verify(j => j.MoveScoreCards(oldJournal, newJournal), Times.Once);
        }

        [Fact]
        public void RemoveBaseScoreCardRendersView()
        {
            // Arrange
            var baseScoreCard = new BaseScoreCard { Id = 5 };
            var baseScoreCardRepository = new Mock<IBaseScoreCardRepository>();
            baseScoreCardRepository.Setup(b => b.Find(baseScoreCard.Id)).Returns(baseScoreCard);

            var adminController = this.CreateAdminController(baseScoreCardRepository: baseScoreCardRepository.Object);

            // Act
            var actionResult = adminController.RemoveBaseScoreCard(CreateRemoveBaseScoreCardViewModel());

            // Assert
            actionResult.AssertViewRendered();
        }

        [Fact]
        public void RemoveBaseScoreCardPassesModelArgumentToView()
        {
            // Arrange
            var baseScoreCard = new BaseScoreCard { Id = 5 };
            var baseScoreCardRepository = new Mock<IBaseScoreCardRepository>();
            baseScoreCardRepository.Setup(b => b.Find(baseScoreCard.Id)).Returns(baseScoreCard);

            var adminController = this.CreateAdminController(baseScoreCardRepository: baseScoreCardRepository.Object);

            var removeBaseScoreCardViewModel = CreateRemoveBaseScoreCardViewModel();

            // Act
            var actionResult = adminController.RemoveBaseScoreCard(removeBaseScoreCardViewModel);

            // Assert
            Assert.Same(removeBaseScoreCardViewModel, actionResult.AssertViewRendered().WithViewData<RemoveBaseScoreCardViewModel>());
        }

        [Fact]
        public void RemoveBaseScoreCardWithUnknownIdReturnsNotFound()
        {
            // Arrange
            var baseScoreCardRepository = new Mock<IBaseScoreCardRepository>();
            baseScoreCardRepository.Setup(b => b.Find(It.IsAny<int>())).Returns((BaseScoreCard)null);

            var adminController = this.CreateAdminController(baseScoreCardRepository: baseScoreCardRepository.Object);

            // Act
            var actionResult = adminController.RemoveBaseScoreCard(CreateRemoveBaseScoreCardViewModel());

            // Assert
            actionResult.AssertResultIs<HttpNotFoundResult>();
        }

        [Fact]
        public void RemoveBaseScoreCardPostWithUnknownIdReturnsNotFound()
        {
            // Arrange
            var baseScoreCardRepository = new Mock<IBaseScoreCardRepository>();
            baseScoreCardRepository.Setup(b => b.Find(It.IsAny<int>())).Returns((BaseScoreCard)null);

            var adminController = this.CreateAdminController(baseScoreCardRepository: baseScoreCardRepository.Object);

            // Act
            var actionResult = adminController.RemoveBaseScoreCardPost(CreateRemoveBaseScoreCardViewModel());

            // Assert
            actionResult.AssertResultIs<HttpNotFoundResult>();
        }

        [Fact]
        public void RemoveBaseScoreCardPostWithInvalidModelRendersView()
        {
            // Arrange
            var baseScoreCard = new BaseScoreCard { Id = 5 };
            var baseScoreCardRepository = new Mock<IBaseScoreCardRepository>();
            baseScoreCardRepository.Setup(b => b.Find(baseScoreCard.Id)).Returns(baseScoreCard);

            var adminController = this.CreateAdminController(baseScoreCardRepository: baseScoreCardRepository.Object);
            adminController.ModelState.AddModelError("ISSNs", "Empty");

            // Act
            var actionResult = adminController.RemoveBaseScoreCardPost(CreateRemoveBaseScoreCardViewModel());

            // Assert
            actionResult.AssertViewRendered();
        }

        [Fact]
        public void RemoveBaseScoreCardPosttWithInvalidModelPassesModelArgumentToView()
        {
            // Arrange
            var baseScoreCard = new BaseScoreCard { Id = 5 };
            var baseScoreCardRepository = new Mock<IBaseScoreCardRepository>();
            baseScoreCardRepository.Setup(b => b.Find(baseScoreCard.Id)).Returns(baseScoreCard);

            var adminController = this.CreateAdminController(baseScoreCardRepository: baseScoreCardRepository.Object);
            adminController.ModelState.AddModelError("ISSNs", "Empty");

            var removeBaseScoreCardViewModel = CreateRemoveBaseScoreCardViewModel();

            // Act
            var actionResult = adminController.RemoveBaseScoreCardPost(removeBaseScoreCardViewModel);

            // Assert
            Assert.Same(removeBaseScoreCardViewModel, actionResult.AssertViewRendered().WithViewData<RemoveBaseScoreCardViewModel>());
        }

        [Fact]
        public void RemoveBaseScoreCardPostWithValidModelRedirectsToRemovedBaseScoreCardAction()
        {
            // Arrange
            var baseScoreCard = new BaseScoreCard { Id = 5 };
            var baseScoreCardRepository = new Mock<IBaseScoreCardRepository>();
            baseScoreCardRepository.Setup(b => b.Find(baseScoreCard.Id)).Returns(baseScoreCard);

            var adminController = this.CreateAdminController(baseScoreCardRepository: baseScoreCardRepository.Object);

            // Act
            var actionResult = adminController.RemoveBaseScoreCardPost(new RemoveBaseScoreCardViewModel { Id = baseScoreCard.Id });

            // Assert
            actionResult.AssertActionRedirect().ToAction("RemovedBaseScoreCard");
        }

        [Fact]
        public void RemoveBaseScoreCardPostWithUnknownIdDoesNotRemoveBaseScoreCard()
        {
            // Arrange
            var removeBaseScoreCardViewModel = CreateRemoveBaseScoreCardViewModel();

            var baseScoreCardRepository = new Mock<IBaseScoreCardRepository>();
            baseScoreCardRepository.Setup(j => j.Find(It.IsAny<int>())).Returns((BaseScoreCard)null);

            var adminController = this.CreateAdminController(baseScoreCardRepository: baseScoreCardRepository.Object);

            // Act
            adminController.RemoveBaseScoreCardPost(removeBaseScoreCardViewModel);

            // Assert
            baseScoreCardRepository.Verify(b => b.Delete(It.IsAny<BaseScoreCard>()), Times.Never);
        }

        [Fact]
        public void RemoveBaseScoreCardPostWithValidModelDeletesBaseScoreCardWithSpecifiedId()
        {
            // Arrange
            var removeBaseScoreCardViewModel = CreateRemoveBaseScoreCardViewModel();
            var baseScoreCard = new BaseScoreCard();

            var baseScoreCardRepository = new Mock<IBaseScoreCardRepository>();
            baseScoreCardRepository.Setup(j => j.Find(removeBaseScoreCardViewModel.Id)).Returns(baseScoreCard);

            var adminController = this.CreateAdminController(baseScoreCardRepository: baseScoreCardRepository.Object);

            // Act
            adminController.RemoveBaseScoreCardPost(removeBaseScoreCardViewModel);

            // Assert
            baseScoreCardRepository.Verify(b => b.Delete(baseScoreCard), Times.Once);
        }

        [Fact]
        public void RemoveBaseScoreCardPostWithValidModelSavesChanges()
        {
            // Arrange
            var removeBaseScoreCardViewModel = CreateRemoveBaseScoreCardViewModel();
            var baseScoreCard = new BaseScoreCard();

            var baseScoreCardRepository = new Mock<IBaseScoreCardRepository>();
            baseScoreCardRepository.Setup(j => j.Find(removeBaseScoreCardViewModel.Id)).Returns(baseScoreCard);

            var adminController = this.CreateAdminController(baseScoreCardRepository: baseScoreCardRepository.Object);

            // Act
            adminController.RemoveBaseScoreCardPost(removeBaseScoreCardViewModel);

            // Assert
            baseScoreCardRepository.Verify(b => b.Save(), Times.Once);
        }

        [Fact]
        public void RemoveValuationScoreCardRendersView()
        {
            // Arrange
            var valuationScoreCard = new ValuationScoreCard { Id = 7 };
            var valuationScoreCardRepository = new Mock<IValuationScoreCardRepository>();
            valuationScoreCardRepository.Setup(b => b.Find(valuationScoreCard.Id)).Returns(valuationScoreCard);

            var adminController = this.CreateAdminController(valuationScoreCardRepository: valuationScoreCardRepository.Object);

            // Act
            var actionResult = adminController.RemoveValuationScoreCard(CreateRemoveValuationScoreCardViewModel());

            // Assert
            actionResult.AssertViewRendered();
        }
        
        [Fact]
        public void RemoveValuationScoreCardPassesModelArgumentToView()
        {
            // Arrange
            var valuationScoreCard = new ValuationScoreCard { Id = 7 };
            var valuationScoreCardRepository = new Mock<IValuationScoreCardRepository>();
            valuationScoreCardRepository.Setup(b => b.Find(valuationScoreCard.Id)).Returns(valuationScoreCard);

            var adminController = this.CreateAdminController(valuationScoreCardRepository: valuationScoreCardRepository.Object);

            var removeValuationScoreCardViewModel = CreateRemoveValuationScoreCardViewModel();

            // Act
            var actionResult = adminController.RemoveValuationScoreCard(removeValuationScoreCardViewModel);

            // Assert
            Assert.Same(removeValuationScoreCardViewModel, actionResult.AssertViewRendered().WithViewData<RemoveValuationScoreCardViewModel>());
        }
        
        [Fact]
        public void RemoveValuationScoreCardWithUnknownIdReturnsNotFound()
        {
            // Arrange
            var valuationScoreCardRepository = new Mock<IValuationScoreCardRepository>();
            valuationScoreCardRepository.Setup(b => b.Find(It.IsAny<int>())).Returns((ValuationScoreCard)null);

            var adminController = this.CreateAdminController(valuationScoreCardRepository: valuationScoreCardRepository.Object);

            // Act
            var actionResult = adminController.RemoveValuationScoreCard(CreateRemoveValuationScoreCardViewModel());

            // Assert
            actionResult.AssertResultIs<HttpNotFoundResult>();
        }

        [Fact]
        public void RemoveValuationScoreCardPostWithUnknownIdReturnsNotFound()
        {
            // Arrange
            var valuationScoreCardRepository = new Mock<IValuationScoreCardRepository>();
            valuationScoreCardRepository.Setup(b => b.Find(It.IsAny<int>())).Returns((ValuationScoreCard)null);

            var adminController = this.CreateAdminController(valuationScoreCardRepository: valuationScoreCardRepository.Object);

            // Act
            var actionResult = adminController.RemoveValuationScoreCardPost(CreateRemoveValuationScoreCardViewModel());

            // Assert
            actionResult.AssertResultIs<HttpNotFoundResult>();
        }

        [Fact]
        public void RemoveValuationScoreCardPostWithInvalidModelRendersView()
        {
            // Arrange
            var valuationScoreCard = new ValuationScoreCard { Id = 7 };
            var valuationScoreCardRepository = new Mock<IValuationScoreCardRepository>();
            valuationScoreCardRepository.Setup(b => b.Find(valuationScoreCard.Id)).Returns(valuationScoreCard);

            var adminController = this.CreateAdminController(valuationScoreCardRepository: valuationScoreCardRepository.Object);
            adminController.ModelState.AddModelError("ISSNs", "Empty");

            // Act
            var actionResult = adminController.RemoveValuationScoreCardPost(CreateRemoveValuationScoreCardViewModel());

            // Assert
            actionResult.AssertViewRendered();
        }

        [Fact]
        public void RemoveValuationScoreCardPostWithInvalidModelPassesModelArgumentToView()
        {
            // Arrange
            var valuationScoreCard = new ValuationScoreCard { Id = 7 };
            var valuationScoreCardRepository = new Mock<IValuationScoreCardRepository>();
            valuationScoreCardRepository.Setup(b => b.Find(valuationScoreCard.Id)).Returns(valuationScoreCard);

            var adminController = this.CreateAdminController(valuationScoreCardRepository: valuationScoreCardRepository.Object);
            adminController.ModelState.AddModelError("ISSNs", "Empty");

            var removeValuationScoreCardViewModel = CreateRemoveValuationScoreCardViewModel();

            // Act
            var actionResult = adminController.RemoveValuationScoreCardPost(removeValuationScoreCardViewModel);

            // Assert
            Assert.Same(removeValuationScoreCardViewModel, actionResult.AssertViewRendered().WithViewData<RemoveValuationScoreCardViewModel>());
        }

        [Fact]
        public void RemoveValuationScoreCardPostWithValidModelRedirectsToRemovedValuationScoreCardAction()
        {
            // Arrange
            var valuationScoreCard = new ValuationScoreCard { Id = 7 };
            var valuationScoreCardRepository = new Mock<IValuationScoreCardRepository>();
            valuationScoreCardRepository.Setup(b => b.Find(valuationScoreCard.Id)).Returns(valuationScoreCard);

            var adminController = this.CreateAdminController(valuationScoreCardRepository: valuationScoreCardRepository.Object);

            // Act
            var actionResult = adminController.RemoveValuationScoreCardPost(new RemoveValuationScoreCardViewModel { Id = valuationScoreCard.Id });

            // Assert
            actionResult.AssertActionRedirect().ToAction("RemovedValuationScoreCard");
        }

        [Fact]
        public void RemoveValuationScoreCardPostWithUnknownIdDoesNotRemoveValuationScoreCard()
        {
            // Arrange
            var removeValuationScoreCardViewModel = CreateRemoveValuationScoreCardViewModel();

            var valuationScoreCardRepository = new Mock<IValuationScoreCardRepository>();
            valuationScoreCardRepository.Setup(j => j.Find(It.IsAny<int>())).Returns((ValuationScoreCard)null);

            var adminController = this.CreateAdminController(valuationScoreCardRepository: valuationScoreCardRepository.Object);

            // Act
            adminController.RemoveValuationScoreCardPost(removeValuationScoreCardViewModel);

            // Assert
            valuationScoreCardRepository.Verify(b => b.Delete(It.IsAny<ValuationScoreCard>()), Times.Never);
        }

        [Fact]
        public void RemoveValuationScoreCardPostWithValidModelDeletesValuationScoreCardWithSpecifiedId()
        {
            // Arrange
            var removeValuationScoreCardViewModel = CreateRemoveValuationScoreCardViewModel();
            var valuationScoreCard = new ValuationScoreCard();

            var valuationScoreCardRepository = new Mock<IValuationScoreCardRepository>();
            valuationScoreCardRepository.Setup(j => j.Find(removeValuationScoreCardViewModel.Id)).Returns(valuationScoreCard);

            var adminController = this.CreateAdminController(valuationScoreCardRepository: valuationScoreCardRepository.Object);

            // Act
            adminController.RemoveValuationScoreCardPost(removeValuationScoreCardViewModel);

            // Assert
            valuationScoreCardRepository.Verify(b => b.Delete(valuationScoreCard), Times.Once);
        }
        
        [Fact]
        public void RemoveValuationScoreCardPostWithValidModelSavesChanges()
        {
            // Arrange
            var removeValuationScoreCardViewModel = CreateRemoveValuationScoreCardViewModel();
            var valuationScoreCard = new ValuationScoreCard();

            var valuationScoreCardRepository = new Mock<IValuationScoreCardRepository>();
            valuationScoreCardRepository.Setup(j => j.Find(removeValuationScoreCardViewModel.Id)).Returns(valuationScoreCard);

            var adminController = this.CreateAdminController(valuationScoreCardRepository: valuationScoreCardRepository.Object);

            // Act
            adminController.RemoveValuationScoreCardPost(removeValuationScoreCardViewModel);

            // Assert
            valuationScoreCardRepository.Verify(b => b.Save(), Times.Once);
        }

        [Fact]
        public void BulkAddInstitutionsRejectsInvalidImportFilesAndShowsAnErrorToTheUser()
        {
            var viewModel = PrepareFileUpload<UpsertViewModel>();

            var controller = CreateAdminController();
            _institutionImporter.Setup(x => x.Execute(_uploadFile.Object.InputStream)).Throws<ArgumentException>();

            controller.BulkAddInstitution(viewModel);

            Assert.Equal(1, controller.ModelState["generalError"].Errors.Count);
        }

        [Fact]
        public void BulkAddInstitutionsSavesNewInstitutions()
        {
            var viewModel = PrepareFileUpload<UpsertViewModel>();

            var data = Builder<Institution>.CreateListOfSize(5).Build();
            var institutionRepository = new Mock<IInstitutionRepository>();
            institutionRepository.Setup(x => x.Exists(It.IsAny<string>())).Returns(false);

            var controller = CreateAdminController(institutionRepository: institutionRepository.Object);

            _institutionImporter.Setup(x => x.Execute(_uploadFile.Object.InputStream)).Returns(data);

            controller.BulkAddInstitution(viewModel);

            institutionRepository.Verify(x => x.InsertOrUpdate(It.IsAny<Institution>()), Times.Exactly(5));
            institutionRepository.Verify(x => x.Save(), Times.Exactly(5));
        }

        [Fact]
        public void BulkAddInstitutionsSkipsExistingInstitutionNames()
        {
            var viewModel = PrepareFileUpload<UpsertViewModel>();

            var data = Builder<Institution>.CreateListOfSize(5)
                .TheFirst(1)
                .With(x => x.Name = "Test University")
                .Build();

            var institutionRepository = new Mock<IInstitutionRepository>();
            institutionRepository.Setup(x => x.Exists("Test University")).Returns(true);

            var controller = CreateAdminController(institutionRepository: institutionRepository.Object);

            _institutionImporter.Setup(x => x.Execute(_uploadFile.Object.InputStream)).Returns(data);

            controller.BulkAddInstitution(viewModel);

            institutionRepository.Verify(x => x.InsertOrUpdate(It.IsAny<Institution>()), Times.Exactly(4));
            institutionRepository.Verify(x => x.Save(), Times.Exactly(4));
        }

        [Fact]
        public void BulkAddInstitutionsSkipsExistingDomains()
        {
            var viewModel = PrepareFileUpload<UpsertViewModel>();

            var data = Builder<Institution>.CreateListOfSize(5)
                .TheFirst(1)
                .With(x => x.ShortName = "ru.nl")
                .Build();

            var institutionRepository = new Mock<IInstitutionRepository>();
            institutionRepository.Setup(x => x.DomainExists("ru.nl")).Returns(true);

            var controller = CreateAdminController(institutionRepository: institutionRepository.Object);

            _institutionImporter.Setup(x => x.Execute(_uploadFile.Object.InputStream)).Returns(data);

            controller.BulkAddInstitution(viewModel);

            institutionRepository.Verify(x => x.InsertOrUpdate(It.IsAny<Institution>()), Times.Exactly(4));
            institutionRepository.Verify(x => x.Save(), Times.Exactly(4));
        }


        [Fact]
        public void BulkAddInstitutionsSkipsEmptyNamesOrDomains()
        {
            var viewModel = PrepareFileUpload<UpsertViewModel>();

            var data = Builder<Institution>.CreateListOfSize(5)
                .TheFirst(1)
                .With(x => x.ShortName = "")
                .TheNext(1)
                .With(x => x.Name = "")
                .Build();

            var institutionRepository = new Mock<IInstitutionRepository>();

            var controller = CreateAdminController(institutionRepository: institutionRepository.Object);

            _institutionImporter.Setup(x => x.Execute(_uploadFile.Object.InputStream)).Returns(data);

            controller.BulkAddInstitution(viewModel);

            institutionRepository.Verify(x => x.InsertOrUpdate(It.IsAny<Institution>()), Times.Exactly(3));
            institutionRepository.Verify(x => x.Save(), Times.Exactly(3));
        }

        [Fact]
        public void ImportSubmissionLinksRejectsInvalidImportFilesAndShowsAnErrorToTheUser()
        {
            var viewModel = PrepareFileUpload<ImportSubmissionLinksViewModel>();

            var controller = CreateAdminController();
            _bulkImporter.Setup(x => x.Execute(_uploadFile.Object.InputStream)).Throws<ArgumentException>();

            controller.ImportSubmissionLinks(viewModel);

            Assert.Equal(1, controller.ModelState["generalError"].Errors.Count);
        }

        [Fact]
        public void ImportSubmissionLinksSetsTheLinkInTheCorresponsingJournal()
        {
            var viewModel = PrepareFileUpload<ImportSubmissionLinksViewModel>();

            var data = SubmissionPageLinkStubs.Links();
            var journals = SubmissionPageLinkStubs.Journals();
            var controller = CreateAdminController(journalRepository: CreateJournalRepository(journals));

            _bulkImporter.Setup(x => x.Execute(_uploadFile.Object.InputStream)).Returns(data);

            controller.ImportSubmissionLinks(viewModel);

            for (int i = 0; i < data.Count; i++)
            {
                Assert.Equal(data[i].Url, journals[i].SubmissionPageLink);
            }
        }

        [Fact]
        public void ImportSubmissionLinksVerifiesDatTheSubmissionLinkDomainCorrespondsWithTheJournalDomain()
        {
            var viewModel = PrepareFileUpload<ImportSubmissionLinksViewModel>();

            var data = SubmissionPageLinkStubs.InvalidLinks();
            var journals = SubmissionPageLinkStubs.Journals();
            var controller = CreateAdminController(journalRepository: CreateJournalRepository(journals));

            _bulkImporter.Setup(x => x.Execute(_uploadFile.Object.InputStream)).Returns(data);

            controller.ImportSubmissionLinks(viewModel);

            for (int i = 0; i < data.Count - 1; i++)
            {
                Assert.NotEqual(data[i].Url, journals[i].SubmissionPageLink);
            }
        }

        #region Private Methods

        private static JournalsImport CreateJournalsImport()
        {
            return new JournalsImport(Mock.Of<IJournalRepository>(), Mock.Of<ILanguageRepository>(), Mock.Of<ICountryRepository>(), Mock.Of<ISubjectRepository>(), Mock.Of<IPublisherRepository>(), new GeneralImportSettings());
        }

        private static JournalsExport CreateJournalsExport()
        {
            return CreateJournalsExport(Mock.Of<IJournalRepository>());
        }

        private static JournalsExport CreateJournalsExport(IJournalRepository journalRepository)
        {
            return new JournalsExport(journalRepository);
        }

        private static DoajImport CreateDoajImport()
        {
            return new DoajImport(new DoajSettings(), Mock.Of<IBlockedISSNRepository>());
        }

        private static IJournalRepository CreateJournalRepository()
        {
            return CreateJournalRepository(CreateJournals());
        }

        private static IJournalRepository CreateJournalRepository(IList<Journal> journals)
        {
            var journalRepository = new Mock<IJournalRepository>();
            journalRepository.Setup(j => j.AllIncluding(It.IsAny<Expression<Func<Journal, object>>[]>()))
                .Returns(journals);
            journalRepository.Setup(j => j.SearchByISSN(It.IsAny<IEnumerable<string>>()))
                .Returns<IEnumerable<string>>(x => journals.Where(j => x.Contains(j.ISSN)).AsQueryable());
            journalRepository.Setup(j => j.FindByIssn(It.IsAny<string>()))
                .Returns<string>(issn => journals.FirstOrDefault(j => j.ISSN == issn));
            journalRepository.Setup(j => j.AllWhereIncluding(It.IsAny<Expression<Func<Journal,bool>>>(), It.IsAny<Expression<Func<Journal, object>>[]>()))
                .Returns(journals.Where(j => j.OpenAccess).ToList());

            return journalRepository.Object;
        }

        private AdminController CreateAdminController(JournalsImport journalsImport = null, UlrichsImport ulrichsImport = null, DoajImport doajImport = null, JournalsExport journalsExport = null, IJournalRepository journalRepository = null, IUserProfileRepository userProfileRepository = null, IAuthentication authentication = null, IInstitutionRepository institutionRepository = null, IBlockedISSNRepository blockedIssnRepository = null, IBaseScoreCardRepository baseScoreCardRepository = null, IValuationScoreCardRepository valuationScoreCardRepository = null)
        {
            _bulkImporter = new Mock<IBulkImporter<SubmissionPageLink>>();
            _institutionImporter = new Mock<IBulkImporter<Institution>>();
            return new AdminController(journalsImport ?? CreateJournalsImport(), ulrichsImport ?? CreateUlrichsImport(), doajImport ?? CreateDoajImport(), journalsExport ?? CreateJournalsExport(), journalRepository ?? CreateJournalRepository(), userProfileRepository ?? Mock.Of<IUserProfileRepository>(), authentication ?? Mock.Of<IAuthentication>(), institutionRepository ?? Mock.Of<IInstitutionRepository>(), blockedIssnRepository ?? Mock.Of<IBlockedISSNRepository>(), baseScoreCardRepository ?? Mock.Of<IBaseScoreCardRepository>(), valuationScoreCardRepository ?? Mock.Of<IValuationScoreCardRepository>(), _bulkImporter.Object, _institutionImporter.Object);
        }

        private static UlrichsImport CreateUlrichsImport()
        {
            var ulrichsSettings = new UlrichsSettings();

            return new Mock<UlrichsImport>(new UlrichsClient(ulrichsSettings), new UlrichsCache(ulrichsSettings), Mock.Of<IBlockedISSNRepository>()).Object;
        }

        private static MoveScoreCardsViewModel CreateMoveScoreCardsViewModel()
        {
            return new MoveScoreCardsViewModel
            {
                NewIssn = NewIssn,
                OldIssn = OldIssn
            };
        }

        private static List<Journal> CreateJournals()
        {
            return new List<Journal>
            {
                new Journal
                {
                    Title = "027.7 : Zeitschrift fuer Bibliothekskultur",
                    ISSN = OldIssn,
                    Link = "http://www.0277.ch/ojs/index.php/cdrs_0277",
                    DateAdded = DateTime.Parse("2-10-2013 9:52:51"),
                    Country = new Country
                    {
                        Name = "Switzerland"
                    },
                    Publisher = new Publisher
                    {
                        Name = "<none indicated>"
                    },
                    Languages = new List<Language>
                    {
                        new Language
                        {
                            Name = "English"
                        },
                        new Language
                        {
                            Name = "German"
                        }
                    },
                    Subjects = new List<Subject>
                    {
                        new Subject
                        {
                            Name = "library and information sciences"
                        }
                    },
                    DataSource = "DOAJ",
                    OpenAccess = true
                },
                new Journal
                {
                    Title = "16:9",
                    ISSN = NewIssn,
                    Link = "http://www.16-9.dk",
                    DateAdded = DateTime.Parse("2-10-2013 9:52:51"),
                    Country = new Country
                    {
                        Name = "Denmark"
                    },
                    Publisher = new Publisher
                    {
                        Name = "Springer"
                    },
                    Languages = new List<Language>
                    {
                        new Language
                        {
                            Name = "English"
                        },
                        new Language
                        {
                            Name = "Danish"
                        }
                    },
                    Subjects = new List<Subject>
                    {
                        new Subject
                        {
                            Name = "motion pictures"
                        },
                        new Subject
                        {
                            Name = "films"
                        }
                    },
                    DataSource = "Ulrich",
                    OpenAccess = true
                },
                new Journal
                {
                    Title = "ACIMED",
                    ISSN = "1024-9435",
                    Link = "http://scielo.sld.cu/scielo.php?script=sci_serial&pid=1024-9435&lng=en&nrm=iso",
                    DateAdded = DateTime.Parse("2-10-2013 9:52:51"),
                    Country = new Country
                    {
                        Name = "Cuba"
                    },
                    Publisher = new Publisher
                    {
                        Name = "Centro Nacional de Información de Ciencias Médicas"
                    },
                    Languages = new List<Language>
                    {
                        new Language
                        {
                            Name = "<none indicated>"
                        }
                    },
                    Subjects = new List<Subject>
                    {
                        new Subject
                        {
                            Name = "health sciences"
                        }
                    },
                    DataSource = "Ulrich",
                    OpenAccess = false
                }
            };
        }

        private static RemoveBaseScoreCardViewModel CreateRemoveBaseScoreCardViewModel()
        {
            return new RemoveBaseScoreCardViewModel
            {
                Id = 5
            };
        }

        private static RemoveValuationScoreCardViewModel CreateRemoveValuationScoreCardViewModel()
        {
            return new RemoveValuationScoreCardViewModel
            {
                Id = 7
            };
        }

        T PrepareFileUpload<T>() where T: IFileUploadViewModel, new()
        {
            _uploadFile = new Mock<HttpPostedFileBase>();
            return new T { File = _uploadFile.Object };
        }

        #endregion

    }
}