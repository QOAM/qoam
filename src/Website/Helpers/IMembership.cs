namespace QOAM.Website.Helpers
{
    using System.Web.Security;

    public interface IMembership
    {
        bool ValidateUser(string username, string password);

        bool DeleteUser(string username, bool deleteAllRelatedData);

        MembershipCreateStatus CreateUser(string username, string password, string email);

        MembershipUser GetUser(string username, bool userIsOnline);
    }
}