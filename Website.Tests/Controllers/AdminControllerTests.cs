namespace QOAM.Website.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;

    using Moq;

    using QOAM.Core;
    using QOAM.Core.Export;
    using QOAM.Core.Import;
    using QOAM.Core.Repositories;
    using QOAM.Website.Controllers;
    using QOAM.Website.Helpers;
    using QOAM.Website.ViewModels.Import;

    using Xunit;

    public class AdminControllerTests
    {
        private const string ExpectedJournalsCsv = @"Title;ISSN;Link;DateAdded;Country;Publisher;Languages;Subjects
027.7 : Zeitschrift fuer Bibliothekskultur;2296-0597;http://www.0277.ch/ojs/index.php/cdrs_0277;2-10-2013 09:52:51;Switzerland;<none indicated>;English,German;library and information sciences
16:9;1603-5194;http://www.16-9.dk;2-10-2013 09:52:51;Denmark;Springer;English,Danish;motion pictures,films
";

        [Fact]
        public void ConstructorWithNullJournalsImportThrowsArgumentNullException()
        {
            // Arrange
            JournalsImport nullJournalsImport = null;

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new AdminController(nullJournalsImport, this.CreateUlrichsClient(), CreateDoajImport(), CreateJournalsExport(), Mock.Of<IJournalRepository>(), Mock.Of<IUserProfileRepository>(), Mock.Of<IAuthentication>(), Mock.Of<IInstitutionRepository>(), Mock.Of<IBlockedISSNRepository>()));
        }

        [Fact]
        public void ConstructorWithNullUlrichsImportThrowsArgumentNullException()
        {
            // Arrange
            UlrichsImport nullUlrichsImport = null;

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new AdminController(CreateJournalsImport(), nullUlrichsImport, CreateDoajImport(), CreateJournalsExport(), Mock.Of<IJournalRepository>(), Mock.Of<IUserProfileRepository>(), Mock.Of<IAuthentication>(), Mock.Of<IInstitutionRepository>(), Mock.Of<IBlockedISSNRepository>()));
        }

        [Fact]
        public void ConstructorWithNullDoajImportThrowsArgumentNullException()
        {
            // Arrange
            DoajImport nullDoajImport = null;

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new AdminController(CreateJournalsImport(), this.CreateUlrichsClient(), nullDoajImport, CreateJournalsExport(), Mock.Of<IJournalRepository>(), Mock.Of<IUserProfileRepository>(), Mock.Of<IAuthentication>(), Mock.Of<IInstitutionRepository>(), Mock.Of<IBlockedISSNRepository>()));
        }

        [Fact]
        public void ConstructorWithNullJournalsExportThrowsArgumentNullException()
        {
            // Arrange
            JournalsExport nullJournalsExport = null;

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new AdminController(CreateJournalsImport(), this.CreateUlrichsClient(), CreateDoajImport(), nullJournalsExport, Mock.Of<IJournalRepository>(), Mock.Of<IUserProfileRepository>(), Mock.Of<IAuthentication>(), Mock.Of<IInstitutionRepository>(), Mock.Of<IBlockedISSNRepository>()));
        }

        [Fact]
        public void ConstructorWithNullJournalRepositoryThrowsArgumentNullException()
        {
            // Arrange
            IJournalRepository nullJournalRepository = null;

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new AdminController(CreateJournalsImport(), this.CreateUlrichsClient(), CreateDoajImport(), CreateJournalsExport(), nullJournalRepository, Mock.Of<IUserProfileRepository>(), Mock.Of<IAuthentication>(), Mock.Of<IInstitutionRepository>(), Mock.Of<IBlockedISSNRepository>()));
        }

        [Fact]
        public void ConstructorWithNullUserProfileRepositoryThrowsArgumentNullException()
        {
            // Arrange
            IUserProfileRepository nullUserProfileRepository = null;

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new AdminController(CreateJournalsImport(), this.CreateUlrichsClient(), CreateDoajImport(), CreateJournalsExport(), Mock.Of<IJournalRepository>(), nullUserProfileRepository, Mock.Of<IAuthentication>(), Mock.Of<IInstitutionRepository>(), Mock.Of<IBlockedISSNRepository>()));
        }

        [Fact]
        public void ConstructorWithNullAuthenticationThrowsArgumentNullException()
        {
            // Arrange
            IAuthentication nullAuthentication = null;

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new AdminController(CreateJournalsImport(), this.CreateUlrichsClient(), CreateDoajImport(), CreateJournalsExport(), Mock.Of<IJournalRepository>(), Mock.Of<IUserProfileRepository>(), nullAuthentication, Mock.Of<IInstitutionRepository>(), Mock.Of<IBlockedISSNRepository>()));
        }

        [Fact]
        public void DownloadReturnsFileContentResultWithCorrectCsvFileForAllJournals()
        {
            // Arrange
            var adminController = this.CreateAdminController(CreateJournalsExport(CreateJournalsRepository(CreateJournals())));

            // Act
            var fileContentResult = adminController.Download();

            // Assert
            var fileContentResultAsString = Encoding.Default.GetString(fileContentResult.FileContents);
            Assert.Equal(ExpectedJournalsCsv, fileContentResultAsString);
        }

        [Fact]
        public void DownloadReturnsFileContentResultWithCsvContentType()
        {
            // Arrange
            var adminController = this.CreateAdminController(CreateJournalsExport(CreateJournalsRepository(CreateJournals())));

            // Act
            var fileContentResult = adminController.Download();

            // Assert
            Assert.Equal("application/csv", fileContentResult.ContentType);
        }

        [Fact]
        public void DownloadReturnsFileContentResultWithFileDownloadNameSet()
        {
            // Arrange
            var adminController = this.CreateAdminController(CreateJournalsExport(CreateJournalsRepository(CreateJournals())));

            // Act
            var fileContentResult = adminController.Download();

            // Assert
            Assert.Equal("journals.csv", fileContentResult.FileDownloadName);
        }

        [Fact]
        public void CheckWithValidModelWillSetFoundISSNsToISSNsThatAreInJournalRepository()
        {
            // Arrange
            var adminController = this.CreateAdminController(CreateJournalsRepository(CreateJournals()));

            // Act
            var viewResult = adminController.Check(new CheckViewModel { ISSNs = "2296-0597\n1603-5194\n1443-8675\n8872-3754" });

            // Assert
            Assert.Equal(new[] { "2296-0597", "1603-5194" }, ((CheckViewModel)viewResult.Model).FoundISSNs);
        }

        [Fact]
        public void CheckWithValidModelWillSetNotFoundISSNsToISSNsThatAreNotInJournalRepository()
        {
            // Arrange
            var adminController = this.CreateAdminController(CreateJournalsRepository(CreateJournals()));

            // Act
            var viewResult = adminController.Check(new CheckViewModel { ISSNs = "2296-0597\n1603-5194\n1443-8675\n8872-3754" });

            // Assert
            Assert.Equal(new[] { "1443-8675", "8872-3754" }, ((CheckViewModel)viewResult.Model).NotFoundISSNs);
        }

        [Fact]
        public void CheckWithValidModelWillReturnUniqueISSNs()
        {
            // Arrange
            var adminController = this.CreateAdminController(CreateJournalsRepository(CreateJournals()));

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

        private static IJournalRepository CreateJournalsRepository(IList<Journal> journals)
        {
            var mockJournalRepository = new Mock<IJournalRepository>();
            mockJournalRepository.Setup(j => j.AllIncluding(It.IsAny<Expression<Func<Journal, object>>[]>())).Returns(journals);
            mockJournalRepository.Setup(j => j.SearchByISSN(It.IsAny<IEnumerable<string>>())).Returns<IEnumerable<string>>(x => journals.Where(j => x.Contains(j.ISSN)).AsQueryable());

            return mockJournalRepository.Object;
        }

        private static List<Journal> CreateJournals()
        {
            return new List<Journal>
                   {
                       new Journal
                       {
                           Title = "027.7 : Zeitschrift fuer Bibliothekskultur",
                           ISSN = "2296-0597",
                           Link = "http://www.0277.ch/ojs/index.php/cdrs_0277",
                           DateAdded = DateTime.Parse("2-10-2013 9:52:51"),
                           Country = new Country { Name = "Switzerland" },
                           Publisher = new Publisher { Name = "<none indicated>" },
                           Languages = new List<Language> { new Language { Name = "English" }, new Language { Name = "German" } },
                           Subjects = new List<Subject> { new Subject { Name = "library and information sciences" } }
                       },
                       new Journal
                       {
                           Title = "16:9",
                           ISSN = "1603-5194",
                           Link = "http://www.16-9.dk",
                           DateAdded = DateTime.Parse("2-10-2013 9:52:51"),
                           Country = new Country { Name = "Denmark" },
                           Publisher = new Publisher { Name = "Springer" },
                           Languages = new List<Language> { new Language { Name = "English" }, new Language { Name = "Danish" } },
                           Subjects = new List<Subject> { new Subject { Name = "motion pictures" }, new Subject { Name = "films" } }
                       }
                   };
        }

        private AdminController CreateAdminController(IJournalRepository journalRepository)
        {
            return new AdminController(CreateJournalsImport(), this.CreateUlrichsClient(), CreateDoajImport(), CreateJournalsExport(journalRepository), journalRepository, Mock.Of<IUserProfileRepository>(), Mock.Of<IAuthentication>(), Mock.Of<IInstitutionRepository>(), Mock.Of<IBlockedISSNRepository>());
        }

        private AdminController CreateAdminController(JournalsExport journalsExport)
        {
            return new AdminController(CreateJournalsImport(), this.CreateUlrichsClient(), CreateDoajImport(), journalsExport, Mock.Of<IJournalRepository>(), Mock.Of<IUserProfileRepository>(), Mock.Of<IAuthentication>(), Mock.Of<IInstitutionRepository>(), Mock.Of<IBlockedISSNRepository>());
        }

        private AdminController CreateAdminController()
        {
            return new AdminController(CreateJournalsImport(), this.CreateUlrichsClient(), CreateDoajImport(), CreateJournalsExport(), Mock.Of<IJournalRepository>(), Mock.Of<IUserProfileRepository>(), Mock.Of<IAuthentication>(), Mock.Of<IInstitutionRepository>(), Mock.Of<IBlockedISSNRepository>());
        }

        private UlrichsImport CreateUlrichsClient()
        {
            var ulrichsSettings = new UlrichsSettings();

            return new Mock<UlrichsImport>(new UlrichsClient(ulrichsSettings), new UlrichsCache(ulrichsSettings), Mock.Of<IBlockedISSNRepository>()).Object;
        }
    }
}