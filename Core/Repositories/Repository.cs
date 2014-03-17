namespace QOAM.Core.Repositories
{
    using System;
    using System.Configuration;

    using Validation;

    public abstract class Repository : IDisposable
    {
        protected Repository(ApplicationDbContext dbContext)
        {
            Requires.NotNull(dbContext, "dbContext");

            this.DbContext = dbContext;
        }

        protected ApplicationDbContext DbContext { get; private set; }

        protected static string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            }
        }
        
        public void Save()
        {
            this.DbContext.SaveChanges();
        }

        public void Dispose()
        {
            this.DbContext.Dispose();
        }
    }
}