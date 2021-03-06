﻿namespace QOAM.Core.Tests.Cleanup
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
            var toBeRemovedWindow = TimeSpan.FromDays(settings.NumberOfInactiveDaysBeforeArchivingInactiveProfiles);

            // Act
            unpublishedScoreCardsCleanup.Cleanup();

            // Assert
            userProfileRepository.Verify(b => b.RemoveInactive(toBeRemovedWindow), Times.Once);
        }

        private static CleanupSettings CreateInactiveProfilesCleanupSettings()
        {
            return new CleanupSettings { NumberOfInactiveDaysBeforeArchivingInactiveProfiles = 50 };
        }

        private static InactiveProfilesCleanup CreateUnpublishedScoreCardsCleanup(
            IUserProfileRepository userProfileRepository = null,
            CleanupSettings settings = null)
        {
            return new InactiveProfilesCleanup(
                userProfileRepository ?? Mock.Of<IUserProfileRepository>(),
                settings ?? CreateInactiveProfilesCleanupSettings());
        }
    }
}