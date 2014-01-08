namespace RU.Uci.OAMarket.Website.Tests.Controllers
{
    using System;
    using System.Net.Mail;
    using System.Web.Mvc;

    using Moq;

    using RU.Uci.OAMarket.Domain.Repositories;
    using RU.Uci.OAMarket.Domain.Services;
    using RU.Uci.OAMarket.Website.Controllers;
    using RU.Uci.OAMarket.Website.Helpers;
    using RU.Uci.OAMarket.Website.Models;
    using RU.Uci.OAMarket.Website.ViewModels.Home;

    using Xunit;

    public class HomeControllerTests
    {
        private const string ContactFormSubject = "contact form subject";
        private const string ContactFormTo = "contact@to.com";
        private const string ContactFormEmail = "contact@from.com";
        private const string ContactFormMessage = "contact message";
        private const string ContactName = "contact name";

        [Fact]
        public async void ContactPostWithValidModelStateWillSendMailMessage()
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
                                                                 a.Sender.Address == ContactFormEmail &&
                                                                 a.Sender.DisplayName == ContactName)));
        }

        [Fact]
        public async void ContactPostAfterSendingMailWillRedirectToContactSentAction()
        {
            // Arrange
            var homeController = new HomeController(Mock.Of<IJournalRepository>(), Mock.Of<IMailSender>(), new ContactSettings(), Mock.Of<IUserProfileRepository>(), Mock.Of<IAuthentication>());

            // Act
            var actionResult = (RedirectToRouteResult)await homeController.Contact(new ContactViewModel());

            // Assert
            Assert.Equal("Home", actionResult.RouteValues["controller"]);
            Assert.Equal("ContactSent", actionResult.RouteValues["action"]);
        }

        [Fact]
        public async void ContactPostWithErrorInMailSenderReturnsViewWithModelError()
        {
            // Arrange
            var mailSenderMock = new Mock<IMailSender>();
            mailSenderMock.Setup(m => m.Send(It.IsAny<MailMessage>())).Throws<InvalidOperationException>();

            var homeController = new HomeController(Mock.Of<IJournalRepository>(), mailSenderMock.Object, new ContactSettings(), Mock.Of<IUserProfileRepository>(), Mock.Of<IAuthentication>());

            // Act
            await homeController.Contact(new ContactViewModel());

            // Assert
            Assert.False(homeController.ModelState.IsValid);
        }

        [Fact]
        public void ContactPostWithInvalidModelStateWillNotSendMail()
        {
            // Arrange
            var mailSenderMock = new Mock<IMailSender>();
            var homeController = new HomeController(Mock.Of<IJournalRepository>(), mailSenderMock.Object, new ContactSettings(), Mock.Of<IUserProfileRepository>(), Mock.Of<IAuthentication>());
            homeController.ModelState.AddModelError("key", "error message");

            // Act
            homeController.Contact(new ContactViewModel());

            // Assert
            mailSenderMock.Verify(m => m.Send(It.IsAny<MailMessage>()), Times.Never());
        }

        [Fact]
        public async void ContactPostWithInvalidModelStateWillRenderView()
        {
            // Arrange
            var homeController = new HomeController(Mock.Of<IJournalRepository>(), Mock.Of<IMailSender>(), new ContactSettings(), Mock.Of<IUserProfileRepository>(), Mock.Of<IAuthentication>());
            homeController.ModelState.AddModelError("key", "error message");

            // Act
            var actionResult = await homeController.Contact(new ContactViewModel());

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