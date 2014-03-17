namespace QOAM.Website.ViewModels.Journals
{
    using PagedList;

    using QOAM.Core;
    using QOAM.Core.Repositories.Filters;

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