namespace RU.Uci.OAMarket.Domain.Repositories.Filters
{
    using System.Web.Helpers;

    public class UserProfileFilter
    {
        public string Name { get; set; }
        public int? InstitutionId { get; set; }
        public UserProfileSortMode SortMode { get; set; }
        public SortDirection SortDirection { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}