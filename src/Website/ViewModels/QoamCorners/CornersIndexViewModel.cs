using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Routing;
using QOAM.Core;
using QOAM.Core.Helpers;
using QOAM.Core.Repositories.Filters;
using QOAM.Website.ViewModels.Journals;

namespace QOAM.Website.ViewModels.QoamCorners
{
    public class CornersIndexViewModel : IndexViewModel
    {
        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("QOAMcorner")]
        public int? Corner { get; set; }

        public IList<Corner> Corners { get; set; }
        public UserProfile CornerAdmin { get; set; }
        public string CornerName { get; set; }
        public bool IsCornerAdmin { get; set; }

        public override RouteValueDictionary ToRouteValueDictionary(int page)
        {
            var routeValueDictionary = base.ToRouteValueDictionary(page);
            routeValueDictionary.Add(nameof(Corner), Corner);

            return routeValueDictionary;
        }

        public new QoamCornerJournalFilter ToFilter()
        {
            return new QoamCornerJournalFilter
            {
                Title = Title.TrimSafe(),
                Issn = Issn.TrimSafe(),
                Publisher = Publisher.TrimSafe(),
                Disciplines = SelectedDisciplines ?? Enumerable.Empty<int>(),
                Languages = Languages ?? Enumerable.Empty<string>(),
                SubmittedOnly = SubmittedOnly,
                MustHaveBeenScored = !string.IsNullOrEmpty(SwotMatrix),
                SortMode = SortBy,
                SortDirection = Sort,
                PageNumber = Page,
                PageSize = PageSize,
                SwotMatrix = !string.IsNullOrEmpty(SwotMatrix) ? SwotMatrix.Split(',').ToList() : new List<string>(),
                OpenAccess = OpenAccess,
                InstitutionalDiscounts = InstitutionalDiscounts,
                CornerId = Corner,
                InJournalTOCs = InJournalTOCs,
                NoFee = NoFee,
                PlanS = PlanS
            };
        }
    }
}