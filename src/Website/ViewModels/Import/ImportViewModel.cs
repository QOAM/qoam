namespace QOAM.Website.ViewModels.Import
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using Core.Import;

    public class ImportViewModel
    {
        [DisplayName("ISSNs")]
        [Required]
        public string ISSNs { get; set; }

        [DisplayName("Source")]
        public JournalsImportSource Source => JournalsImportSource.JournalTOCs;
    }
}