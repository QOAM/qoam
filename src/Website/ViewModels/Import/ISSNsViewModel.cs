namespace QOAM.Website.ViewModels.Import
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class ISSNsViewModel
    {
        [DisplayName("ISSNs")]
        [Required]
        public string ISSNs { get; set; }
    }

    public class ProcessJournalLabelViewModel : ISSNsViewModel
    {
        public string LabelType { get; set; }
        public bool AddJournalLabel { get; set; }
        public string ActionMethod { get; set; }
    }
}