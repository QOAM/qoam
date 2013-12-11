namespace RU.Uci.OAMarket.Data
{
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;

    using RU.Uci.OAMarket.Data.Helpers;
    using RU.Uci.OAMarket.Domain;
    using RU.Uci.OAMarket.Domain.Repositories;

    public class CountryRepository : Repository, ICountryRepository
    {
        public CountryRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        public IList<Country> All
        {
            get
            {
                return this.DbContext.Countries.ToList();
            }
        }

        public void InsertBulk(IEnumerable<Country> subjects)
        {
            using (var bulkCopy = new SqlBulkCopy(ConnectionString))
            {
                bulkCopy.ColumnMappings.Add("Id", "Id");
                bulkCopy.ColumnMappings.Add("Name", "Name");

                bulkCopy.DestinationTableName = this.DbContext.GetTableName<Country>();
                bulkCopy.WriteToServer(subjects.AsDataReader());
            }
        }
    }
}