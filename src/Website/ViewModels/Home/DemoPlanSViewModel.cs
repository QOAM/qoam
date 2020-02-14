using System.Collections.Generic;
using System.Web.Mvc;

namespace QOAM.Website.ViewModels.Home
{
    public class DemoPlanSViewModel
    {
        public int? InstitutionId { get; set; }
        public IEnumerable<SelectListItem> Institutions { get; set; }
    }
}