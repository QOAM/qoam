namespace RU.Uci.OAMarket.Website.Tests.Controllers
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Web.Mvc;

    using DotNetOpenAuth.AspNet;

    using Microsoft.Web.WebPages.OAuth;

    using Moq;

    using RU.Uci.OAMarket.Domain;
    using RU.Uci.OAMarket.Domain.Repositories;
    using RU.Uci.OAMarket.Website.Controllers;
    using RU.Uci.OAMarket.Website.Helpers;
    using RU.Uci.OAMarket.Website.Models;
    using RU.Uci.OAMarket.Website.Tests.Controllers.Helpers;

    using Xunit;

    public class AccountControllerTests
    {
        private const string ReturnUrl = "/home/about/";
        private const string Provider = "provider";

        [Fact]
        public void LoginStoresReturnUrlInViewBag()
        {
            // Arrange
            var accountController = CreateAccountController();

            // Act
            var viewResult = accountController.Login(ReturnUrl);

            // Assert
            Assert.Equal(ReturnUrl, viewResult.ViewBag.ReturnUrl);
        }

        [Fact]
        public void LoginStoresSurfContextClientDataInViewBag()
        {
            // Arrange
            var accountController = CreateAccountController();

            // Act
            var viewResult = accountController.Login(ReturnUrl);

            // Assert
            Assert.IsType<SurfConextClient>(viewResult.ViewBag.SurfContextClientData.AuthenticationClient);
        }

        [Fact]
        public void LogOffDoesLogoutOfAuthenticatedUser()
        {
            // Arrange
            var authenticationMock = new Mock<IAuthentication>();
            var accountController = CreateAccountController(authenticationMock.Object);

            // Act
            accountController.LogOff();

            // Assert
            authenticationMock.Verify(a => a.Logout(), Times.Once());
        }

        [Fact]
        public void LogOffWillRedirectToHomePage()
        {
            // Arrange
            var accountController = CreateAccountController();

            // Act
            var redirectToRouteResult = accountController.LogOff();

            // Assert
            Assert.Equal("Home", redirectToRouteResult.RouteValues["controller"]);
            Assert.Equal("Index", redirectToRouteResult.RouteValues["action"]);
        }

        [Fact]
        public void ExternalLoginReturnsExternalLoginResultWithPassedProvider()
        {
            // Arrange
            var accountController = CreateAccountController();

            // Act
            var externalLoginResult = accountController.ExternalLogin(Provider, ReturnUrl);

            // Assert
            Assert.Equal(Provider, externalLoginResult.Provider);
        }

        [Fact]
        public void ExternalLoginReturnsExternalLoginResultWithPassedReturnUrl()
        {
            // Arrange
            var accountController = CreateAccountController();

            // Act
            var externalLoginResult = accountController.ExternalLogin(Provider, ReturnUrl);

            // Assert
            Assert.Equal("/account/externallogincallback?ReturnUrl=%2Fhome%2Fabout%2F", externalLoginResult.ReturnUrl);
        }

        [Fact]
        public void ExternalLoginFailureReturnsPassedLoginFailureReasonAsModel()
        {
            // Arrange
            var accountController = CreateAccountController();
            var loginFailureReason = LoginFailureReason.UsernameAlreadyExists;

            // Actr

            var externalLoginResult = accountController.ExternalLoginFailure(loginFailureReason);

            // Assert
            Assert.Equal(loginFailureReason, externalLoginResult.Model);
        }

        [Fact]
        public void ExternalLoginCallbackWithAuthenticationInvalidRedirectsToExternalLoginFailureAction()
        {
            // Arrange
            var authenticationMock = new Mock<IAuthentication>();
            authenticationMock.Setup(a => a.VerifyAuthentication(It.IsAny<string>())).Returns(new AuthenticationResult(false));

            var accountController = CreateAccountController(authenticationMock.Object);

            // Act
            var redirectToRouteResult = (RedirectToRouteResult)accountController.ExternalLoginCallback(ReturnUrl);

            // Assert
            Assert.Null(redirectToRouteResult.RouteValues["controller"]);
            Assert.Equal("ExternalLoginFailure", redirectToRouteResult.RouteValues["action"]);
        }

        [Fact]
        public void ExternalLoginCallbackWithAuthenticationInvalidRedirectsToExternalLoginFailureActionWithExternalAuthenticationFailedLoginFailureReason()
        {
            // Arrange
            var authenticationMock = new Mock<IAuthentication>();
            authenticationMock.Setup(a => a.VerifyAuthentication(It.IsAny<string>())).Returns(new AuthenticationResult(false));

            var accountController = CreateAccountController(authenticationMock.Object);

            // Act
            var redirectToRouteResult = (RedirectToRouteResult)accountController.ExternalLoginCallback(ReturnUrl);

            // Assert
            Assert.Equal(LoginFailureReason.ExternalAuthenticationFailed, (LoginFailureReason)redirectToRouteResult.RouteValues["reason"]);
        }

        [Fact]
        public void ExternalLoginCallbackWithExistingUserAndLocalReturnUrlRedirectsToReturnUrl()
        {
            // Arrange
            var authenticationMock = new Mock<IAuthentication>();
            authenticationMock.Setup(a => a.VerifyAuthentication(It.IsAny<string>())).Returns(new AuthenticationResult(true));
            authenticationMock.Setup(a => a.Login(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(true);

            var accountController = CreateAccountController(authenticationMock.Object);

            // Act
            var redirectResult = (RedirectResult)accountController.ExternalLoginCallback(ReturnUrl);

            // Assert
            Assert.Equal(ReturnUrl, redirectResult.Url);
        }

        [Fact]
        public void ExternalLoginCallbackWithExistingUserAndNonLocalReturnUrlRedirectsToHomeUrl()
        {
            // Arrange
            var authenticationMock = new Mock<IAuthentication>();
            authenticationMock.Setup(a => a.VerifyAuthentication(It.IsAny<string>())).Returns(new AuthenticationResult(true));
            authenticationMock.Setup(a => a.Login(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(true);

            var accountController = CreateAccountController(authenticationMock.Object);

            // Act
            var redirectToRouteResult = (RedirectToRouteResult)accountController.ExternalLoginCallback("http://www.google.nl");

            // Assert
            Assert.Equal("Home", redirectToRouteResult.RouteValues["controller"]);
            Assert.Equal("Index", redirectToRouteResult.RouteValues["action"]);
        }

        [Fact]
        public void ExternalLoginCallbackWithExistingUserDoesNotInsertAnotherUser()
        {
            // Arrange
            var authenticationMock = new Mock<IAuthentication>();
            authenticationMock.Setup(a => a.VerifyAuthentication(It.IsAny<string>())).Returns(new AuthenticationResult(false));

            var accountController = CreateAccountController(authenticationMock.Object);

            // Act
            var redirectToRouteResult = (RedirectToRouteResult)accountController.ExternalLoginCallback(ReturnUrl);

            // Assert
            Assert.Null(redirectToRouteResult.RouteValues["controller"]);
            Assert.Equal("ExternalLoginFailure", redirectToRouteResult.RouteValues["action"]);
        }

        [Fact]
        public void ExternalLoginCallbackWithUsernameAlreadyExistsForOtherUserRedirectsToExternalLoginFailureAction()
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
            var redirectToRouteResult = (RedirectToRouteResult)accountController.ExternalLoginCallback("http://www.google.nl");

            // Assert
            Assert.Null(redirectToRouteResult.RouteValues["controller"]);
            Assert.Equal("ExternalLoginFailure", redirectToRouteResult.RouteValues["action"]);
        }

        [Fact]
        public void ExternalLoginCallbackWithUsernameAlreadyExistsForOtherUserRedirectsToExternalLoginFailureActionWithUsernameAlreadyExistsLoginFailureReason()
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

            var accountController = CreateAccountController(userProfileRepositoryMock.Object, authenticationMock.Object);

            // Act
            var redirectToRouteResult = (RedirectToRouteResult)accountController.ExternalLoginCallback(ReturnUrl);

            // Assert
            Assert.Equal(LoginFailureReason.UsernameAlreadyExists, (LoginFailureReason)redirectToRouteResult.RouteValues["reason"]);
        }

        private static AccountController CreateAccountController()
        {
            var authenticationMock = new Mock<IAuthentication>();
            authenticationMock.Setup(a => a.RegisteredClientData).Returns(new Collection<AuthenticationClientData> { new AuthenticationClientData(new SurfConextClient(new SurfContextSettings()), "SurfContext", null) });

            return CreateAccountController(authenticationMock.Object);
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