namespace QOAM.Website.ViewModels.Journals
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using QOAM.Core;

    public class InstitutionJournalLicenseViewModel
    {
        public InstitutionJournalLicenseViewModel()
        {
        }

        public InstitutionJournalLicenseViewModel(Journal journal, InstitutionJournal institutionJournal, string refererUrl)
        {
            this.JournalTitle = journal.Title;
            this.JournalLink = journal.Link;
            this.JournalPublisher = journal.Publisher.Name;
            this.RefererUrl = refererUrl;

            if (institutionJournal == null)
            {
                return;
            }

            this.Link = institutionJournal.Link;
        }

        public string JournalTitle { get; set; }
        public string JournalLink { get; set; }
        public string JournalPublisher { get; set; }

        [Required]
        [Url]
        public string Link { get; set; }

        [DisplayName("Update all journals with the same publisher")]
        public bool UpdateAllJournalsOfPublisher { get; set; }

        public string RefererUrl { get; set; }
    }
}