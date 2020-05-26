namespace QOAM.Website.Tests.ViewModels.Institutions
{
    using System.Web.Helpers;
    using Website.ViewModels.Institutions;
    using Xunit;

    public class DetailsViewModelTests
    {

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