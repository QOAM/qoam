namespace RU.Uci.OAMarket.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using RU.Uci.OAMarket.Domain;
    using RU.Uci.OAMarket.Domain.Repositories;

    public class InstitutionRepository : Repository, IInstitutionRepository
    {
        public InstitutionRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        public IList<Institution> All
        {
            get
            {
                return this.DbContext.Institutions.OrderBy(i => i.Name).ToList();
            }
        }

        public Institution Find(string shortName)
        {
            return this.DbContext.Institutions.FirstOrDefault(i => i.ShortName == shortName);
        }

        public Institution Find(int id)
        {
            return this.DbContext.Institutions.Find(id);
        }

        public void Insert(Institution publisher)
        {
            this.DbContext.Institutions.Add(publisher);
        }
    }
}