namespace QOAM.Website
{
    using Microsoft.Web.WebPages.OAuth;

    using QOAM.Website.Models;

    public static class AuthConfig
    {
        private const string SurfContextDisplayName = "SurfConext";

        public static void RegisterAuth()
        {
            OAuthWebSecurity.RegisterClient(new SurfConextClient(OAMarketSettings.Current.SurfContext), SurfContextDisplayName, null);
        }
    }
}