namespace QOAM.Console.ExpirationChecker.IntegrationTests
{
    using Autofac;

    using QOAM.Console.ExpirationChecker;
    using QOAM.Core.Services;

    using Xunit;

    public class DependencyInjectionConfigTests
    {
        [Fact]
        public void ExpirationCheckerComponentRegistered()
        {
            // Arrange
            var container = DependencyInjectionConfig.RegisterComponents();

            // Act
            var expirationChecker = container.Resolve<ExpirationChecker>();

            // Assert
            Assert.NotNull(expirationChecker);
        }
    }
}
