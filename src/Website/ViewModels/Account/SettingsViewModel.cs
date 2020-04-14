namespace QOAM.Website.ViewModels.Account
{
    using System.ComponentModel.DataAnnotations;

    public class SettingsViewModel
    {
        [Required]
        [Display(Name = "Display name"), MaxLength(100)]
        public string DisplayName { get; set; }

        [Display(Name = "ORCID")]
        public string OrcId { get; set; }
    }
}