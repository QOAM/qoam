namespace QOAM.Website.Tests.ViewModels.Home
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Website.Models;
    using Website.ViewModels.Home;
    using Xunit;

    public class ContactViewModelTests
    {
        private const string ContactFormSubject = "contact form subject";
        private const string ContactFormTo = "contact@to.com";
        private const string Email = "contact@from.com";
        private const string Message = "contact message";
        private const string Name = "contact name";

        [Fact]
        public void ToMailMessageReturnsMailMessageWithMessageAsBody()
        {
            // Arrange
            var contactViewModel = CreateContactViewModel();

            // Act
            var mailMessage = contactViewModel.ToMailMessage(CreateContactSettings());

            // Assert
            Assert.Equal(Message, mailMessage.Body);
        }

        [Fact]
        public void ToMailMessageReturnsMailMessageWithEmailAsSenderAddress()
        {
            // Arrange
            var contactViewModel = CreateContactViewModel();

            // Act
            var mailMessage = contactViewModel.ToMailMessage(CreateContactSettings());

            // Assert
            Assert.Equal(Email, mailMessage.From.Address);
        }

        [Fact]
        public void ToMailMessageReturnsMailMessageWithNameAsSenderName()
        {
            // Arrange
            var contactViewModel = CreateContactViewModel();

            // Act
            var mailMessage = contactViewModel.ToMailMessage(CreateContactSettings());

            // Assert
            Assert.Equal(Name, mailMessage.From.DisplayName);
        }

        [Fact]
        public void ToMailMessageReturnsMailMessageWithContactSettingsToAsReceiver()
        {
            // Arrange
            var contactViewModel = CreateContactViewModel();

            // Act
            var mailMessage = contactViewModel.ToMailMessage(CreateContactSettings());

            // Assert
            Assert.Equal(1, mailMessage.To.Count);
            Assert.Equal(ContactFormTo, mailMessage.To[0].Address);
        }

        [Fact]
        public void ToMailMessageReturnsMailMessageWithContactSettingsSubjectAsSubject()
        {
            // Arrange
            var contactViewModel = CreateContactViewModel();

            // Act
            var mailMessage = contactViewModel.ToMailMessage(CreateContactSettings());

            // Assert;
            Assert.Equal(ContactFormSubject, mailMessage.Subject);
        }

        [Fact]
        public void ToMailMessageWithNullNameThrowsValidationException()
        {
            // Arrange
            var contactViewModel = CreateContactViewModel();
            contactViewModel.Name = null;

            // Act

            // Assert
            Assert.Throws<ValidationException>(() => contactViewModel.ToMailMessage(CreateContactSettings()));
        }

        [Fact]
        public void ToMailMessageWithEmptyNameThrowsValidationException()
        {
            // Arrange
            var contactViewModel = CreateContactViewModel();
            contactViewModel.Name = string.Empty;

            // Act

            // Assert
            Assert.Throws<ValidationException>(() => contactViewModel.ToMailMessage(CreateContactSettings()));
        }

        [Fact]
        public void ToMailMessageWithNullMessageThrowsValidationException()
        {
            // Arrange
            var contactViewModel = CreateContactViewModel();
            contactViewModel.Message = null;

            // Act

            // Assert
            Assert.Throws<ValidationException>(() => contactViewModel.ToMailMessage(CreateContactSettings()));
        }

        [Fact]
        public void ToMailMessageWithEmptyMessageThrowsValidationException()
        {
            // Arrange
            var contactViewModel = CreateContactViewModel();
            contactViewModel.Message = string.Empty;

            // Act

            // Assert
            Assert.Throws<ValidationException>(() => contactViewModel.ToMailMessage(CreateContactSettings()));
        }

        [Fact]
        public void ToMailMessageWithNullEmailThrowsValidationException()
        {
            // Arrange
            var contactViewModel = CreateContactViewModel();
            contactViewModel.Email = null;

            // Act

            // Assert
            Assert.Throws<ValidationException>(() => contactViewModel.ToMailMessage(CreateContactSettings()));
        }

        [Fact]
        public void ToMailMessageWithEmptyEmailThrowsValidationException()
        {
            // Arrange
            var contactViewModel = CreateContactViewModel();
            contactViewModel.Email = string.Empty;

            // Act

            // Assert
            Assert.Throws<ValidationException>(() => contactViewModel.ToMailMessage(CreateContactSettings()));
        }

        [Fact]
        public void ToMailMessageWithInvalidEmailThrowsFormatException()
        {
            // Arrange
            var contactViewModel = CreateContactViewModel();
            contactViewModel.Email = "no email address";

            // Act

            // Assert
            Assert.Throws<FormatException>(() => contactViewModel.ToMailMessage(CreateContactSettings()));
        }

        [Fact]
        public void ToMailMessageWithNullContactSettingsThrowsArgumentNullException()
        {
            // Arrange
            var contactViewModel = CreateContactViewModel();
            ContactSettings nullContactSettings = null;

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => contactViewModel.ToMailMessage(nullContactSettings));
        }

        private static ContactViewModel CreateContactViewModel()
        {
            return new ContactViewModel { Email = Email, Message = Message, Name = Name };
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