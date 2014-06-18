namespace QOAM.Website.Tests.Controllers
{
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.SessionState;

    using DotNetOpenAuth.AspNet;

    using Moq;

    using MvcContrib.TestHelper.Fakes;

    using QOAM.Core;
    using QOAM.Core.Repositories;
    using QOAM.Website.Controllers;
    using QOAM.Website.Helpers;
    using QOAM.Website.Models;
    using QOAM.Website.Models.SAML;
    using QOAM.Website.Tests.Controllers.Helpers;

    using SAML2.Identity;
    using SAML2.Schema.Core;

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
        public void LoginCallbackWithNullSessionRedirectsToLoginFailureAction()
        {
            // Arrange
            HttpSessionStateBase nullHttpSessionState = null;
            var accountController = CreateAccountController(nullHttpSessionState);

            // Act
            var redirectToRouteResult = (RedirectToRouteResult)accountController.LoginCallback(ReturnUrl);

            // Assert
            Assert.Null(redirectToRouteResult.RouteValues["controller"]);
            Assert.Equal("LoginFailure", redirectToRouteResult.RouteValues["action"]);
        }

        [Fact]
        public void LoginCallbackWithSaml20IdentityNotInSessionRedirectsToLoginFailureAction()
        {
            // Arrange
            var httpSessionStateMock = new FakeHttpSessionState(new SessionStateItemCollection());
            var accountController = CreateAccountController(httpSessionStateMock);

            // Act
            var redirectToRouteResult = (RedirectToRouteResult)accountController.LoginCallback(ReturnUrl);

            // Assert
            Assert.Null(redirectToRouteResult.RouteValues["controller"]);
            Assert.Equal("LoginFailure", redirectToRouteResult.RouteValues["action"]);
        }

        [Fact]
        public void LoginCallbackWithUserNotAuthenticatedRedirectsToLoginFailureAction()
        {
            // Arrange
            var saml20IdentityMock = new Mock<ISaml20Identity>();
            saml20IdentityMock.Setup(s => s.IsAuthenticated).Returns(false);

            var accountController = CreateAccountController(saml20IdentityMock.Object);

            // Act
            var redirectToRouteResult = (RedirectToRouteResult)accountController.LoginCallback(ReturnUrl);

            // Assert
            Assert.Null(redirectToRouteResult.RouteValues["controller"]);
            Assert.Equal("LoginFailure", redirectToRouteResult.RouteValues["action"]);
        }

        [Fact]
        public void LoginCallbackWithExistingUserAndLocalReturnUrlRedirectsToReturnUrl()
        {
            // Arrange
            var authenticationMock = new Mock<IAuthentication>();
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
            authenticationMock.Setup(a => a.Login(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(true);

            var accountController = CreateAccountController(authenticationMock.Object);

            // Act
            var redirectToRouteResult = (RedirectToRouteResult)accountController.LoginCallback("http://www.google.nl");

            // Assert
            Assert.Equal("Home", redirectToRouteResult.RouteValues["controller"]);
            Assert.Equal("Index", redirectToRouteResult.RouteValues["action"]);
        }

        [Fact]
        public void LoginCallbackWithUsernameAlreadyExistsForOtherUserRedirectsToLoginFailureAction()
        {
            // Arrange
            var authenticationMock = new Mock<IAuthentication>();
            authenticationMock.Setup(a => a.Login(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(false);

            var userProfileRepositoryMock = new Mock<IUserProfileRepository>();
            userProfileRepositoryMock.Setup(u => u.Find(It.IsAny<string>())).Returns(new UserProfile());

            var accountController = CreateAccountController(userProfileRepositoryMock.Object, authenticationMock.Object);

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

            var userProfileRepositoryMock = new Mock<IUserProfileRepository>();
            userProfileRepositoryMock.Setup(u => u.Find(It.IsAny<string>())).Returns(new UserProfile());

            var accountController = CreateAccountController(userProfileRepositoryMock.Object, authenticationMock.Object);

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
            return new AccountController(Mock.Of<IInstitutionRepository>(), Mock.Of<IUserProfileRepository>(), authentication, CreateHttpSessionState())
                   {
                       Url = HttpContextHelper.CreateUrlHelper()
                   };
        }

        private static HttpSessionStateBase CreateHttpSessionState()
        {
            return CreateHttpSessionState(CreateSaml20IdentityMock());
        }

        private static ISaml20Identity CreateSaml20IdentityMock()
        {
            var mock = new Mock<ISaml20Identity>();
            mock.Setup(s => s.IsAuthenticated).Returns(true);
            mock.Setup(s => s[SamlAttributes.SchacHomeOrganization]).Returns(new List<SamlAttribute> { new SamlAttribute { AttributeValue = new[] { "ru.nl" } } });

            return mock.Object;
        }

        private static HttpSessionStateBase CreateHttpSessionState(ISaml20Identity saml20Identity)
        {
            var fakeHttpSessionState = new FakeHttpSessionState(new SessionStateItemCollection());
            fakeHttpSessionState[typeof(Saml20Identity).FullName] = saml20Identity;

            return fakeHttpSessionState;
        }

        private static AccountController CreateAccountController(HttpSessionStateBase httpSessionStateMock)
        {
            return new AccountController(Mock.Of<IInstitutionRepository>(), Mock.Of<IUserProfileRepository>(), Mock.Of<IAuthentication>(), httpSessionStateMock)
                   {
                       Url = HttpContextHelper.CreateUrlHelper()
                   };
        }

        private static AccountController CreateAccountController(IUserProfileRepository userProfileRepository, IAuthentication authentication)
        {
            return new AccountController(Mock.Of<IInstitutionRepository>(), userProfileRepository, authentication, CreateHttpSessionState())
                   {
                       Url = HttpContextHelper.CreateUrlHelper()
                   };
        }

        private static AccountController CreateAccountController(ISaml20Identity saml20Identity)
        {
            return new AccountController(Mock.Of<IInstitutionRepository>(), Mock.Of<IUserProfileRepository>(), Mock.Of<IAuthentication>(), CreateHttpSessionState(saml20Identity))
                   {
                       Url = HttpContextHelper.CreateUrlHelper()
                   };
        }
    }
}