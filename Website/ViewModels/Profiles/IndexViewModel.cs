namespace RU.Uci.OAMarket.Website.ViewModels.Profiles
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Helpers;
    using System.Web.Mvc;

    using PagedList;

    using RU.Uci.OAMarket.Domain;
    using RU.Uci.OAMarket.Domain.Repositories.Filters;

    public class IndexViewModel : PagedViewModel
    {
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