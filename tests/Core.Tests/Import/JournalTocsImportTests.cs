using System.Collections.Generic;
using Moq;
using QOAM.Core.Import;
using QOAM.Core.Repositories;
using QOAM.Core.Tests.Import.Resources;
using Xunit;

namespace QOAM.Core.Tests.Import
{
    public class JournalTocsImportTests
    {
        Mock<IJournalTocsClient> _client;
        Mock<IBlockedISSNRepository> _issnRepo;


        [Fact]
        public void DownloadJournals_parses_journal_xml_and_converts_it_to_Journals()
        {
            // Arrange
            var blockedIssns = new List<BlockedISSN>
            {
                new BlockedISSN { ISSN = "0001-3765" },
                new BlockedISSN { ISSN = "2282-0035" }
            };


            var journalTocsImport = CreateJournalTocsImport();
            _client.Setup(x => x.DownloadJournals(JournalTocsFetchMode.Update)).Returns(new List<string> { GetJournalTocsFirst500Xml(), GetJournalTocsNextXml() });
            _issnRepo.Setup(x => x.All).Returns(blockedIssns);

            // Act
            var journals = journalTocsImport.DownloadJournals();

            // Assert
            Assert.Equal(1000, journals.Count);
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

        static string GetJournalTocsNoMoreItemsNotice()
        {
            return ResourceReader.GetContentsOfResource("JournalTocs-no-more-items-notice.xml").Replace("&", "&amp;");
        }

        JournalTocsImport CreateJournalTocsImport()
        {
            _client = new Mock<IJournalTocsClient>();
            _issnRepo = new Mock<IBlockedISSNRepository>();

            //var journalTocsSettings = new JournalTocsSettings();;
            return new JournalTocsImport(_client.Object, _issnRepo.Object);
        }
    }
}