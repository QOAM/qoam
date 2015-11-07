namespace QOAM.Core.Tests.Helpers
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using QOAM.Core.Helpers;

    using Xunit;

    public class ValidatorExtensionsTests
    {
        [Fact]
        public void ValidateOnNullInstanceThrowsArgumentNullException()
        {
            // Arrange
            TestableEntity entity = null;

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => entity.Validate());
        }

        [Fact]
        public void ValidateWithInvalidObjectThrowsException()
        {
            // Arrange
            var entity = TestableEntity.CreateInvalidEntity();

            // Act

            // Assert
            Assert.Throws<ValidationException>(() => entity.Validate());
        }

        [Fact]
        public void IsValidOnNullInstanceThrowsArgumentNullException()
        {
            // Arrange
            TestableEntity entity = null;

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => entity.IsValid());
        }

        [Fact]
        public void IsValidWithValidObjectReturnsFalse()
        {
            // Arrange
            var entity = TestableEntity.CreateInvalidEntity();

            // Act
            var isValid = entity.IsValid();

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void IsValidWithValidObjectReturnsTrue()
        {
            // Arrange
            var entity = TestableEntity.CreateValidEntity();

            // Act
            var isValid = entity.IsValid();

            // Assert
            Assert.True(isValid);
        }

        private class TestableEntity
        {
            [Required]
            public string Name { get; set; }

            internal static TestableEntity CreateValidEntity()
            {
                return new TestableEntity { Name = "Test" };
            }

            internal static TestableEntity CreateInvalidEntity()
            {
                return new TestableEntity { Name = null };
            }
        }
    }
}