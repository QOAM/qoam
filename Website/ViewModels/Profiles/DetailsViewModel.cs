namespace QOAM.Website.ViewModels.Profiles
{
    using PagedList;

    using QOAM.Core;
    using QOAM.Core.Repositories.Filters;

    public class DetailsViewModel : PagedViewModel
    {
        public int Id { get; set; }

        public UserProfile UserProfile { get; set; }
        public IPagedList<ScoreCard> ScoreCards { get; set; }
        public ScoreCardStats ScoreCardStats { get; set; }

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