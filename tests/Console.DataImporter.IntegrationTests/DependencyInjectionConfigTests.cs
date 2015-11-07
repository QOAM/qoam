namespace QOAM.Console.DataImporter.IntegrationTests
{
    using Autofac;

    using QOAM.Core.Import;

    using Xunit;

    public class DependencyInjectionConfigTests
    {
        [Fact]
        public void JournalsImportComponentRegistered()
        {
            // Arrange
            var container = DependencyInjectionConfig.RegisterComponents();

            // Act
            var journalsImport = container.Resolve<JournalsImport>();

            // Assert
            Assert.NotNull(journalsImport);
        }

        [Fact]
        public void DoajImportComponentRegistered()
        {
            // Arrange
            var container = DependencyInjectionConfig.RegisterComponents();

            // Act
            var doajImport = container.Resolve<DoajImport>();

            // Assert
            Assert.NotNull(doajImport);
        }

        [Fact]
        public void UlrichsImportComponentRegistered()
        {
            // Arrange
            var container = DependencyInjectionConfig.RegisterComponents();

            // Act
            var ulrichsImport = container.Resolve<UlrichsImport>();

            // Assert
            Assert.NotNull(ulrichsImport);
        }
    }
}
