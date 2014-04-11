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
        public void LoginCallbackActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            "~/account/login/callback/".WithMethod(HttpVerbs.Get).ShouldMapTo<AccountController>(x => x.LoginCallback(null));
        }

        [Fact]
        public void LoginCallbackActionDoesNotRequireHttps()
        {
            // Assert
            Assert.True(ActionRequiresHttps(x => x.LoginCallback(null)));
        }

        [Fact]
        public void LoginFailureActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            var method = "~/account/login/failure/".WithMethod(HttpVerbs.Get);
            method.Values["reason"] = LoginFailureReason.ExternalAuthenticationFailed;

            method.ShouldMapTo<AccountController>(x => x.LoginFailure(LoginFailureReason.ExternalAuthenticationFailed));
        }

        [Fact]
        public void LoginFailureActionDoesNotRequireHttps()
        {
            // Assert
            Assert.True(ActionRequiresHttps(x => x.LoginFailure(LoginFailureReason.ExternalAuthenticationFailed)));
        }

    }
}