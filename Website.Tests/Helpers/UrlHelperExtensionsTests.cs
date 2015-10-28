namespace QOAM.Website.Tests.Helpers
{
    using System.Web.Helpers;

    using QOAM.Core.Repositories.Filters;
    using Website.Helpers;
    using Xunit;

    public class UrlHelperExtensionsTests
    {
        [Theory]
        [InlineData(JournalSortMode.Name, SortDirection.Ascending, SortDirection.Descending)]
        [InlineData(JournalSortMode.Name, SortDirection.Descending, SortDirection.Ascending)]
        [InlineData(JournalSortMode.ValuationScore, SortDirection.Ascending, SortDirection.Descending)]
        [InlineData(JournalSortMode.ValuationScore, SortDirection.Descending, SortDirection.Ascending)]
        [InlineData(JournalSortMode.BaseScore, SortDirection.Ascending, SortDirection.Descending)]
        [InlineData(JournalSortMode.BaseScore, SortDirection.Descending, SortDirection.Ascending)]
        public void GetOrderDirectionWithJournalSortModeAndCurrentSortModeIsNewSortModeReturnsOppositeDirection(JournalSortMode sortMode, SortDirection sortDirection, SortDirection expectedSortDirection)
        {
            // Arrange
            var currentSortMode = sortMode;

            // Act
            var orderDirection = UrlHelperExtensions.GetOrderDirection(sortMode, currentSortMode, sortDirection);

            // Assert
            Assert.Equal(expectedSortDirection, orderDirection);
        }

        [Theory]
        [InlineData(JournalSortMode.Name, JournalSortMode.ValuationScore, SortDirection.Ascending)]
        [InlineData(JournalSortMode.ValuationScore, JournalSortMode.Name, SortDirection.Descending)]
        [InlineData(JournalSortMode.BaseScore, JournalSortMode.Name, SortDirection.Descending)]
        public void GetOrderDirectionWithJournalSortModeIsNotEqualToCurrentSortModeReturnsDefaultDirection(JournalSortMode newSortMode, JournalSortMode currentSortMode, SortDirection expectedSortDirection)
        {
            // Arrange

            // Act
            var orderDirection = UrlHelperExtensions.GetOrderDirection(newSortMode, currentSortMode, SortDirection.Ascending);

            // Assert
            Assert.Equal(expectedSortDirection, orderDirection);
        }

        [Theory]
        [InlineData(UserProfileSortMode.Name, SortDirection.Ascending, SortDirection.Descending)]
        [InlineData(UserProfileSortMode.Name, SortDirection.Descending, SortDirection.Ascending)]
        [InlineData(UserProfileSortMode.Institution, SortDirection.Ascending, SortDirection.Descending)]
        [InlineData(UserProfileSortMode.Institution, SortDirection.Descending, SortDirection.Ascending)]
        [InlineData(UserProfileSortMode.DateRegistered, SortDirection.Ascending, SortDirection.Descending)]
        [InlineData(UserProfileSortMode.DateRegistered, SortDirection.Descending, SortDirection.Ascending)]
        [InlineData(UserProfileSortMode.NumberOfBaseJournalScoreCards, SortDirection.Ascending, SortDirection.Descending)]
        [InlineData(UserProfileSortMode.NumberOfBaseJournalScoreCards, SortDirection.Descending, SortDirection.Ascending)]
        [InlineData(UserProfileSortMode.NumberOfValuationJournalScoreCards, SortDirection.Ascending, SortDirection.Descending)]
        [InlineData(UserProfileSortMode.NumberOfValuationJournalScoreCards, SortDirection.Descending, SortDirection.Ascending)]
        public void GetOrderDirectionWithUserProfileSortModeAndCurrentSortModeIsNewSortModeReturnsOppositeDirection(UserProfileSortMode sortMode, SortDirection sortDirection, SortDirection expectedSortDirection)
        {
            // Arrange
            var currentSortMode = sortMode;

            // Act
            var orderDirection = UrlHelperExtensions.GetOrderDirection(sortMode, currentSortMode, sortDirection);

            // Assert
            Assert.Equal(expectedSortDirection, orderDirection);
        }

        [Theory]
        [InlineData(UserProfileSortMode.Name, UserProfileSortMode.NumberOfBaseJournalScoreCards, SortDirection.Ascending)]
        [InlineData(UserProfileSortMode.Institution, UserProfileSortMode.Name, SortDirection.Ascending)]
        [InlineData(UserProfileSortMode.DateRegistered, UserProfileSortMode.Name, SortDirection.Descending)]
        [InlineData(UserProfileSortMode.NumberOfBaseJournalScoreCards, UserProfileSortMode.Name, SortDirection.Descending)]
        [InlineData(UserProfileSortMode.NumberOfValuationJournalScoreCards, UserProfileSortMode.Name, SortDirection.Descending)]
        public void GetOrderDirectionWithUserProfileSortModeIsNotEqualToCurrentSortModeReturnsDefaultDirection(UserProfileSortMode newSortMode, UserProfileSortMode currentSortMode, SortDirection expectedSortDirection)
        {
            // Arrange

            // Act
            var orderDirection = UrlHelperExtensions.GetOrderDirection(newSortMode, currentSortMode, SortDirection.Ascending);

            // Assert
            Assert.Equal(expectedSortDirection, orderDirection);
        }

        [Theory]
        [InlineData(InstitutionSortMode.Name, SortDirection.Ascending, SortDirection.Descending)]
        [InlineData(InstitutionSortMode.Name, SortDirection.Descending, SortDirection.Ascending)]
        [InlineData(InstitutionSortMode.NumberOfBaseJournalScoreCards, SortDirection.Ascending, SortDirection.Descending)]
        [InlineData(InstitutionSortMode.NumberOfBaseJournalScoreCards, SortDirection.Descending, SortDirection.Ascending)]
        [InlineData(InstitutionSortMode.NumberOfValuationJournalScoreCards, SortDirection.Ascending, SortDirection.Descending)]
        [InlineData(InstitutionSortMode.NumberOfValuationJournalScoreCards, SortDirection.Descending, SortDirection.Ascending)]
        public void GetOrderDirectionWithInstitutionSortModeAndCurrentSortModeIsNewSortModeReturnsOppositeDirection(InstitutionSortMode sortMode, SortDirection sortDirection, SortDirection expectedSortDirection)
        {
            // Arrange
            var currentSortMode = sortMode;

            // Act
            var orderDirection = UrlHelperExtensions.GetOrderDirection(sortMode, currentSortMode, sortDirection);

            // Assert
            Assert.Equal(expectedSortDirection, orderDirection);
        }

        [Theory]
        [InlineData(InstitutionSortMode.Name, InstitutionSortMode.NumberOfBaseJournalScoreCards, SortDirection.Ascending)]
        [InlineData(InstitutionSortMode.NumberOfBaseJournalScoreCards, InstitutionSortMode.Name, SortDirection.Descending)]
        [InlineData(InstitutionSortMode.NumberOfValuationJournalScoreCards, InstitutionSortMode.Name, SortDirection.Descending)]
        public void GetOrderDirectionWithInstitutionSortModeIsNotEqualToCurrentSortModeReturnsDefaultDirection(InstitutionSortMode newSortMode, InstitutionSortMode currentSortMode, SortDirection expectedSortDirection)
        {
            // Arrange

            // Act
            var orderDirection = UrlHelperExtensions.GetOrderDirection(newSortMode, currentSortMode, SortDirection.Ascending);

            // Assert
            Assert.Equal(expectedSortDirection, orderDirection);
        }
    }
}