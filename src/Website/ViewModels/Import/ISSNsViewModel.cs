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

    public class ProcessNoFeeLabelViewModel : ISSNsViewModel
    {
        public bool AddNoFeeLabel { get; set; }
    }
}