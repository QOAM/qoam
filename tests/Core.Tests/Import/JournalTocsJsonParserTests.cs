using System.Collections.Generic;
using System.Linq;
using QOAM.Core.Import.JournalTOCs;
using Xunit;

namespace QOAM.Core.Tests.Import
{
    public class JournalTocsJsonParserTests
    {
        [Fact]
        public void DownloadJournals_parses_journal_json_and_converts_it_to_Journals()
        {
            var parser = new JournalTocsJsonParser();
            
            // Act
            var journals = parser.Parse(new List<string> { JournalTocsJson() });

            // Assert
            Assert.Equal(2, journals.Count);
            Assert.Equal("Revista de Investigación en Educación", journals.First().Title);
            Assert.Equal("1697-5200", journals.First().ISSN);
            Assert.Equal("1891-5396", journals.Last().ISSN);
        }

        [Fact]
        public void DownloadJournals_parses_articles_per_year()
        {
            var parser = new JournalTocsJsonParser();
            
            // Act
            var journals = parser.Parse(new List<string> { JournalTocsJson() });

            Assert.Equal(0, journals[0].ArticlesPerYear.Count);
            Assert.Equal(2, journals[1].ArticlesPerYear.Count);
            Assert.Equal(8, journals[1].ArticlesPerYear.Single(x => x.Year == 2019).NumberOfArticles);
            Assert.Equal(6, journals[1].ArticlesPerYear.Single(x => x.Year == 2018).NumberOfArticles);
        }

        [Fact]
        public void DownloadJournals_returns_an_empty_list_when_there_is_no_valid_journal_data()
        {
            var parser = new JournalTocsJsonParser();

            // Act
            var journals = parser.Parse(new List<string> { InvalidJournalTocsJson() });

            Assert.Equal(0, journals.Count);
        }

        [Fact]
        public void DownloadJournals_returns_an_empty_list_when_there_is_an_error_message()
        {
            var parser = new JournalTocsJsonParser();

            // Act
            var journals = parser.Parse(new List<string> { ErrorJson() });

            Assert.Equal(0, journals.Count);
        }

        static string JournalTocsJson()
        {
            return "{ \"rss\": { \"version\": \"2.0\", \"channel\": { \"title\": \"JournalTOCs API - Journal Metadata\", \"link\": \"http://www.journaltocs.ac.uk/API/RSS/GetJournalByIssn.php\", \"description\": \"Returning 1 journals found for your query matching the GET parameters of your API request.\", \"copyright\": \"Copyright 2020 JournalTOCs\", \"pubDate\": \"Thu, 23 Jan 2020 16:38:29 GMT\", \"webMaster\": \"journaltocs@hw.ac.uk\", \"items\": [ { \"title\": \"Revista de Investigación en Educación\", \"link\": \"http://webs.uvigo.es/reined/ojs/index.php/reined/index\", \"guid\": \"http://webs.uvigo.es/reined/ojs/index.php/reined/index\", \"description\": \"<p><![CDATA[Journal HomePage: http://webs.uvigo.es/reined/ojs/index.php/reined/index<br> printISSN: 1697-5200<br> journaltocID: 20461<br> Publisher: Universidade de Vigo<br> Access Rights: Open Access<br>Last Updated: Thu, 14 Jun 2018 00:00:00 GMT]]>\", \"prism:issn\": \"1697-5200\", \"prism:e-issn\": \"1697-5200\", \"dc:identifier\": \"20461\", \"dc:publisher\": \"Universidade de Vigo\", \"pubDate\": \"Thu, 14 Jun 2018 00:00:00 GMT\", \"dc:subject\": \"EDUCATION\", \"dc:rights\" : \"Open Access\", \"rating:JournalTOCs\" : \"0\", \"prism:publicationName\": \"Revista de Investigación en Educación\", \"articles-per-year\": null }, { \"title\": \"Fauna Norvegica\", \"link\": \"http://www.ntnu.no/ojs/index.php/fauna_norvegica\", \"guid\": \"http://www.ntnu.no/ojs/index.php/fauna_norvegica\", \"description\": \"<p><![CDATA[Journal HomePage: http://www.ntnu.no/ojs/index.php/fauna_norvegica<br> printISSN: 1502-4873<br> journaltocID: 20463<br> Publisher: NTNU<br> Access Rights: Open Access<br>Last Updated: Wed, 14 Oct 2015 00:00:00 GMT]]>\", \"prism:issn\": \"1502-4873\", \"prism:e-issn\": \"1891-5396\", \"dc:identifier\": \"20463\", \"dc:publisher\": \"NTNU\", \"pubDate\": \"Wed, 14 Oct 2015 00:00:00 GMT\", \"dc:subject\": \"BIOLOGY\", \"dc:rights\" : \"Open Access\", \"rating:JournalTOCs\" : \"0\", \"prism:publicationName\": \"Fauna Norvegica\", \"articles-per-year\": [[2012,24],[2010,11],[2013,10],[2019,8],[2018,6],[2016,5],[2015,5],[2017,4],[2009,4],[2014,3],[2008,1]] } ] } } }";
        }

        static string InvalidJournalTocsJson()
        {
            return "{\r\n\t\"rss\":\r\n\t{\r\n\t\t\"version\": \"2.0\",\r\n\t\t\"channel\":\r\n\t\t{\r\n\t\t\t\"title\": \"JournalTOCs API - Journal Metadata\",\r\n\t\t\t\"link\": \"http://www.journaltocs.ac.uk/API/RSS/GetJournalByIssn.php\",\r\n\t\t\t\"description\": \"The API returns the metadata of the journals matching the ISSNs provided through its GET verb.\",\r\n\t\t\t\"copyright\": \"Copyright 2017 JournalTOCs\",\r\n\t\t\t\"pubDate\": \"Mon, 27 Mar 2017 19:05:03 GMT\",\r\n\t\t\t\"webMaster\": \"journaltocs@hw.ac.uk\"\r\n\t\t\t}\r\n\t}\r\n}";
        }

        static string ErrorJson()
        {
            return "{ \"status\": \"error\", \"error-code\": \"11\", \"message\": \"I didn't find more items to return, from within your authorised range of journals.\" }";
        }
    }
}