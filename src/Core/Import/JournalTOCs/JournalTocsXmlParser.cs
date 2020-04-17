using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using QOAM.Core.Helpers;

namespace QOAM.Core.Import.JournalTOCs
{
    public class JournalTocsXmlParser : IJournalTocsParser
    {
        public IList<Journal> Parse(IEnumerable<string> data)
        {
            var recordElements = new List<XElement>();

            foreach (var doc in data)
                recordElements.AddRange(XDocument.Parse(doc).Descendants("journals").Descendants("journal").ToList());

            return recordElements.SelectMany(ParseJournal).Where(j => j.IsValid()).ToList();
        }

        #region Private Methods
        
        public IEnumerable<Journal> ParseJournal(XElement recordElement)
        {
            var regularJournal = new Journal
            {
                Title = recordElement.Element("title")?.Value,
                ISSN = ParseIssn(recordElement),
                PISSN = recordElement.Element("p_issn")?.Value,
                Link = recordElement.Element("homepage_url")?.Value,
                Publisher = ParsePublisher(recordElement),
                Subjects = ParseSubjects(recordElement),
                DataSource = JournalsImportSource.JournalTOCs.ToString(),
                OpenAccess = IsOpenAccessJournal(recordElement),
                Country = new Country { Name = "" },
                ArticlesPerYear = ParseArticlesPerYear(recordElement)
            };

            yield return regularJournal;
        }

        static string ParseIssn(XElement recordElement)
        {
            var issn = recordElement.Element("e_issn")?.Value;

            return !string.IsNullOrWhiteSpace(issn) ? issn : recordElement.Element("p_issn")?.Value;
        }

        static Publisher ParsePublisher(XElement recordElement)
        {
            var publisherInfoXmlElement = recordElement.Element("publisher");

            var publisherName = publisherInfoXmlElement?.Value ?? Import.MissingPublisherName;

            return new Publisher { Name = publisherName };
        }

        static Language ParseLanguage(XElement languageElement)
        {
            return new Language { Name = languageElement.Element("Language")?.Value.Trim() };
        }

        static IList<Subject> ParseSubjects(XElement recordElement)
        {
            var subjectsElement = recordElement.Element("subjects");

            return subjectsElement?.Elements("subject").Select(ParseSubject).ToList() ?? new List<Subject>();
        }

        static Subject ParseSubject(XElement subjectElement)
        {
            return new Subject { Name = subjectElement.Value.Trim().ToLowerInvariant() };
        }

        static bool IsOpenAccessJournal(XElement recordElement)
        {
            return recordElement.Element("rights")?.Value == "Open Access";
        }

        static List<ArticlesPerYear> ParseArticlesPerYear(XElement recordElement)
        {
            var articlesPerYearElement = recordElement.Element("articles-per-year");
            var articlesIssued = articlesPerYearElement?.Elements("articles-issued").ToList();

            if(articlesIssued == null || !articlesIssued.Any())
                return new List<ArticlesPerYear>();

            var parsed = articlesIssued.Select(ParseArticlesIssued).ToList();
            return parsed.Where(a => a.Year >= 2018).ToList();
        }

        static ArticlesPerYear ParseArticlesIssued(XElement articlesPerYearElement)
        {
            int.TryParse(articlesPerYearElement.Value?.Trim(), out var numberOfArticles);
            int.TryParse(articlesPerYearElement.Attribute("year")?.Value, out var year);

            return new ArticlesPerYear
            {
                NumberOfArticles = numberOfArticles,
                Year = year
            };
        }

        #endregion
    }
}