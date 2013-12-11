namespace RU.Uci.OAMarket.Data
{
    using System.Data;
    using System.Data.Entity;
    using System.Linq;

    using PagedList;

    using RU.Uci.OAMarket.Domain;
    using RU.Uci.OAMarket.Domain.Repositories;
    using RU.Uci.OAMarket.Domain.Repositories.Filters;

    public class JournalPriceRepository : Repository, IJournalPriceRepository
    {
        public JournalPriceRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        public JournalPrice Find(int id)
        {
            return this.DbContext.JournalsPrices.Find(id);
        }

        public JournalPrice Find(int journalId, int userProfileId)
        {
            return this.DbContext.JournalsPrices.FirstOrDefault(j => j.JournalId == journalId && j.UserProfileId == userProfileId);
        }

        public IPagedList<JournalPrice> Find(JournalPriceFilter filter)
        {
            var query = this.DbContext.JournalsPrices.Include(j => j.UserProfile);

            if (filter.JournalId.HasValue)
            {
                query = query.Where(j => j.JournalId == filter.JournalId);
            }

            if (filter.UserProfileId.HasValue)
            {
                query = query.Where(j => j.UserProfileId == filter.UserProfileId);
            }

            return query.OrderByDescending(j => j.DateAdded).ToPagedList(filter.PageNumber, filter.PageSize);
        }

        public void Insert(JournalPrice journalPrice)
        {
            this.DbContext.JournalsPrices.Add(journalPrice);
        }

        public void Update(JournalPrice journalPrice)
        {
            this.DbContext.Entry(journalPrice).State = EntityState.Modified;
        }

        public void Delete(JournalPrice journalPrice)
        {
            this.DbContext.JournalsPrices.Remove(journalPrice);
        }
    }
}