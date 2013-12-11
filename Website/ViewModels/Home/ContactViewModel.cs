namespace RU.Uci.OAMarket.Website.ViewModels.Home
{
    using System.ComponentModel.DataAnnotations;
    using System.Net.Mail;

    using RU.Uci.OAMarket.Website.Models;
    using RU.Uci.OAMarket.Website.Resources;

    using Validation;

    public class ContactViewModel
    {
        [Required]
        [Display(Name = "Name", ResourceType = typeof(Strings))]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email", ResourceType = typeof(Strings))]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Message", ResourceType = typeof(Strings))]
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