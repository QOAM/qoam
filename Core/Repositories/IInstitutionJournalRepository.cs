namespace QOAM.Core.Repositories
{
    using System.Collections.Generic;

    using PagedList;

    using QOAM.Core.Repositories.Filters;

    public interface IInstitutionJournalRepository
    {
        InstitutionJournal Find(int journalId, int userProfileId);
        IPagedList<InstitutionJournal> Find(InstitutionJournalFilter filter);
        IList<InstitutionJournal> FindAll(InstitutionJournalFilter filter);

        void InsertOrUpdate(InstitutionJournal journalPrice);
        void Save();
    }
}