using System.Collections.Generic;
using QOAM.Core.Import.JournalTOCs;
using QOAM.Core.Tests.Import.Resources;
using Xunit;

namespace QOAM.Core.Tests.Import
{
    public class JournalTocsXmlParserTests
    {
        [Fact]
        public void DownloadJournals_parses_journal_xml_and_converts_it_to_Journals()
        {
            var parser = new JournalTocsXmlParser();
            
            // Act
            var journals = parser.Parse(new List<string> { GetJournalTocsFirst500Xml(), GetJournalTocsNextXml() });

            // Assert
            Assert.Equal(1000, journals.Count);
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
    }
}