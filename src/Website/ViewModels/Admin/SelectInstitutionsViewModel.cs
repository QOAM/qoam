using System.Collections.Generic;
using System.Web.Mvc;

namespace QOAM.Website.ViewModels.Admin
{
    public class SelectInstitutionsViewModel
    {
        public List<int> SelectedIntitutionIds { get; set; }
        public IEnumerable<SelectListItem> Institutions { get; set; }
    }
}