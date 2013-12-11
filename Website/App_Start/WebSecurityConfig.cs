namespace RU.Uci.OAMarket.Website.App_Start
{
    using WebMatrix.WebData;

    public static class WebSecurityConfig
    {
        public static void Configure()
        {
            WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfiles", "Id", "UserName", autoCreateTables: false);
        }
    }
}