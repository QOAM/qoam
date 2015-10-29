namespace QOAM.Console.Cleanup.IntegrationTests
{
    using Autofac;
    using Cleanup;
    using Core.Cleanup;
    using Xunit;

    public class DependencyInjectionConfigTests
    {
        [Fact]
        public void CleanupUnpublishedScoreCardsComponentRegistered()
        {
            // Arrange
            var container = DependencyInjectionConfig.RegisterComponents();

            // Act
            var unpublishedScoreCardsCleanup = container.Resolve<UnpublishedScoreCardsCleanup>();

            // Assert
            Assert.NotNull(unpublishedScoreCardsCleanup);
        }
    }
}
