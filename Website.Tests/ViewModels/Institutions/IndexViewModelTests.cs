namespace QOAM.Website.Tests.ViewModels.Institutions
{
    using System.Web.Helpers;

    using QOAM.Core.Repositories.Filters;
    using QOAM.Website.ViewModels.Institutions;

    using Xunit;

    public class IndexViewModelTests
    {
        [Fact]
        public void ConstructorSetsSortByToNumberOfJournalScoreCards()
        {
            // Arrange
            var indexViewModel = new IndexViewModel();

            // Act

            // Assert
            Assert.Equal(InstitutionSortMode.NumberOfBaseJournalScoreCards, indexViewModel.SortBy);
        }

        [Fact]
        public void ConstructorSetsSortDirectionToDescending()
        {
            // Arrange
            var indexViewModel = new IndexViewModel();

            // Act

            // Assert
            Assert.Equal(SortDirection.Descending, indexViewModel.Sort);
        }
    }
}