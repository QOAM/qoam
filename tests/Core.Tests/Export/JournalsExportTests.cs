namespace QOAM.Core.Tests.Export
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq.Expressions;
    using System.Text;

    using Moq;

    using QOAM.Core.Export;
    using Repositories;
    using TestHelpers;
    using Xunit;

    public class JournalsExportTests
    {
        const string ExpectedJournalsCsv = "\"sep=;\"\r\nTitle;ISSN;Link;Date Added;Country;Publisher;In DOAJ;Languages;Subjects;Score;No-Fee Journal;Number of Articles (Doi Count);Score cards in 2020\r\n027.7 : Zeitschrift fuer Bibliothekskultur;2296-0597;http://www.0277.ch/ojs/index.php/cdrs_0277;2-10-2013 09:52:51;Switzerland;<none indicated>;Yes;English,German;library and information sciences;0,0;No;0;1\r\n16:9;1603-5194;http://www.16-9.dk;2-10-2013 09:52:51;Denmark;Springer;No;English,Danish;motion pictures,films;0,0;No;21;1\r\n";

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
        [UseCulture("nl-NL")]
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
            var csvContent = Encoding.UTF8.GetString(memoryStream.ToArray());
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

        static List<Journal> CreateJournals()
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
                           DataSource = "JournalTOCs",
                           InDoaj = true,
                           Languages = new List<Language> { new Language { Name = "English" }, new Language { Name = "German" } },
                           Subjects = new List<Subject> { new Subject { Name = "library and information sciences" } },
                           DoajSeal = false,
                           NumberOfArticles = 0,
                           ArticlesPerYear = new List<ArticlesPerYear>(),
                           ValuationScoreCards = new List<ValuationScoreCard> 
                           {
                               new ValuationScoreCard { DatePublished = new DateTime(2018, 5, 5) },
                               new ValuationScoreCard { DatePublished = new DateTime(2019, 7, 5) },
                               new ValuationScoreCard { DatePublished = new DateTime(2020, 1, 5) }
                           }
                       },
                       new Journal
                       {
                           Title = "16:9",
                           ISSN = "1603-5194",
                           Link = "http://www.16-9.dk",
                           DateAdded = DateTime.Parse("2-10-2013 9:52:51"),
                           Country = new Country { Name = "Denmark" },
                           Publisher = new Publisher { Name = "Springer" },
                           DataSource = "JournalTOCs",
                           InDoaj = false,
                           Languages = new List<Language> { new Language { Name = "English" }, new Language { Name = "Danish" } },
                           Subjects = new List<Subject> { new Subject { Name = "motion pictures" }, new Subject { Name = "films" } },
                           DoajSeal = true,
                           NumberOfArticles = 21,
                           ArticlesPerYear = new List<ArticlesPerYear>
                           {
                               new ArticlesPerYear { Year = 2020, NumberOfArticles = 2 },
                               new ArticlesPerYear { Year = 2019, NumberOfArticles = 19 }
                           },
                           ValuationScoreCards = new List<ValuationScoreCard> 
                           {
                               new ValuationScoreCard { DatePublished = new DateTime(2018, 7, 5) },
                               new ValuationScoreCard { DatePublished = new DateTime(2019, 1, 5) },
                               new ValuationScoreCard { DatePublished = new DateTime(2020, 5, 5) }
                           }
                       }
                   };
        }
    }
}