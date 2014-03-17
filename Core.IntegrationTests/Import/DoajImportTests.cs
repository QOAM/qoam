namespace QOAM.Core.IntegrationTests.Import
{
    using QOAM.Core.Import;

    using Xunit;

    public class DoajImportTests
    {
        [Fact]
        public void GetJournalsWillImportJournals()
        {
            // Arrange
            var doajImport = new DoajImport(ImportSettings.Current.Doaj);

            // Act
            var journalsImportResult = doajImport.GetJournals();

            // Assert
            Assert.True(journalsImportResult.Count > 0);
        }
    }
}