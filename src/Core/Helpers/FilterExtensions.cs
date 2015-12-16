using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Helpers;
using LinqKit;
using PagedList;
using QOAM.Core.Repositories.Filters;

namespace QOAM.Core.Helpers
{
    public static class FilterExtensions
    {
        public static IPagedList<Journal> Search(this IQueryable<Journal> query, JournalFilter filter)
        {
            query = query.AsExpandable();

            if (!string.IsNullOrWhiteSpace(filter.Title))
                query = query.Where(j => j.Title.ToLower().Contains(filter.Title.ToLower()));

            if (!string.IsNullOrWhiteSpace(filter.Issn))
                query = query.Where(j => j.ISSN.ToLower().Contains(filter.Issn.ToLower()));

            if (!string.IsNullOrWhiteSpace(filter.Publisher))
                query = query.Where(j => j.Publisher.Name.ToLower().Contains(filter.Publisher.ToLower()));

            if (filter.Discipline.HasValue)
                query = query.Where(j => j.Subjects.Any(k => k.Id == filter.Discipline));

            if (filter.Language.HasValue)
                query = query.Where(j => j.Languages.Any(l => l.Id == filter.Language));

            if (filter.SubmittedOnly)
                query = query.Where(j => j.ValuationScoreCards.Any(v => v.State == ScoreCardState.Published));

            if (filter.MustHaveBeenScored && filter.SwotMatrix.Count == 0)
                query = query.Where(j => j.JournalScore.NumberOfBaseReviewers > 0 || j.JournalScore.NumberOfValuationReviewers > 0);
            else if (filter.MustHaveBeenScored && filter.SwotMatrix.Count > 0)
                query = query.Where(j => j.JournalScore.NumberOfBaseReviewers > 0 && j.JournalScore.NumberOfValuationReviewers > 0);

            if (filter.SwotMatrix.Count > 0)
                query = query.Where(AddSwotMatrixFilter(filter));

            return ApplyOrdering(query, filter).ToPagedList(filter.PageNumber, filter.PageSize);
        }

        static Expression<Func<Journal, bool>> AddSwotMatrixFilter(JournalFilter filter)
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

        static IOrderedQueryable<Journal> ApplyOrdering(IQueryable<Journal> query, JournalFilter filter)
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
    }
}