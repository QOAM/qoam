namespace QOAM.Core.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using PagedList;

    using Filters;

    public interface IJournalRepository : IRepository
    {
        ApplicationDbContext DbContext { get; }
        bool EnableProxyCreation { get; set; }
        IList<Journal> All { get; }

        IQueryable<string> AllIssns { get; }

        IPagedList<Journal> Search(JournalFilter filter);

        IList<Journal> SearchAll(JournalFilter filter);

        Journal Find(int id);

        Journal FindByIssn(string issn);

        IQueryable<string> Titles(string query);

        IQueryable<string> Publishers(string query);

        IQueryable<string> Issns(string query);

        IQueryable<string> Subjects(string query);

        IQueryable<string> Languages(string query);

        IList<Journal> AllIncluding(params Expression<Func<Journal, object>>[] includeProperties);

        void InsertOrUpdate(Journal journal);

        void Save();

        void Delete(Journal journal);

        int BaseScoredJournalsCount();

        IQueryable<Journal> SearchByISSN(IEnumerable<string> issns);
        int ValuationScoredJournalsCount();
        int JournalsWithSwotCount();
        IList<Journal> AllWhereIncluding(Expression<Func<Journal, bool>> whereClause, params Expression<Func<Journal, object>>[] includeProperties);
        ListPrice FindListPriceByJournalId(int journalId);
        IList<Journal> AllWhere(Expression<Func<Journal, bool>> whereClause);
        Journal FindByIssnOrPIssn(string issn);
    }
}