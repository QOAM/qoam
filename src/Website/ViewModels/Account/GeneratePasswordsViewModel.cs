namespace QOAM.Website.ViewModels.Account
{
    using System.ComponentModel.DataAnnotations;

    public class GeneratePasswordsViewModel
    {
        [Required]
        public bool Confirm { get; set; }
    }
}