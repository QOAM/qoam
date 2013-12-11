namespace RU.Uci.OAMarket.Data
{
    using System;
    using System.Data.Entity;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Helpers;

    using PagedList;

    using RU.Uci.OAMarket.Domain;
    using RU.Uci.OAMarket.Domain.Repositories;
    using RU.Uci.OAMarket.Domain.Repositories.Filters;

    public class UserProfileRepository : Repository, IUserProfileRepository
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

        public IList<string> AllNames
        {
            get
            {
                return this.DbContext.UserProfiles.Select(j => j.DisplayName).ToList();
            }
        }

        public UserProfile Find(int id)
        {
            return this.DbContext.UserProfiles.Find(id);
        }

        public UserProfile Find(string username)
        {
            return this.DbContext.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == username.ToLower());
        }

        public void Insert(UserProfile userProfile)
        {
            this.DbContext.UserProfiles.Add(userProfile);
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

        private static IOrderedQueryable<UserProfile> ApplyOrdering(IQueryable<UserProfile> query, UserProfileFilter filter)
        {
            switch (filter.SortMode)
            {
                case UserProfileSortMode.Name:
                    return filter.SortDirection == SortDirection.Ascending ? query.OrderBy(u => u.DisplayName) : query.OrderByDescending(u => u.DisplayName);
                case UserProfileSortMode.DateRegistered:
                    return filter.SortDirection == SortDirection.Ascending ? query.OrderBy(u => u.DateRegistered) : query.OrderByDescending(u => u.DateRegistered);
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