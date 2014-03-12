namespace RU.Uci.OAMarket.Domain.Repositories.Filters
{
    using System.Web.Helpers;

    public class InstitutionFilter
    {
        public string Name { get; set; }
        public InstitutionSortMode SortMode { get; set; }
        public SortDirection SortDirection { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}