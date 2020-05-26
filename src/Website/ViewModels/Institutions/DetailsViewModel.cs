namespace QOAM.Website.ViewModels.Institutions
{
    using System.Web.Helpers;

    using PagedList;

    using QOAM.Core;
    using QOAM.Core.Repositories.Filters;

    public class DetailsViewModel : PagedViewModel
    {
        public DetailsViewModel()
        {
            this.SortBy = UserProfileSortMode.Name;//.NumberOfBaseJournalScoreCards;
            this.Sort = SortDirection.Descending;
        }

        public int Id { get; set; }

        public Institution Institution { get; set; }
        public IPagedList<UserProfile> UserProfiles { get; set; }
        public ScoreCardStats BaseScoreCardStats { get; set; }
        public ScoreCardStats ValuationScoreCardStats { get; set; }
        public UserProfileSortMode SortBy { get; set; }
        public SortDirection Sort { get; set; }

        public UserProfileFilter ToUserProfileFilter()
        {
            return new UserProfileFilter
            {
                InstitutionId = this.Institution.Id,
                SortMode = this.SortBy,
                SortDirection = this.Sort,
                PageNumber = this.Page,
                PageSize = this.PageSize,
            };
        }
    }
}