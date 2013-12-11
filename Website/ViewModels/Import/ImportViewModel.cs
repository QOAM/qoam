namespace RU.Uci.OAMarket.Website.ViewModels.Import
{
    using System.ComponentModel.DataAnnotations;

    using RU.Uci.OAMarket.Domain.Import;

    public class ImportViewModel
    {
        public ImportViewModel()
        {
            this.Source = JournalsImportSource.Ulrichs;
        }

        [Display(Name = "ISSNs", ResourceType = typeof(Resources.Strings))]
        [Required]
        public string ISSNs { get; set; }

        [Display(Name = "Source", ResourceType = typeof(Resources.Strings))]
        public JournalsImportSource Source { get; set; }
    }
}