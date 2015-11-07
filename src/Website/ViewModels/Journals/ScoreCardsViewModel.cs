namespace QOAM.Website.ViewModels.Journals
{
    using PagedList;

    using QOAM.Core;
    using QOAM.Core.Repositories.Filters;

    public class ScoreCardsViewModel : PagedViewModel
    {
        public int Id { get; set; }

        public IPagedList<BaseScoreCard> BaseScoreCards { get; set; }
        public IPagedList<ValuationScoreCard> ValuationScoreCards { get; set; }

        public ScoreCardFilter ToFilter()
        {
            return new ScoreCardFilter
                       {
                           JournalId = this.Id,
                           PageNumber = this.Page,
                           PageSize = this.PageSize
                       };
        }
    }
}