namespace QOAM.Console.DataImporter.Tests
{
    using System;
    using System.Collections.Generic;

    using QOAM.Core.Import;

    using Xunit;
    using Xunit.Extensions;

    public class ProgramTests
    {
        [Fact]
        public void GetImportTypeWithEmptyArgsReturnsUlrichsJournalsImportSource()
        {
            // Arrange
            var args = new List<string>();

            // Act
            var journalsImportSource = Program.GetImportType(args);

            // Assert
            Assert.Equal(JournalsImportSource.Ulrichs, journalsImportSource);
        }

        [Theory]
        [InlineData("Ulrichs")]
        [InlineData("ulrichs")]
        [InlineData("ULRICHS")]
        [InlineData(" Ulrichs ")]
        public void GetImportTypeWithFirstArgumentIsUlrichsReturnsUlrichsJournalsImportSource(string ulrichsArgument)
        {
            // Arrange
            var args = new List<string> { ulrichsArgument };

            // Act
            var journalsImportSource = Program.GetImportType(args);

            // Assert
            Assert.Equal(JournalsImportSource.Ulrichs, journalsImportSource);
        }

        [Theory]
        [InlineData("Doaj")]
        [InlineData("doaj")]
        [InlineData("DOAJ")]
        [InlineData(" Doaj ")]
        public void GetImportTypeWithFirstArgumentIsDoajReturnsDoajJournalsImportSource(string doajArgument)
        {
            // Arrange
            var args = new List<string> { doajArgument };

            // Actur
            var journalsImportSource = Program.GetImportType(args);

            // Assert
            Assert.Equal(JournalsImportSource.DOAJ, journalsImportSource);
        }

        [Theory]
        [InlineData("invalid")]
        [InlineData("_doaj_")]
        [InlineData("#ulrichs#")]
        public void GetImportTypeWithInvalidArgsThrowsArgumentException(string invalidArgument)
        {
            // Arrange
            var args = new List<string> { invalidArgument };

            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => Program.GetImportType(args));
        }
        
        [Fact]
        public void GetImportModeWithEmptyArgsReturnsInsertOnlyJournalsImportMode()
        {
            // Arrange
            var args = new List<string>();

            // Act
            var journalsImportMode = Program.GetImportMode(args);

            // Assert
            Assert.Equal(JournalsImportMode.InsertOnly, journalsImportMode);
        }

        [Fact]
        public void GetImportModeWithOneArgumentReturnsInsertOnlyJournalsImportMode()
        {
            // Arrange
            var args = new List<string> { "Ulrichs" };

            // Act
            var journalsImportMode = Program.GetImportMode(args);

            // Assert
            Assert.Equal(JournalsImportMode.InsertOnly, journalsImportMode);
        }

        [Theory]
        [InlineData("InsertOnly")]
        [InlineData("insertonly")]
        [InlineData("INSERTONLY")]
        [InlineData(" InsertOnly ")]
        public void GetImportModeWithSecondArgumentIsInsertOnlyReturnsInsertOnlyJournalsImportMode(string insertOnlyArgument)
        {
            // Arrange
            var args = new List<string> { "Ulrichs", insertOnlyArgument };

            // Act
            var journalsImportMode = Program.GetImportMode(args);

            // Assert
            Assert.Equal(JournalsImportMode.InsertOnly, journalsImportMode);
        }

        [Theory]
        [InlineData("UpdateOnly")]
        [InlineData("updateonly")]
        [InlineData("UPDATEONLY")]
        [InlineData(" UpdateOnly ")]
        public void GetImportModeWithSecondArgumentIsUpdateOnlyReturnsUpdateOnlyJournalsImportMode(string updateOnlyArgument)
        {
            // Arrange
            var args = new List<string> { "Ulrichs", updateOnlyArgument };

            // Act
            var journalsImportMode = Program.GetImportMode(args);

            // Assert
            Assert.Equal(JournalsImportMode.UpdateOnly, journalsImportMode);
        }

        [Theory]
        [InlineData("InsertAndUpdate")]
        [InlineData("insertandupdate")]
        [InlineData("INSERTANDUPDATE")]
        [InlineData(" InsertAndUpdate ")]
        public void GetImportModeWithSecondArgumentIsInsertAndUpdateReturnsInsertAndUpdateJournalsImportMode(string insertAndUpdateArgument)
        {
            // Arrange
            var args = new List<string> { "Ulrichs", insertAndUpdateArgument };

            // Actur
            var journalsImportMode = Program.GetImportMode(args);

            // Assert
            Assert.Equal(JournalsImportMode.InsertAndUpdate, journalsImportMode);
        }

        [Theory]
        [InlineData("invalid")]
        [InlineData("_InsertOnly_")]
        [InlineData("#UpdateOnly#")]
        [InlineData("!InsertAndUpdate!")]
        public void GetImportModeWithInvalidArgsThrowsArgumentException(string invalidArgument)
        {
            // Arrange
            var args = new List<string> { "Ulrichs", invalidArgument };

            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => Program.GetImportMode(args));
        }
    }
}