namespace RU.Uci.OAMarket.Domain.Import
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    using Validation;

    public class UlrichsImport
    {
        private const string MissingPublisherName = "<none indicated>";

        private readonly UlrichsClient ulrichsClient;
        private readonly UlrichsCache ulrichsCache;

        public UlrichsImport(UlrichsClient ulrichsClient, UlrichsCache ulrichsCache)
        {
            Requires.NotNull(ulrichsClient, "ulrichsClient");
            Requires.NotNull(ulrichsCache, "ulrichsCache");

            this.ulrichsClient = ulrichsClient;
            this.ulrichsCache = ulrichsCache;
        }

        public enum UlrichsJournalType
        {
            All = 0,
            OpenAccess
        }

        public IList<Journal> GetJournals(UlrichsJournalType journalType)
        {
            this.DownloadJournals();

            var journals = new List<Journal>();

            foreach (var journalsXml in this.ulrichsCache.GetAll())
            {
                journals.AddRange(this.ParseJournals(journalsXml, journalType));
            }

            return journals;
        }

        public IList<Journal> ParseJournals(string xml, UlrichsJournalType journalType)
        {
            var ulrichsElements = XDocument.Parse(xml).Descendants("UlrichsDataRecords").Descendants("Record");
            var journalElements = journalType == UlrichsJournalType.All ? ulrichsElements.Where(IsJournal) : ulrichsElements.Where(IsOpenAccessJournal);

            return journalElements.SelectMany(this.ParseJournal).ToList();
        }

        public IEnumerable<Journal> ParseJournal(XElement recordElement)
        {
            var regularJournal = new Journal
                                 {
                                     Title = recordElement.Element("Title").Value,
                                     ISSN = recordElement.Element("ISSN").Value,
                                     Link = recordElement.Element("JournalWebsiteURL").Value,
                                     Publisher = ParsePublisher(recordElement),
                                     Country = ParseCountry(recordElement),
                                     Languages = this.ParseLanguages(recordElement),
                                     Subjects = this.ParseSubjects(recordElement)
                                 };

            yield return regularJournal;

            foreach (var alternateEditionElement in GetAlternateEditionElements(recordElement))
            {
                yield return new Journal
                             {
                                 Title = regularJournal.Title,
                                 ISSN = alternateEditionElement.Element("ISSN").Value,
                                 Link = regularJournal.Link,
                                 Publisher = ParsePublisher(recordElement),
                                 Country = ParseCountry(recordElement),
                                 Languages = this.ParseLanguages(recordElement),
                                 Subjects = this.ParseSubjects(recordElement)
                             };
            }
        }

        private static IEnumerable<XElement> GetAlternateEditionElements(XElement recordElement)
        {
            return recordElement.Descendants("AlternateEdition").Where(IsAlternateEdition);
        }

        private static bool IsAlternateEdition(XElement alternateEditionElement)
        {
            var statusElement = alternateEditionElement.Element("Status");

            if (statusElement != null && statusElement.Value == "Ceased")
            {
                return false;
            }

            return alternateEditionElement.Element("ISSN") != null;
        }

        private static Publisher ParsePublisher(XElement recordElement)
        {
            var publisherInfoXmlElement = recordElement.Element("PublisherInfo");

            if (publisherInfoXmlElement == null)
            {
                return new Publisher { Name = MissingPublisherName };
            }

            return new Publisher { Name = publisherInfoXmlElement.Element("Name").Value.Trim() };
        }

        private static Country ParseCountry(XElement recordElement)
        {
            return new Country { Name = recordElement.Element("CountryCode").Element("Name").Value.Trim() };
        }

        private static Language ParseLanguage(XElement languageElement)
        {
            return new Language { Name = languageElement.Element("Language").Value.Trim() };
        }

        private static Subject ParseSubject(XElement subjectElement)
        {
            return new Subject { Name = subjectElement.Value.Trim().ToLowerInvariant() };
        }

        private static bool IsJournal(XElement journalElement)
        {
            if (journalElement.Element("Title") == null ||
                journalElement.Element("ISSN") == null ||
                journalElement.Element("JournalWebsiteURL") == null ||
                journalElement.Element("DocumentType") == null ||
                journalElement.Element("CountryCode") == null)
            {
                return false;
            }

            var documentTypeElement = journalElement.Element("DocumentType").Element("Type");

            if (documentTypeElement == null)
            {
                return false;
            }

            return documentTypeElement.Value == "JL";
        }

        private static bool IsOpenAccessJournal(XElement journalElement)
        {
            if (IsJournal(journalElement))
            {
                var openAccessElement = journalElement.Element("OpenAccessIndicator");

                if (openAccessElement == null)
                {
                    return false;
                }

                return openAccessElement.Value == "Y";
            }

            return false;
        }

        private void DownloadJournals()
        {
            if (!this.ulrichsClient.TryLogOn())
            {
                throw new InvalidOperationException("Could not log-in to Ulrichs.");
            }

            if (!this.ulrichsClient.RequestReportToken())
            {
                throw new InvalidOperationException("Could not request report token.");
            }

            for (var page = 1; page <= this.ulrichsClient.NumberOfPages; ++page)
            {
                if (this.ulrichsCache.HasExpired(page))
                {
                    this.ulrichsCache.Add(this.DownloadJournalsForPage(page), page);
                }
            }

            this.ulrichsClient.LogOff();
        }

        private IList<Language> ParseLanguages(XElement recordElement)
        {
            var languagesElement = recordElement.Element("LanguageSet");

            if (languagesElement == null)
            {
                return new List<Language>();
            }

            return languagesElement.Elements("Language").Select(ParseLanguage).ToList();
        }

        private IList<Subject> ParseSubjects(XElement recordElement)
        {
            var subjectsElement = recordElement.Element("SubjectSet");

            if (subjectsElement == null)
            {
                return new List<Subject>();
            }

            return subjectsElement.Elements("Subject").Select(ParseSubject).ToList();
        }

        private string DownloadJournalsForPage(int page)
        {
            return this.ulrichsClient.MakePageRequestAttempts(page);
        }
    }
}