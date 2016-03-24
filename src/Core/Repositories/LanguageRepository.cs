namespace QOAM.Core.Repositories
{
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;

    using QOAM.Core.Helpers;

    public class LanguageRepository : Repository<Language>, ILanguageRepository
    {
        public LanguageRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        public IList<Language> All => this.DbContext.Languages.OrderBy(l => l.Name).ToList();

        public IList<Language> Active => this.DbContext.Languages.Where(l => l.Journals.Any()).OrderBy(l => l.Name).ToList();

        public void InsertBulk(IEnumerable<Language> languages)
        {
            using (var bulkCopy = new SqlBulkCopy(ConnectionString))
            {
                bulkCopy.ColumnMappings.Add("Id", "Id");
                bulkCopy.ColumnMappings.Add("Name", "Name");

                bulkCopy.DestinationTableName = this.DbContext.GetTableName<Language>();
                bulkCopy.WriteToServer(languages.AsDataReader());
            }
        }
    }
}