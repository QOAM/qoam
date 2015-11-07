namespace QOAM.Website.Tests.Controllers
{
    using System;
    using System.Net.Mail;
    using System.Web.Mvc;
    using Core;
    using Moq;
    using MvcContrib.TestHelper;
    using QOAM.Core.Repositories;
    using QOAM.Website.Tests.Controllers.Helpers;
    using Website.Controllers;
    using Website.Helpers;
    using Website.ViewModels.Account;
    using Xunit;

    public class AccountControllerTests
    {
        [Fact]
        public void LoginWithValidLoginUpdatesDateLastLoginToCurrentDate()
        {
            // Arrange
            var userProfile = new UserProfile { Id = 2, DateLastLogin = new DateTime(2015, 2, 8) };

            var userProfileRepository = new Mock<IUserProfileRepository>();
            userProfileRepository.Setup(u => u.FindByEmail(It.IsAny<string>())).Returns(userProfile);

            var accountController = CreateAccountController(userProfileRepository: userProfileRepository.Object);

            // Act
            accountController.Login(new LoginViewModel(), string.Empty);

            // Assert
            userProfileRepository.Verify(u => u.InsertOrUpdate(It.Is<UserProfile>(p => p.Id == userProfile.Id && DateTime.Now - p.DateLastLogin < TimeSpan.FromSeconds(2))), Times.Once);
        }

        [Fact]
        public void LoginWithValidLoginSavesUserProfile()
        {
            // Arrange
            var userProfile = new UserProfile { Id = 2, DateLastLogin = new DateTime(2015, 2, 8) };

            var userProfileRepository = new Mock<IUserProfileRepository>();
            userProfileRepository.Setup(u => u.FindByEmail(It.IsAny<string>())).Returns(userProfile);

            var accountController = CreateAccountController(userProfileRepository: userProfileRepository.Object);

            // Act
            accountController.Login(new LoginViewModel(), string.Empty);

            // Assert
            userProfileRepository.Verify(u => u.Save(), Times.Once);
        }

        [Fact]
        public void LoginWithInvalidLoginDoesNotUpdateDateLastLogin()
        {
            // Arrange
            var userProfile = new UserProfile { Id = 2, DateLastLogin = new DateTime(2015, 2, 8) };

            var userProfileRepository = new Mock<IUserProfileRepository>();
            userProfileRepository.Setup(u => u.FindByEmail(It.IsAny<string>())).Returns(userProfile);

            var authentication = new Mock<IAuthentication>();
            authentication.Setup(a => a.Login(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(false);

            var accountController = CreateAccountController(userProfileRepository: userProfileRepository.Object, authentication: authentication.Object);

            // Act
            accountController.Login(new LoginViewModel(), string.Empty);

            // Assert
            userProfileRepository.Verify(u => u.InsertOrUpdate(It.IsAny<UserProfile>()), Times.Never);
        }

        [Fact]
        public void ChangeEmailRendersView()
        {
            // Arrange
            var accountController = CreateAccountController();

            // Act
            var viewResult = accountController.ChangeEmail(false);

            // Assert
            Assert.IsType<ViewResult>(viewResult);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(false)]
        [InlineData(true)]
        public void ChangeEmailStoresSaveSuccessfulStatusInViewBag(bool? saveSuccessful)
        {
            // Arrange
            var accountController = CreateAccountController();

            // Act
            var viewResult = accountController.ChangeEmail(saveSuccessful);

            // Assert
            Assert.Equal(saveSuccessful, viewResult.ViewBag.SaveSuccessful);
        }

        [Fact]
        public void ChangeEmailWithInvalidPasswordAddsModelErrorForPassword()
        {
            // Arrange
            var authentication = new Mock<IAuthentication>();
            authentication.Setup(a => a.ValidateUser(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            var accountController = CreateAccountController(authentication: authentication.Object);
            var changeEmailViewModel = CreateChangeEmailViewModel();

            // Act
            accountController.ChangeEmail(changeEmailViewModel);

            // Assert
            Assert.True(accountController.ModelState.ContainsKey(nameof(changeEmailViewModel.Password)));
        }

        [Fact]
        public void ChangeEmailWithValidPasswordUpdatesUserWithNewEmail()
        {
            // Arrange
            var changeEmailViewModel = CreateChangeEmailViewModel();
            var userProfile = new UserProfile { UserName = "foo", Email = "old@test.com" };
            
            var userProfileRepository = new Mock<IUserProfileRepository>();
            userProfileRepository.Setup(u => u.Find(It.IsAny<string>())).Returns(userProfile);
            
            var accountController = CreateAccountController(userProfileRepository: userProfileRepository.Object);
            
            // Act
            accountController.ChangeEmail(changeEmailViewModel);

            // Assert
            userProfileRepository.Verify(u => u.InsertOrUpdate(It.Is<UserProfile>(p => p == userProfile && p.Email == changeEmailViewModel.NewEmail)), Times.Once);
        }

        [Fact]
        public void ChangeEmailWithValidPasswordSavesChanges()
        {
            // Arrange
            var userProfileRepository = new Mock<IUserProfileRepository>();
            userProfileRepository.Setup(u => u.Find(It.IsAny<string>())).Returns(new UserProfile());

            var accountController = CreateAccountController(userProfileRepository: userProfileRepository.Object);

            // Act
            accountController.ChangeEmail(CreateChangeEmailViewModel());

            // Assert
            userProfileRepository.Verify(u => u.Save(), Times.Once);
        }

        [Fact]
        public void ChangeEmailWithSameEmailAsCurrentEmailDoesNotUpdateUser()
        {
            // Arrange
            var changeEmailViewModel = CreateChangeEmailViewModel();
            var userProfile = new UserProfile { UserName = "foo", Email = changeEmailViewModel.NewEmail };

            var userProfileRepository = new Mock<IUserProfileRepository>();
            userProfileRepository.Setup(u => u.Find(It.IsAny<string>())).Returns(userProfile);

            var accountController = CreateAccountController(userProfileRepository: userProfileRepository.Object);

            // Act
            accountController.ChangeEmail(changeEmailViewModel);

            // Assert
            userProfileRepository.Verify(u => u.InsertOrUpdate(userProfile), Times.Never);
        }

        [Fact]
        public void ChangeEmailWithEmailExistsOnOtherUserAddsModelErrorForNewEmail()
        {
            // Arrange
            var otherUserProfile = new UserProfile { Id = 1, UserName = "bar", Email = "existing@test.com" };
            var userProfile = new UserProfile { Id = 2, UserName = "foo", Email = "old@test.com" };

            var changeEmailViewModel = CreateChangeEmailViewModel();
            changeEmailViewModel.NewEmail = otherUserProfile.Email;

            var userProfileRepository = new Mock<IUserProfileRepository>();
            userProfileRepository.Setup(u => u.Find(It.IsAny<string>())).Returns(userProfile);
            userProfileRepository.Setup(u => u.FindByEmail(changeEmailViewModel.NewEmail)).Returns(otherUserProfile);

            var accountController = CreateAccountController(userProfileRepository: userProfileRepository.Object);

            // Act
            accountController.ChangeEmail(changeEmailViewModel);

            // Assert
            Assert.True(accountController.ModelState.ContainsKey(nameof(changeEmailViewModel.NewEmail)));
        }

        [Fact]
        public void ChangeEmailWithEmailExistsOnOtherUserDoesNotUpdateUser()
        {
            // Arrange
            var otherUserProfile = new UserProfile { Id = 1, UserName = "bar", Email = "existing@test.com" };
            var userProfile = new UserProfile { Id = 2, UserName = "foo", Email = "old@test.com" };

            var changeEmailViewModel = CreateChangeEmailViewModel();
            changeEmailViewModel.NewEmail = otherUserProfile.Email;

            var userProfileRepository = new Mock<IUserProfileRepository>();
            userProfileRepository.Setup(u => u.Find(It.IsAny<string>())).Returns(userProfile);
            userProfileRepository.Setup(u => u.FindByEmail(changeEmailViewModel.NewEmail)).Returns(otherUserProfile);

            var accountController = CreateAccountController(userProfileRepository: userProfileRepository.Object);

            // Act
            accountController.ChangeEmail(changeEmailViewModel);

            // Assert
            userProfileRepository.Verify(u => u.InsertOrUpdate(It.IsAny<UserProfile>()), Times.Never);
        }

        [Fact]
        public void ChangeEmailWithInvalidDomainAddsModelErrorForNewEmail()
        {
            // Arrange
            var institutionRepository = new Mock<IInstitutionRepository>();
            institutionRepository.Setup(i => i.Find(It.IsAny<MailAddress>())).Returns((Institution)null);

            var accountController = CreateAccountController(institutionRepository: institutionRepository.Object);

            var changeEmailViewModel = CreateChangeEmailViewModel();

            // Act
            accountController.ChangeEmail(changeEmailViewModel);

            // Assert
            Assert.True(accountController.ModelState.ContainsKey(nameof(changeEmailViewModel.NewEmail)));
        }

        [Fact]
        public void ChangeEmailWithInvalidDomainDoesNotUpdateUser()
        {
            // Arrange
            var institutionRepository = new Mock<IInstitutionRepository>();
            institutionRepository.Setup(i => i.Find(It.IsAny<MailAddress>())).Returns((Institution)null);

            var userProfileRepository = new Mock<IUserProfileRepository>();
            userProfileRepository.Setup(u => u.Find(It.IsAny<string>())).Returns(new UserProfile());

            var accountController = CreateAccountController(institutionRepository: institutionRepository.Object, userProfileRepository: userProfileRepository.Object);

            // Act
            accountController.ChangeEmail(CreateChangeEmailViewModel());

            // Assert
            userProfileRepository.Verify(u => u.InsertOrUpdate(It.IsAny<UserProfile>()), Times.Never);
        }
        
        [Fact]
        public void ChangeEmailWithInvalidModelRendersView()
        {
            // Arrange
            var accountController = CreateAccountController();
            accountController.ModelState.AddModelError("Email", "Invalid");

            // Act
            var actionResult = accountController.ChangeEmail(CreateChangeEmailViewModel());

            // Assert
            actionResult.AssertViewRendered();
        }

        [Fact]
        public void ChangeEmailWithValidModelRedirectsToChangeEmailAction()
        {
            // Arrange
            var accountController = CreateAccountController();

            // Act
            var actionResult = accountController.ChangeEmail(CreateChangeEmailViewModel());

            // Assert
            actionResult.AssertActionRedirect().ToAction("ChangeEmail");
        }

        [Fact]
        public void ChangeEmailWithValidModelRedirectsWithSaveSuccessfulParameterSetToTrue()
        {
            // Arrange
            var accountController = CreateAccountController();

            // Act
            var actionResult = accountController.ChangeEmail(CreateChangeEmailViewModel());

            // Assert
            actionResult.AssertActionRedirect().WithParameter("saveSuccessful", true);
        }

        private static ChangeEmailViewModel CreateChangeEmailViewModel()
        {
            return new ChangeEmailViewModel
            {
                NewEmail = "new@test.com",
                ConfirmEmail = "new@test.com",
                Password = "passw0rd"
            };
        }

        private static AccountController CreateAccountController(
            IBaseScoreCardRepository baseScoreCardRepository = null,
            IValuationScoreCardRepository valuationScoreCardRepository = null,
            IUserProfileRepository userProfileRepository = null,
            IAuthentication authentication = null,
            IInstitutionRepository institutionRepository = null,
            IJournalRepository journalRepository = null)
        {
            return new AccountController(
                baseScoreCardRepository ?? Mock.Of<IBaseScoreCardRepository>(),
                valuationScoreCardRepository ?? Mock.Of<IValuationScoreCardRepository>(),
                userProfileRepository ?? CreateUserProfileRepository(),
                authentication ?? CreateAuthentication(),
                institutionRepository ?? CreateInstitutionRepository(),
                journalRepository ?? Mock.Of<IJournalRepository>())
            {
                Url = HttpContextHelper.CreateUrlHelper()
            };
        }

        private static IInstitutionRepository CreateInstitutionRepository()
        {
            var institutionRepository = new Mock<IInstitutionRepository>();
            institutionRepository.Setup(i => i.Find(It.IsAny<MailAddress>())).Returns(new Institution());

            return institutionRepository.Object;
        }

        private static IUserProfileRepository CreateUserProfileRepository()
        {
            var userProfileRepository = new Mock<IUserProfileRepository>();
            userProfileRepository.Setup(u => u.Find(It.IsAny<string>())).Returns(new UserProfile());

            return userProfileRepository.Object;
        }

        private static IAuthentication CreateAuthentication()
        {
            var authentication = new Mock<IAuthentication>();
            authentication.Setup(a => a.ValidateUser(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            authentication.Setup(a => a.Login(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(true);

            return authentication.Object;
        }
    }
}