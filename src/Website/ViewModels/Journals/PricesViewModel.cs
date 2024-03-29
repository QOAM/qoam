﻿using System.Collections.Generic;

namespace QOAM.Website.ViewModels.Journals
{
    using PagedList;

    using QOAM.Core;
    using QOAM.Core.Repositories.Filters;

    public class PricesViewModel : PagedViewModel
    {
        public int Id { get; set; }
        public Journal Journal { get; set; }
        public IList<InstitutionJournal> InstitutionJournals { get; set; }
        public IPagedList<BaseJournalPrice> BaseJournalPrices { get; set; }
        public IPagedList<ValuationJournalPrice> ValuationJournalPrices { get; set; }
        public string RefererUrl { get; set; }
        public string FilterInstitution { get; set; }

        public InstitutionJournalFilter ToInstitutionJournalPriceFilter()
        {
            return new InstitutionJournalFilter
                       {
                           JournalId = this.Id,
                           PageNumber = this.Page,
                           PageSize = this.PageSize,
                       };
        }

        public JournalPriceFilter ToJournalPriceFilter(FeeType? feeType)
        {
            return new JournalPriceFilter
            {
                JournalId = this.Id,
                PageNumber = this.Page,
                PageSize = this.PageSize,
                FeeType = feeType
            };
        }
    }
}