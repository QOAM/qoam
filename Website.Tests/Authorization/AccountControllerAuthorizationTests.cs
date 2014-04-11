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
        public void LoginCallbackActionDoesNotRequireAuthorizedUser()
        {
            // Assert
            Assert.True(ActionDoesNotRequireAuthorizedUser(x => x.LoginCallback(null)));
        }

        [Fact]
        public void LoginFailureActionDoesNotRequireAuthorizedUser()
        {
            // Assert
            Assert.True(ActionDoesNotRequireAuthorizedUser(x => x.LoginFailure(LoginFailureReason.ExternalAuthenticationFailed)));
        }
    }
}