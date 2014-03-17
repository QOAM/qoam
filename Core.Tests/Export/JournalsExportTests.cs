namespace QOAM.Core.Tests.Export
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq.Expressions;
    using System.Text;

    using Moq;

    using QOAM.Core.Export;
    using QOAM.Core.Repositories;

    using Xunit;

    public class JournalsExportTests
    {
        private const string ExpectedJournalsCsv = @"Title;ISSN;Link;DateAdded;Country;Publisher;Languages;Subjects
027.7 : Zeitschrift fuer Bibliothekskultur;2296-0597;http://www.0277.ch/ojs/index.php/cdrs_0277;2-10-2013 9:52:51;Switzerland;<none indicated>;English,German;library and information sciences
16:9;1603-5194;http://www.16-9.dk;2-10-2013 9:52:51;Denmark;Springer;English,Danish;motion pictures,films
";

        [Fact]
        public void ConstructorWithNullJournalRepositoryThrowsArgumentNullException()
        {
            // Arrange
            IJournalRepository nullJournalRepository = null;

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new JournalsExport(nullJournalRepository));
        }

        [Fact]
        public void ExportAllJournalsExportAllJournalsInCorrectCsvFormat()
        {
            // Arrange
            var mockJournalRepository = new Mock<IJournalRepository>();
            mockJournalRepository.Setup(j => j.AllIncluding(It.IsAny<Expression<Func<Journal, object>>[]>())).Returns(CreateJournals());

            var journalsExport = new JournalsExport(mockJournalRepository.Object);
            var memoryStream = new MemoryStream();

            // Act
            journalsExport.ExportAllJournals(memoryStream);

            // Assert
            var csvContent = Encoding.Default.GetString(memoryStream.ToArray());
            Assert.Equal(ExpectedJournalsCsv, csvContent);
        }

        [Fact]
        public void ExportAllJournalsWithNullStreamThrowsArgumentNullException()
        {
            // Arrange
            var journalsExport = new JournalsExport(Mock.Of<IJournalRepository>());
            Stream nullStream = null;

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => journalsExport.ExportAllJournals(nullStream));
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
    }
}