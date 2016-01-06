using System.Linq;

namespace QOAM.Website.ViewModels.Score
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Web.Helpers;
    using System.Web.Mvc;

    using PagedList;

    using QOAM.Core;
    using QOAM.Core.Helpers;
    using QOAM.Core.Repositories.Filters;

    public class IndexViewModel : PagedViewModel
    {
        public IndexViewModel()
        {
            this.Journals = new PagedList<Journal>(new Journal[0], this.Page, this.PageSize);
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

        public JournalSortMode SortBy { get; set; }

        public SortDirection Sort { get; set; }

        public IPagedList<Journal> Journals { get; set; }

        public IEnumerable<SelectListItem> Disciplines { get; set; }

        public IEnumerable<SelectListItem> Languages { get; set; }

        public IEnumerable<int> JournalIdsInMyQOAM { get; set; }

        public JournalFilter ToFilter()
        {
            return new JournalFilter
                   {
                       Title = this.Title.TrimSafe(), 
                       Issn = this.Issn.TrimSafe(), 
                       Publisher = this.Publisher.TrimSafe(), 
                       Discipline = this.Discipline, 
                       Language = this.Language, 
                       SortMode = this.SortBy, 
                       SortDirection = this.Sort, 
                       PageNumber = this.Page, 
                       PageSize = this.PageSize,
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
                SortMode = SortBy,
                SortDirection = Sort,
                PageNumber = Page,
                PageSize = PageSize,
                UserProfileId = userProfileId
            };
        }

        public bool IsInMyQOAM(int journalId)
        {
            return JournalIdsInMyQOAM.Any(id => id == journalId);
        }
    }
}