namespace QOAM.Website.ViewModels.Institutions
{
    using System.ComponentModel;
    using System.Web.Helpers;

    using PagedList;

    using QOAM.Core;
    using QOAM.Core.Repositories.Filters;

    public class IndexViewModel : PagedViewModel
    {
        public IndexViewModel()
        {
            this.SortBy = InstitutionSortMode.NumberOfJournalScoreCards;
            this.Sort = SortDirection.Descending;
        }

        [DisplayName("Name")]
        public string Name { get; set; }

        public InstitutionSortMode SortBy { get; set; }

        public SortDirection Sort { get; set; }

        public IPagedList<Institution> Institutions { get; set; }

        public InstitutionFilter ToFilter()
        {
            return new InstitutionFilter { Name = this.Name, SortMode = this.SortBy, SortDirection = this.Sort, PageNumber = this.Page, PageSize = this.PageSize, };
        }
    }
}