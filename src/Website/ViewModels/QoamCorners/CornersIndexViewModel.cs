using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using System.Web.Routing;
using PagedList;
using QOAM.Core;

namespace QOAM.Website.ViewModels.QoamCorners
{
    public class CornersIndexViewModel : PagedViewModel
    {
        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("QOAMcorner")]
        public int? Corner { get; set; }

        public IEnumerable<SelectListItem> Corners { get; set; }
        public IPagedList<Journal> Journals { get; set; }
        public UserProfile CornerAdmin { get; set; }

        public RouteValueDictionary ToRouteValueDictionary(int page)
        {
            var routeValueDictionary = new RouteValueDictionary
            {
                [nameof(page)] = page
            };

            return routeValueDictionary;
        }
    }
}