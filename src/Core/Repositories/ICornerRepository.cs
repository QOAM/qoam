using System.Collections.Generic;
using PagedList;

namespace QOAM.Core.Repositories
{
    public interface ICornerRepository
    {
        Corner Find(string name);
        IList<Corner> FindForUser(int userProfileId);
        void Save();
        void Dispose();
        void InsertOrUpdate(Corner entity);
        void Delete(Corner entity);
        IList<Corner> All();
        IPagedList<Journal> GetJournalsForCorner(int cornerId, int page, int pageSize);
        Corner Find(int id);
    }
}