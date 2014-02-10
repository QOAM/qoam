namespace RU.Uci.OAMarket.Domain.Tests.Import
{
    using System;

    using RU.Uci.OAMarket.Domain.Import;

    using Xunit;

    public class JournalIssnEqualityComparerTests
    {
        [Fact]
        public void EqualsWithInstancesWithEqualISSNsReturnsTrue()
        {
            // Arrange
            var journalIssnEqualityComparer = new JournalIssnEqualityComparer();
            var journalX = new Journal { ISSN = "abc" };
            var journalY = new Journal { ISSN = "abc" };

            // Act
            var journalsAreEqual = journalIssnEqualityComparer.Equals(journalX, journalY);

            // Assert
            Assert.True(journalsAreEqual);
        }

        [Fact]
        public void EqualsWithInstancesWithDifferentISSNsReturnsFalse()
        {
            // Arrange
            var journalIssnEqualityComparer = new JournalIssnEqualityComparer();
            var journalX = new Journal { ISSN = "abc" };
            var journalY = new Journal { ISSN = "123" };

            // Act
            var journalsAreEqual = journalIssnEqualityComparer.Equals(journalX, journalY);

            // Assert
            Assert.False(journalsAreEqual);
        }

        [Fact]
        public void EqualsWithBothInstancesAreNullReturnsTrue()
        {
            // Arrange
            var journalIssnEqualityComparer = new JournalIssnEqualityComparer();
            Journal nullJournalX = null;
            Journal nullJournalY = null;

            // Act
            var journalsAreEqual = journalIssnEqualityComparer.Equals(nullJournalX, nullJournalY);

            // Assert
            Assert.True(journalsAreEqual);
        }

        [Fact]
        public void EqualsWithOnlyOneInstanceIsNullReturnsFalse()
        {
            // Arrange
            var journalIssnEqualityComparer = new JournalIssnEqualityComparer();
            Journal nullJournalX = null;
            Journal journalY = new Journal();

            // Act
            var journalsAreEqual = journalIssnEqualityComparer.Equals(nullJournalX, journalY);

            // Assert
            Assert.False(journalsAreEqual);
        }

        [Fact]
        public void GetHashCodeOnInstancesWithSameISSNReturnsSameHashCode()
        {
            // Arrange
            var journalIssnEqualityComparer = new JournalIssnEqualityComparer();
            var journalX = new Journal { ISSN = "abc" };
            var journalY = new Journal { ISSN = "abc" };

            // Act
            var hashCodeX = journalIssnEqualityComparer.GetHashCode(journalX);
            var hashCodeY = journalIssnEqualityComparer.GetHashCode(journalY);

            // Assert
            Assert.Equal(hashCodeX, hashCodeY);
        }

        [Fact]
        public void GetHashCodeOnInstancesWithDifferentISSNReturnsDifferentHashCode()
        {
            // Arrange
            var journalIssnEqualityComparer = new JournalIssnEqualityComparer();
            var journalX = new Journal { ISSN = "abc" };
            var journalY = new Journal { ISSN = "123" };

            // Act
            var hashCodeX = journalIssnEqualityComparer.GetHashCode(journalX);
            var hashCodeY = journalIssnEqualityComparer.GetHashCode(journalY);

            // Assert
            Assert.NotEqual(hashCodeX, hashCodeY);
        }

        [Fact]
        public void GetHashCodeWithNullInstanceThrowsArgumentNullException()
        {
            // Arrange
            var journalIssnEqualityComparer = new JournalIssnEqualityComparer();
            Journal nullJournal = null;

            // Act
            
            // Assert
            Assert.Throws<ArgumentNullException>(() => journalIssnEqualityComparer.GetHashCode(nullJournal));
        }
    }
}