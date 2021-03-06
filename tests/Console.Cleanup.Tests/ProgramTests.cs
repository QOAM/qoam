﻿namespace QOAM.Console.Cleanup.Tests
{
    using System;
    using System.Collections.Generic;
    using Cleanup;
    using Core.Cleanup;
    using Xunit;

    public class ProgramTests
    {
        [Fact]
        public void GetCleanupModeWithEmptyArgsReturnsUnpublishedScoreCardsCleanupMode()
        {
            // Arrange
            var args = new List<string>();

            // Act
            var cleanupMode = Program.GetCleanupMode(args);

            // Assert
            Assert.Equal(CleanupMode.UnpublishedScoreCards, cleanupMode);
        }

        [Theory]
        [InlineData("UnpublishedScoreCards", CleanupMode.UnpublishedScoreCards)]
        [InlineData("unpublishedscorecards", CleanupMode.UnpublishedScoreCards)]
        [InlineData("UNPUBLISHEDSCORECARDS", CleanupMode.UnpublishedScoreCards)]
        [InlineData("DuplicateScoreCards", CleanupMode.DuplicateScoreCards)]
        [InlineData("duplicatescorecards", CleanupMode.DuplicateScoreCards)]
        [InlineData("DUPLICATESCORECARDS", CleanupMode.DuplicateScoreCards)]
        [InlineData("InactiveProfiles", CleanupMode.InactiveProfiles)]
        [InlineData("inactiveprofiles", CleanupMode.InactiveProfiles)]
        [InlineData("INACTIVEPROFILES", CleanupMode.InactiveProfiles)]
        public void GetCleanupModeReturnsCorrectCleanupMode(string argument, CleanupMode expected)
        {
            // Arrange
            var args = new List<string> { argument };

            // Act
            var cleanupMode = Program.GetCleanupMode(args);

            // Assert
            Assert.Equal(expected, cleanupMode);
        }

        [Theory]
        [InlineData("invalid")]
        [InlineData("_doaj_")]
        [InlineData("#ulrichs#")]
        public void GetCleanupModeWithInvalidArgsThrowsArgumentException(string invalidArgument)
        {
            // Arrange
            var args = new List<string> { invalidArgument };

            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => Program.GetCleanupMode(args));
        }

        [Theory]
        [InlineData(CleanupMode.UnpublishedScoreCards, typeof(UnpublishedScoreCardsCleanup))]
        [InlineData(CleanupMode.DuplicateScoreCards, typeof(DuplicateScoreCardsCleanup))]
        [InlineData(CleanupMode.InactiveProfiles, typeof(InactiveProfilesCleanup))]
        public void GetCleanupReturnsCorrectCleanupType(CleanupMode cleanupMode, Type expected)
        {
            // Arrange

            // Act
            var cleanup = Program.GetCleanup(cleanupMode);

            // Assert
            Assert.IsType(expected, cleanup);
        }
    }
}