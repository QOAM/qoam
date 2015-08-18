namespace QOAM.Core
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Journal : Entity
    {
        public Journal()
        {
            this.InstitutionJournalPrices = new List<InstitutionJournal>();
            this.BaseJournalPrices = new List<BaseJournalPrice>();
            this.ValuationJournalPrices = new List<ValuationJournalPrice>();
            this.JournalScores = new List<JournalScore>();
            this.Languages = new List<Language>();
            this.BaseScoreCards = new List<BaseScoreCard>();
            this.ValuationScoreCards = new List<ValuationScoreCard>();
            this.Subjects = new List<Subject>();
        }

        [Required]
        [StringLength(1000)]
        public string Title { get; set; }

        [Required]
        [StringLength(32)]
        public string ISSN { get; set; }

        [Required]
        public string Link { get; set; }
        
        public DateTime DateAdded { get; set; }
        public int CountryId { get; set; }
        public int PublisherId { get; set; }
        public int? JournalScoreId { get; set; }
        public bool DoajSeal { get; set; }
        public virtual Country Country { get; set; }
        public virtual Publisher Publisher { get; set; }
        public virtual JournalScore JournalScore { get; set; }
        public virtual ICollection<InstitutionJournal> InstitutionJournalPrices { get; set; }
        public virtual ICollection<BaseJournalPrice> BaseJournalPrices { get; set; }
        public virtual ICollection<ValuationJournalPrice> ValuationJournalPrices { get; set; }
        public virtual ICollection<JournalScore> JournalScores { get; set; }
        public virtual ICollection<Language> Languages { get; set; }
        public virtual ICollection<BaseScoreCard> BaseScoreCards { get; set; }
        public virtual ICollection<ValuationScoreCard> ValuationScoreCards { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }
    }
}