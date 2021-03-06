﻿namespace QOAM.Core.Tests.Export
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq.Expressions;
    using System.Text;

    using Moq;

    using QOAM.Core.Export;
    using QOAM.Core.Repositories;
    using TestHelpers;
    using Xunit;

    public class JournalsExportTests
    {
        const string ExpectedJournalsCsv = "\"sep=;\"\r\nTitle;ISSN;Link;Date Added;Country;Publisher;Data source;Languages;Subjects;DOAJ Seal;Plan S Journal;Score;No-Fee Journal;Score cards in 2019;Score cards in 2020;Articles in 2019;Articles in 2020\r\n027.7 : Zeitschrift fuer Bibliothekskultur;2296-0597;http://www.0277.ch/ojs/index.php/cdrs_0277;2-10-2013 09:52:51;Switzerland;<none indicated>;DOAJ;English,German;library and information sciences;No;No;0,0;No;1;1;0;0\r\n16:9;1603-5194;http://www.16-9.dk;2-10-2013 09:52:51;Denmark;Springer;Ulrich;English,Danish;motion pictures,films;Yes;No;0,0;No;1;1;19;2\r\n";

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
                           DataSource = "DOAJ",
                           Languages = new List<Language> { new Language { Name = "English" }, new Language { Name = "German" } },
                           Subjects = new List<Subject> { new Subject { Name = "library and information sciences" } },
                           DoajSeal = false,
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
                           DataSource = "Ulrich",
                           Languages = new List<Language> { new Language { Name = "English" }, new Language { Name = "Danish" } },
                           Subjects = new List<Subject> { new Subject { Name = "motion pictures" }, new Subject { Name = "films" } },
                           DoajSeal = true,
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