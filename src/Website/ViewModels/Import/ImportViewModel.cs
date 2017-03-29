namespace QOAM.Website.ViewModels.Import
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using QOAM.Core.Import;

    public class ImportViewModel
    {
        public ImportViewModel()
        {
            //this.Source = JournalsImportSource.Ulrichs;
        }

        [DisplayName("ISSNs")]
        [Required]
        public string ISSNs { get; set; }

        [DisplayName("Source")]
        public JournalsImportSource Source => JournalsImportSource.JournalTOCs;
    }
}