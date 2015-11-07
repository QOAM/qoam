namespace QOAM.Website.ViewModels.Account
{
    using System.ComponentModel.DataAnnotations;

    public class ResetPasswordViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string Email { get; set; }
    }
}