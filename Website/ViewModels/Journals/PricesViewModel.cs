namespace QOAM.Website.ViewModels.Journals
{
    using PagedList;

    using QOAM.Core;
    using QOAM.Core.Repositories.Filters;

    public class PricesViewModel : PagedViewModel
    {
        public int Id { get; set; }
        public Journal Journal { get; set; }
        public InstitutionJournal InstitutionJournal { get; set; }
        public IPagedList<InstitutionJournal> InstitutionJournals { get; set; }
        public IPagedList<JournalPrice> JournalPrices { get; set; }
        public string RefererUrl { get; set; }

        public InstitutionJournalFilter ToInstitutionJournalPriceFilter()
        {
            return new InstitutionJournalFilter
                       {
                           JournalId = this.Id,
                           PageNumber = this.Page,
                           PageSize = this.PageSize,
                       };
        }

        public JournalPriceFilter ToJournalPriceFilter()
        {
            return new JournalPriceFilter
            {
                JournalId = this.Id,
                PageNumber = this.Page,
                PageSize = this.PageSize,
            };
        }
    }
}