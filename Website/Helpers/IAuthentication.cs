namespace QOAM.Website.Helpers
{
    using System.Collections.Generic;

    using DotNetOpenAuth.AspNet;

    using Microsoft.Web.WebPages.OAuth;

    public interface IAuthentication
    {
        int CurrentUserId { get; }
        ICollection<AuthenticationClientData> RegisteredClientData { get; }
        AuthenticationResult VerifyAuthentication(string returnUrl);
        bool Login(string providerName, string providerUserId, bool createPersistentCookie);
        void CreateOrUpdateAccount(string providerName, string providerUserId, string userName);
        void RequestAuthentication(string provider, string returnUrl);
        void Logout();
    }
}