namespace RU.Uci.OAMarket.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class UserProfile : Entity
    {
        public UserProfile()
        {
            this.InstitutionJournalPrices = new List<InstitutionJournal>();
            this.JournalPrices = new List<JournalPrice>();
            this.ScoreCards = new List<ScoreCard>();
        }

        [Required]
        [StringLength(255)]
        public string UserName { get; set; }

        [Required]
        [StringLength(255)]
        public string DisplayName { get; set; }

        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }

        public DateTime DateRegistered { get; set; }
        public int InstitutionId { get; set; }

        public virtual Institution Institution { get; set; }
        public virtual ICollection<InstitutionJournal> InstitutionJournalPrices { get; set; }
        public virtual ICollection<JournalPrice> JournalPrices { get; set; }
        public virtual ICollection<ScoreCard> ScoreCards { get; set; }
    }
}