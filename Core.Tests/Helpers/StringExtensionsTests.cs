namespace QOAM.Core.Tests.Helpers
{
    using System;
    using Core.Helpers;
    using FsCheck;
    using FsCheck.Xunit;
    using Xunit;

    public class StringExtensionsTests
    {
        private class NonNullStringsArbitrary
        {
            public static Arbitrary<string> String()
            {
                return Arb.Default.String().Generator.Where(s => s != null).ToArbitrary();
            }
        }

        private class NonEmptyStringsArbitrary
        {
            public static Arbitrary<string> String()
            {
                return Arb.Default.String().Generator.Where(s => !string.IsNullOrEmpty(s)).ToArbitrary();
            }
        }

        private class PositiveIntegersArbitrary
        {
            public static Arbitrary<int> Positive()
            {
                return Arb.Default.Int32().Generator.Where(n => n > 0).ToArbitrary();
            }
        }

        [Property(Arbitrary = new[] { typeof(PositiveIntegersArbitrary) })]
        public void TruncateWithLengthLessThanOrEqualToMaximumLengthDoesNotTruncate(int length, int lengthDifference)
        {
            // Arrange
            var str = new string('c', Math.Max(0, length - lengthDifference));

            // Act
            var truncated = str.Truncate(length);

            // Assert
            Assert.Equal(str, truncated);
        }

        [Property(Arbitrary = new[] { typeof(PositiveIntegersArbitrary) })]
        public void TruncateWithLengthEqualToMaximumLengthDoesNotTruncate(int length)
        {
            // Arrange
            var str = new string('c', length);

            // Act
            var truncated = str.Truncate(length);

            // Assert
            Assert.Equal(str, truncated);
        }

        [Property(Arbitrary = new[] { typeof(NonEmptyStringsArbitrary), typeof(PositiveIntegersArbitrary) })]
        public void TruncateWithLengthGreaterThanMaximumLengthTruncatesToMaximumLength(int length, int lengthDifference, string truncation)
        {
            // Arrange
            var str = new string('c', length + truncation.Length + lengthDifference);

            // Act
            var truncated = str.Truncate(length, truncation);

            // Assert
            Assert.Equal(length, truncated.Length);
        }

        [Property(Arbitrary = new[] { typeof(NonEmptyStringsArbitrary), typeof(PositiveIntegersArbitrary) })]
        public void TruncateWithLengthGreaterThanOrEqualToMaximumLengthPlusTruncationLengthReturnsStringThatEndsWithTruncation(string truncation, int lengthDifference)
        {
            // Arrange
            var str = new string('c', truncation.Length * 2 + lengthDifference);

            // Act
            var truncated = str.Truncate(truncation.Length * 2, truncation);

            // Assert
            Assert.True(truncated.EndsWith(truncation));
        }

        [Property]
        public void TruncateOnNullStringReturnsNull(int length, string truncation)
        {
            // Arrange
            string nullStr = null;

            // Act
            var truncated = nullStr.Truncate(length, truncation);

            // Assert
            Assert.Null(truncated);
        }

        [Property(Arbitrary = new[] { typeof(NonNullStringsArbitrary), typeof(PositiveIntegersArbitrary) })]
        public void TruncateOnNonNullStringWithLengthIsNegativeReturnsEmptyString(string str, int length)
        {
            // Arrange

            // Act
            var truncated = str.Truncate(0 - length);

            // Assert
            Assert.Equal(string.Empty, truncated);
        }
    }
}