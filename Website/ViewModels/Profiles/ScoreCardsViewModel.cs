namespace RU.Uci.OAMarket.Website.ViewModels.Profiles
{
    using RU.Uci.OAMarket.Domain;
    using RU.Uci.OAMarket.Domain.Repositories.Filters;

    public class ScoreCardsViewModel : PagedViewModel
    {
        public int Id { get; set; }
        
        public ScoreCardFilter ToScoreCardFilter(ScoreCardState? state)
        {
            return new ScoreCardFilter
                       {
                           UserProfileId = this.Id,
                           PageNumber = this.Page,
                           PageSize = this.PageSize,
                           State = state
                       };
        }
    }
}