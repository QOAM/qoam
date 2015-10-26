namespace QOAM.Website.Tests.Controllers
{
    using System;
    using System.Net.Mail;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    using Moq;
    using MvcContrib.TestHelper;
    using QOAM.Core.Repositories;
    using QOAM.Core.Services;
    using QOAM.Website.Controllers;
    using QOAM.Website.Helpers;
    using QOAM.Website.Models;
    using QOAM.Website.ViewModels.Home;

    using Xunit;

    public class HomeControllerTests
    {
        private const string ContactFormSubject = "contact form subject";
        private const string ContactFormTo = "contact@to.com";
        private const string ContactFormEmail = "contact@from.com";
        private const string ContactFormMessage = "contact message";
        private const string ContactName = "contact name";

        [Fact]
        public async Task ContactPostWithValidModelStateWillSendMailMessage()
        {
            // Arrange
            var mailSenderMock = new Mock<IMailSender>();
            var homeController = new HomeController(Mock.Of<IJournalRepository>(), mailSenderMock.Object, CreateContactSettings(), Mock.Of<IUserProfileRepository>(), Mock.Of<IAuthentication>());

            // Act
            await homeController.Contact(CreateContactViewModel());

            // Assert
            mailSenderMock.Verify(m => m.Send(It.Is<MailMessage>(a =>
                                                                 a.Subject == ContactFormSubject &&
                                                                 a.Body == ContactFormMessage &&
                                                                 a.To[0].Address == ContactFormTo &&
                                                                 a.From.Address == ContactFormEmail &&
                                                                 a.From.DisplayName == ContactName)), Times.Once);
        }

        [Fact]
        public async Task ContactPostAfterSendingMailWillRedirectToContactSentAction()
        {
            // Arrange
            var homeController = new HomeController(Mock.Of<IJournalRepository>(), Mock.Of<IMailSender>(), CreateContactSettings(), Mock.Of<IUserProfileRepository>(), Mock.Of<IAuthentication>());

            // Act
            var actionResult = await homeController.Contact(CreateContactViewModel());

            // Assert
            actionResult.AssertActionRedirect().ToAction("ContactSent");
        }

        [Fact]
        public async Task ContactPostWithErrorInMailSenderReturnsViewWithModelError()
        {
            // Arrange
            var mailSenderMock = new Mock<IMailSender>();
            mailSenderMock.Setup(m => m.Send(It.IsAny<MailMessage>())).Throws<InvalidOperationException>();

            var homeController = new HomeController(Mock.Of<IJournalRepository>(), mailSenderMock.Object, CreateContactSettings(), Mock.Of<IUserProfileRepository>(), Mock.Of<IAuthentication>());

            // Act
            await homeController.Contact(CreateContactViewModel());

            // Assert
            Assert.False(homeController.ModelState.IsValid);
        }

        [Fact]
        public async Task ContactPostWithInvalidModelStateWillNotSendMail()
        {
            // Arrange
            var mailSenderMock = new Mock<IMailSender>();
            var homeController = new HomeController(Mock.Of<IJournalRepository>(), mailSenderMock.Object, CreateContactSettings(), Mock.Of<IUserProfileRepository>(), Mock.Of<IAuthentication>());
            homeController.ModelState.AddModelError("key", "error message");

            // Act
            await homeController.Contact(CreateContactViewModel());

            // Assert
            mailSenderMock.Verify(m => m.Send(It.IsAny<MailMessage>()), Times.Never());
        }

        [Fact]
        public async Task ContactPostWithInvalidModelStateWillRenderView()
        {
            // Arrange
            var homeController = new HomeController(Mock.Of<IJournalRepository>(), Mock.Of<IMailSender>(), CreateContactSettings(), Mock.Of<IUserProfileRepository>(), Mock.Of<IAuthentication>());
            homeController.ModelState.AddModelError("key", "error message");

            // Act
            var actionResult = await homeController.Contact(CreateContactViewModel());

            // Assert
            Assert.IsType<ViewResult>(actionResult);
        }

        private static ContactViewModel CreateContactViewModel()
        {
            return new ContactViewModel
                       {
                           Email = ContactFormEmail,
                           Message = ContactFormMessage,
                           Name = ContactName
                       };
        }

        private static ContactSettings CreateContactSettings()
        {
            return new ContactSettings
                       {
                           ContactFormSubject = ContactFormSubject,
                           ContactFormTo = ContactFormTo
                       };
        }
    }
}