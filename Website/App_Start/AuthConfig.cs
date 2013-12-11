namespace RU.Uci.OAMarket.Website.App_Start
{
    using Microsoft.Web.WebPages.OAuth;

    using RU.Uci.OAMarket.Website.Models;

    public static class AuthConfig
    {
        private const string SurfContextDisplayName = "SurfConext";

        public static void RegisterAuth()
        {
            OAuthWebSecurity.RegisterClient(new SurfConextClient(OAMarketSettings.Current.SurfContext), SurfContextDisplayName, null);
        }
    }
}