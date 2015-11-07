namespace QOAM.Core.Repositories
{
    using PagedList;

    using QOAM.Core.Repositories.Filters;

    public interface IBaseJournalPriceRepository
    {
        BaseJournalPrice Find(int id);
        BaseJournalPrice Find(int journalId, int userProfileId);
        IPagedList<BaseJournalPrice> Find(JournalPriceFilter filter);

        void InsertOrUpdate(BaseJournalPrice journalPrice);
        void Delete(BaseJournalPrice journalPrice);
        void Save();
    }
}