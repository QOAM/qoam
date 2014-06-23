namespace QOAM.Website.Models.SAML
{
    using SAML2.Identity;

    public static class SamlIdentityExtensions
    {
        public static string GetProvider(this ISaml20Identity saml20Identity)
        {
            return "SurfConext";
        }

        public static string GetProviderUserId(this ISaml20Identity saml20Identity)
        {
            return saml20Identity.GetAttributeValue(SamlAttributes.EduPersonTargetedID);
        }

        public static string GetDisplayName(this ISaml20Identity saml20Identity)
        {
            return saml20Identity.GetAttributeValue(SamlAttributes.DisplayName);
        }

        public static string GetEmail(this ISaml20Identity saml20Identity)
        {
            return saml20Identity.GetAttributeValue(SamlAttributes.Mail);
        }

        public static string GetInstitutionShortName(this ISaml20Identity saml20Identity)
        {
            return saml20Identity.GetAttributeValue(SamlAttributes.SchacHomeOrganization);
        }

        private static string GetAttributeValue(this ISaml20Identity saml20Identity, string attribute)
        {
            return saml20Identity[attribute][0].AttributeValue[0];
        }
    }
}