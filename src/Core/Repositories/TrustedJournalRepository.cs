using System.Collections.Generic;

namespace QOAM.Core.Repositories
{
    using System.Linq;

    public class TrustedJournalRepository : Repository<TrustedJournal>, ITrustedJournalRepository
    {
        public TrustedJournalRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        public bool EnableProxyCreation
        {
            get => DbContext.Configuration.ProxyCreationEnabled;
            set => DbContext.Configuration.ProxyCreationEnabled = value;
        }

        public IList<Journal> ListOfTrustedJournals()
        {
                return DbContext.TrustedJournals.Select(tj => tj.Journal).OrderBy(j => j.Title).ToList();
    }
        public TrustedJournal Find(int trustedJournalId)
        {
            return DbContext.TrustedJournals.Find(trustedJournalId);
        }

        public TrustedJournal Find(int journalId, int institutionId)
        {
            return (from i in this.DbContext.TrustedJournals
                    where i.JournalId == journalId
                    where i.InstitutionId == institutionId
                    select i).FirstOrDefault();
        }

        public int CountTrustedByForJournal(int journalId)
        {
            return DbContext.TrustedJournals.Count(tj => tj.JournalId == journalId);
        }

        //public IPagedList<TrustedJournal> Find(TrustedJournalFilter filter)
        //{
        //    var query = this.DbContext.TrustedJournals.Include(i => i.Institution);

        //    if (filter.JournalId.HasValue)
        //    {
        //        query = query.Where(i => i.JournalId == filter.JournalId.Value);
        //    }

        //    if (filter.UserProfileId.HasValue)
        //    {
        //        query = query.Where(i => i.UserProfileId == filter.UserProfileId.Value);
        //    }

        //    return query.OrderByDescending(i => i.DateAdded).ToPagedList(filter.PageNumber.Value, filter.PageSize.Value);
        //}

        //public IList<TrustedJournal> FindAll(TrustedJournalFilter filter)
        //{
        //    var query = this.DbContext.TrustedJournals.Include(i => i.Institution);

        //    if (filter.JournalId.HasValue)
        //    {
        //        query = query.Where(i => i.JournalId == filter.JournalId.Value);
        //    }

        //    if (filter.UserProfileId.HasValue)
        //    {
        //        query = query.Where(i => i.UserProfileId == filter.UserProfileId.Value);
        //    }

        //    if (filter.InstitutionId.HasValue)
        //    {
        //        query = filter.AssociatedInstitutionIds != null && filter.AssociatedInstitutionIds.Any() ? 
        //            query.Where(ij => ij.InstitutionId == filter.InstitutionId.Value || filter.AssociatedInstitutionIds.Contains(ij.InstitutionId)) : 
        //            query.Where(i => i.InstitutionId == filter.InstitutionId.Value);
        //    }

        //    if (filter.PublisherId.HasValue)
        //    {
        //        query = query.Where(i => i.Journal.PublisherId == filter.PublisherId.Value);
        //    }

        //    return query.OrderByDescending(i => i.DateAdded).ToList();
        //}
    }
}