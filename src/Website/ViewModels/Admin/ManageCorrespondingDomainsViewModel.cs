using System.Collections.Generic;
using System.Web.Mvc;
using QOAM.Core;

namespace QOAM.Website.ViewModels.Admin
{
    public class ManageCorrespondingDomainsViewModel
    {
        public int? InstitutionId { get; set; }
        public IEnumerable<SelectListItem> Institutions { get; set; }
        public List<Institution> CorrespondingInstitutions { get; set; } = new List<Institution>();
    }
}