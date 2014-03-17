namespace QOAM.Website.ViewModels.Profiles
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
        public IndexViewModel()
        {
            this.SortBy = UserProfileSortMode.NumberOfJournalScoreCards;
            this.Sort = SortDirection.Descending;
        }

        [Display(Name = "Name", ResourceType = typeof(Resources.Strings))]
        public string Name { get; set; }

        [Display(Name = "Institution", ResourceType = typeof(Resources.Strings))]
        public int? Institution { get; set; }

        public UserProfileSortMode SortBy { get; set; }
        public SortDirection Sort { get; set; }
        
        public IPagedList<UserProfile> Profiles { get; set; }
        public IEnumerable<SelectListItem> Institutions { get; set; }

        public UserProfileFilter ToFilter()
        {
            return new UserProfileFilter
                       {
                           Name = this.Name,
                           InstitutionId = this.Institution,
                           SortMode = this.SortBy,
                           SortDirection = this.Sort,
                           PageNumber = this.Page,
                           PageSize = this.PageSize,
                       };
        }
    }
}