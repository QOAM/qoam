namespace QOAM.Core.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using EntityFramework.Extensions;
    using PagedList;

    using QOAM.Core.Repositories.Filters;

    public class ValuationScoreCardRepository : Repository<ValuationScoreCard>, IValuationScoreCardRepository
    {
        public ValuationScoreCardRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        public ValuationScoreCard Find(int id)
        {
            return this.DbContext.ValuationScoreCards
                .Include(s => s.Journal)
                .Include(s => s.Journal.Publisher)
                .Include(s => s.Journal.Languages)
                .Include(s => s.Journal.Subjects)
                .Include(s => s.QuestionScores)
                .Include(s => s.QuestionScores.Select(q => q.Question))
                .First(s => s.Id == id);
        }

        public ValuationScoreCard Find(int journalId, int userProfileId)
        {
            return this.DbContext.ValuationScoreCards
                .Include(s => s.Journal)
                .Include(s => s.Journal.Publisher)
                .Include(s => s.Journal.Languages)
                .Include(s => s.Journal.Subjects)
                .Include(s => s.QuestionScores)
                .Include(s => s.QuestionScores.Select(q => q.Question))
                .FirstOrDefault(s => s.JournalId == journalId && s.UserProfileId == userProfileId);
        }

        public IPagedList<ValuationScoreCard> Find(ScoreCardFilter filter)
        {
            var query = this.DbContext.ValuationScoreCards
                .Include(s => s.UserProfile)
                .Where(s => s.JournalId == filter.JournalId)
                .Where(s => s.State == ScoreCardState.Published);

            if (filter.RequireComment)
            {
                query = query.Where(s => s.Remarks != null);
            }

            return query.OrderByDescending(s => s.DatePublished).ToPagedList(filter.PageNumber, filter.PageSize);
        }

        public IPagedList<ValuationScoreCard> FindForUser(ScoreCardFilter filter)
        {
            var query = this.DbContext.ValuationScoreCards
                .Include(s => s.Journal)
                .Where(s => s.UserProfileId == filter.UserProfileId);

            if (filter.State.HasValue)
            {
                query = query.Where(s => s.State == filter.State);
            }

            return query.OrderByDescending(s => s.DatePublished).ToPagedList(filter.PageNumber, filter.PageSize);
        }

        public IList<ValuationScoreCard> AllPublished()
        {
            return DbContext.ValuationScoreCards
                .Include(s => s.UserProfile)
                .Include(s => s.Journal)
                .Where(s => s.State == ScoreCardState.Published).ToList();
        }

        public ScoreCardStats CalculateStats(UserProfile userProfile)
        {
            var groupedStates = this.DbContext.ValuationScoreCards.Where(s => s.UserProfileId == userProfile.Id)
                .GroupBy(s => s.State)
                .Select(g => new { State = g.Key, Count = g.Count() })
                .ToList();
            
            return new ScoreCardStats
                   {
                       NumberOfArchivedScoreCards = groupedStates.Where(g => g.State == ScoreCardState.Archived).Select(g => g.Count).FirstOrDefault(),
                       NumberOfPublishedScoreCards = groupedStates.Where(g => g.State == ScoreCardState.Published).Select(g => g.Count).FirstOrDefault(),
                       NumberOfUnpublishedScoreCards = groupedStates.Where(g => g.State == ScoreCardState.Unpublished).Select(g => g.Count).FirstOrDefault(),
                   };
        }

        public ScoreCardStats CalculateStats(Institution institution)
        {
            var groupedStates = this.DbContext.ValuationScoreCards.Where(s => s.UserProfile.InstitutionId == institution.Id)
                .GroupBy(s => s.State)
                .Select(g => new { State = g.Key, Count = g.Count() })
                .ToList();

            return new ScoreCardStats
                   {
                       NumberOfArchivedScoreCards = groupedStates.Where(g => g.State == ScoreCardState.Archived).Select(g => g.Count).FirstOrDefault(),
                       NumberOfPublishedScoreCards = groupedStates.Where(g => g.State == ScoreCardState.Published).Select(g => g.Count).FirstOrDefault(),
                       NumberOfUnpublishedScoreCards = groupedStates.Where(g => g.State == ScoreCardState.Unpublished).Select(g => g.Count).FirstOrDefault(),
                   };
        }

        public IList<ValuationScoreCard> FindScoreCardsToBeArchived()
        {
            return this.DbContext.ValuationScoreCards.Where(s => s.State != ScoreCardState.Archived && s.DateExpiration < DateTime.Now).ToList();
        }

        public IList<ValuationScoreCard> FindScoreCardsThatWillBeArchived(TimeSpan toBeArchivedWindow)
        {
            var soonToBeArchivedDate = DateTime.Now + toBeArchivedWindow;

            return this.DbContext.ValuationScoreCards
                .Include(s => s.Journal)
                .Include(s => s.UserProfile)
                .Where(s => s.State == ScoreCardState.Published && DbFunctions.TruncateTime(s.DateExpiration) == DbFunctions.TruncateTime(soonToBeArchivedDate)).ToList();
        }

        public void MoveScoreCards(Journal oldJournal, Journal newJournal)
        {
            this.DbContext.ValuationScoreCards
                .Where(b => b.JournalId == oldJournal.Id)
                .Update(b => new ValuationScoreCard { JournalId = newJournal.Id });

            this.ArchiveDuplicateScoreCards();
        }

        public int Count(ScoreCardFilter filter)
        {
            var query = this.DbContext.ValuationScoreCards.AsQueryable();

            if (filter.State.HasValue)
            {
                query = query.Where(v => v.State == filter.State.Value);
            }

            return query.Count();
        }

        public DateTime? LastUpdate()
        {
            return this.DbContext.ValuationScoreCards
                .Where(v => v.State == ScoreCardState.Published)
                .OrderByDescending(v => v.DatePublished.Value).Select(v => v.DatePublished)
                .FirstOrDefault();
        }

        public void RemoveUnpublishedScoreCards(TimeSpan toBeRemovedWindow)
        {
            var toBeRemovedDate = DateTime.Now - toBeRemovedWindow;

            this.DbContext.ValuationJournalPrices
                .Where(b => b.ValuationScoreCard.State == ScoreCardState.Unpublished)
                .Where(b => b.ValuationScoreCard.DateStarted <= toBeRemovedDate)
                .Delete();

            this.DbContext.ValuationScoreCards
                .Where(b => b.State == ScoreCardState.Unpublished)
                .Where(b => b.DateStarted <= toBeRemovedDate)
                .Delete();
        }

        public void ArchiveDuplicateScoreCards()
        {
            this.DbContext.ValuationScoreCards
                .Where(b => b.State == ScoreCardState.Published)
                .Where(b => this.DbContext.ValuationScoreCards.Any(b2 => b2.State == ScoreCardState.Published && b2.JournalId == b.JournalId && b2.UserProfileId == b.UserProfileId && b2.DatePublished > b.DatePublished))
                .Update(b => new ValuationScoreCard { DateExpiration = DateTime.Now, State = ScoreCardState.Archived });
        }

        public override void Delete(ValuationScoreCard entity)
        {
            this.DbContext.ValuationJournalPrices
                .Where(b => b.ValuationScoreCardId == entity.Id)
                .Delete();

            base.Delete(entity);
        }
    }
}