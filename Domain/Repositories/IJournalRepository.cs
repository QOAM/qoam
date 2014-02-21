namespace RU.Uci.OAMarket.Domain.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

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

        IList<Journal> AllIncluding(params Expression<Func<Journal, object>>[] includeProperties);

        void Update(Journal journal);
        void Insert(Journal journal);
        void Save();

        int ScoredJournalsCount();

        IQueryable<Journal> SearchByISSN(IEnumerable<string> issns);
    }
}