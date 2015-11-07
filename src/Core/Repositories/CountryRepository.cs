namespace QOAM.Core.Repositories
{
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;

    using QOAM.Core.Helpers;

    public class CountryRepository : Repository<Country>, ICountryRepository
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