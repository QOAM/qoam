namespace RU.Uci.OAMarket.Website.ViewModels.Journals
{
    using System.ComponentModel.DataAnnotations;

    using RU.Uci.OAMarket.Domain;

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

        [Display(Name = "UpdateAllJournalsOfPublisher", ResourceType = typeof(Resources.Strings))]
        public bool UpdateAllJournalsOfPublisher { get; set; }

        public string RefererUrl { get; set; }
    }
}