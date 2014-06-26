namespace QOAM.Core.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

    using PagedList;

    using QOAM.Core.Repositories.Filters;

    public interface IUserProfileRepository
    {
        IList<UserProfile> All { get; }

        IQueryable<string> Names(string query);

        UserProfile Find(int id);
        UserProfile Find(string username);

        void InsertOrUpdate(UserProfile userProfile);
        void Save();
        IPagedList<UserProfile> Search(UserProfileFilter filter);

        UserProfile FindByEmail(string email);

        void UpdateProviderUserId(UserProfile userProfile, string providerUserId);
    }
}