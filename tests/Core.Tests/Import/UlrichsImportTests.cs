namespace QOAM.Core.Tests.Import
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Moq;
    using QOAM.Core.Import;
    using QOAM.Core.Tests.Import.Resources;
    using Repositories;
    using Xunit;

    public class UlrichsImportTests
    {
        [Fact]
        public void ParseJournalsWithJournalTypeIsAllAndXmlWithoutAlternateEditionsWillParseAllJournalsFromXml()
        {
            // Arrange
            var ulrichsImport = CreateUlrichsImport();

            // Act
            var journals = ulrichsImport.ParseJournals(GetUlrichsXml(), UlrichsImport.UlrichsJournalType.All);

            // Assert
            Assert.Equal(281, journals.Count);
        }

        [Fact]
        public void ParseJournalsWithJournalTypeIsAllAndXmlWithAlternateEditionsWillParseAllJournalsAndAlternateEditionsFromXml()
        {
            // Arrange
            var ulrichsImport = CreateUlrichsImport();

            // Act
            var journals = ulrichsImport.ParseJournals(GetUlrichsXmlWithAlternateEditions(), UlrichsImport.UlrichsJournalType.All);

            // Assert
            Assert.Equal(483, journals.Count);
        }

        [Fact]
        public void ParseJournalsWithJournalTypeIsOpenAccessAndXmlWithoutAlternateEditionsWillParseOnlyOpenAccessJournalsFromXml()
        {
            // Arrange
            var ulrichsImport = CreateUlrichsImport();

            // Act
            var journals = ulrichsImport.ParseJournals(GetUlrichsXml(), UlrichsImport.UlrichsJournalType.OpenAccess);

            // Assert
            Assert.Equal(77, journals.Count);
            Assert.True(journals.All(j => j.OpenAccess));
        }

        [Fact]
        public void ParseJournalsWithJournalTypeIsOpenAccessAndXmlWithAlternateEditionsWillParseOnlyOpenAccessJournalsFromXml()
        {
            // Arrange
            var ulrichsImport = CreateUlrichsImport();

            // Act
            var journals = ulrichsImport.ParseJournals(GetUlrichsXmlWithAlternateEditions(), UlrichsImport.UlrichsJournalType.OpenAccess);

            // Assert
            Assert.Equal(123, journals.Count);
            Assert.True(journals.All(j => j.OpenAccess));
        }

        [Fact]
        public void ParseJournalWithJournalWithoutAlternateEditionsReturnsCorrectlyParsedJournal()
        {
            // Arrange
            var ulrichsImport = CreateUlrichsImport();

            // Act
            var journals = ulrichsImport.ParseJournal(XElement.Parse(GetUlrichsJournalXmlWithoutAlternateEditions()));

            // Assert
            AssertEqualJournal(GetExpectedJournal(), journals.First());
        }

        [Fact]
        public void ParseJournalWithJournalWithoutAlternateEditionsReturnsOneJournal()
        {
            // Arrange
            var ulrichsImport = CreateUlrichsImport();

            // Act
            var journals = ulrichsImport.ParseJournal(XElement.Parse(GetUlrichsJournalXmlWithoutAlternateEditions()));

            // Assert
            Assert.Equal(1, journals.Count());
        }

        [Fact]
        public void ParseJournalWithJournalWithAlternateEditionsReturnsCorrectlyParsedJournal()
        {
            // Arrange
            var ulrichsImport = CreateUlrichsImport();

            // Act
            var journals = ulrichsImport.ParseJournal(XElement.Parse(GetUlrichsJournalXmlWithAlternateEditions()));

            // Assert
            AssertEqualJournal(GetExpectedJournal(), journals.First());
        }

        [Fact]
        public void ParseJournalWithJournalWithAlternateEditionsReturnsMultipleJournals()
        {
            // Arrange
            var ulrichsImport = CreateUlrichsImport();

            // Act
            var journals = ulrichsImport.ParseJournal(XElement.Parse(GetUlrichsJournalXmlWithAlternateEditions()));

            // Assert
            Assert.Equal(2, journals.Count());
        }

        [Fact]
        public void ParseJournalWithJournalWithCeasedAlternateEditionReturnsOneJournal()
        {
            // Arrange
            var ulrichsImport = CreateUlrichsImport();

            // Act
            var journals = ulrichsImport.ParseJournal(XElement.Parse(GetUlrichsJournalXmlWithCeasedAlternateEdition()));

            // Assert
            Assert.Equal(1, journals.Count());
            AssertEqualJournal(GetExpectedJournal(), journals.First());
        }

        [Fact]
        public void ParseJournalWithJournalWithoutPublisherReturnsJournalWithDefaultPublisherName()
        {
            // Arrange
            var ulrichsImport = CreateUlrichsImport();

            // Act
            var journals = ulrichsImport.ParseJournal(XElement.Parse(GetUlrichsJournalXmlWithoutPublisher()));

            // Assert
            var parsedJournal = journals.First();
            Assert.Equal("<none indicated>", parsedJournal.Publisher.Name);
        }

        private static Journal GetExpectedJournal()
        {
            return new Journal
                   {
                       Title = "Postepy Rehabilitacji",
                       ISSN = "0860-6161",
                       Link = "http://www.asla.org.au/pubs/access/access.htm",
                       Publisher = new Publisher { Name = "L E D S Publishing Co. Inc." },
                       Country = new Country { Name = "Poland" },
                       Languages = new List<Language> { new Language { Name = "English" }, new Language { Name = "Polish" } },
                       Subjects = new List<Subject> { new Subject { Name = "medical sciences-physical medicine and rehabilitation" } },
                       OpenAccess = false
                   };
        }

        private static void AssertEqualJournal(Journal expectedJournal, Journal parsedJournal)
        {
            Assert.Equal(expectedJournal.Title, parsedJournal.Title);
            Assert.Equal(expectedJournal.ISSN, parsedJournal.ISSN);
            Assert.Equal(expectedJournal.Link, parsedJournal.Link);
            Assert.Equal(expectedJournal.Publisher.Name, parsedJournal.Publisher.Name);
            Assert.Equal(expectedJournal.Country.Name, parsedJournal.Country.Name);
            Assert.Equal(expectedJournal.Languages.Select(l => l.Name), parsedJournal.Languages.Select(l => l.Name));
            Assert.Equal(expectedJournal.Subjects.Select(s => s.Name), parsedJournal.Subjects.Select(s => s.Name));
            Assert.Equal(expectedJournal.OpenAccess, parsedJournal.OpenAccess);
        }

        private static string GetUlrichsXml()
        {
            return ResourceReader.GetContentsOfResource("ulrichs.xml");
        }

        private static string GetUlrichsXmlWithAlternateEditions()
        {
            return ResourceReader.GetContentsOfResource("ulrichswithalternateeditions.xml");
        }

        private static string GetUlrichsJournalXmlWithoutAlternateEditions()
        {
            return ResourceReader.GetContentsOfResource("ulrichsjournalwithoutalternateeditions.xml");
        }

        private static string GetUlrichsJournalXmlWithAlternateEditions()
        {
            return ResourceReader.GetContentsOfResource("ulrichsjournalwithalternateeditions.xml");
        }

        private static string GetUlrichsJournalXmlWithCeasedAlternateEdition()
        {
            return ResourceReader.GetContentsOfResource("ulrichsjournalwithceasedalternateedition.xml");
        }

        private static string GetUlrichsJournalXmlWithoutPublisher()
        {
            return ResourceReader.GetContentsOfResource("ulrichsjournalwithoutpublisher.xml");
        }

        private static UlrichsImport CreateUlrichsImport()
        {
            var ulrichsSettings = new UlrichsSettings();
            return new UlrichsImport(new UlrichsClient(ulrichsSettings), new UlrichsCache(ulrichsSettings), Mock.Of<IBlockedISSNRepository>());
        }
    }
}