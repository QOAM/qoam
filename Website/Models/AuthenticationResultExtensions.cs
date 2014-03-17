namespace QOAM.Website.Models
{
    using DotNetOpenAuth.AspNet;

    using Validation;

    public static class AuthenticationResultExtensions
    {
        private const string UsernameField = "account_username";
        private const string DisplayNameField = "displayName";
        private const string EmailField = "emails_value";
        private const string InstitutionShortNameField = "organisations_name";

        public static string GetUsername(this AuthenticationResult result)
        {
            Requires.NotNull(result, "result");

            return result.ExtraData[UsernameField];
        }

        public static string GetDisplayName(this AuthenticationResult result)
        {
            Requires.NotNull(result, "result");

            return result.ExtraData[DisplayNameField];
        }

        public static string GetEmail(this AuthenticationResult result)
        {
            Requires.NotNull(result, "result");

            return result.ExtraData.ContainsKey(EmailField) ? result.ExtraData[EmailField] : null;
        }

        public static string GetInstitutionShortName(this AuthenticationResult result)
        {
            Requires.NotNull(result, "result");

            return result.ExtraData[InstitutionShortNameField];
        }
    }
}