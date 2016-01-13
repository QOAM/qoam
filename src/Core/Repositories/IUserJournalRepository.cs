using System.Collections.Generic;
using PagedList;
using QOAM.Core.Repositories.Filters;

namespace QOAM.Core.Repositories
{
    public interface IUserJournalRepository
    {
        UserJournal Find(int journalId, int userProfileId);
        IPagedList<UserJournal> Find(UserJournalFilter filter);
        IList<UserJournal> FindAll(UserJournalFilter filter);
        void Save();
        void Dispose();
        void InsertOrUpdate(UserJournal entity);
        void Delete(UserJournal entity);
        IPagedList<Journal> Search(UserJournalFilter filter);
        IList<UserJournal> All(int userProfileId);
    }
}