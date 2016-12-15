using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using LinqKit;
using PagedList;
using QOAM.Core.Helpers;
using QOAM.Core.Repositories.Filters;

namespace QOAM.Core.Repositories
{
    public class CornerRepository : Repository<Corner>, ICornerRepository
    {
        public CornerRepository(ApplicationDbContext dbContext) : base(dbContext)
        {}

        public Corner Find(int id)
        {
            return DbContext.Corners.SingleOrDefault(c => c.Id == id);
        }

        public Corner Find(string name)
        {
            return DbContext.Corners.SingleOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public IList<Corner> FindForUser(int userProfileId)
        {
            return DbContext.Corners.Where(uj => uj.UserProfileId == userProfileId).ToList();
        }

        public IList<Corner> All()
        {
            return DbContext.Corners.ToList();
        }

        public IPagedList<Journal> GetJournalsForCorner(QoamCornerJournalFilter filter)
        {
            var cornerId = filter.CornerId.GetValueOrDefault();

            var query = DbContext.CornerJournals
                .Include(cj => cj.Journal)
                .Include(cj => cj.Journal.Publisher)
                .Include(cj => cj.Journal.Languages)
                .Include(cj => cj.Journal.Subjects)
                .AsExpandable()
                .Where(cj => cj.CornerId == cornerId);

            return query.Select(uj => uj.Journal).Search(filter);
        }
    }
}