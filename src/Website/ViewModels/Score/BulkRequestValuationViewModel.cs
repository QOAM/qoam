using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace QOAM.Website.ViewModels.Score
{
    public class BulkRequestValuationViewModel : IFileUploadViewModel
    {
        [Required(ErrorMessage = "Please select a file")]
        public HttpPostedFileBase File { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Email is not valid")]
        public string EmailFrom { get; set; }

        [Required(ErrorMessage = "Please enter a message")]
        [AllowHtml]
        public string EmailBody { get; set; }

        [AllowHtml]
        public string EmailSubject { get; set; }
    }
}