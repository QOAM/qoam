namespace QOAM.Website.Helpers
{
    public interface IAuthentication
    {
        int CurrentUserId { get; }
        
        bool Login(string providerName, string providerUserId, bool createPersistentCookie);
        void CreateOrUpdateAccount(string providerName, string providerUserId, string userName);
        void Logout();
    }
}