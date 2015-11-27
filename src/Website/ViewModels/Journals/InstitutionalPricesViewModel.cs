namespace QOAM.Website.ViewModels.Journals
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    public class InstitutionalPricesViewModel
    {
        [Required, FileExtensions(Extensions = "xslx", ErrorMessage = "The selected file does not contain valid license information!")]
        [DisplayName("Select a file to import (xlsx)")]
        public HttpPostedFileBase File { get; set; }
    }
}