using System.ComponentModel.DataAnnotations;
using System.Web;
using QOAM.Website.Helpers;

namespace QOAM.Website.ViewModels.Admin
{
    public class ImportJournalRelatedLinksViewModel : IFileUploadViewModel
    {
        [Required(ErrorMessage = "Please select a file"), HttpPostedFileExtensions(Extensions = "xlsx", ErrorMessage = "The selected file does not contain valid license information!")]
        public HttpPostedFileBase File { get; set; }
    }
}