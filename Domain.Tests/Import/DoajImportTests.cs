namespace RU.Uci.OAMarket.Domain.Tests.Import
{
    using RU.Uci.OAMarket.Domain.Import;
    using RU.Uci.OAMarket.Domain.Tests.Import.Resources;

    using Xunit;

    public class DoajImportTests
    {
        [Fact]
        public void ParseJournalsWillParseJournalsFromCsv()
        {
            // Arrange
            var doajImport = new DoajImport(new DoajSettings());

            // Act
            var journals = doajImport.ParseJournals(GetDoajCsv());

            // Assert
            Assert.True(journals.Count > 0);
        }

        private static string GetDoajCsv()
        {
            return ResourceReader.GetContentsOfResource("doaj.csv");
        }
    }
}