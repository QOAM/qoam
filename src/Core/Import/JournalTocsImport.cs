using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using NLog;
using QOAM.Core.Helpers;
using QOAM.Core.Repositories;

namespace QOAM.Core.Import
{
    public class JournalTocsImport : Import
    {
        readonly IJournalTocsClient _client;
        static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public JournalTocsImport(IJournalTocsClient client, IBlockedISSNRepository blockedIssnRepository) : base(blockedIssnRepository)
        {
            _client = client;
        }

        public IList<Journal> DownloadJournals(JournalTocsFetchMode action = JournalTocsFetchMode.Update)
        {
            return ExcludeBlockedIssns(ParseJournals(action));
        }

        #region Private Methods

        IList<Journal> ParseJournals(JournalTocsFetchMode action = JournalTocsFetchMode.Update)
        {
            var xml = GetXml(action);

            var journalElements = new List<XElement>();

            foreach (var doc in xml)
                journalElements.AddRange(XDocument.Parse(doc).Descendants("journals").Descendants("journal").ToList());
                                   
            return journalElements.SelectMany(ParseJournal).Where(j => j.IsValid()).ToList();
        }

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
                OpenAccess = IsOpenAccessJournal(recordElement)
            };

            yield return regularJournal;
        }

        IEnumerable<string> GetXml(JournalTocsFetchMode action = JournalTocsFetchMode.Update)
        {
            return _client.DownloadJournals(action);
        }

        static string ParseIssn(XElement journalElement)
        {
            var pIssn = journalElement.Element("p_issn")?.Value;

            return !string.IsNullOrWhiteSpace(pIssn) ? pIssn : journalElement.Element("e_issn")?.Value;
        }

        static Publisher ParsePublisher(XElement journalElement)
        {
            var publisherInfoXmlElement = journalElement.Element("publisher");

            var publisherName = publisherInfoXmlElement?.Value ?? MissingPublisherName;

            return new Publisher { Name = publisherName };
        }

        static Language ParseLanguage(XElement languageElement)
        {
            return new Language { Name = languageElement.Element("Language")?.Value.Trim() };
        }

        IList<Subject> ParseSubjects(XElement recordElement)
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