namespace QOAM.Website.Tests.Authorization
{
    using Website.Controllers;
    using Website.ViewModels.Account;
    using Xunit;

    public class AccountControllerAuthorizationTests : ControllerAuthorizationTests<AccountController>
    {
        [Fact]
        public void LoginActionDoesNotRequireAuthorizedUser()
        {
            // Assert
            Assert.True(ActionDoesNotRequireAuthorizedUser(x => x.Login((string)null, null)));
        }

        [Fact]
        public void LoginActionWithModelDoesNotRequireAuthorizedUser()
        {
            // Assert
            Assert.True(ActionDoesNotRequireAuthorizedUser(x => x.Login((LoginViewModel)null, null)));
        }

        [Fact]
        public void LogoutActionDoesNotRequireAuthorizedUser()
        {
            // Assert
            Assert.True(ActionDoesNotRequireAuthorizedUser(x => x.Logout()));
        }

        [Fact]
        public void RegisterActionDoesNotRequireAuthorizedUser()
        {
            // Assert
            Assert.True(ActionDoesNotRequireAuthorizedUser(x => x.Register(null, null)));
        }

        [Fact]
        public void RegisterActionWithModelDoesNotRequireAuthorizedUser()
        {
            // Assert
            Assert.True(ActionDoesNotRequireAuthorizedUser(x => x.Register((RegisterViewModel)null)));
        }

        [Fact]
        public void RegistrationPendingActionDoesNotRequireAuthorizedUser()
        {
            // Assert
            Assert.True(ActionDoesNotRequireAuthorizedUser(x => x.RegistrationPending()));
        }

        [Fact]
        public void RegisterConfirmationActionDoesNotRequireAuthorizedUser()
        {
            // Assert
            Assert.True(ActionDoesNotRequireAuthorizedUser(x => x.RegisterConfirmation(null)));
        }

        [Fact]
        public void RegisterSuccessActionDoesNotRequireAuthorizedUser()
        {
            // Assert
            Assert.True(ActionDoesNotRequireAuthorizedUser(x => x.RegisterSuccess()));
        }

        [Fact]
        public void RegisterSuccessWithLinkActionDoesNotRequireAuthorizedUser()
        {
            // Assert
            Assert.True(ActionDoesNotRequireAuthorizedUser(x => x.RegisterSuccessWithLink(null)));
        }

        [Fact]
        public void RegisterFailureActionDoesNotRequireAuthorizedUser()
        {
            // Assert
            Assert.True(ActionDoesNotRequireAuthorizedUser(x => x.RegisterFailure()));
        }

        [Fact]
        public void SettingsActionRequiresAuthorizedUser()
        {
            // Assert
            Assert.True(ActionRequiresAuthorizedUser(x => x.Settings(false)));
        }

        [Fact]
        public void SettingsActionWithModelRequiresAuthorizedUser()
        {
            // Assert
            Assert.True(ActionRequiresAuthorizedUser(x => x.Settings(null)));
        }

        [Fact]
        public void ChangePasswordActionRequiresAuthorizedUser()
        {
            // Assert
            Assert.True(ActionRequiresAuthorizedUser(x => x.ChangePassword(false)));
        }

        [Fact]
        public void ChangePasswordActionWithModelRequiresAuthorizedUser()
        {
            // Assert
            Assert.True(ActionRequiresAuthorizedUser(x => x.ChangePassword(null)));
        }

        [Fact]
        public void ResetPasswordActionDoesNotRequireAuthorizedUser()
        {
            // Assert
            Assert.True(ActionDoesNotRequireAuthorizedUser(x => x.ResetPassword()));
        }

        [Fact]
        public void ResetPasswordActionWithModelDoesNotRequireAuthorizedUser()
        {
            // Assert
            Assert.True(ActionDoesNotRequireAuthorizedUser(x => x.ResetPassword(null)));
        }
        
        [Fact]
        public void ResetPasswordPendingActionDoesNotRequireAuthorizedUser()
        {
            // Assert
            Assert.True(ActionDoesNotRequireAuthorizedUser(x => x.ResetPasswordPending()));
        }

        [Fact]
        public void ResetPasswordConfirmedActionDoesNotRequireAuthorizedUser()
        {
            // Assert
            Assert.True(ActionDoesNotRequireAuthorizedUser(x => x.ResetPasswordConfirmed((string)null)));
        }

        [Fact]
        public void ResetPasswordConfirmedWithModelActionDoesNotRequireAuthorizedUser()
        {
            // Assert
            Assert.True(ActionDoesNotRequireAuthorizedUser(x => x.ResetPasswordConfirmed((ResetPasswordConfirmedViewModel)null)));
        }

        [Fact]
        public void ResetPasswordSuccessActionDoesNotRequireAuthorizedUser()
        {
            // Assert
            Assert.True(ActionDoesNotRequireAuthorizedUser(x => x.ResetPasswordSuccess()));
        }

        [Fact]
        public void ResetPasswordFailureActionDoesNotRequireAuthorizedUser()
        {
            // Assert
            Assert.True(ActionDoesNotRequireAuthorizedUser(x => x.ResetPasswordFailure()));
        }

        [Fact]
        public void ChangeEmailActionRequiresAuthorizedUser()
        {
            // Assert
            Assert.True(ActionRequiresAuthorizedUser(x => x.ChangeEmail(false)));
        }

        [Fact]
        public void ChangeEmailActionWithModelRequiresAuthorizedUser()
        {
            // Assert
            Assert.True(ActionRequiresAuthorizedUser(x => x.ChangeEmail((ChangeEmailViewModel)null)));
        }
    }
}