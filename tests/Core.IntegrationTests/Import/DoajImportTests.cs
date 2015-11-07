namespace QOAM.Core.IntegrationTests.Import
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using Core.Import;
    using Moq;
    using Repositories;
    using Xunit;

    public class DoajImportTests
    {
        [Fact]
        public void GetJournalsWillImportJournals()
        {
            // Arrange
            var doajImport = new DoajImport(ImportSettings.Current.Doaj, new BlockedISSNRepository(new ApplicationDbContext { BlockedISSNs = CreateDbSet(new List<BlockedISSN>()) }));

            // Act
            var journalsImportResult = doajImport.GetJournals();

            // Assert
            Assert.True(journalsImportResult.Count > 0);
        }

        [Fact]
        public void GetJournalsWillNotReturnJournalsWithBlockedIssn()
        {
            // Arrange
            var blockedIssns = new List<BlockedISSN>
            {
                new BlockedISSN { ISSN = "0001-3765" },
                new BlockedISSN { ISSN = "2282-0035" }
            };

            var doajImport = new DoajImport(ImportSettings.Current.Doaj, new BlockedISSNRepository(new ApplicationDbContext { BlockedISSNs = CreateDbSet(blockedIssns) }));

            // Act
            var journals = doajImport.GetJournals();

            // Assert
            foreach (var blockedIssn in blockedIssns)
            {
                Assert.False(journals.Any(j => j.ISSN == blockedIssn.ISSN));
            }
        }

        private static DbSet<BlockedISSN> CreateDbSet(IEnumerable<BlockedISSN> data)
        {
            var queryable = data.AsQueryable();

            var mockSet = new Mock<DbSet<BlockedISSN>>();
            mockSet.As<IQueryable<BlockedISSN>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<BlockedISSN>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<BlockedISSN>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<BlockedISSN>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

            return mockSet.Object;
        }
    }
}