namespace QOAM.Website.ViewModels.Journals
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    using QOAM.Core;

    public class InstitutionJournalLicenseViewModel
    {
        public InstitutionJournalLicenseViewModel()
        {
            this.Institutions = new List<SelectListItem>();
        }

        public InstitutionJournalLicenseViewModel(Journal journal, InstitutionJournal institutionJournal, string refererUrl, IEnumerable<SelectListItem> institutions)
        {
            this.JournalTitle = journal.Title;
            this.JournalLink = journal.Link;
            this.JournalPublisher = journal.Publisher.Name;
            this.RefererUrl = refererUrl;
            this.Institutions = institutions;

            if (institutionJournal == null)
            {
                return;
            }

            this.Link = institutionJournal.Link;
        }

        public string JournalTitle { get; set; }

        public string JournalLink { get; set; }

        public string JournalPublisher { get; set; }

        public IEnumerable<SelectListItem> Institutions { get; set; }

        public int? Institution { get; set; }

        [Required]
        [Url]
        public string Link { get; set; }

        [DisplayName("Update all journals with the same publisher")]
        public bool UpdateAllJournalsOfPublisher { get; set; }

        public string RefererUrl { get; set; }
    }
}