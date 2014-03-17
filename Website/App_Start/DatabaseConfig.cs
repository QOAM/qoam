namespace QOAM.Website
{
    using System.Data.Entity;

    using QOAM.Core.Repositories;

    public static class DatabaseConfig
    {
        public static void Configure()
        {
            Database.SetInitializer<ApplicationDbContext>(null);
        }
    }
}