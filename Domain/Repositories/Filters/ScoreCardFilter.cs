namespace RU.Uci.OAMarket.Domain.Repositories.Filters
{
    public class ScoreCardFilter
    {
        public int? JournalId { get; set; }
        public int? UserProfileId { get; set; }
        public ScoreCardState? State { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public bool RequireComment { get; set; }
    }
}