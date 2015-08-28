namespace QOAM.Core.Tests.Import
{
    using Moq;
    using QOAM.Core.Import;
    using QOAM.Core.Tests.Import.Resources;
    using Repositories;
    using Xunit;

    public class DoajImportTests
    {
        [Fact]
        public void ParseJournalsWillParseJournalsFromCsv()
        {
            // Arrange
            var doajImport = new DoajImport(new DoajSettings(), Mock.Of<IBlockedISSNRepository>());

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