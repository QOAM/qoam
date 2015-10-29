namespace QOAM.Website.ViewModels.Profiles
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
            this.SortBy = UserProfileSortMode.DateRegistered;
            this.Sort = SortDirection.Descending;
        }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Institution")]
        public int? Institution { get; set; }

        public UserProfileSortMode SortBy { get; set; }

        public SortDirection Sort { get; set; }

        public IPagedList<UserProfile> Profiles { get; set; }

        public IEnumerable<SelectListItem> Institutions { get; set; }
        public int NumberOfBaseScoreCards { get; set; }
        public int NumberOfValuationScoreCards { get; set; }

        public UserProfileFilter ToFilter()
        {
            return new UserProfileFilter
                   {
                       Name = this.Name.TrimSafe(),
                       InstitutionId = this.Institution,
                       SortMode = this.SortBy,
                       SortDirection = this.Sort,
                       PageNumber = this.Page,
                       PageSize = this.PageSize,
                   };
        }
    }
}