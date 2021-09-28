using System.Collections.Generic;
using System.Linq;
using QOAM.Core.Repositories;

namespace QOAM.Core.Import.CrossRef
{
    public class CrossRefImport : Import
    {
        readonly ICrossRefClient _client;

        public CrossRefImport(ICrossRefClient client, IBlockedISSNRepository blockedIssnRepository) : base(blockedIssnRepository)
        {
            _client = client;
        }

        public IList<Journal> DownloadJournals()
        {
            var items = _client.DownloadJournals();
            return ExcludeBlockedIssns(ParseJournals(items));
        }

        public IList<Journal> DownloadJournals(List<string> issns)
        {
            var items = new List<Item>();

            foreach (var issn in issns)
            {
                var item = _client.DownloadJournal(issn);
                items.AddRange(item);
            }

            return ExcludeBlockedIssns(ParseJournals(items));
        }

        #region Private Methdos

        static IList<Journal> ParseJournals(IEnumerable<Item> items) 
        {
            var journals = (from i in items
                            select new Journal
                            {
                                ISSN = i.IssnTypes.FirstOrDefault(x => x.Type == "electronic")?.Value ?? i.IssnTypes.FirstOrDefault()?.Value ?? "",
                                PISSN = i.IssnTypes.FirstOrDefault(x => x.Type == "print")?.Value ?? "",
                                NumberOfArticles = i.Counts.CurrentDois,
                                Publisher = new Publisher { Name = i.Publisher },
                                Subjects = i.Subjects.Select(s => new Subject { Name = s.Name }).ToList()
                            }).ToList();

            return journals;
        }

        #endregion
    }
}