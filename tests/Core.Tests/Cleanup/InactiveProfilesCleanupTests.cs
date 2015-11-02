namespace QOAM.Core.Tests.Cleanup
{
    using System;
    using Core.Cleanup;
    using Moq;
    using Repositories;
    using Xunit;

    public class InactiveProfilesCleanupTests
    {
        [Fact]
        public void CleanupRemovesInactiveProfilesUsingSpecifiedNumberOfUnpublishedDays()
        {
            // Arrange
            var settings = CreateInactiveProfilesCleanupSettings();
            var userProfileRepository = new Mock<IUserProfileRepository>();

            var unpublishedScoreCardsCleanup = CreateUnpublishedScoreCardsCleanup(userProfileRepository.Object, settings: settings);
            var toBeRemovedWindow = TimeSpan.FromDays(settings.NumberOfInactiveDaysBeforeRemoval);

            // Act
            unpublishedScoreCardsCleanup.Cleanup();

            // Assert
            userProfileRepository.Verify(b => b.RemoveInactive(toBeRemovedWindow), Times.Once);
        }

        private static InactiveProfilesCleanupSettings CreateInactiveProfilesCleanupSettings()
        {
            return new InactiveProfilesCleanupSettings { NumberOfInactiveDaysBeforeRemoval = 50 };
        }

        private static InactiveProfilesCleanup CreateUnpublishedScoreCardsCleanup(
            IUserProfileRepository userProfileRepository = null,
            InactiveProfilesCleanupSettings settings = null)
        {
            return new InactiveProfilesCleanup(
                userProfileRepository ?? Mock.Of<IUserProfileRepository>(),
                settings ?? CreateInactiveProfilesCleanupSettings());
        }
    }
}