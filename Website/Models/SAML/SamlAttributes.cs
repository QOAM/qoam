namespace QOAM.Website.Models.SAML
{
    using System.Collections.Generic;

    public static class SamlAttributes
    {
        public const string DisplayName = "urn:mace:dir:attribute-def:displayName";
        public const string Mail = "urn:mace:dir:attribute-def:mail";
        public const string SchacHomeOrganization = "urn:mace:terena.org:attribute-def:schacHomeOrganization";
        
        public static ISet<string> GetRequiredAttributes()
        {
            return new HashSet<string> { DisplayName, Mail, SchacHomeOrganization };
        }
    }
}