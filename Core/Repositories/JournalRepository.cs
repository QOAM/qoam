namespace QOAM.Core.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Helpers;

    using LinqKit;

    using PagedList;

    using QOAM.Core.Repositories.Filters;

    public class JournalRepository : Repository<Journal>, IJournalRepository
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

        public Journal FindByIssn(string issn)
        {
            return this.DbContext.Journals.FirstOrDefault(j => j.ISSN.ToLower() == issn.ToLower());
        }

        public IQueryable<string> Titles(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return Enumerable.Empty<string>().AsQueryable();
            } 

            return this.DbContext.Journals.Where(j => j.Title.ToLower().StartsWith(query.ToLower())).Select(j => j.Title);
        }

        public IQueryable<string> Publishers(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return Enumerable.Empty<string>().AsQueryable();
            } 

            return this.DbContext.Journals.Where(j => j.Publisher.Name.ToLower().StartsWith(query.ToLower())).Select(j => j.Publisher.Name).Distinct();
        }

        public IQueryable<string> Issns(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return Enumerable.Empty<string>().AsQueryable();
            } 

            return this.DbContext.Journals.Where(j => j.ISSN.ToLower().StartsWith(query.ToLower())).Select(j => j.ISSN);
        }

        public IQueryable<string> AllIssns
        {
            get
            {
                return this.DbContext.Journals.Select(j => j.ISSN);    
            }
        }

        public IPagedList<Journal> Search(JournalFilter filter)
        {
            var query = this.DbContext.Journals
                .Include(j => j.Publisher)
                .Include(j => j.Languages)
                .Include(j => j.Subjects)
                .Include(j => j.JournalScore).
                AsExpandable();

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

            if (filter.SubmittedOnly)
            {
                query = query.Where(j => j.ValuationScoreCards.Any(v => v.State == ScoreCardState.Published));
            }

            if (filter.MustHaveBeenScored && filter.SwotMatrix.Count == 0)
            {
                query = query.Where(j => j.JournalScore.NumberOfBaseReviewers > 0 || j.JournalScore.NumberOfValuationReviewers > 0);
            }
            else if (filter.MustHaveBeenScored && filter.SwotMatrix.Count > 0)
            {
                query = query.Where(j => j.JournalScore.NumberOfBaseReviewers > 0 && j.JournalScore.NumberOfValuationReviewers > 0);
            }

            if (filter.SwotMatrix.Count > 0)
            {
                query = query.Where(AddSwotMatrixFilter(filter));
            }

            return ApplyOrdering(query, filter).ToPagedList(filter.PageNumber, filter.PageSize);
        }

        private static Expression<Func<Journal, bool>> AddSwotMatrixFilter(JournalFilter filter)
        {
            var predicate = PredicateBuilder.False<Journal>();

            foreach (var swot in filter.SwotMatrix)
            {
                switch (swot)
                {
                    case SwotVerdict.StrongJournal:
                        predicate = predicate.Or(j => j.JournalScore.OverallScore.AverageScore >= 3 && j.JournalScore.ValuationScore.AverageScore >= 3);
                        break;
                    case SwotVerdict.WeakerJournal:
                        predicate = predicate.Or(j => j.JournalScore.OverallScore.AverageScore < 3 && j.JournalScore.ValuationScore.AverageScore < 3);
                        break;
                    case SwotVerdict.ThreatToAuthor:
                        predicate = predicate.Or(j => j.JournalScore.OverallScore.AverageScore >= 3 && j.JournalScore.ValuationScore.AverageScore < 3);
                        break;
                    case SwotVerdict.OpportunityToPublisher:
                        predicate = predicate.Or(j => j.JournalScore.OverallScore.AverageScore < 3 && j.JournalScore.ValuationScore.AverageScore >= 3);
                        break;
                }
            }

            return predicate;
        }

        public IQueryable<Journal> SearchByISSN(IEnumerable<string> issns)
        {
            return this.DbContext.Journals.Where(j => issns.Contains(j.ISSN));
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

        public int ScoredJournalsCount()
        {
            return this.DbContext.Journals.Count(j => j.JournalScore.NumberOfBaseReviewers > 0);
        }

        public IList<Journal> AllIncluding(params Expression<Func<Journal, object>>[] includeProperties)
        {
            IQueryable<Journal> query = this.DbContext.Journals;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query.ToList();
        }
    }
}