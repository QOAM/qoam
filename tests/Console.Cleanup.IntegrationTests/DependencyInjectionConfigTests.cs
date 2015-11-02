namespace QOAM.Console.Cleanup.IntegrationTests
{
    using Autofac;
    using Cleanup;
    using Core.Cleanup;
    using Xunit;

    public class DependencyInjectionConfigTests
    {
        [Fact]
        public void UnpublishedScoreCardsCleanupComponentRegistered()
        {
            // Arrange
            var container = DependencyInjectionConfig.RegisterComponents();

            // Act
            var unpublishedScoreCardsCleanup = container.Resolve<UnpublishedScoreCardsCleanup>();

            // Assert
            Assert.NotNull(unpublishedScoreCardsCleanup);
        }

        [Fact]
        public void DuplicateScoreCardsCleanupComponentRegistered()
        {
            // Arrange
            var container = DependencyInjectionConfig.RegisterComponents();

            // Act
            var duplicateScoreCardsCleanup = container.Resolve<DuplicateScoreCardsCleanup>();

            // Assert
            Assert.NotNull(duplicateScoreCardsCleanup);
        }

        [Fact]
        public void InactiveProfilesCleanupComponentRegistered()
        {
            // Arrange
            var container = DependencyInjectionConfig.RegisterComponents();

            // Act
            var inactiveProfilesCleanup = container.Resolve<InactiveProfilesCleanup>();

            // Assert
            Assert.NotNull(inactiveProfilesCleanup);
        }
    }
}
