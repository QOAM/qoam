namespace QOAM.Website.ViewModels.Import
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class DeleteViewModel
    {
        [DisplayName("ISSNs")]
        [Required]
        public string ISSNs { get; set; }
    }
}