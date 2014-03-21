namespace QOAM.Core.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Core.Objects;
    using System.Linq;

    using PagedList;

    using QOAM.Core.Repositories.Filters;

    public class ScoreCardRepository : Repository, IScoreCardRepository
    {
        public ScoreCardRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        public ScoreCard Find(int id)
        {
            return this.DbContext.ScoreCards
                .Include(s => s.Journal)
                .Include(s => s.Journal.Publisher)
                .Include(s => s.Journal.Languages)
                .Include(s => s.Journal.Subjects)
                .Include(s => s.QuestionScores)
                .Include(s => s.QuestionScores.Select(q => q.Question))
                .First(s => s.Id == id);
        }

        public ScoreCard Find(int journalId, int userProfileId)
        {
            return this.DbContext.ScoreCards
                .Include(s => s.Journal)
                .Include(s => s.Journal.Publisher)
                .Include(s => s.Journal.Languages)
                .Include(s => s.Journal.Subjects)
                .Include(s => s.QuestionScores)
                .Include(s => s.QuestionScores.Select(q => q.Question))
                .FirstOrDefault(s => s.JournalId == journalId && s.UserProfileId == userProfileId);
        }

        public IPagedList<ScoreCard> Find(ScoreCardFilter filter)
        {
            var query = this.DbContext.ScoreCards
                .Include(s => s.UserProfile)
                .Where(s => s.JournalId == filter.JournalId)
                .Where(s => s.State == ScoreCardState.Published);

            if (filter.RequireComment)
            {
                query = query.Where(s => s.Remarks != null);
            }

            return query.OrderByDescending(s => s.DatePublished).ToPagedList(filter.PageNumber, filter.PageSize);
        }

        public IPagedList<ScoreCard> FindForUser(ScoreCardFilter filter)
        {
            var query = this.DbContext.ScoreCards
                .Include(s => s.Journal)
                .Include(s => s.Journal.JournalScore)
                .Where(s => s.UserProfileId == filter.UserProfileId);

            if (filter.State.HasValue)
            {
                query = query.Where(s => s.State == filter.State);
            }

            return query.OrderByDescending(s => s.DatePublished).ToPagedList(filter.PageNumber, filter.PageSize);
        }

        public ScoreCardStats CalculateStats(UserProfile userProfile)
        {
            var groupedStates = this.DbContext.ScoreCards.Where(s => s.UserProfileId == userProfile.Id)
                                    .GroupBy(s => s.State)
                                    .Select(g => new { State = g.Key, Count = g.Count() })
                                    .ToList();
            
            return new ScoreCardStats
                       {
                           NumberOfExpiredScoreCards = groupedStates.Where(g => g.State == ScoreCardState.Expired).Select(g => g.Count).FirstOrDefault(),
                           NumberOfPublishedScoreCards = groupedStates.Where(g => g.State == ScoreCardState.Published).Select(g => g.Count).FirstOrDefault(),
                           NumberOfUnpublishedScoreCards = groupedStates.Where(g => g.State == ScoreCardState.Unpublished).Select(g => g.Count).FirstOrDefault(),
                       };
        }

        public ScoreCardStats CalculateStats(Institution institution)
        {
            var groupedStates = this.DbContext.ScoreCards.Where(s => s.UserProfile.InstitutionId == institution.Id)
                                    .GroupBy(s => s.State)
                                    .Select(g => new { State = g.Key, Count = g.Count() })
                                    .ToList();

            return new ScoreCardStats
            {
                NumberOfExpiredScoreCards = groupedStates.Where(g => g.State == ScoreCardState.Expired).Select(g => g.Count).FirstOrDefault(),
                NumberOfPublishedScoreCards = groupedStates.Where(g => g.State == ScoreCardState.Published).Select(g => g.Count).FirstOrDefault(),
                NumberOfUnpublishedScoreCards = groupedStates.Where(g => g.State == ScoreCardState.Unpublished).Select(g => g.Count).FirstOrDefault(),
            };
        }

        public IList<ScoreCard> FindScoreCardsToBeArchived()
        {
            return this.DbContext.ScoreCards.Where(s => s.State != ScoreCardState.Expired && s.DateExpiration < DateTime.Now).ToList();
        }

        public IList<ScoreCard> FindScoreCardsThatWillBeArchived(TimeSpan toBeArchivedWindow)
        {
            var soonToBeArchivedDate = DateTime.Now + toBeArchivedWindow;

            return this.DbContext.ScoreCards
                .Include(s => s.Journal)
                .Include(s => s.UserProfile)
                .Where(s => s.State == ScoreCardState.Published && DbFunctions.TruncateTime(s.DateExpiration) == DbFunctions.TruncateTime(soonToBeArchivedDate)).ToList();
        }

        public void Insert(ScoreCard scoreCard)
        {
            this.DbContext.ScoreCards.Add(scoreCard);
        }

        public void Update(ScoreCard scoreCard)
        {
            this.DbContext.Entry(scoreCard).State = EntityState.Modified;
        }
    }
}