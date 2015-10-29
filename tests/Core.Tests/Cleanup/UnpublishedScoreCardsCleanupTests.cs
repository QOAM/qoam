using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOAM.Core.Tests.Cleanup
{
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

            var unpublishedScoreCardsCleanup = CreateUnpublishedScoreCardsCleanup(baseScoreCardRepository: baseScoreCardRepository.Object, settings: settings);
            var toBeRemovedWindow = TimeSpan.FromDays(settings.NumberOfDaysBeforeArchiving);

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
            var toBeRemovedWindow = TimeSpan.FromDays(settings.NumberOfDaysBeforeArchiving);

            // Act
            unpublishedScoreCardsCleanup.Cleanup();

            // Assert
            valuationScoreCardRepository.Verify(b => b.RemoveUnpublishedScoreCards(toBeRemovedWindow), Times.Once);
        }

        private static UnpublishedScoreCardsCleanupSettings CreateUnpublishedScoreCardsCleanupSettings()
        {
            return new UnpublishedScoreCardsCleanupSettings { NumberOfDaysBeforeArchiving = 50 };
        }

        private static UnpublishedScoreCardsCleanup CreateUnpublishedScoreCardsCleanup(
            IBaseScoreCardRepository baseScoreCardRepository = null,
            IValuationScoreCardRepository valuationScoreCardRepository = null,
            UnpublishedScoreCardsCleanupSettings settings = null)
        {
            return new UnpublishedScoreCardsCleanup(
                baseScoreCardRepository ?? Mock.Of<IBaseScoreCardRepository>(),
                valuationScoreCardRepository ?? Mock.Of<IValuationScoreCardRepository>(),
                settings: settings ?? CreateUnpublishedScoreCardsCleanupSettings());
        }
    }
}
