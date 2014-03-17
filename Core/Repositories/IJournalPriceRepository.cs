namespace QOAM.Core.Repositories
{
    using PagedList;

    using QOAM.Core.Repositories.Filters;

    public interface IJournalPriceRepository
    {
        JournalPrice Find(int id);
        JournalPrice Find(int journalId, int userProfileId);
        IPagedList<JournalPrice> Find(JournalPriceFilter filter);

        void Insert(JournalPrice journalPrice);
        void Update(JournalPrice journalPrice);
        void Delete(JournalPrice journalPrice);
        void Save();
    }
}