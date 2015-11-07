namespace QOAM.Website.ViewModels.Journals
{
    using PagedList;

    using QOAM.Core;
    using QOAM.Core.Repositories.Filters;

    public class CommentsViewModel : PagedViewModel
    {
        public int Id { get; set; }

        public IPagedList<BaseScoreCard> CommentedBaseScoreCards { get; set; }
        public IPagedList<ValuationScoreCard> CommentedValuationScoreCards { get; set; }

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