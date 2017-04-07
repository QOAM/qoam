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
            var journalElements = new List<XElement>();

            foreach (var doc in data)
                journalElements.AddRange(XDocument.Parse(doc).Descendants("journals").Descendants("journal").ToList());

            return journalElements.SelectMany(ParseJournal).Where(j => j.IsValid()).ToList();
        }

        #region Private Methods
        
        public IEnumerable<Journal> ParseJournal(XElement recordElement)
        {
            var regularJournal = new Journal
            {
                Title = recordElement.Element("title")?.Value,
                ISSN = ParseIssn(recordElement),
                Link = recordElement.Element("homepage_url")?.Value,
                Publisher = ParsePublisher(recordElement),
                Subjects = ParseSubjects(recordElement),
                DataSource = JournalsImportSource.JournalTOCs.ToString(),
                OpenAccess = IsOpenAccessJournal(recordElement),
                Country = new Country { Name = "" }
            };

            yield return regularJournal;
        }
        

        static string ParseIssn(XElement journalElement)
        {
            var issn = journalElement.Element("e_issn")?.Value;

            return !string.IsNullOrWhiteSpace(issn) ? issn : journalElement.Element("p_issn")?.Value;
        }

        static Publisher ParsePublisher(XElement journalElement)
        {
            var publisherInfoXmlElement = journalElement.Element("publisher");

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

        static bool IsOpenAccessJournal(XElement journalElement)
        {
            return journalElement.Element("rights")?.Value == "Open Access";
        }

        #endregion
    }
}