namespace QOAM.Website.ViewModels.Journals
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Web.Helpers;
    using System.Web.Mvc;

    using PagedList;

    using QOAM.Core;
    using QOAM.Core.Helpers;
    using QOAM.Core.Repositories.Filters;

    public class IndexViewModel : PagedViewModel
    {
        public const int MinimumScoreValue = 0;
        public const int MaximumScoreValue = 5;

        public IndexViewModel()
        {
            this.Journals = new PagedList<Journal>(new Journal[0], this.Page, this.PageSize);
            this.SortBy = JournalSortMode.BaseScore;
            this.Sort = SortDirection.Descending;
            this.SwotMatrix = string.Empty;
        }

        [DisplayName("Title")]
        public string Title { get; set; }

        [DisplayName("ISSN")]
        public string Issn { get; set; }

        [DisplayName("Publisher")]
        public string Publisher { get; set; }

        [DisplayName("Discipline")]
        public int? Discipline { get; set; }

        [DisplayName("Language")]
        public int? Language { get; set; }

        [DisplayName("All journals with Valuation Score Card")]
        public bool SubmittedOnly { get; set; }

        public string SwotMatrix { get; set; } 

        public JournalSortMode SortBy { get; set; }
        public SortDirection Sort { get; set; }

        public IPagedList<Journal> Journals { get; set; }
        public IEnumerable<SelectListItem> Disciplines { get; set; }
        public IEnumerable<SelectListItem> Languages { get; set; }

        public JournalFilter ToFilter()
        {
            return new JournalFilter
                       {
                           Title = this.Title.TrimSafe(),
                           Issn = this.Issn.TrimSafe(),
                           Publisher = this.Publisher.TrimSafe(),
                           Discipline = this.Discipline,
                           Language = this.Language,
                           SubmittedOnly = this.SubmittedOnly,
                           MustHaveBeenScored = true,
                           SortMode = this.SortBy,
                           SortDirection = this.Sort,
                           PageNumber = this.Page,
                           PageSize = this.PageSize,
                           SwotMatrix = !string.IsNullOrEmpty(this.SwotMatrix) ? this.SwotMatrix.Split(',').ToList() : new List<string>()
                       };
        }

        public UserJournalFilter ToFilter(int userProfileId)
        {
            return new UserJournalFilter
            {
                Title = Title.TrimSafe(),
                Issn = Issn.TrimSafe(),
                Publisher = Publisher.TrimSafe(),
                Discipline = Discipline,
                Language = Language,
                SubmittedOnly = SubmittedOnly,
                MustHaveBeenScored = false,
                SortMode = SortBy,
                SortDirection = Sort,
                PageNumber = Page,
                PageSize = PageSize,
                SwotMatrix = !string.IsNullOrEmpty(SwotMatrix) ? SwotMatrix.Split(',').ToList() : new List<string>(),
                UserProfileId = userProfileId
            };
        }
    }
}