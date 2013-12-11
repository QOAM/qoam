namespace RU.Uci.OAMarket.Website.Helpers
{
    using System.Collections.Generic;

    using DotNetOpenAuth.AspNet;

    using Microsoft.Web.WebPages.OAuth;

    using WebMatrix.WebData;

    public class Authentication : IAuthentication
    {
        public int CurrentUserId
        {
            get
            {
                return WebSecurity.CurrentUserId;
            }
        }

        public ICollection<AuthenticationClientData> RegisteredClientData
        {
            get
            {
                return OAuthWebSecurity.RegisteredClientData;
            }
        }

        public AuthenticationResult VerifyAuthentication(string returnUrl)
        {
            return OAuthWebSecurity.VerifyAuthentication(returnUrl);
        }

        public bool Login(string providerName, string providerUserId, bool createPersistentCookie)
        {
            return OAuthWebSecurity.Login(providerName, providerUserId, createPersistentCookie);
        }

        public void CreateOrUpdateAccount(string providerName, string providerUserId, string userName)
        {
            OAuthWebSecurity.CreateOrUpdateAccount(providerName, providerUserId, userName);
        }

        public void RequestAuthentication(string provider, string returnUrl)
        {
            OAuthWebSecurity.RequestAuthentication(provider, returnUrl);
        }

        public void Logout()
        {
            WebSecurity.Logout();
        }
    }
}