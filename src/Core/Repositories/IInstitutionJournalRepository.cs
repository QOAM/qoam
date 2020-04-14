namespace QOAM.Core.Repositories
{
    using System.Collections.Generic;

    using PagedList;

    using QOAM.Core.Repositories.Filters;

    public interface IInstitutionJournalRepository : IRepository
    {
        InstitutionJournal Find(int journalId, int institutionId);

        IPagedList<InstitutionJournal> Find(InstitutionJournalFilter filter);

        IList<InstitutionJournal> FindAll(InstitutionJournalFilter filter);

        void InsertOrUpdate(InstitutionJournal journalPrice);

        void Delete(InstitutionJournal journalPrice);

        void Save();
    }
}