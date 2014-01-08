namespace RU.Uci.OAMarket.Domain.Repositories
{
    using System.Collections.Generic;

    using PagedList;

    using RU.Uci.OAMarket.Domain.Repositories.Filters;

    public interface IJournalRepository
    {
        IPagedList<Journal> Search(JournalFilter filter);
        IList<Journal> SearchAll(JournalFilter filter);
        Journal Find(int id);

        IList<string> AllTitles { get; }
        IList<string> AllPublishers { get; }
        IList<string> AllIssns { get; }
        IList<Journal> All { get; }

        void Update(Journal journal);
        void Insert(Journal journal);
        void Save();

        int ScoredJournalsCount();
    }
}