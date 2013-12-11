namespace RU.Uci.OAMarket.Data
{
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;

    using RU.Uci.OAMarket.Data.Helpers;
    using RU.Uci.OAMarket.Domain;
    using RU.Uci.OAMarket.Domain.Repositories;

    public class SubjectRepository : Repository, ISubjectRepository
    {
        public SubjectRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        public IList<Subject> All
        {
            get
            {
                return this.DbContext.Subjects.OrderBy(k => k.Name).ToList();
            }
        }

        public void InsertBulk(IEnumerable<Subject> subjects)
        {
            using (var bulkCopy = new SqlBulkCopy(ConnectionString))
            {
                bulkCopy.ColumnMappings.Add("Id", "Id");
                bulkCopy.ColumnMappings.Add("Name", "Name");

                bulkCopy.DestinationTableName = this.DbContext.GetTableName<Subject>();
                bulkCopy.WriteToServer(subjects.AsDataReader());
            }
        }
    }
}