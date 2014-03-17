namespace QOAM.Website.ViewModels.Journals
{
    using System.Collections.Generic;
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

        [Display(Name = "Title", ResourceType = typeof(Resources.Strings))]
        public string Title { get; set; }

        [Display(Name = "ISSN", ResourceType = typeof(Resources.Strings))]
        public string Issn { get; set; }

        [Display(Name = "Publisher", ResourceType = typeof(Resources.Strings))]
        public string Publisher { get; set; }

        [Display(Name = "Discipline", ResourceType = typeof(Resources.Strings))]
        public int? Discipline { get; set; }

        [Display(Name = "Language", ResourceType = typeof(Resources.Strings))]
        public int? Language { get; set; }

        [Display(Name = "MinimumBaseScore", ResourceType = typeof(Resources.Strings))]
        [Range(MinimumScoreValue, MaximumScoreValue)]
        public float? MinimumBaseScore { get; set; }

        [Display(Name = "MinimumValuationScore", ResourceType = typeof(Resources.Strings))]
        [Range(MinimumScoreValue, MaximumScoreValue)]
        public float? MinimumValuationScore { get; set; }

        [Display(Name = "SubmittedOnly", ResourceType = typeof(Resources.Strings))]
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