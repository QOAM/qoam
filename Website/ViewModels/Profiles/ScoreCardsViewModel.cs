namespace QOAM.Website.ViewModels.Profiles
{
    using QOAM.Core;
    using QOAM.Core.Repositories.Filters;

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