namespace QOAM.Console.DataImporter.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Import;
    using Xunit;

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
        [InlineData("JournalTocs")]
        [InlineData("JournalTOCs")]
        [InlineData("JOURNALTOCS")]
        [InlineData("journaltocs")]
        [InlineData(" JournalTocs ")]
        public void GetImportTypeWithFirstArgumentIsJournalTocsReturnsJournalTocsJournalsImportSource(string journalTocsArgument)
        {
            // Arrange
            var args = new List<string> { journalTocsArgument };

            // Actur
            var journalsImportSource = Program.GetImportType(args);

            // Assert
            Assert.Equal(JournalsImportSource.JournalTOCs, journalsImportSource);
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
        
        [Theory]
        [InlineData(JournalsImportSource.Ulrichs)]
        [InlineData(JournalsImportSource.DOAJ)]
        [InlineData(JournalsImportSource.JournalTOCs)]
        public void GetImportModeWithEmptyArgsReturnsInsertOnlyJournalsImportMode(JournalsImportSource source)
        {
            // Arrange
            var args = new List<string>();

            // Act
            var journalsImportMode = Program.GetImportMode(source, args);

            // Assert
            Assert.Equal(JournalsImportMode.InsertOnly, journalsImportMode);
        }

        [Fact]
        public void GetImportModeWithOneArgumentReturnsInsertOnlyJournalsImportMode()
        {
            // Arrange
            var args = new List<string> { "Ulrichs" };

            // Act
            var journalsImportMode = Program.GetImportMode(JournalsImportSource.Ulrichs, args);

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
            var journalsImportMode = Program.GetImportMode(JournalsImportSource.Ulrichs, args);

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
            var journalsImportMode = Program.GetImportMode(JournalsImportSource.Ulrichs, args);

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
            var journalsImportMode = Program.GetImportMode(JournalsImportSource.Ulrichs, args);

            // Assert
            Assert.Equal(JournalsImportMode.InsertAndUpdate, journalsImportMode);
        }

        [Fact]
        public void GetImportMode_when_source_is_JournalTocs_and_fetchMode_is_setup_returns_InsertAndUpdate()
        {
            var args = new List<string> { "JournalTocs", "setup" };
            
            var journalsImportMode = Program.GetImportMode(JournalsImportSource.JournalTOCs, args, JournalTocsFetchMode.Setup);
            
            Assert.Equal(JournalsImportMode.InsertAndUpdate, journalsImportMode);
        }

        [Fact]
        public void GetImportMode_when_source_is_JournalTocs_and_fetchMode_is_update_returns_UpdateOnly()
        {
            var args = new List<string> { "JournalTocs", "setup" };

            var journalsImportMode = Program.GetImportMode(JournalsImportSource.JournalTOCs, args, JournalTocsFetchMode.Update);

            Assert.Equal(JournalsImportMode.UpdateOnly, journalsImportMode);
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
            Assert.Throws<ArgumentException>(() => Program.GetImportMode(JournalsImportSource.Ulrichs, args));
        }

        [Fact]
        public void GetFetchMode_returns_null_when_source_is_not_JournalTocs()
        {
            var args = new List<string> { "Ulrichs", "bla" };

            var fetchMode = Program.GetFetchMode(JournalsImportSource.Ulrichs, args);

            Assert.Null(fetchMode);
        }

        [Theory]
        [InlineData("update")]
        [InlineData("Update")]
        [InlineData("UPDATE")]
        [InlineData(" Update ")]
        public void GetFetchMode_is_parsed_as_update_when_source_is_JournalTocs(string updateArgument)
        {
            var args = new List<string> { "JournalTocs", updateArgument };

            var fetchMode = Program.GetFetchMode(JournalsImportSource.JournalTOCs, args);

            Assert.Equal(JournalTocsFetchMode.Update, fetchMode);
        }

        [Theory]
        [InlineData("setup")]
        [InlineData("Setup")]
        [InlineData("SETUP")]
        [InlineData(" Setup ")]
        public void GetFetchMode_is_parsed_as_Setup_when_source_is_JournalTocs(string setupArgument)
        {
            var args = new List<string> { "JournalTocs", setupArgument };

            var fetchMode = Program.GetFetchMode(JournalsImportSource.JournalTOCs, args);

            Assert.Equal(JournalTocsFetchMode.Setup, fetchMode);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void GetJournalUpdatePropertiesWithTooFewArgumentsReturnsSetWithAllJournalUpdatePropertiesExceptDoajSeal(int numberOfArguments)
        {
            // Arrange
            var args = Enumerable.Repeat("", numberOfArguments).ToList();

            // Act
            var journalUpdateProperties = Program.GetJournalUpdateProperties(args);

            // Assert
            var expected = new HashSet<JournalUpdateProperty>((JournalUpdateProperty[])Enum.GetValues(typeof(JournalUpdateProperty)));
            expected.Remove(JournalUpdateProperty.DoajSeal);

            Assert.Equal(expected, journalUpdateProperties);
        }

        [Theory]
        [InlineData("doajseal", JournalUpdateProperty.DoajSeal)]
        [InlineData("country", JournalUpdateProperty.Country)]
        [InlineData("languages", JournalUpdateProperty.Languages)]
        [InlineData("link", JournalUpdateProperty.Link)]
        [InlineData("publisher", JournalUpdateProperty.Publisher)]
        [InlineData("subjects", JournalUpdateProperty.Subjects)]
        [InlineData("title", JournalUpdateProperty.Title)]
        public void GetJournalUpdatePropertiesWithOnePropertyReturnsCorrectJournalUpdateProperty(string propertiesArgument, JournalUpdateProperty expected)
        {
            // Arrange
            var args = new List<string> { "Ulrichs", "InsertOnly", propertiesArgument };

            // Act
            var journalUpdateProperties = Program.GetJournalUpdateProperties(args);

            // Assert
            Assert.Contains(expected, journalUpdateProperties);
            Assert.Equal(1, journalUpdateProperties.Count);
        }

        [Theory]
        [InlineData("doajSEAL", JournalUpdateProperty.DoajSeal)]
        [InlineData("Country", JournalUpdateProperty.Country)]
        [InlineData("LanguageS", JournalUpdateProperty.Languages)]
        [InlineData("LINK", JournalUpdateProperty.Link)]
        public void GetJournalUpdatePropertiesIsCaseInsensitive(string propertiesArgument, JournalUpdateProperty expected)
        {
            // Arrange
            var args = new List<string> { "Ulrichs", "InsertOnly", propertiesArgument };

            // Act
            var journalUpdateProperties = Program.GetJournalUpdateProperties(args);

            // Assert
            Assert.Contains(expected, journalUpdateProperties);
            Assert.Equal(1, journalUpdateProperties.Count);
        }

        [Theory]
        [InlineData("doajseal,country", JournalUpdateProperty.DoajSeal, JournalUpdateProperty.Country)]
        [InlineData("link;subjects", JournalUpdateProperty.Link, JournalUpdateProperty.Subjects)]
        public void GetJournalUpdatePropertiesSupportsMultiplesProperties(string propertiesArgument, JournalUpdateProperty expected1, JournalUpdateProperty expected2)
        {
            // Arrange
            var args = new List<string> { "Ulrichs", "InsertOnly", propertiesArgument };

            // Act
            var journalUpdateProperties = Program.GetJournalUpdateProperties(args);

            // Assert
            var expected = new HashSet<JournalUpdateProperty> { expected1, expected2 };
            Assert.Equal(expected, journalUpdateProperties);
        }
    }
}