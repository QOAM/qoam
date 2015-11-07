namespace QOAM.Core.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

    public class BlockedISSNRepository: Repository<BlockedISSN>, IBlockedISSNRepository
    {
        public BlockedISSNRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        public IList<BlockedISSN> All
        {
            get
            {
                return this.DbContext.BlockedISSNs.OrderBy(j => j.ISSN).ToList();
            }
        }

        public bool Exists(string issn)
        {
            return this.DbContext.BlockedISSNs.Any(b => b.ISSN == issn);
        }


        public BlockedISSN Find(int id)
        {
            return this.DbContext.BlockedISSNs.FirstOrDefault(b => b.Id == id);
        }
    }
}
