namespace QOAM.Core.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Web.Helpers;
    using EntityFramework.Extensions;
    using PagedList;

    using QOAM.Core.Repositories.Filters;

    public class UserProfileRepository : Repository<UserProfile>, IUserProfileRepository
    {
        public UserProfileRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        public IList<UserProfile> All
        {
            get
            {
                return this.DbContext.UserProfiles.ToList();
            }
        }

        public IQueryable<string> Names(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return Enumerable.Empty<string>().AsQueryable();
            } 

            return this.DbContext.UserProfiles.Where(u => u.DisplayName.ToLower().StartsWith(query.ToLower())).Select(j => j.DisplayName);
        }

        public UserProfile Find(int id)
        {
            return this.DbContext.UserProfiles.Find(id);
        }

        public UserProfile Find(string username)
        {
            return this.DbContext.UserProfiles.FirstOrDefault(u => u.UserName == username);
        }

        public IPagedList<UserProfile> Search(UserProfileFilter filter)
        {
            var query = this.DbContext.UserProfiles.Include(j => j.Institution);

            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                query = query.Where(u => u.DisplayName.ToLower().Contains(filter.Name.ToLower()));
            }

            if (filter.InstitutionId.HasValue)
            {
                query = query.Where(u => u.InstitutionId == filter.InstitutionId);
            }

            return ApplyOrdering(query, filter).ToPagedList(filter.PageNumber, filter.PageSize);
        }

        public UserProfile FindByEmail(string email)
        {
            return this.DbContext.UserProfiles.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
        }

        public void UpdateProviderUserId(UserProfile userProfile, string providerUserId)
        {
            userProfile.UserName = providerUserId;

            this.DbContext.Database.ExecuteSqlCommand("UPDATE [dbo].[webpages_OAuthMembership] SET [ProviderUserId] = @providerUserId WHERE [UserId] = @userId", new SqlParameter("@providerUserId", providerUserId), new SqlParameter("@userId", userProfile.Id));
        }

        public void RemoveInactive(TimeSpan toBeRemovedWindow)
        {
            var toBeRemovedDate = DateTime.Now - toBeRemovedWindow;

            this.DbContext.UserProfiles
                .Where(u => !u.BaseScoreCards.Any(b => b.State == ScoreCardState.Published || b.State == ScoreCardState.Archived))
                .Where(u => !u.ValuationScoreCards.Any(b => b.State == ScoreCardState.Published || b.State == ScoreCardState.Archived))
                .Where(u => !u.JournalPrices.Any())
                .Where(u => !u.InstitutionJournalPrices.Any())
                .Where(u => (u.DateLastLogin.HasValue && u.DateLastLogin < toBeRemovedDate) || (u.DateLastLogin == null && u.DateRegistered < toBeRemovedDate))
                .Delete();
        }

        private static IOrderedQueryable<UserProfile> ApplyOrdering(IQueryable<UserProfile> query, UserProfileFilter filter)
        {
            switch (filter.SortMode)
            {
                case UserProfileSortMode.Name:
                    return filter.SortDirection == SortDirection.Ascending ? query.OrderBy(u => u.DisplayName) : query.OrderByDescending(u => u.DisplayName);
                case UserProfileSortMode.DateRegistered:
                    return filter.SortDirection == SortDirection.Ascending ? query.OrderBy(u => u.DateRegistered) : query.OrderByDescending(u => u.DateRegistered);
                case UserProfileSortMode.NumberOfBaseJournalScoreCards:
                    return filter.SortDirection == SortDirection.Ascending ?
                        query.OrderBy(u => u.NumberOfBaseScoreCards).ThenBy(u => u.DisplayName) :
                        query.OrderByDescending(u => u.NumberOfBaseScoreCards).ThenBy(u => u.DisplayName);
                case UserProfileSortMode.NumberOfValuationJournalScoreCards:
                    return filter.SortDirection == SortDirection.Ascending ?
                        query.OrderBy(u => u.NumberOfValuationScoreCards).ThenBy(u => u.DisplayName) :
                        query.OrderByDescending(u => u.NumberOfValuationScoreCards).ThenBy(u => u.DisplayName);
                case UserProfileSortMode.Institution:
                    return filter.SortDirection == SortDirection.Ascending ?
                        query.OrderBy(u => u.Institution.Name).ThenBy(u => u.DisplayName) :
                        query.OrderByDescending(u => u.Institution.Name).ThenBy(u => u.DisplayName);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}