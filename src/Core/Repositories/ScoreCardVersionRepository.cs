namespace QOAM.Core.Repositories
{
    using System.Linq;

    public class ScoreCardVersionRepository : Repository<ScoreCardVersion>, IScoreCardVersionRepository
    {
        public ScoreCardVersionRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        public ScoreCardVersion FindCurrent()
        {
            return this.DbContext.ScoreCardVersions
                .OrderByDescending(s => s.Number)
                .First();
        }
    }
}