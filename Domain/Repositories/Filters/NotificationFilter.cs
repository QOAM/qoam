namespace RU.Uci.OAMarket.Domain.Repositories.Filters
{
    public class NotificationFilter
    {
        public int UserProfileId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}