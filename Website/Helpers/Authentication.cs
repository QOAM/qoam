namespace QOAM.Website.Helpers
{
    using System.Web.Security;
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

        public string CurrentUserName
        {
            get
            {
                return WebSecurity.CurrentUserName;
            }
        }

        public bool Login(string userName, string password, bool createPersistentCookie)
        {
            return WebSecurity.Login(userName, password, createPersistentCookie);
        }

        public string CreateUserAndAccount(string userName, string password, object propertyValues)
        {
            return WebSecurity.CreateUserAndAccount(userName, password, propertyValues, true);
        }

        public void CreateAccount(string userName, string password)
        {
            WebSecurity.CreateAccount(userName, password);
        }

        public bool ConfirmAccount(string token)
        {
            return WebSecurity.ConfirmAccount(token);
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            return WebSecurity.ChangePassword(userName, oldPassword, newPassword);
        }

        public string GeneratePasswordResetToken(string userName)
        {
            return WebSecurity.GeneratePasswordResetToken(userName);
        }

        public bool ResetPassword(string token, string newPassword)
        {
            return WebSecurity.ResetPassword(token, newPassword);
        }

        public bool UserExists(string userName)
        {
            return WebSecurity.UserExists(userName);
        }

        public bool UserIsConfirmed(string userName)
        {
            return WebSecurity.IsConfirmed(userName);
        }

        public void Logout()
        {
            WebSecurity.Logout();
        }

        public bool ValidateUser(string userName, string password)
        {
            return Membership.ValidateUser(userName, password);
        }
    }
}