using System.ComponentModel;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using QOAM.Core;
using QOAM.Website.Helpers;
using QOAM.Website.Models;
using Validation;

namespace QOAM.Website.ViewModels.Home
{
    public class ContactViewModel
    {
        [Required]
        [DisplayName("Your name")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [DisplayName("Your email address")]
        public string Email { get; set; }

        [Required]
        [DisplayName("Message")]
        public string Message { get; set; }

        [DisplayName("Include an optional file (xlsx):"), HttpPostedFileExtensions(Extensions = "xlsx,xls")]
        public HttpPostedFileBase File { get; set; }

        public MailMessage ToMailMessage(ContactSettings contactSettings)
        {
            Requires.NotNull(contactSettings, nameof(contactSettings));
            Validator.ValidateObject(this, new ValidationContext(this));

            var mailMessage = new MailMessage(new MailAddress(this.Email, this.Name), new MailAddress(contactSettings.ContactFormTo))
            {
                Body = this.Message,
                Subject = contactSettings.ContactFormSubject
            };

            if(File != null)
                mailMessage.Attachments.Add(new Attachment(File.InputStream, File.FileName));

            return mailMessage;
        }
    }
}