using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;

namespace QOAM.Core.Import.JournalTOCs
{
    public class JournalTocsJsonParser : IJournalTocsParser
    {
        public IList<Journal> Parse(IEnumerable<string> data)
        {
            var journals = new List<Journal>();
            var json = Json.Decode(data.First().Replace("prism:", "").Replace("dc:", "").Replace("e-issn", "eIssn"));

            if ((json.status != null && json.status == "error") || json.rss?.channel?.items == null)
                return journals;
            
            foreach (dynamic item in json.rss.channel.items)
            {
                journals.Add(ParseJournal(item));
            }

            return journals;
        }

        #region Private Methods

        Journal ParseJournal(dynamic data)
        {
            return new Journal
            {
                ISSN = data.eIssn ?? data.issn,
                PISSN = data.issn,
                Title = data.title,
                Link = data.link,
                Publisher = ParsePublisher(data),
                Subjects = ParseSubjects(data),
                OpenAccess = data.rights == "Open Access",
                Country = new Country { Name = "" },
                DataSource = JournalsImportSource.JournalTOCs.ToString()
            };
        }

        Publisher ParsePublisher(dynamic data)
        {
           var publisherName = data.publisher ?? Import.MissingPublisherName;

           return new Publisher { Name = publisherName };
        }

        List<Subject> ParseSubjects(dynamic data)
        {
            return data.subject == null ? new List<Subject>() : new List<Subject> { new Subject { Name = data.subject } };
        }

        #endregion
    }
}