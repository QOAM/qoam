namespace RU.Uci.OAMarket.Website.ViewModels.Institutions
{
    using System.Web.Helpers;

    using PagedList;

    using RU.Uci.OAMarket.Domain;
    using RU.Uci.OAMarket.Domain.Repositories.Filters;

    public class DetailsViewModel : PagedViewModel
    {
        public DetailsViewModel()
        {
            this.SortBy = UserProfileSortMode.NumberOfJournalScoreCards;
            this.Sort = SortDirection.Descending;
        }

        public int Id { get; set; }

        public Institution Institution { get; set; }
        public IPagedList<UserProfile> UserProfiles { get; set; }
        public ScoreCardStats ScoreCardStats { get; set; }

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