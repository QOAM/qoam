namespace QOAM.Website.ViewModels.Account
{
    using System.ComponentModel.DataAnnotations;

    public class ChangeEmailViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "New e-mail address")]
        public string NewEmail { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Confirm new e-mail address")]
        [Compare("NewEmail", ErrorMessage = "The new email address and confirmation email address do not match.")]
        public string ConfirmEmail { get; set; }
    }
}