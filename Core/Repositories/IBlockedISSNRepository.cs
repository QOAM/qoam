namespace QOAM.Core.Repositories
{
    using System.Collections.Generic;

    public interface IBlockedISSNRepository
    {
        IList<BlockedISSN> All { get; }

        BlockedISSN Find(int id);

        void InsertOrUpdate(BlockedISSN blockedISSN);

        void Save();

        void Delete(BlockedISSN blockedISSN);

        bool Exists(string issn);
    }
}