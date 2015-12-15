using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace QOAM.Website.ViewModels.Score
{
    public class BulkRequestValuationViewModel : IFileUploadViewModel
    {
        [Required(ErrorMessage = "Please select a file"), FileExtensions(Extensions = "xslx", ErrorMessage = "The selected file does not contain valid author information!")]
        public HttpPostedFileBase File { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Email is not valid")]
        public string EmailFrom { get; set; }

        [Required(ErrorMessage = "Please enter a message")]
        [AllowHtml]
        public string EmailBody { get; set; }

        [AllowHtml]
        public string EmailSubject { get; set; }
    }
}