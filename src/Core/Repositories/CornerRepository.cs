using System;
using System.Collections.Generic;
using System.Linq;
using PagedList;

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

        public IPagedList<Journal> GetJournalsForCorner(int cornerId, int page, int pageSize)
        {
            return DbContext.CornerJournals
                .Where(cj => cj.CornerId == cornerId)
                .Select(cj => cj.Journal)
                .OrderBy(j => j.Title)
                .ToPagedList(page, pageSize);
        }
    }
}