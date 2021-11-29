using System.Data.Entity.Core.Objects;
using QOAM.Core.Helpers;

namespace QOAM.Core.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;
    using PagedList;

    using Filters;

    public class JournalRepository : Repository<Journal>, IJournalRepository
    {
        public JournalRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        public bool EnableProxyCreation
        {
            get => DbContext.Configuration.ProxyCreationEnabled;
            set => DbContext.Configuration.ProxyCreationEnabled = value;
        }

        public IList<Journal> All
        {
            get
            {
                return DbContext.Journals.OrderBy(j => j.Title).ToList();
            }
        }

        public IList<Journal> AllWhere(Expression<Func<Journal, bool>> whereClause)
        {
            var query = DbContext.Journals.Where(whereClause);
            return query.ToList();
        }

        public Journal FindByIssn(string issn)
        {
            return DbContext.Journals.FirstOrDefault(j => j.ISSN.ToLower() == issn.ToLower());
        }

        public Journal FindByIssnOrPIssn(string issn)
        {
            return DbContext.Journals.FirstOrDefault(j => j.ISSN.ToLower() == issn.ToLower() || j.PISSN.ToLower() == issn.ToLower());
        }

        public IQueryable<string> Titles(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return Enumerable.Empty<string>().AsQueryable();
            } 

            return DbContext.Journals.Where(j => j.Title.ToLower().StartsWith(query.ToLower())).Select(j => j.Title);
        }

        public IQueryable<string> Publishers(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return Enumerable.Empty<string>().AsQueryable();
            } 

            return DbContext.Journals.Where(j => j.Publisher.Name.ToLower().StartsWith(query.ToLower())).Select(j => j.Publisher.Name).Distinct();
        }

        public IQueryable<string> Issns(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return Enumerable.Empty<string>().AsQueryable();
            } 

            return DbContext.Journals.Where(j => j.ISSN.ToLower().StartsWith(query.ToLower())).Select(j => j.ISSN);
        }

        public IQueryable<string> Subjects(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return Enumerable.Empty<string>().AsQueryable();
            }

            return DbContext.Subjects.Where(s => s.Name.ToLower().StartsWith(query.ToLower())).Where(s => s.Journals.Any()).Select(s => s.Name);
        }

        public IQueryable<string> Languages(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return Enumerable.Empty<string>().AsQueryable();
            }

            return DbContext.Languages.Where(s => s.Name.ToLower().StartsWith(query.ToLower())).Where(s => s.Journals.Any()).Select(s => s.Name);
        }

        public IQueryable<string> AllIssns
        {
            get
            {
                return DbContext.Journals.Select(j => j.ISSN);    
            }
        }

        public IPagedList<Journal> Search(JournalFilter filter)
        {
            var query = DbContext.Journals
                .Include(j => j.Publisher)
                .Include(j => j.Languages)
                .Include(j => j.Subjects);

            return query.Search(filter);
        }

        public IQueryable<Journal> SearchByISSN(IEnumerable<string> issns)
        {
            return DbContext.Journals.Where(j => issns.Contains(j.ISSN));
        }

        public IList<Journal> SearchAll(JournalFilter filter)
        {
            var query = DbContext.Journals.AsQueryable();

            if (filter.PublisherId.HasValue)
            {
                query = query.Where(j => j.PublisherId == filter.PublisherId.Value);
            }

            return query.ToList();
        }

        public Journal Find(int id)
        {
            return DbContext.Journals.Find(id);
        }

        public int BaseScoredJournalsCount()
        {
            return DbContext.Journals.Count(j => j.NumberOfBaseReviewers > 0);
        }

        public int ValuationScoredJournalsCount()
        {
            return DbContext.Journals.Count(j => j.NumberOfValuationReviewers > 0);
        }

        public int JournalsWithSwotCount()
        {
            return DbContext.Journals.Count(j => j.OverallScore.AverageScore > 0 && j.ValuationScore.AverageScore > 0);
        }

        public IList<Journal> AllIncluding(params Expression<Func<Journal, object>>[] includeProperties)
        {
            //Log to Output window...
            //DbContext.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);

            IQueryable<Journal> query = DbContext.Journals;
            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return query.ToList();
        }

        public IList<Journal> AllWhereIncluding(Expression<Func<Journal, bool>> whereClause, params Expression<Func<Journal, object>>[] includeProperties)
        {
            var query = DbContext.Journals.Where(whereClause);
            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return query.ToList();
        }

        public ListPrice FindListPriceByJournalId(int journalId)
        {
            return DbContext.ListPrices.FirstOrDefault(x => x.JournalId == journalId);
        }
    }
}