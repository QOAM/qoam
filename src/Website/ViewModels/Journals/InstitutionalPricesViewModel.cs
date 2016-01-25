using QOAM.Website.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace QOAM.Website.ViewModels.Journals
{
    public class InstitutionalPricesViewModel : IFileUploadViewModel
    {
        [Required(ErrorMessage = "Please select a file"), HttpPostedFileExtensions(Extensions = "xlsx,xsl", ErrorMessage = "The selected file does not contain valid license information!")]
        public HttpPostedFileBase File { get; set; }
    }
}