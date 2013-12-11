namespace RU.Uci.OAMarket.Website.App_Start
{
    using System.Data.Entity;

    using RU.Uci.OAMarket.Data;

    public static class DatabaseConfig
    {
        public static void Configure()
        {
            Database.SetInitializer<ApplicationDbContext>(null);
        }
    }
}