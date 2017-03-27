using System.Collections.Generic;
using Moq;
using QOAM.Core.Import.JournalTOCs;
using QOAM.Core.Repositories;
using QOAM.Core.Tests.Import.Resources;
using Xunit;

namespace QOAM.Core.Tests.Import
{
    public class JournalTocsImportTests
    {
        Mock<IJournalTocsClient> _client;
        Mock<IBlockedISSNRepository> _issnRepo;
        Mock<IJournalTocsParser> _parser;

        [Fact]
        public void DownloadJournals_downloads_and_parses_journals()
        {
            // Arrange
            var blockedIssns = new List<BlockedISSN>
            {
                new BlockedISSN { ISSN = "0001-3765" },
                new BlockedISSN { ISSN = "2282-0035" }
            };

            var xml = new List<string> { GetJournalTocsFirst500Xml(), GetJournalTocsNextXml() };

            var journalTocsImport = CreateJournalTocsImport();
            _client.Setup(x => x.DownloadJournals(JournalTocsFetchMode.Update)).Returns(xml);
            _issnRepo.Setup(x => x.All).Returns(blockedIssns);
            _parser.Setup(x => x.Parse(xml)).Returns(new List<Journal> {new Journal() });

            // Act
            var journals = journalTocsImport.DownloadJournals();

            // Assert
            Assert.Equal(1, journals.Count);
            _issnRepo.Verify(x => x.All, Times.Once());
        }

        static string GetJournalTocsFirst500Xml()
        {
            return ResourceReader.GetContentsOfResource("JournalTocs-setup-first-500.xml").Replace("&", "&amp;");
        }

        static string GetJournalTocsNextXml()
        {
            return ResourceReader.GetContentsOfResource("JournalTocs-setup-next-500.xml").Replace("&", "&amp;");
        }

        JournalTocsImport CreateJournalTocsImport()
        {
            _client = new Mock<IJournalTocsClient>();
            _issnRepo = new Mock<IBlockedISSNRepository>();
            _parser = new Mock<IJournalTocsParser>();

            //var journalTocsSettings = new JournalTocsSettings();;
            return new JournalTocsImport(_client.Object, _issnRepo.Object, _parser.Object);
        }
    }
}