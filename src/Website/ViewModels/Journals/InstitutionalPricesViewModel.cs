namespace QOAM.Website.ViewModels.Journals
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    public class InstitutionalPricesViewModel : IFileUploadViewModel
    {
        [Required(ErrorMessage = "Please select a file"), FileExtensions(Extensions = "xslx", ErrorMessage = "The selected file does not contain valid license information!")]
        public HttpPostedFileBase File { get; set; }
    }
}