namespace QOAM.Website.Tests.Authorization
{
    using Website.Controllers;
    using Xunit;

    public class InstitutionsControllerAuthorizationTests : ControllerAuthorizationTests<InstitutionsController>
    {
        [Fact]
        public void IndexActionDoesRequireAuthorizedUser()
        {
            // Assert
            Assert.True(ActionRequiresAuthorizedUser(x => x.Index(null)));
        }

        [Fact]
        public void DetailsActionDoesRequireAuthorizedUser()
        {
            // Assert
            Assert.True(ActionRequiresAuthorizedUser(x => x.Details(null)));
        }

        [Fact]
        public void NamesActionDoesRequireAuthorizedUser()
        {
            // Assert
            Assert.True(ActionRequiresAuthorizedUser(x => x.Names(null)));
        }
    }
}