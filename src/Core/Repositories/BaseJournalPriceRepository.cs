namespace QOAM.Core.Repositories
{
    using System.Data.Entity;
    using System.Linq;

    using PagedList;

    using QOAM.Core.Repositories.Filters;

    public class BaseJournalPriceRepository : Repository<BaseJournalPrice>, IBaseJournalPriceRepository
    {
        public BaseJournalPriceRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        public BaseJournalPrice Find(int id)
        {
            return this.DbContext.BaseJournalPrices.Find(id);
        }

        public BaseJournalPrice Find(int journalId, int userProfileId)
        {
            return this.DbContext.BaseJournalPrices.FirstOrDefault(j => j.JournalId == journalId && j.UserProfileId == userProfileId);
        }

        public IPagedList<BaseJournalPrice> Find(JournalPriceFilter filter)
        {
            var query = this.DbContext.BaseJournalPrices
                .Include(j => j.UserProfile)
                .Where(v => v.BaseScoreCard.State == ScoreCardState.Published);

            if (filter.JournalId.HasValue)
            {
                query = query.Where(j => j.JournalId == filter.JournalId);
            }

            if (filter.UserProfileId.HasValue)
            {
                query = query.Where(j => j.UserProfileId == filter.UserProfileId);
            }

            if (filter.FeeType.HasValue)
            {
                query = query.Where(j => j.Price.FeeType == filter.FeeType);
            }

            return query.OrderByDescending(j => j.DateAdded).ToPagedList(filter.PageNumber, filter.PageSize);
        }
    }
}