namespace QOAM.Website.ViewModels.Home
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Net.Mail;

    using QOAM.Website.Models;

    using Validation;

    public class ContactViewModel
    {
        [Required]
        [DisplayName("Name")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [DisplayName("Email")]
        public string Email { get; set; }

        [Required]
        [DisplayName("Message")]
        public string Message { get; set; }

        public MailMessage ToMailMessage(ContactSettings contactSettings)
        {
            Requires.NotNull(contactSettings, "contactSettings");
            Validator.ValidateObject(this, new ValidationContext(this));

            return new MailMessage(new MailAddress(this.Email, this.Name), new MailAddress(contactSettings.ContactFormTo))
                       {
                           Body = this.Message,
                           Subject = contactSettings.ContactFormSubject
                       };
        }
    }
}