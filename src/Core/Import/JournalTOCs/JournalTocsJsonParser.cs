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
            var json = Json.Decode(data.First()
                .Replace("prism:", "")
                .Replace("dc:", "")
                .Replace("e-issn", "eIssn")
                .Replace("articles-per-year", "articlesPerYear"));

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
                ISSN = !string.IsNullOrWhiteSpace(data.eIssn) ? data.eIssn : data.issn,
                PISSN = data.issn,
                Title = data.title,
                Link = data.link,
                Publisher = ParsePublisher(data),
                Subjects = ParseSubjects(data),
                OpenAccess = data.rights == "Open Access",
                Country = new Country { Name = "" },
                DataSource = JournalsImportSource.JournalTOCs.ToString(),
                ArticlesPerYear = ParseArticlesPerYear(data)
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

        List<ArticlesPerYear> ParseArticlesPerYear(dynamic data)
        {
            var list = new List<ArticlesPerYear>();

            if(data.articlesPerYear == null)
                return list;

            foreach (var entry in data.articlesPerYear)
            {
                var perYear = new ArticlesPerYear
                {
                    Year = entry[0],
                    NumberOfArticles = entry[1]
                };

                list.Add(perYear);
            }

            return list.Where(a => a.Year >= 2018).ToList();
        }

        #endregion
    }
}