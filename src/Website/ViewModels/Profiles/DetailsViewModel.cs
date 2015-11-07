namespace QOAM.Website.ViewModels.Profiles
{
    using PagedList;

    using QOAM.Core;
    using QOAM.Core.Repositories.Filters;

    public class DetailsViewModel : PagedViewModel
    {
        public int Id { get; set; }

        public UserProfile UserProfile { get; set; }
        public IPagedList<BaseScoreCard> BaseScoreCards { get; set; }
        public ScoreCardStats BaseScoreCardStats { get; set; }
        public IPagedList<ValuationScoreCard> ValuationScoreCards { get; set; }
        public ScoreCardStats ValuationScoreCardStats { get; set; }

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