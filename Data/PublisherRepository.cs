namespace RU.Uci.OAMarket.Data
{
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;

    using RU.Uci.OAMarket.Data.Helpers;
    using RU.Uci.OAMarket.Domain;
    using RU.Uci.OAMarket.Domain.Repositories;

    public class PublisherRepository : Repository, IPublisherRepository
    {
        public PublisherRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        public IList<Publisher> All
        {
            get
            {
                return this.DbContext.Publishers.ToList();
            }
        }

        public void InsertBulk(IEnumerable<Publisher> publishers)
        {
            using (var bulkCopy = new SqlBulkCopy(ConnectionString))
            {
                bulkCopy.ColumnMappings.Add("Id", "Id");
                bulkCopy.ColumnMappings.Add("Name", "Name");

                bulkCopy.DestinationTableName = this.DbContext.GetTableName<Publisher>();
                bulkCopy.WriteToServer(publishers.AsDataReader());
            }
        }
    }
}