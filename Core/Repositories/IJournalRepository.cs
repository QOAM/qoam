namespace QOAM.Core.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using PagedList;

    using QOAM.Core.Repositories.Filters;

    public interface IJournalRepository
    {
        IPagedList<Journal> Search(JournalFilter filter);
        IList<Journal> SearchAll(JournalFilter filter);
        Journal Find(int id);
        IQueryable<string> Titles(string query);
        IQueryable<string> Publishers(string query);
        IQueryable<string> Issns(string query);
        IList<Journal> All { get; }
        IQueryable<string> AllIssns { get; }
        IList<Journal> AllIncluding(params Expression<Func<Journal, object>>[] includeProperties);

        void InsertOrUpdate(Journal journal);
        void Save();

        int ScoredJournalsCount();

        IQueryable<Journal> SearchByISSN(IEnumerable<string> issns);
    }
}