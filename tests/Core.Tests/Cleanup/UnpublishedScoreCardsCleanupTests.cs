namespace QOAM.Core.Tests.Cleanup
{
    using System;
    using Core.Cleanup;
    using Moq;
    using Repositories;
    using Xunit;

    public class UnpublishedScoreCardsCleanupTests
    {
        [Fact]
        public void CleanupRemovesUnpublishedBaseScoreCardsUsingSpecifiedNumberOfUnpublishedDays()
        {
            // Arrange
            var settings = CreateUnpublishedScoreCardsCleanupSettings();
            var baseScoreCardRepository = new Mock<IBaseScoreCardRepository>();

            var unpublishedScoreCardsCleanup = CreateUnpublishedScoreCardsCleanup(baseScoreCardRepository.Object, settings: settings);
            var toBeRemovedWindow = TimeSpan.FromDays(settings.NumberOfDaysBeforeArchivingUnpublishedScoreCards);

            // Act
            unpublishedScoreCardsCleanup.Cleanup();

            // Assert
            baseScoreCardRepository.Verify(b => b.RemoveUnpublishedScoreCards(toBeRemovedWindow), Times.Once);
        }

        [Fact]
        public void CleanupRemovesUnpublishedValuationScoreCardsUsingSpecifiedNumberOfUnpublishedDays()
        {
            // Arrange
            var settings = CreateUnpublishedScoreCardsCleanupSettings();
            var valuationScoreCardRepository = new Mock<IValuationScoreCardRepository>();

            var unpublishedScoreCardsCleanup = CreateUnpublishedScoreCardsCleanup(valuationScoreCardRepository: valuationScoreCardRepository.Object, settings: settings);
            var toBeRemovedWindow = TimeSpan.FromDays(settings.NumberOfDaysBeforeArchivingUnpublishedScoreCards);

            // Act
            unpublishedScoreCardsCleanup.Cleanup();

            // Assert
            valuationScoreCardRepository.Verify(b => b.RemoveUnpublishedScoreCards(toBeRemovedWindow), Times.Once);
        }

        private static CleanupSettings CreateUnpublishedScoreCardsCleanupSettings()
        {
            return new CleanupSettings { NumberOfDaysBeforeArchivingUnpublishedScoreCards = 50 };
        }

        private static UnpublishedScoreCardsCleanup CreateUnpublishedScoreCardsCleanup(
            IBaseScoreCardRepository baseScoreCardRepository = null,
            IValuationScoreCardRepository valuationScoreCardRepository = null,
            CleanupSettings settings = null)
        {
            return new UnpublishedScoreCardsCleanup(
                baseScoreCardRepository ?? Mock.Of<IBaseScoreCardRepository>(),
                valuationScoreCardRepository ?? Mock.Of<IValuationScoreCardRepository>(), settings ?? CreateUnpublishedScoreCardsCleanupSettings());
        }
    }
}