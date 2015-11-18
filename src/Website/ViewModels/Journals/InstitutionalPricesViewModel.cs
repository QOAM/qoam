using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace QOAM.Website.ViewModels.Journals
{
    public class InstitutionalPricesViewModel
    {
        [Required, FileExtensions(Extensions = "xslx", ErrorMessage = "The selected file does not conatin valid license information!")]
        [DisplayName("Select a file to import (xlsx)")]
        public HttpPostedFileBase File { get; set; }
    }
}