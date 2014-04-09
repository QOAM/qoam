namespace QOAM.Core.Repositories
{
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;

    using QOAM.Core.Helpers;

    public class SubjectRepository : Repository<Subject>, ISubjectRepository
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