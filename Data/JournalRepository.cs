namespace RU.Uci.OAMarket.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Helpers;

    using PagedList;

    using RU.Uci.OAMarket.Domain;
    using RU.Uci.OAMarket.Domain.Repositories;
    using RU.Uci.OAMarket.Domain.Repositories.Filters;

    public class JournalRepository : Repository, IJournalRepository
    {
        public JournalRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        public IList<Journal> All
        {
            get
            {
                return this.DbContext.Journals.OrderBy(j => j.Title).ToList();
            }
        }

        public IList<string> AllTitles
        {
            get
            {
                return this.DbContext.Journals.Select(j => j.Title).ToList();
            }
        }

        public IList<string> AllPublishers
        {
            get
            {
                return this.DbContext.Journals.Select(j => j.Publisher.Name).Distinct().ToList();
            }
        }

        public IList<string> AllIssns
        {
            get
            {
                return this.DbContext.Journals.Select(j => j.ISSN).ToList();
            }
        }

        public IPagedList<Journal> Search(JournalFilter filter)
        {
            var query = this.DbContext.Journals
                .Include(j => j.Publisher)
                .Include(j => j.Languages)
                .Include(j => j.Subjects)
                .Include(j => j.JournalScore)
                .Include(j => j.JournalPrice);

            if (!string.IsNullOrWhiteSpace(filter.Title))
            {
                query = query.Where(j => j.Title.ToLower().Contains(filter.Title.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(filter.Issn))
            {
                query = query.Where(j => j.ISSN.ToLower().Contains(filter.Issn.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(filter.Publisher))
            {
                query = query.Where(j => j.Publisher.Name.ToLower().Contains(filter.Publisher.ToLower()));
            }

            if (filter.Discipline.HasValue)
            {
                query = query.Where(j => j.Subjects.Any(k => k.Id == filter.Discipline));
            }

            if (filter.Language.HasValue)
            {
                query = query.Where(j => j.Languages.Any(l => l.Id == filter.Language));
            }

            if (filter.MinimumBaseScore.HasValue)
            {
                query = query.Where(j => j.JournalScore.OverallScore.AverageScore >= filter.MinimumBaseScore.Value);
            }

            if (filter.MinimumValuationScore.HasValue)
            {
                query = query.Where(j => j.JournalScore.ValuationScore.AverageScore >= filter.MinimumValuationScore.Value);
            }

            if (filter.SubmittedOnly)
            {
                query = query.Where(j => j.ScoreCards.Any(s => s.Submitted));
            }

            if (filter.MustHaveBeenScored)
            {
                query = query.Where(j => j.JournalScore.NumberOfReviewers > 0);
            }

            return ApplyOrdering(query, filter).ToPagedList(filter.PageNumber, filter.PageSize);
        }

        public IList<Journal> SearchAll(JournalFilter filter)
        {
            var query = this.DbContext.Journals.AsQueryable();

            if (filter.PublisherId.HasValue)
            {
                query = query.Where(j => j.PublisherId == filter.PublisherId.Value);
            }

            return query.ToList();
        }

        private static IOrderedQueryable<Journal> ApplyOrdering(IQueryable<Journal> query, JournalFilter filter)
        {
            switch (filter.SortMode)
            {
                case JournalSortMode.BaseScore:
                    return filter.SortDirection == SortDirection.Ascending ? 
                        query.OrderBy(j => j.JournalScore.OverallScore.AverageScore).ThenBy(j => j.JournalScore.ValuationScore.AverageScore).ThenBy(j => j.Title) :
                        query.OrderByDescending(j => j.JournalScore.OverallScore.AverageScore).ThenByDescending(j => j.JournalScore.ValuationScore.AverageScore).ThenBy(j => j.Title);
                case JournalSortMode.ValuationScore:
                    return filter.SortDirection == SortDirection.Ascending ?
                        query.OrderBy(j => j.JournalScore.ValuationScore.AverageScore).ThenBy(j => j.JournalScore.OverallScore.AverageScore).ThenBy(j => j.Title) :
                        query.OrderByDescending(j => j.JournalScore.ValuationScore.AverageScore).ThenByDescending(j => j.JournalScore.OverallScore.AverageScore).ThenBy(j => j.Title);
                case JournalSortMode.Name:
                    return filter.SortDirection == SortDirection.Ascending ? query.OrderBy(j => j.Title) : query.OrderByDescending(j => j.Title);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public Journal Find(int id)
        {
            return this.DbContext.Journals.Find(id);
        }

        public void Insert(Journal journal)
        {
            this.DbContext.Journals.Add(journal);
        }

        public int ScoredJournalsCount()
        {
            return this.DbContext.Journals.Count(j => j.JournalScore.NumberOfReviewers > 0);
        }

        public void Update(Journal journal)
        {
            this.DbContext.Entry(journal).State = EntityState.Modified;
        }
    }
}