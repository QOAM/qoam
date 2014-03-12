namespace RU.Uci.OAMarket.Website.Tests.ViewModels.Institutions
{
    using System.Web.Helpers;

    using RU.Uci.OAMarket.Domain.Repositories.Filters;
    using RU.Uci.OAMarket.Website.ViewModels.Institutions;

    using Xunit;

    public class DetailsViewModelTests
    {
        [Fact]
        public void ConstructorSetsSortByToNumberOfJournalScoreCards()
        {
            // Arrange
            var indexViewModel = new DetailsViewModel();

            // Act

            // Assert
            Assert.Equal(UserProfileSortMode.NumberOfJournalScoreCards, indexViewModel.SortBy);
        }

        [Fact]
        public void ConstructorSetsSortDirectionToDescending()
        {
            // Arrange
            var indexViewModel = new DetailsViewModel();

            // Act

            // Assert
            Assert.Equal(SortDirection.Descending, indexViewModel.Sort);
        }
    }
}