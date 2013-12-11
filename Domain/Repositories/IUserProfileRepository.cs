namespace RU.Uci.OAMarket.Domain.Repositories
{
    using System.Collections.Generic;

    using PagedList;

    using RU.Uci.OAMarket.Domain.Repositories.Filters;

    public interface IUserProfileRepository
    {
        IList<UserProfile> All { get; }
        IList<string> AllNames { get; }

        UserProfile Find(int id);
        UserProfile Find(string username);

        void Insert(UserProfile userProfile);
        void Save();
        IPagedList<UserProfile> Search(UserProfileFilter filter);
    }
}