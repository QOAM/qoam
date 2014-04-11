namespace QOAM.Website.Tests.Models.SAML
{
    using System.Collections.Generic;

    using QOAM.Website.Models.SAML;

    using SAML2.Identity;
    using SAML2.Schema.Core;

    using Xunit;

    public class SamlIdentityExtensionsTests
    {
        [Fact]
        public void GetProviderReturnCorrectProvider()
        {
            // Arrange
            ISaml20Identity saml20Identity = new Saml20Identity("test", new List<SamlAttribute>(), null);

            // Act
            var provider = saml20Identity.GetProvider();

            // Assert
            Assert.Equal("SurfConext", provider);
        }

        [Fact]
        public void GetProviderUserIdReturnCorrectProviderUserId()
        {
            // Arrange
            var saml20Identity = new Saml20Identity("test", new List<SamlAttribute>
                                                            {
                                                                new SamlAttribute { Name = SamlAttributes.SchacHomeOrganization, AttributeValue = new[] { "org1.com" } },
                                                                new SamlAttribute { Name = SamlAttributes.UID, AttributeValue = new[] { "user1" } },
                                                            }, null);

            // Act
            var providerUserId = saml20Identity.GetProviderUserId();

            // Assert
            Assert.Equal("urn:collab:person:org1.com:user1", providerUserId);
        }

        [Fact]
        public void GetUserNameReturnCorrectUserName()
        {
            // Arrange
            var saml20Identity = new Saml20Identity("test", new List<SamlAttribute> { new SamlAttribute { Name = SamlAttributes.UID, AttributeValue = new[] { "user1" } }, }, null);

            // Act
            var providerUserId = saml20Identity.GetUserName();

            // Assert
            Assert.Equal("user1", providerUserId);
        }

        [Fact]
        public void GetDisplayNameReturnCorrectDisplayName()
        {
            // Arrange
            var saml20Identity = new Saml20Identity("test", new List<SamlAttribute> { new SamlAttribute { Name = SamlAttributes.DisplayName, AttributeValue = new[] { "User, test" } }, }, null);

            // Act
            var displayName = saml20Identity.GetDisplayName();

            // Assert
            Assert.Equal("User, test", displayName);
        }

        [Fact]
        public void GetEmailReturnCorrectEmail()
        {
            // Arrange
            var saml20Identity = new Saml20Identity("test", new List<SamlAttribute> { new SamlAttribute { Name = SamlAttributes.Mail, AttributeValue = new[] { "test@mail.com" } }, }, null);

            // Act
            var email = saml20Identity.GetEmail();

            // Assert
            Assert.Equal("test@mail.com", email);
        }

        [Fact]
        public void GetInstitutionShortNameReturnCorrectInstitutionShortName()
        {
            // Arrange
            var saml20Identity = new Saml20Identity("test", new List<SamlAttribute> { new SamlAttribute { Name = SamlAttributes.SchacHomeOrganization, AttributeValue = new[] { "org1.com" } }, }, null);

            // Act
            var institutionShortName = saml20Identity.GetInstitutionShortName();

            // Assert
            Assert.Equal("org1.com", institutionShortName);
        }
    }
}