using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mail;

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
    using Website.Controllers;
    using Website.Helpers;
    using Website.Models;
    using Website.ViewModels.Home;
    using Xunit;

    public class HomeControllerTests
    {
        private const string ContactFormSubject = "contact form subject";
        private const string ContactFormTo = "contact@to.com";
        private const string ContactFormEmail = "contact@from.com";
        private const string ContactFormMessage = "contact message";
        private const string ContactName = "contact name";

        [Fact]
        public void IndexRendersView()
        {
            // Arrange
            var homeController = CreateHomeController();

            // Act
            var viewResult = homeController.Index();

            // Assert
            Assert.IsType<ViewResult>(viewResult);
        }

        [Fact]
        public async Task ContactPostWithValidModelStateWillSendMailMessage()
        {
            // Arrange
            var mailSenderMock = new Mock<IMailSender>();
            var homeController = CreateHomeController(mailSender: mailSenderMock.Object);

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
        public async Task ContactPostWithAttachmentWillSendMailMessage()
        {
            // Arrange
            var mailSenderMock = new Mock<IMailSender>();
            var homeController = CreateHomeController(mailSender: mailSenderMock.Object);
            var mockFile = new Mock<HttpPostedFileBase>();
            var fileStream = new FileStream("Controllers\\Stubs\\Upload file stub.xlsx", FileMode.Open, FileAccess.Read);

            mockFile.Setup(f => f.FileName).Returns(fileStream.Name);
            mockFile.Setup(f => f.InputStream).Returns(fileStream);
            mockFile.Setup(x => x.ContentLength).Returns((int)fileStream.Length);

            // Act
            await homeController.Contact(CreateContactViewModel(mockFile.Object));
            
            // Assert

            var attachment = new Attachment("Controllers\\Stubs\\Upload file stub.xlsx");

            mailSenderMock.Verify(m => m.Send(It.Is<MailMessage>(a => a.Attachments.Any(x => x.Name == mockFile.Object.FileName) )), Times.Once);
        }

        [Fact]
        public async Task ContactPostAfterSendingMailWillRedirectToContactSentAction()
        {
            // Arrange
            var homeController = CreateHomeController();

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

            var homeController = CreateHomeController(mailSender: mailSenderMock.Object);

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
            var homeController = CreateHomeController(mailSender: mailSenderMock.Object);
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
            var homeController = CreateHomeController();
            homeController.ModelState.AddModelError("key", "error message");

            // Act
            var actionResult = await homeController.Contact(CreateContactViewModel());

            // Assert
            Assert.IsType<ViewResult>(actionResult);
        }

        private static HomeController CreateHomeController(
            IBaseScoreCardRepository baseScoreCardRepository = null,
            IValuationScoreCardRepository valuationScoreCardRepository = null,
            IJournalRepository journalRepository = null,
            IMailSender mailSender = null,
            ContactSettings contactSettings = null,
            IUserProfileRepository userProfileRepository = null,
            IAuthentication authentication = null)
        {
            return new HomeController(
                baseScoreCardRepository ?? Mock.Of<IBaseScoreCardRepository>(),
                valuationScoreCardRepository ?? Mock.Of<IValuationScoreCardRepository>(),
                journalRepository ?? Mock.Of<IJournalRepository>(),
                mailSender ?? Mock.Of<IMailSender>(),
                contactSettings ?? CreateContactSettings(),
                userProfileRepository ?? Mock.Of<IUserProfileRepository>(),
                authentication ?? Mock.Of<IAuthentication>());
        }

        private static ContactViewModel CreateContactViewModel(HttpPostedFileBase file = null)
        {
            return new ContactViewModel
                       {
                           Email = ContactFormEmail,
                           Message = ContactFormMessage,
                           Name = ContactName,
                           File = file
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