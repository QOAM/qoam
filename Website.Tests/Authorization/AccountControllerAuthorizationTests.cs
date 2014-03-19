namespace QOAM.Website.Tests.Authorization
{
    using QOAM.Website.Controllers;
    using QOAM.Website.Models;

    using Xunit;

    public class AccountControllerAuthorizationTests : ControllerAuthorizationTests<AccountController>
    {
        [Fact]
        public void LoginActionDoesNotRequireAuthorizedUser()
        {
            // Assert
            Assert.True(ActionDoesNotRequireAuthorizedUser(x => x.Login(null)));
        }

        [Fact]
        public void LogOffActioRequiresAuthorizedUser()
        {
            // Assert
            Assert.True(ActionRequiresAuthorizedUser(x => x.LogOff()));
        }

        [Fact]
        public void ExternalLoginActionDoesNotRequireAuthorizedUser()
        {
            // Assert
            Assert.True(ActionDoesNotRequireAuthorizedUser(x => x.ExternalLogin(null, null)));
        }

        [Fact]
        public void ExternalLoginCallbackActionDoesNotRequireAuthorizedUser()
        {
            // Assert
            Assert.True(ActionDoesNotRequireAuthorizedUser(x => x.ExternalLoginCallback(null)));
        }

        [Fact]
        public void ExternalLoginFailureActionDoesNotRequireAuthorizedUser()
        {
            // Assert
            Assert.True(ActionDoesNotRequireAuthorizedUser(x => x.ExternalLoginFailure(LoginFailureReason.ExternalAuthenticationFailed)));
        }
    }
}