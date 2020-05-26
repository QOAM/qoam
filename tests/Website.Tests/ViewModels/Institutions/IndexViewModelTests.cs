namespace QOAM.Website.Tests.ViewModels.Institutions
{
    using System.Web.Helpers;
    using Website.ViewModels.Institutions;
    using Xunit;

    public class IndexViewModelTests
    {
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