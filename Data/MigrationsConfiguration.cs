namespace RU.Uci.OAMarket.Data
{
    using System.Data.Entity.Migrations;

    public class MigrationsConfiguration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public MigrationsConfiguration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }
    }
}