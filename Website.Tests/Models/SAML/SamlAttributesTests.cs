namespace QOAM.Website.Tests.Models.SAML
{
    using QOAM.Website.Models.SAML;

    using Xunit;
    using Xunit.Extensions;

    public class SamlAttributesTests
    {
        [Theory]
        [InlineData(SamlAttributes.UID)]
        [InlineData(SamlAttributes.DisplayName)]
        [InlineData(SamlAttributes.Mail)]
        [InlineData(SamlAttributes.SchacHomeOrganization)]
        public void GetRequiredAttributesReturnsSetWithRequiredAttribute(string requiredAttribute)
        {
            // Arrange
            var requiredAttributes = SamlAttributes.GetRequiredAttributes();

            // Act
            var attributeInRequiredAttributesSet = requiredAttributes.Contains(requiredAttribute);

            // Assert
            Assert.True(attributeInRequiredAttributesSet);
        }

        [Fact]
        public void GetRequiredAttributesReturnsSetWithOnlyRequiredAttributes()
        {
            // Arrange
            
            // Act
            var requiredAttributes = SamlAttributes.GetRequiredAttributes();

            // Assert
            Assert.Equal(4, requiredAttributes.Count);
        }
    }
}