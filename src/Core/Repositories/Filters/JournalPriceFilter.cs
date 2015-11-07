namespace QOAM.Core.Repositories.Filters
{
    public class JournalPriceFilter
    {
        public int? JournalId { get; set; }
        public int? UserProfileId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public FeeType? FeeType { get; set; }
    }
}