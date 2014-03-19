namespace QOAM.Website.Tests.Routes
{
    using System.Web.Mvc;

    using MvcContrib.TestHelper;

    using QOAM.Website.Controllers;
    using QOAM.Website.Models;

    using Xunit;

    public class AccountControllerRoutingTests : ControllerRoutingTests<AccountController>
    {
        [Fact]
        public void LoginActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            "~/account/login/".WithMethod(HttpVerbs.Get).ShouldMapTo<AccountController>(x => x.Login(null));
        }

        [Fact]
        public void LoginActionDoesNotRequireHttps()
        {
            // Assert
            Assert.True(ActionRequiresHttps(x => x.Login(null)));
        }

        [Fact]
        public void LogOffActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            "~/account/logoff/".WithMethod(HttpVerbs.Post).ShouldMapTo<AccountController>(x => x.LogOff());
        }

        [Fact]
        public void LogOffActionDoesNotRequireHttps()
        {
            // Assert
            Assert.True(ActionRequiresHttps(x => x.LogOff()));
        }

        [Fact]
        public void ExternalLoginActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            "~/account/externallogin/".WithMethod(HttpVerbs.Post).ShouldMapTo<AccountController>(x => x.ExternalLogin(null, null));
        }

        [Fact]
        public void ExternalLoginActionDoesNotRequireHttps()
        {
            // Assert
            Assert.True(ActionRequiresHttps(x => x.ExternalLogin(null, null)));
        }

        [Fact]
        public void ExternalLoginCallbackActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            "~/account/externallogincallback/".WithMethod(HttpVerbs.Get).ShouldMapTo<AccountController>(x => x.ExternalLoginCallback(null));
        }

        [Fact]
        public void ExternalLoginCallbackActionDoesNotRequireHttps()
        {
            // Assert
            Assert.True(ActionRequiresHttps(x => x.ExternalLoginCallback(null)));
        }

        [Fact]
        public void ExternalLoginFailureActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            var method = "~/account/externalloginfailure/".WithMethod(HttpVerbs.Get);
            method.Values["reason"] = LoginFailureReason.ExternalAuthenticationFailed;

            method.ShouldMapTo<AccountController>(x => x.ExternalLoginFailure(LoginFailureReason.ExternalAuthenticationFailed));
        }

        [Fact]
        public void ExternalLoginFailureActionDoesNotRequireHttps()
        {
            // Assert
            Assert.True(ActionRequiresHttps(x => x.ExternalLoginFailure(LoginFailureReason.ExternalAuthenticationFailed)));
        }

    }
}