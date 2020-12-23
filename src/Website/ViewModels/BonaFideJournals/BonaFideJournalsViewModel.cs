using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using QOAM.Core;
using QOAM.Core.Helpers;
using QOAM.Core.Repositories.Filters;
using QOAM.Website.ModelExtensions;
using QOAM.Website.ViewModels.Journals;

namespace QOAM.Website.ViewModels.BonaFideJournals
{
    public class BonaFideJournalsViewModel : IndexViewModel
    {
        public bool? Blue { get; set; }
        public bool? Lightblue { get; set; }
        public bool? Grey { get; set; }
        
        public override string JournalLinkCssClass(Journal journal)
        {
            if(journal.NoFee || journal.IsIncludedInDoaj() || journal.InstitutionJournalPrices.Any() || journal.TrustingInstitutions.Count >= 3)
                return "";

            if (journal.TrustingInstitutions.Count >= 1)
                return "lightblue";

            return "grey";
        }
        
        public new JournalFilter ToFilter()
        {
            return new JournalFilter
            {
                Title = this.Title.TrimSafe(),
                Issn = this.Issn.TrimSafe(),
                Publisher = this.Publisher.TrimSafe(),
                Disciplines = this.SelectedDisciplines ?? Enumerable.Empty<int>(),
                Languages = this.Languages ?? Enumerable.Empty<string>(),
                SubmittedOnly = this.SubmittedOnly,
                MustHaveBeenScored = !string.IsNullOrEmpty(this.SwotMatrix),
                SortMode = this.SortBy,
                SortDirection = this.Sort,
                PageNumber = this.Page,
                PageSize = this.PageSize,
                SwotMatrix = !string.IsNullOrEmpty(this.SwotMatrix) ? this.SwotMatrix.Split(',').ToList() : new List<string>(),
                Blue = Blue,
                Lightblue = Lightblue,
                Grey = Grey
            };
        }
        
        public override RouteValueDictionary ToRouteValueDictionary(int page)
        {
            var routeValueDictionary = base.ToRouteValueDictionary(page);
            routeValueDictionary.Add(nameof(Blue), Blue);
            routeValueDictionary.Add(nameof(Lightblue), Lightblue);
            routeValueDictionary.Add(nameof(Grey), Grey);

            return routeValueDictionary;
        }
    }
}