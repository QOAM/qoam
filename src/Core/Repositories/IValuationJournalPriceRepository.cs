namespace QOAM.Core.Repositories
{
    using PagedList;

    using QOAM.Core.Repositories.Filters;

    public interface IValuationJournalPriceRepository
    {
        ValuationJournalPrice Find(int id);
        ValuationJournalPrice Find(int journalId, int userProfileId);
        IPagedList<ValuationJournalPrice> Find(JournalPriceFilter filter);

        void InsertOrUpdate(ValuationJournalPrice journalPrice);
        void Delete(ValuationJournalPrice journalPrice);
        void Save();
    }
}