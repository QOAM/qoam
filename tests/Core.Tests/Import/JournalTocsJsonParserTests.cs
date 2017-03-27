using System.Collections.Generic;
using System.Linq;
using QOAM.Core.Import.JournalTOCs;
using Xunit;

namespace QOAM.Core.Tests.Import
{
    public class JournalTocsJsonParserTests
    {
        [Fact]
        public void DownloadJournals_parses_journal_xml_and_converts_it_to_Journals()
        {
            var parser = new JournalTocsJsonParser();
            
            // Act
            var journals = parser.Parse(new List<string> { GetJournalTocsJson() });

            // Assert
            Assert.Equal(2, journals.Count);
            Assert.Equal("Revista de Investigación en Educación", journals.First().Title);
            Assert.Equal("1697-5200", journals.First().ISSN);
            Assert.Equal("1891-5396", journals.Last().ISSN);
        }

        static string GetJournalTocsJson()
        {
            return "{\r\n\t\"rss\":\r\n\t{\r\n\t\t\"version\": \"2.0\",\r\n\t\t\"channel\":\r\n\t\t{\r\n\t\t\t\"title\": \"JournalTOCs API - Journal Metadata\",\r\n\t\t\t\"link\": \"http://www.journaltocs.ac.uk/API/RSS/GetJournalByIssn.php\",\r\n\t\t\t\"description\": \"The API returns the metadata of the journals matching the ISSNs provided through its GET verb.\",\r\n\t\t\t\"copyright\": \"Copyright 2017 JournalTOCs\",\r\n\t\t\t\"pubDate\": \"Mon, 27 Mar 2017 19:05:03 GMT\",\r\n\t\t\t\"webMaster\": \"journaltocs@hw.ac.uk\",\r\n\t\t\t\"items\": [\r\n  {\r\n\t\t\t\t\t\"title\": \"Revista de Investigación en Educación\",\r\n\t\t\t\t\t\"link\": \"http://webs.uvigo.es/reined/ojs/index.php/reined/index\",\r\n\t\t\t\t\t\"guid\": \"http://webs.uvigo.es/reined/ojs/index.php/reined/index\",\r\n\t\t\t\t\t\"description\": \"<p><![CDATA[Journal HomePage: http://webs.uvigo.es/reined/ojs/index.php/reined/index<br> printISSN: 1697-5200<br> journaltocID: 20461<br> Publisher: Universidade de Vigo<br> Access Rights: Open Access<br>Last Updated: Wed, 1 Feb 2012 00:00:00 GMT]]>\",\r\n\t\t\t\t\t\"prism:issn\": \"1697-5200\",\r\n\t\t\t\t\t\"prism:e-issn\": \"1697-5200\",\r\n\t\t\t\t\t\"dc:identifier\": \"20461\",\r\n\t\t\t\t\t\"dc:publisher\": \"Universidade de Vigo\",\r\n\t\t\t\"pubDate\": \"Wed, 1 Feb 2012 00:00:00 GMT\",\n\t\t\t\t\t\"dc:subject\": \"EDUCATION\",\r\n       \"dc:rights\" : \"Open Access\",\n       \"rating:JournalTOCs\" : \"1\",\n\r\n\r\n\t\t\t\t\t\"prism:publicationName\": \"Revista de Investigación en Educación\"\r\n\t\t\t\t},\n\n\t\t\t\t{\r\n\t\t\t\t\t\"title\": \"Fauna Norvegica\",\r\n\t\t\t\t\t\"link\": \"http://www.ntnu.no/ojs/index.php/fauna_norvegica\",\r\n\t\t\t\t\t\"guid\": \"http://www.ntnu.no/ojs/index.php/fauna_norvegica\",\r\n\t\t\t\t\t\"description\": \"<p><![CDATA[Journal HomePage: http://www.ntnu.no/ojs/index.php/fauna_norvegica<br> printISSN: 1502-4873<br> journaltocID: 20463<br> Publisher: NTNU<br> Access Rights: Open Access<br>Last Updated: Wed, 14 Oct 2015 00:00:00 GMT]]>\",\r\n\t\t\t\t\t\"prism:issn\": \"1502-4873\",\r\n\t\t\t\t\t\"prism:e-issn\": \"1891-5396\",\r\n\t\t\t\t\t\"dc:identifier\": \"20463\",\r\n\t\t\t\t\t\"dc:publisher\": \"NTNU\",\r\n\t\t\t\"pubDate\": \"Wed, 14 Oct 2015 00:00:00 GMT\",\n\t\t\t\t\t\"dc:subject\": \"BIOLOGY\",\r\n       \"dc:rights\" : \"Open Access\",\n       \"rating:JournalTOCs\" : \"0\",\n\r\n\r\n\t\t\t\t\t\"prism:publicationName\": \"Fauna Norvegica\"\r\n\t\t\t\t}\r\n            ]\r\n\t\t}\r\n\t}\r\n}";
        }
    }
}