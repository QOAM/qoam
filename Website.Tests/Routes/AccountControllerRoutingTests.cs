namespace QOAM.Website.Tests.Routes
{
    using System.Net.Http;
    using System.Web.Mvc;

    using MvcContrib.TestHelper;
    using MvcRouteTester;
    using QOAM.Website.Controllers;
    using QOAM.Website.Models;
    using Website.ViewModels.Account;
    using Xunit;

    public class AccountControllerRoutingTests : ControllerRoutingTests<AccountController>
    {
        [Fact]
        public void LoginActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/login/").To<AccountController>(HttpMethod.Get, x => x.Login((string)null, null));
        }

        [Fact]
        public void LoginActionRequiresHttps()
        {
            // Assert
            Assert.True(ActionRequiresHttps(x => x.Login((string)null, null)));
        }

        [Fact]
        public void LoginActionWithModelRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/login/").To<AccountController>(HttpMethod.Post, x => x.Login((LoginViewModel)null, null));
        }

        [Fact]
        public void LoginActionWithModelRequiresHttps()
        {
            // Assert
            Assert.True(ActionRequiresHttps(x => x.Login((LoginViewModel)null, null)));
        }

        [Fact]
        public void LogoutActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/logout/").To<AccountController>(HttpMethod.Get, x => x.Logout());
        }

        [Fact]
        public void LogoutActionRequiresHttps()
        {
            // Assert
            Assert.True(ActionRequiresHttps(x => x.Logout()));
        }

        [Fact]
        public void RegisterActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/register/").To<AccountController>(HttpMethod.Get, x => x.Register(null, null));
        }

        [Fact]
        public void RegisterActionRequiresHttps()
        {
            // Assert
            Assert.True(ActionRequiresHttps(x => x.Register(null, null)));
        }

        [Fact]
        public void RegisterActionWithModelRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/register/").To<AccountController>(HttpMethod.Post, x => x.Register(null));
        }

        [Fact]
        public void RegisterActionWithModelRequiresHttps()
        {
            // Assert
            Assert.True(ActionRequiresHttps(x => x.Register(null)));
        }

        [Fact]
        public void RegistrationPendingActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/registrationpending/").To<AccountController>(HttpMethod.Get, x => x.RegistrationPending());
        }

        [Fact]
        public void RegistrationPendingActionRequiresHttps()
        {
            // Assert
            Assert.True(ActionRequiresHttps(x => x.RegistrationPending()));
        }

        [Fact]
        public void RegisterConfirmationActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/registerconfirmation/abcd/").To<AccountController>(HttpMethod.Get, x => x.RegisterConfirmation(null));
        }

        [Fact]
        public void RegisterConfirmationActionRequiresHttps()
        {
            // Assert
            Assert.True(ActionRequiresHttps(x => x.RegisterConfirmation(null)));
        }

        [Fact]
        public void RegisterSuccessActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/registersuccess/").To<AccountController>(HttpMethod.Get, x => x.RegisterSuccess());
        }

        [Fact]
        public void RegisterSuccessActionRequiresHttps()
        {
            // Assert
            Assert.True(ActionRequiresHttps(x => x.RegisterSuccess()));
        }

        [Fact]
        public void RegisterSuccessWithLinkActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/registersuccesswithlink/").To<AccountController>(HttpMethod.Get, x => x.RegisterSuccessWithLink(null));
        }

        [Fact]
        public void RegisterSuccessWithLinkActionRequiresHttps()
        {
            // Assert
            Assert.True(ActionRequiresHttps(x => x.RegisterSuccessWithLink(null)));
        }
        
        [Fact]
        public void RegisterFailureActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/registerfailure/").To<AccountController>(HttpMethod.Get, x => x.RegisterFailure());
        }

        [Fact]
        public void RegisterFailureActionRequiresHttps()
        {
            // Assert
            Assert.True(ActionRequiresHttps(x => x.RegisterFailure()));
        }
        
        [Fact]
        public void SettingsActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/account/settings/?saveSuccessful=false").To<AccountController>(HttpMethod.Get, x => x.Settings(false));
        }

        [Fact]
        public void SettingsActionRequiresHttps()
        {
            // Assert
            Assert.True(ActionRequiresHttps(x => x.Settings(false)));
        }

        [Fact]
        public void SettingsActionWithModelRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/account/settings/").To<AccountController>(HttpMethod.Post, x => x.Settings(null));
        }

        [Fact]
        public void SettingsActionWithModelRequiresHttps()
        {
            // Assert
            Assert.True(ActionRequiresHttps(x => x.Settings( null)));
        }
        
        [Fact]
        public void ChangePasswordActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/changepassword/?saveSuccessful=false").To<AccountController>(HttpMethod.Get, x => x.ChangePassword(false));
        }

        [Fact]
        public void ChangePasswordActionRequiresHttps()
        {
            // Assert
            Assert.True(ActionRequiresHttps(x => x.ChangePassword(false)));
        }

        [Fact]
        public void ChangePasswordActionWithModelRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/changepassword/").To<AccountController>(HttpMethod.Post, x => x.ChangePassword(null));
        }

        [Fact]
        public void ChangePasswordActionWithModelRequiresHttps()
        {
            // Assert
            Assert.True(ActionRequiresHttps(x => x.ChangePassword(null)));
        }
        
        [Fact]
        public void ResetPasswordActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/resetpassword/").To<AccountController>(HttpMethod.Get, x => x.ResetPassword());
        }

        [Fact]
        public void ResetPasswordActionRequiresHttps()
        {
            // Assert
            Assert.True(ActionRequiresHttps(x => x.ResetPassword()));
        }

        [Fact]
        public void ResetPasswordActionWithModelRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/resetpassword/").To<AccountController>(HttpMethod.Post, x => x.ResetPassword(null));
        }

        [Fact]
        public void ResetPasswordActionWithModelRequiresHttps()
        {
            // Assert
            Assert.True(ActionRequiresHttps(x => x.ResetPassword(null)));
        }
        
        [Fact]
        public void ResetPasswordPendingActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/resetpasswordpending/").To<AccountController>(HttpMethod.Get, x => x.ResetPasswordPending());
        }

        [Fact]
        public void ResetPasswordPendingActionRequiresHttps()
        {
            // Assert
            Assert.True(ActionRequiresHttps(x => x.ResetPasswordPending()));
        }

        [Fact]
        public void ResetPasswordConfirmedActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/resetpasswordconfirmed/").To<AccountController>(HttpMethod.Get, x => x.ResetPasswordConfirmed((string)null));
        }

        [Fact]
        public void ResetPasswordConfirmedActionRequiresHttps()
        {
            // Assert
            Assert.True(ActionRequiresHttps(x => x.ResetPasswordConfirmed((string)null)));
        }

        [Fact]
        public void ResetPasswordConfirmedActionWithModelRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/resetpasswordconfirmed/").To<AccountController>(HttpMethod.Post, x => x.ResetPasswordConfirmed((ResetPasswordConfirmedViewModel)null));
        }

        [Fact]
        public void ResetPasswordConfirmedActionWithModelRequiresHttps()
        {
            // Assert
            Assert.True(ActionRequiresHttps(x => x.ResetPasswordConfirmed((ResetPasswordConfirmedViewModel)null)));
        }
        
        [Fact]
        public void ResetPasswordSuccessActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/resetpasswordsuccess/").To<AccountController>(HttpMethod.Get, x => x.ResetPasswordSuccess());
        }

        [Fact]
        public void ResetPasswordSuccessActionRequiresHttps()
        {
            // Assert
            Assert.True(ActionRequiresHttps(x => x.ResetPasswordSuccess()));
        }

        [Fact]
        public void ResetPasswordFailureActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/resetpasswordfailure/").To<AccountController>(HttpMethod.Get, x => x.ResetPasswordFailure());
        }

        [Fact]
        public void ResetPasswordFailureActionRequiresHttps()
        {
            // Assert
            Assert.True(ActionRequiresHttps(x => x.ResetPasswordFailure()));
        }

        [Fact]
        public void ChangeEmailActionRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/changeemail/?saveSuccessful=false").To<AccountController>(HttpMethod.Get, x => x.ChangeEmail(false));
        }

        [Fact]
        public void ChangeEmailActionRequiresHttps()
        {
            // Assert
            Assert.True(ActionRequiresHttps(x => x.ChangeEmail(false)));
        }

        [Fact]
        public void ChangeEmailActionWithModelRoutedToWithCorrectUrlAndVerb()
        {
            // Assert    
            ApplicationRoutes.ShouldMap("~/changeemail/").To<AccountController>(HttpMethod.Post, x => x.ChangeEmail((ChangeEmailViewModel)null));
        }

        [Fact]
        public void ChangeEmailActionWithModelRequiresHttps()
        {
            // Assert
            Assert.True(ActionRequiresHttps(x => x.ChangeEmail((ChangeEmailViewModel)null)));
        }
    }
}