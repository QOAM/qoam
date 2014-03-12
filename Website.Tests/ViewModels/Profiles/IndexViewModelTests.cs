namespace RU.Uci.OAMarket.Website.Tests.ViewModels.Profiles
{
    using System.Web.Helpers;

    using RU.Uci.OAMarket.Domain.Repositories.Filters;
    using RU.Uci.OAMarket.Website.ViewModels.Profiles;

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
            Assert.Equal(UserProfileSortMode.NumberOfJournalScoreCards, indexViewModel.SortBy);
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