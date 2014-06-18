namespace QOAM.Website.Helpers
{
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

        public bool Login(string providerName, string providerUserId, bool createPersistentCookie)
        {
            return OAuthWebSecurity.Login(providerName, providerUserId, createPersistentCookie);
        }

        public void CreateOrUpdateAccount(string providerName, string providerUserId, string userName)
        {
            OAuthWebSecurity.CreateOrUpdateAccount(providerName, providerUserId, userName);
        }

        public void Logout()
        {
            WebSecurity.Logout();
        }
    }
}