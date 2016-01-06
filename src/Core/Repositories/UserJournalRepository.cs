using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using LinqKit;
using PagedList;
using QOAM.Core.Helpers;
using QOAM.Core.Repositories.Filters;

namespace QOAM.Core.Repositories
{
    public class UserJournalRepository : Repository<UserJournal>, IUserJournalRepository
    {
        public UserJournalRepository(ApplicationDbContext dbContext) : base(dbContext)
        {}
        
        public UserJournal Find(int journalId, int userProfileId)
        {
            return (from i in DbContext.UserJournals
                    where i.JournalId == journalId
                    where i.UserProfileId == userProfileId
                    select i).FirstOrDefault();
        }

        public IPagedList<UserJournal> Find(UserJournalFilter filter)
        {
            var query = DbContext.UserJournals.Include(x => x.UserProfile);

            if (filter.JournalId.HasValue)
                query = query.Where(x => x.JournalId == filter.JournalId.Value);

            if (filter.UserProfileId.HasValue)
                query = query.Where(x => x.UserProfileId == filter.UserProfileId.Value);

            return query.OrderByDescending(x => x.DateAdded).ToPagedList(filter.PageNumber, filter.PageSize);
        }

        public IList<UserJournal> FindAll(UserJournalFilter filter)
        {
            var query = DbContext.UserJournals.Include(x => x.UserProfile);

            if (filter.JournalId.HasValue)
                query = query.Where(x => x.JournalId == filter.JournalId.Value);

            if (filter.UserProfileId.HasValue)
                query = query.Where(x => x.UserProfileId == filter.UserProfileId.Value);

            return query.OrderByDescending(x => x.DateAdded).ToList();
        }

        public IList<UserJournal> All(int userProfileId)
        {
            return DbContext.UserJournals.Where(uj => uj.UserProfileId == userProfileId).ToList();
        }

        public IPagedList<Journal> Search(UserJournalFilter filter)
        {
            var query = DbContext.UserJournals
                .Include(uj => uj.Journal)
                .Include(uj => uj.UserProfile)
                .AsExpandable();

            if (filter.UserProfileId.HasValue)
                query = query.Where(x => x.UserProfileId == filter.UserProfileId.Value);

            return query.Select(uj => uj.Journal).Search(filter);
        }
    }
}