namespace QOAM.Website.ViewModels.Import
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using Core.Helpers;

    public class MoveScoreCardsViewModel
    {
        [Required]
        [RegularExpression(StringExtensions.IssnRegexPattern, ErrorMessage = "Invalid ISSN format.")]
        [DisplayName("Old ISSN")]
        public string OldIssn { get; set; }

        [Required]
        [RegularExpression(StringExtensions.IssnRegexPattern, ErrorMessage = "Invalid ISSN format.")]
        [DisplayName("New ISSN")]
        public string NewIssn { get; set; }
    }
}