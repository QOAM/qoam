namespace QOAM.Website.Models
{
    using SAML2.Identity;
    using SAML2.Schema.Core;

    public class PersistentPseudonymMapper : IPersistentPseudonymMapper
    {
        public string MapIdentity(NameId samlSubject)
        {
            return samlSubject.Value;
        }
    }
}