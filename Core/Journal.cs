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
            this.JournalPrices = new List<JournalPrice>();
            this.JournalScores = new List<JournalScore>();
            this.Languages = new List<Language>();
            this.ScoreCards = new List<ScoreCard>();
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
        public int? JournalPriceId { get; set; }
        public virtual Country Country { get; set; }
        public virtual Publisher Publisher { get; set; }
        public virtual JournalScore JournalScore { get; set; }
        public virtual JournalPrice JournalPrice { get; set; }
        public virtual ICollection<InstitutionJournal> InstitutionJournalPrices { get; set; }
        public virtual ICollection<JournalPrice> JournalPrices { get; set; }
        public virtual ICollection<JournalScore> JournalScores { get; set; }
        public virtual ICollection<Language> Languages { get; set; }
        public virtual ICollection<ScoreCard> ScoreCards { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }
    }
}