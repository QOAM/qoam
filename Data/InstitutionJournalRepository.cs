namespace RU.Uci.OAMarket.Data
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Linq;

    using PagedList;

    using RU.Uci.OAMarket.Domain;
    using RU.Uci.OAMarket.Domain.Repositories;
    using RU.Uci.OAMarket.Domain.Repositories.Filters;

    public class InstitutionJournalRepository : Repository, IInstitutionJournalRepository
    {
        public InstitutionJournalRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        public InstitutionJournal Find(int journalId, int userProfileId)
        {
            return (from i in this.DbContext.InstitutionJournals
                    from u in this.DbContext.UserProfiles
                    where i.JournalId == journalId
                    where i.InstitutionId == u.InstitutionId
                    where u.Id == userProfileId
                    select i).FirstOrDefault();
        }

        public IPagedList<InstitutionJournal> Find(InstitutionJournalFilter filter)
        {
            var query = this.DbContext.InstitutionJournals.Include(i => i.Institution);

            if (filter.JournalId.HasValue)
            {
                query = query.Where(i => i.JournalId == filter.JournalId.Value);
            }

            if (filter.UserProfileId.HasValue)
            {
                query = query.Where(i => i.UserProfileId == filter.UserProfileId.Value);
            }

            return query.OrderByDescending(i => i.DateAdded)
                        .ToPagedList(filter.PageNumber.Value, filter.PageSize.Value);
        }

        public IList<InstitutionJournal> FindAll(InstitutionJournalFilter filter)
        {
            var query = this.DbContext.InstitutionJournals.Include(i => i.Institution);

            if (filter.JournalId.HasValue)
            {
                query = query.Where(i => i.JournalId == filter.JournalId.Value);
            }

            if (filter.UserProfileId.HasValue)
            {
                query = query.Where(i => i.UserProfileId == filter.UserProfileId.Value);
            }

            if (filter.InstitutionId.HasValue)
            {
                query = query.Where(i => i.InstitutionId == filter.InstitutionId.Value);
            }

            if (filter.PublisherId.HasValue)
            {
                query = query.Where(i => i.Journal.PublisherId == filter.PublisherId.Value);
            }

            return query.OrderByDescending(i => i.DateAdded).ToList();
        }

        public void Insert(InstitutionJournal journalPrice)
        {
            this.DbContext.InstitutionJournals.Add(journalPrice);
        }

        public void Update(InstitutionJournal journalPrice)
        {
            this.DbContext.Entry(journalPrice).State = EntityState.Modified;
        }
    }
}