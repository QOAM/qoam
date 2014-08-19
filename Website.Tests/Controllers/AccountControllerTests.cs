namespace QOAM.Website.Tests.Controllers
{
    using Moq;

    using QOAM.Core.Repositories;
    using QOAM.Website.Controllers;
    using QOAM.Website.Helpers;
    using QOAM.Website.Models;
    using QOAM.Website.Tests.Controllers.Helpers;

    using Xunit;

    public class AccountControllerTests
    {
        [Fact]
        public void LoginFailureReturnsPassedLoginFailureReasonAsModel()
        {
            // Arrange
            var accountController = CreateAccountController();
            var loginFailureReason = LoginFailureReason.UsernameAlreadyExists;

            // Actr

            var externalLoginResult = accountController.LoginFailure(loginFailureReason);

            // Assert
            Assert.Equal(loginFailureReason, externalLoginResult.Model);
        }

        private static AccountController CreateAccountController()
        {
            return new AccountController(Mock.Of<IUserProfileRepository>(), new Mock<IAuthentication>().Object)
                   {
                       Url = HttpContextHelper.CreateUrlHelper()
                   };
        }
    }
}