using System.ComponentModel.DataAnnotations;
using System.Web;
using QOAM.Website.Helpers;

namespace QOAM.Website.ViewModels.QoamCorners
{
    public class CornerImportViewModel : IFileUploadViewModel
    {
        [Required(ErrorMessage = "Please select a file"), HttpPostedFileExtensions(Extensions = "xlsx", ErrorMessage = "The selected file does not contain valid QOAMcorner information!")]
        public HttpPostedFileBase File { get; set; }
    }
}