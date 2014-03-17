namespace QOAM.Website.ViewModels.Journals
{
    using PagedList;

    using QOAM.Core;
    using QOAM.Core.Repositories.Filters;

    public class ScoresViewModel : PagedViewModel
    {
        public int Id { get; set; }

        public IPagedList<ScoreCard> ScoreCards { get; set; }

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