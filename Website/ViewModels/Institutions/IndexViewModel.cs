namespace RU.Uci.OAMarket.Website.ViewModels.Institutions
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Helpers;

    using PagedList;

    using RU.Uci.OAMarket.Domain;
    using RU.Uci.OAMarket.Domain.Repositories.Filters;

    public class IndexViewModel : PagedViewModel
    {
        public IndexViewModel()
        {
            this.SortBy = InstitutionSortMode.NumberOfJournalScoreCards;
            this.Sort = SortDirection.Descending;
        }

        [Display(Name = "Name", ResourceType = typeof(Resources.Strings))]
        public string Name { get; set; }

        public InstitutionSortMode SortBy { get; set; }
        public SortDirection Sort { get; set; }
        
        public IPagedList<Institution> Institutions { get; set; }

        public InstitutionFilter ToFilter()
        {
            return new InstitutionFilter
                       {
                           Name = this.Name,
                           SortMode = this.SortBy,
                           SortDirection = this.Sort,
                           PageNumber = this.Page,
                           PageSize = this.PageSize,
                       };
        }
    }
}