namespace QOAM.Website.Tests.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DotNetOpenAuth.AspNet;

    using Moq;

    using QOAM.Core;
    using QOAM.Core.Repositories;
    using QOAM.Website.Controllers;
    using QOAM.Website.Helpers;
    using QOAM.Website.Models;
    using QOAM.Website.Tests.Controllers.Helpers;

    using Xunit;

    public class AccountControllerTests
    {
        private const string ReturnUrl = "/home/about/";

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

        [Fact]
        public void LoginCallbackWithAuthenticationInvalidRedirectsToLoginFailureAction()
        {
            // Arrange
            var authenticationMock = new Mock<IAuthentication>();
            authenticationMock.Setup(a => a.VerifyAuthentication(It.IsAny<string>())).Returns(new AuthenticationResult(false));

            var accountController = CreateAccountController(authenticationMock.Object);

            // Act
            var redirectToRouteResult = (RedirectToRouteResult)accountController.LoginCallback(ReturnUrl);

            // Assert
            Assert.Null(redirectToRouteResult.RouteValues["controller"]);
            Assert.Equal("LoginFailure", redirectToRouteResult.RouteValues["action"]);
        }

        [Fact]
        public void LoginCallbackWithAuthenticationInvalidRedirectsToLoginFailureActionWithExternalAuthenticationFailedLoginFailureReason()
        {
            // Arrange
            var authenticationMock = new Mock<IAuthentication>();
            authenticationMock.Setup(a => a.VerifyAuthentication(It.IsAny<string>())).Returns(new AuthenticationResult(false));

            var accountController = CreateAccountController(authenticationMock.Object);

            // Act
            var redirectToRouteResult = (RedirectToRouteResult)accountController.LoginCallback(ReturnUrl);

            // Assert
            Assert.Equal(LoginFailureReason.ExternalAuthenticationFailed, (LoginFailureReason)redirectToRouteResult.RouteValues["reason"]);
        }

        [Fact]
        public void LoginCallbackWithExistingUserAndLocalReturnUrlRedirectsToReturnUrl()
        {
            // Arrange
            var authenticationMock = new Mock<IAuthentication>();
            authenticationMock.Setup(a => a.VerifyAuthentication(It.IsAny<string>())).Returns(new AuthenticationResult(true));
            authenticationMock.Setup(a => a.Login(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(true);

            var accountController = CreateAccountController(authenticationMock.Object);

            // Act
            var redirectResult = (RedirectResult)accountController.LoginCallback(ReturnUrl);

            // Assert
            Assert.Equal(ReturnUrl, redirectResult.Url);
        }

        [Fact]
        public void LoginCallbackWithExistingUserAndNonLocalReturnUrlRedirectsToHomeUrl()
        {
            // Arrange
            var authenticationMock = new Mock<IAuthentication>();
            authenticationMock.Setup(a => a.VerifyAuthentication(It.IsAny<string>())).Returns(new AuthenticationResult(true));
            authenticationMock.Setup(a => a.Login(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(true);

            var accountController = CreateAccountController(authenticationMock.Object);

            // Act
            var redirectToRouteResult = (RedirectToRouteResult)accountController.LoginCallback("http://www.google.nl");

            // Assert
            Assert.Equal("Home", redirectToRouteResult.RouteValues["controller"]);
            Assert.Equal("Index", redirectToRouteResult.RouteValues["action"]);
        }

        [Fact]
        public void LoginCallbackWithExistingUserDoesNotInsertAnotherUser()
        {
            // Arrange
            var authenticationMock = new Mock<IAuthentication>();
            authenticationMock.Setup(a => a.VerifyAuthentication(It.IsAny<string>())).Returns(new AuthenticationResult(false));

            var accountController = CreateAccountController(authenticationMock.Object);

            // Act
            var redirectToRouteResult = (RedirectToRouteResult)accountController.LoginCallback(ReturnUrl);

            // Assert
            Assert.Null(redirectToRouteResult.RouteValues["controller"]);
            Assert.Equal("LoginFailure", redirectToRouteResult.RouteValues["action"]);
        }

        [Fact]
        public void LoginCallbackWithUsernameAlreadyExistsForOtherUserRedirectsToLoginFailureAction()
        {
            // Arrange
            var authenticationMock = new Mock<IAuthentication>();
            authenticationMock.Setup(a => a.VerifyAuthentication(It.IsAny<string>())).Returns(new AuthenticationResult(true, null, null, null, new Dictionary<string, string>
                                                                                                                                                   {
                                                                                                                                                       { "organisations_name", "" },
                                                                                                                                                       { "account_username", "" },
                                                                                                                                                   }));
            authenticationMock.Setup(a => a.Login(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(false);

            var userProfileRepositoryMock = new Mock<IUserProfileRepository>();
            userProfileRepositoryMock.Setup(u => u.Find(It.IsAny<string>())).Returns(new UserProfile());

            var accountController = this.CreateAccountController(userProfileRepositoryMock.Object, authenticationMock.Object);

            // Act
            var redirectToRouteResult = (RedirectToRouteResult)accountController.LoginCallback("http://www.google.nl");

            // Assert
            Assert.Null(redirectToRouteResult.RouteValues["controller"]);
            Assert.Equal("LoginFailure", redirectToRouteResult.RouteValues["action"]);
        }

        [Fact]
        public void LoginCallbackWithUsernameAlreadyExistsForOtherUserRedirectsToLoginFailureActionWithUsernameAlreadyExistsLoginFailureReason()
        {
            // Arrange
            var authenticationMock = new Mock<IAuthentication>();
            authenticationMock.Setup(a => a.VerifyAuthentication(It.IsAny<string>())).Returns(new AuthenticationResult(true, null, null, null, new Dictionary<string, string>
                                                                                                                                                   {
                                                                                                                                                       { "organisations_name", "" },
                                                                                                                                                       { "account_username", "" },
                                                                                                                                                   }));
            authenticationMock.Setup(a => a.Login(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(false);

            var userProfileRepositoryMock = new Mock<IUserProfileRepository>();
            userProfileRepositoryMock.Setup(u => u.Find(It.IsAny<string>())).Returns(new UserProfile());

            var accountController = this.CreateAccountController(userProfileRepositoryMock.Object, authenticationMock.Object);

            // Act
            var redirectToRouteResult = (RedirectToRouteResult)accountController.LoginCallback(ReturnUrl);

            // Assert
            Assert.Equal(LoginFailureReason.UsernameAlreadyExists, (LoginFailureReason)redirectToRouteResult.RouteValues["reason"]);
        }

        private static AccountController CreateAccountController()
        {
            return CreateAccountController(new Mock<IAuthentication>().Object);
        }

        private static AccountController CreateAccountController(IAuthentication authentication)
        {
            return new AccountController(Mock.Of<IInstitutionRepository>(), Mock.Of<IUserProfileRepository>(), authentication)
                       {
                           Url = HttpContextHelper.CreateUrlHelper()
                       };
        }

        private AccountController CreateAccountController(IUserProfileRepository userProfileRepository, IAuthentication authentication)
        {
            return new AccountController(Mock.Of<IInstitutionRepository>(), userProfileRepository, authentication)
                       {
                           Url = HttpContextHelper.CreateUrlHelper()
                       };
        }

        private AccountController CreateAccountController(IInstitutionRepository institutionRepository, IUserProfileRepository userProfileRepository)
        {
            return new AccountController(institutionRepository, userProfileRepository, Mock.Of<IAuthentication>())
                       {
                           Url = HttpContextHelper.CreateUrlHelper()
                       };
        }

        private AccountController CreateAccountController(IInstitutionRepository institutionRepository, IUserProfileRepository userProfileRepository, IAuthentication authentication)
        {
            return new AccountController(institutionRepository, userProfileRepository, authentication)
                       {
                           Url = HttpContextHelper.CreateUrlHelper()
                       };
        }
    }
}