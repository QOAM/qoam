namespace RU.Uci.OAMarket.Data
{
    using System.Linq;

    using RU.Uci.OAMarket.Domain;
    using RU.Uci.OAMarket.Domain.Repositories;

    public class ScoreCardVersionRepository : Repository, IScoreCardVersionRepository
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