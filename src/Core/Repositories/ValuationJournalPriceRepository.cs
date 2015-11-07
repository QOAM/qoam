namespace QOAM.Core.Repositories
{
    using System.Data.Entity;
    using System.Linq;

    using PagedList;

    using QOAM.Core.Repositories.Filters;

    public class ValuationJournalPriceRepository : Repository<ValuationJournalPrice>, IValuationJournalPriceRepository
    {
        public ValuationJournalPriceRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        public ValuationJournalPrice Find(int id)
        {
            return this.DbContext.ValuationJournalPrices.Find(id);
        }

        public ValuationJournalPrice Find(int journalId, int userProfileId)
        {
            return this.DbContext.ValuationJournalPrices.FirstOrDefault(j => j.JournalId == journalId && j.UserProfileId == userProfileId);
        }

        public IPagedList<ValuationJournalPrice> Find(JournalPriceFilter filter)
        {
            var query = this.DbContext.ValuationJournalPrices
                .Include(j => j.UserProfile)
                .Where(v => v.ValuationScoreCard.State == ScoreCardState.Published);

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