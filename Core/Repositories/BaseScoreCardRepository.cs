namespace QOAM.Core.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using EntityFramework.Extensions;
    using PagedList;

    using QOAM.Core.Repositories.Filters;

    public class BaseScoreCardRepository : Repository<BaseScoreCard>, IBaseScoreCardRepository
    {
        public BaseScoreCardRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        public BaseScoreCard Find(int id)
        {
            return this.DbContext.BaseScoreCards
                .Include(s => s.Journal)
                .Include(s => s.Journal.Publisher)
                .Include(s => s.Journal.Languages)
                .Include(s => s.Journal.Subjects)
                .Include(s => s.QuestionScores)
                .Include(s => s.QuestionScores.Select(q => q.Question))
                .First(s => s.Id == id);
        }

        public BaseScoreCard Find(int journalId, int userProfileId)
        {
            return this.DbContext.BaseScoreCards
                .Include(s => s.Journal)
                .Include(s => s.Journal.Publisher)
                .Include(s => s.Journal.Languages)
                .Include(s => s.Journal.Subjects)
                .Include(s => s.QuestionScores)
                .Include(s => s.QuestionScores.Select(q => q.Question))
                .FirstOrDefault(s => s.JournalId == journalId && s.UserProfileId == userProfileId);
        }

        public IPagedList<BaseScoreCard> Find(ScoreCardFilter filter)
        {
            var query = this.DbContext.BaseScoreCards
                .Include(s => s.UserProfile)
                .Where(s => s.JournalId == filter.JournalId)
                .Where(s => s.State == ScoreCardState.Published);

            if (filter.RequireComment)
            {
                query = query.Where(s => s.Remarks != null);
            }

            return query.OrderByDescending(s => s.DatePublished).ToPagedList(filter.PageNumber, filter.PageSize);
        }

        public IPagedList<BaseScoreCard> FindForUser(ScoreCardFilter filter)
        {
            var query = this.DbContext.BaseScoreCards
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
            var groupedStates = this.DbContext.BaseScoreCards.Where(s => s.UserProfileId == userProfile.Id)
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
            var groupedStates = this.DbContext.BaseScoreCards.Where(s => s.UserProfile.InstitutionId == institution.Id)
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

        public IList<BaseScoreCard> FindScoreCardsToBeArchived()
        {
            return this.DbContext.BaseScoreCards.Where(s => s.State != ScoreCardState.Expired && s.DateExpiration < DateTime.Now).ToList();
        }

        public IList<BaseScoreCard> FindScoreCardsThatWillBeArchived(TimeSpan toBeArchivedWindow)
        {
            var soonToBeArchivedDate = DateTime.Now + toBeArchivedWindow;

            return this.DbContext.BaseScoreCards
                .Include(s => s.Journal)
                .Include(s => s.UserProfile)
                .Where(s => s.State == ScoreCardState.Published && DbFunctions.TruncateTime(s.DateExpiration) == DbFunctions.TruncateTime(soonToBeArchivedDate)).ToList();
        }

        public void MoveScoreCards(Journal oldJournal, Journal newJournal)
        {
            this.DbContext.BaseScoreCards
                .Where(b => b.JournalId == oldJournal.Id)
                .Update(b => new BaseScoreCard { JournalId = newJournal.Id });
        }

        public override void Delete(BaseScoreCard entity)
        {
            this.DbContext.BaseJournalPrices
                .Where(b => b.BaseScoreCardId == entity.Id)
                .Delete();

            base.Delete(entity);
        }
    }
}