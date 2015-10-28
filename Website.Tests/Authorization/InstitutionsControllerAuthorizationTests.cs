namespace QOAM.Website.Tests.Authorization
{
    using Website.Controllers;
    using Xunit;

    public class InstitutionsControllerAuthorizationTests : ControllerAuthorizationTests<InstitutionsController>
    {
        [Fact]
        public void IndexActionDoesNotRequireAuthorizedUser()
        {
            // Assert
            Assert.True(ActionDoesNotRequireAuthorizedUser(x => x.Index(null)));
        }

        [Fact]
        public void DetailsActionDoesNotRequireAuthorizedUser()
        {
            // Assert
            Assert.True(ActionDoesNotRequireAuthorizedUser(x => x.Details(null)));
        }

        [Fact]
        public void NamesActionDoesNotRequireAuthorizedUser()
        {
            // Assert
            Assert.True(ActionDoesNotRequireAuthorizedUser(x => x.Names(null)));
        }
    }
}