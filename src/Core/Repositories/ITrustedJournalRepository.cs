using System.Collections.Generic;

namespace QOAM.Core.Repositories
{
    public interface ITrustedJournalRepository : IRepository
    {
        TrustedJournal Find(int journalId, int institutionId);

        //IPagedList<TrustedJournal> Find(TrustedJournalFilter filter);

        //IList<TrustedJournal> FindAll(TrustedJournalFilter filter);

        void InsertOrUpdate(TrustedJournal journalPrice);

        void Delete(TrustedJournal journalPrice);

        void Save();
        int CountTrustedByForJournal(int journalId);
        TrustedJournal Find(int trustedJournalId);
        IList<Journal> ListOfTrustedJournals();
        bool EnableProxyCreation { get; set; }
    }
}