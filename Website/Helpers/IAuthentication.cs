namespace QOAM.Website.Helpers
{
    public interface IAuthentication
    {
        int CurrentUserId { get; }

        string CurrentUserName { get; }

        bool Login(string userName, string password, bool createPersistentCookie);

        string CreateUserAndAccount(string userName, string password, object propertyValues);

        void CreateAccount(string userName, string password);

        bool ConfirmAccount(string token);

        bool ChangePassword(string userName, string oldPassword, string newPassword);

        string GeneratePasswordResetToken(string userName);

        bool ResetPassword(string token, string newPassword);

        bool UserExists(string userName);
        
        bool UserIsConfirmed(string userName);

        void Logout();
    }
}