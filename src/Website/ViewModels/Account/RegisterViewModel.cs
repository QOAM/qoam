namespace QOAM.Website.ViewModels.Account
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "User name *"), MaxLength(100)]
        public string UserName { get; set; }

        public string DisplayName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Inst. email address *")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password *")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password *")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "ORCID")]
        public string OrcId { get; set; }

        public DateTime DateRegistered { get; set; }

        public int InstitutionId { get; set; }

        public string AddLink { get; set; }
    }
}