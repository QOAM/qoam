namespace QOAM.Website.Tests.ViewModels.Profiles
{
    using System.Web.Helpers;

    using QOAM.Core.Repositories.Filters;
    using Website.ViewModels.Profiles;
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
            Assert.Equal(UserProfileSortMode.NumberOfBaseJournalScoreCards, indexViewModel.SortBy);
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