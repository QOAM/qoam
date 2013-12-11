namespace RU.Uci.OAMarket.Domain.Import
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    using Validation;

    public class UlrichsImport
    {
        public enum UlrichsJournalType
        {
            All = 0,
            OpenAccess
        }

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

        public IList<Journal> GetJournals(UlrichsJournalType journalType)
        {
            this.DownloadJournals();

            var journals = new List<Journal>();
            
            foreach (var journalsXml in this.ulrichsCache.GetAll())
            {
                if (journalsXml.Contains("1466-8033"))
                {
                    Console.WriteLine();
                }

                journals.AddRange(this.ParseJournals(journalsXml, journalType));
            }

            return journals;
        }

        public IList<Journal> ParseJournals(string xml, UlrichsJournalType journalType)
        {
            var journalElements = XDocument.Parse(xml).Descendants("UlrichsDataRecords").Descendants("Record");
            journalElements = journalType == UlrichsJournalType.All ? journalElements.Where(IsJournal) : journalElements.Where(IsOpenAccessJournal);

            return journalElements.Select(this.ParseJournal).ToList();
        }

        private Journal ParseJournal(XElement recordElement)
        {
            return new Journal
                   {
                       Title = recordElement.Element("Title").Value, 
                       ISSN = recordElement.Element("ISSN").Value, 
                       Link = recordElement.Element("JournalWebsiteURL").Value, 
                       Publisher = ParsePublisher(recordElement), 
                       Country = ParseCountry(recordElement), 
                       Languages = this.ParseLanguages(recordElement), 
                       Subjects = this.ParseSubjects(recordElement)
                   };
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

        private IList<Language> ParseLanguages(XElement recordElement)
        {
            var languagesElement = recordElement.Element("LanguageSet");

            if (languagesElement == null)
            {
                return new List<Language>();
            }

            return languagesElement.Elements("Language").Select(ParseLanguage).ToList();
        }

        private static Language ParseLanguage(XElement languageElement)
        {
            return new Language { Name = languageElement.Element("Language").Value.Trim() };
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

        private static Subject ParseSubject(XElement subjectElement)
        {
            return new Subject { Name = subjectElement.Value.Trim().ToLowerInvariant() };
        }

        private static bool IsJournal(XElement journalElement)
        {
            var xElement = journalElement.Element("ISSN");

            if (xElement != null)
            {
                if (xElement.Value == "1553-3514")
                {
                    Console.WriteLine();
                }
            }

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

        private string DownloadJournalsForPage(int page)
        {
            return this.ulrichsClient.MakePageRequestAttempts(page);
        }
    }
}