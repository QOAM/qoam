namespace RU.Uci.OAMarket.Website.ViewModels.Journals
{
    using PagedList;

    using RU.Uci.OAMarket.Domain;
    using RU.Uci.OAMarket.Domain.Repositories.Filters;

    public class CommentsViewModel : PagedViewModel
    {
        public int Id { get; set; }

        public IPagedList<ScoreCard> CommentedScoreCards { get; set; }

        public ScoreCardFilter ToFilter()
        {
            return new ScoreCardFilter
            {
                JournalId = this.Id,
                RequireComment = true,
                PageNumber = this.Page,
                PageSize = this.PageSize
            };
        }
    }
}