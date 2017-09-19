using System.Web;
using QOAM.Website.Helpers;

namespace QOAM.Website.ViewModels.Institutions
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using QOAM.Core;

    public class UpsertViewModel : IFileUploadViewModel
    {
        public int? Id { get; set; }
        public int NumberOfBaseScoreCards { get; set; }
        public int NumberOfScoreCards { get; set; }
        public int NumberOfValuationScoreCards { get; set; }

        [Required]
        [StringLength(1000)]
        public string Name { get; set; }

        [Required]
        [StringLength(1000)]
        [DisplayName("Domainname")]
        public string ShortName { get; set; }

        [Required(ErrorMessage = "Please select a file"), HttpPostedFileExtensions(Extensions = "xlsx", ErrorMessage = "The selected file does not contain valid institution information!")]
        public HttpPostedFileBase File { get; set; }

        public Institution ToInstitution()
        {
            return new Institution
            {
                Id = Id.GetValueOrDefault(),
                Name = this.Name, 
                ShortName = this.ShortName.Replace("http://", string.Empty).Replace("www.", string.Empty),
                NumberOfBaseScoreCards = NumberOfBaseScoreCards,
                NumberOfScoreCards = NumberOfScoreCards,
                NumberOfValuationScoreCards = NumberOfValuationScoreCards
            };
        }
    }
}