using System.ComponentModel;
using System.Web;

namespace QOAM.Website.ViewModels
{
    public interface IFileUploadViewModel
    {
        [DisplayName("File (xlsx):")]
        HttpPostedFileBase File { get; set; }
    }
}