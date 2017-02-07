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

        public bool EnableProxyCreation
        {
            get { return DbContext.Configuration.ProxyCreationEnabled; }
            set { DbContext.Configuration.ProxyCreationEnabled = value; }
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

        public IList<BaseScoreCard> AllPublished()
        {
            return DbContext.BaseScoreCards
                .Include(s => s.UserProfile)
                .Include(s => s.Journal)
                .Where(s => s.State == ScoreCardState.Published).ToList();
        }

        public IPagedList<BaseScoreCard> FindForUser(ScoreCardFilter filter)
        {
            var query = this.DbContext.BaseScoreCards
                .Include(s => s.Journal)
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
                           NumberOfArchivedScoreCards = groupedStates.Where(g => g.State == ScoreCardState.Archived).Select(g => g.Count).FirstOrDefault(),
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
                NumberOfArchivedScoreCards = groupedStates.Where(g => g.State == ScoreCardState.Archived).Select(g => g.Count).FirstOrDefault(),
                NumberOfPublishedScoreCards = groupedStates.Where(g => g.State == ScoreCardState.Published).Select(g => g.Count).FirstOrDefault(),
                NumberOfUnpublishedScoreCards = groupedStates.Where(g => g.State == ScoreCardState.Unpublished).Select(g => g.Count).FirstOrDefault(),
            };
        }

        public IList<BaseScoreCard> FindScoreCardsToBeArchived()
        {
            return this.DbContext.BaseScoreCards.Where(s => s.State != ScoreCardState.Archived && s.DateExpiration < DateTime.Now).ToList();
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

            this.ArchiveDuplicateScoreCards();
        }

        public int Count(ScoreCardFilter filter)
        {
            var query = this.DbContext.BaseScoreCards.AsQueryable();

            if (filter.State.HasValue)
            {
                query = query.Where(v => v.State == filter.State.Value);
            }

            return query.Count();
        }

        public DateTime? LastUpdate()
        {
            return this.DbContext.BaseScoreCards
                .Where(v => v.State == ScoreCardState.Published)
                .OrderByDescending(v => v.DatePublished.Value).Select(v => v.DatePublished)
                .FirstOrDefault();
        }

        public void RemoveUnpublishedScoreCards(TimeSpan toBeRemovedWindow)
        {
            var toBeRemovedDate = DateTime.Now - toBeRemovedWindow;

            this.DbContext.BaseJournalPrices
                .Where(b => b.BaseScoreCard.State == ScoreCardState.Unpublished)
                .Where(b => b.BaseScoreCard.DateStarted <= toBeRemovedDate)
                .Delete();

            this.DbContext.BaseScoreCards
                .Where(b => b.State == ScoreCardState.Unpublished)
                .Where(b => b.DateStarted <= toBeRemovedDate)
                .Delete();
        }

        public void ArchiveDuplicateScoreCards()
        {
            this.DbContext.BaseScoreCards
                .Where(b => b.State == ScoreCardState.Published)
                .Where(b => this.DbContext.BaseScoreCards.Any(b2 => b2.State == ScoreCardState.Published && b2.JournalId == b.JournalId && b2.UserProfileId == b.UserProfileId && b2.DatePublished > b.DatePublished))
                .Update(b => new BaseScoreCard { DateExpiration = DateTime.Now, State = ScoreCardState.Archived });
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