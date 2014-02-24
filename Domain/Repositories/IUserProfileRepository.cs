namespace RU.Uci.OAMarket.Domain.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

    using PagedList;

    using RU.Uci.OAMarket.Domain.Repositories.Filters;

    public interface IUserProfileRepository
    {
        IList<UserProfile> All { get; }

        IQueryable<string> Names(string query);

        UserProfile Find(int id);
        UserProfile Find(string username);

        void Insert(UserProfile userProfile);
        void Save();
        IPagedList<UserProfile> Search(UserProfileFilter filter);
    }
}