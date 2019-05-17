namespace QOAM.Website.ViewModels.Journals
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Routing;
    using PagedList;

    using Core;
    using Core.Helpers;
    using Core.Repositories.Filters;

    public class IndexViewModel : PagedViewModel
    {
        public const int MinimumScoreValue = 0;
        public const int MaximumScoreValue = 5;

        public IndexViewModel()
        {
            Journals = new PagedList<Journal>(new Journal[0], this.Page, this.PageSize);
            SortBy = JournalSortMode.RobustScores;
            Sort = SortDirection.Descending;
            SwotMatrix = string.Empty;
            Disciplines = new List<SelectListItem>();
            Languages = new List<string>();
            SelectedDisciplines = new List<int>();
        }

        [DisplayName("Title")]
        public string Title { get; set; }

        [DisplayName("ISSN")]
        public string Issn { get; set; }

        [DisplayName("Publisher")]
        public string Publisher { get; set; }

        [DisplayName("Discipline")]
        public IEnumerable<SelectListItem> Disciplines { get; set; }

        public IList<int> SelectedDisciplines { get; set; }
        
        [DisplayName("All journals with Valuation Score Card")]
        public bool SubmittedOnly { get; set; }

        public string SwotMatrix { get; set; } 

        public JournalSortMode SortBy { get; set; }
        public SortDirection Sort { get; set; }

        public IPagedList<Journal> Journals { get; set; }

        [DisplayName("Language")]
        public IList<string> Languages { get; set; }

        public bool? OpenAccess { get; set; }
        public bool? InstitutionalDiscounts { get; set; }
        public bool? InJournalTOCs { get; set; }
        public bool? NoFee { get; set; }

        public JournalFilter ToFilter()
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
                OpenAccess = OpenAccess,
                InstitutionalDiscounts = InstitutionalDiscounts,
                InJournalTOCs = InJournalTOCs,
                NoFee = NoFee
            };
        }

        public UserJournalFilter ToFilter(int userProfileId)
        {
            return new UserJournalFilter
            {
                Title = Title.TrimSafe(),
                Issn = Issn.TrimSafe(),
                Publisher = Publisher.TrimSafe(),
                Disciplines = this.SelectedDisciplines ?? Enumerable.Empty<int>(),
                Languages = this.Languages ?? Enumerable.Empty<string>(),
                SubmittedOnly = SubmittedOnly,
                MustHaveBeenScored = false,
                SortMode = SortBy,
                SortDirection = Sort,
                PageNumber = Page,
                PageSize = PageSize,
                SwotMatrix = !string.IsNullOrEmpty(SwotMatrix) ? SwotMatrix.Split(',').ToList() : new List<string>(),
                UserProfileId = userProfileId,
                OpenAccess = OpenAccess,
                InstitutionalDiscounts = InstitutionalDiscounts,
                InJournalTOCs = InJournalTOCs,
                NoFee = NoFee
            };
        }

        public virtual RouteValueDictionary ToRouteValueDictionary(int page)
        {
            var routeValueDictionary = new RouteValueDictionary
            {
                [nameof(page)] = page,
                [nameof(Title)] = Title,
                [nameof(Issn)] = Issn,
                [nameof(Publisher)] = Publisher,
                [nameof(SubmittedOnly)] = SubmittedOnly,
                [nameof(Sort)] = Sort,
                [nameof(SortBy)] = SortBy,
                [nameof(SwotMatrix)] = SwotMatrix,
                [nameof(OpenAccess)] = OpenAccess,
                [nameof(InstitutionalDiscounts)] = InstitutionalDiscounts,
                [nameof(InJournalTOCs)] = InJournalTOCs,
                [nameof(NoFee)] = NoFee
            };

            for (var i = 0; i < SelectedDisciplines.Count; i++)
            {
                routeValueDictionary[$"SelectedDisciplines[{i}]"] = SelectedDisciplines[i];
            }

            for (var i = 0; i < Languages.Count; i++)
            {
                routeValueDictionary[$"{nameof(Languages)}[{i}]"] = Languages[i];
            }

            return routeValueDictionary;
        }
    }
}