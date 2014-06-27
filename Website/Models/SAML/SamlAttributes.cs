namespace QOAM.Website.Models.SAML
{
    using System.Collections.Generic;

    public static class SamlAttributes
    {
        public const string DisplayName = "displayName";
        public const string Mail = "mail";
        public const string SchacHomeOrganization = "schacHomeOrganization";
        public const string EduPersonTargetedID = "eduPersonTargetedID";
        
        public static ISet<string> GetRequiredAttributes()
        {
            return new HashSet<string> { DisplayName, Mail, SchacHomeOrganization, EduPersonTargetedID };
        }
    }
}