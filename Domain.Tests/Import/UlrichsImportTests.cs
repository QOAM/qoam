namespace RU.Uci.OAMarket.Domain.Tests.Import
{
    using RU.Uci.OAMarket.Domain.Import;
    using RU.Uci.OAMarket.Domain.Tests.Import.Resources;

    using Xunit;

    public class UlrichsImportTests
    {
        [Fact]
        public void ParseJournalsWithJournalTypeIsAllWillAllParseJournalsFromXml()
        {
            // Arrange
            var ulrichsSettings = new UlrichsSettings();
            var ulrichsImport = new UlrichsImport(new UlrichsClient(ulrichsSettings), new UlrichsCache(ulrichsSettings));

            // Act
            var journals = ulrichsImport.ParseJournals(GetUlrichsXml(), UlrichsImport.UlrichsJournalType.All);

            // Assert
            Assert.Equal(281, journals.Count);
        }
        
        [Fact]
        public void ParseJournalsWithJournalTypeIsOpenAccessWillAllParseOnlyOpenAccessJournalsFromXml()
        {
            // Arrange
            var ulrichsSettings = new UlrichsSettings();
            var ulrichsImport = new UlrichsImport(new UlrichsClient(ulrichsSettings), new UlrichsCache(ulrichsSettings));

            // Act
            var journals = ulrichsImport.ParseJournals(GetUlrichsXml(), UlrichsImport.UlrichsJournalType.OpenAccess);

            // Assert
            Assert.Equal(77, journals.Count);
        }

        private static string GetUlrichsXml()
        {
            return ResourceReader.GetContentsOfResource("ulrichs.xml");
        }
    }
}