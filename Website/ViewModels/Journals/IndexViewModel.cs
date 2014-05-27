namespace QOAM.Website.ViewModels.Journals
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Helpers;
    using System.Web.Mvc;

    using PagedList;

    using QOAM.Core;
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

        [DisplayName("Minimum Base Score")]
        [Range(MinimumScoreValue, MaximumScoreValue)]
        public float? MinimumBaseScore { get; set; }

        [DisplayName("Minimum Valuation Score")]
        [Range(MinimumScoreValue, MaximumScoreValue)]
        public float? MinimumValuationScore { get; set; }

        [DisplayName("Only Journal Score Cards of authors and editors")]
        public bool SubmittedOnly { get; set; }

        public JournalSortMode SortBy { get; set; }
        public SortDirection Sort { get; set; }

        public IPagedList<Journal> Journals { get; set; }
        public IEnumerable<SelectListItem> Disciplines { get; set; }
        public IEnumerable<SelectListItem> Languages { get; set; }

        public JournalFilter ToFilter()
        {
            return new JournalFilter
                       {
                           Title = this.Title,
                           Issn = this.Issn,
                           Publisher = this.Publisher,
                           Discipline = this.Discipline,
                           Language = this.Language,
                           MinimumBaseScore = this.MinimumBaseScore,
                           MinimumValuationScore = this.MinimumValuationScore,
                           SubmittedOnly = this.SubmittedOnly,
                           MustHaveBeenScored = true,
                           SortMode = this.SortBy,
                           SortDirection = this.Sort,
                           PageNumber = this.Page,
                           PageSize = this.PageSize,
                       };
        }
    }
}