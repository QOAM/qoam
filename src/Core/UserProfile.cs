namespace QOAM.Core
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class UserProfile : Entity
    {
        public UserProfile()
        {
            this.InstitutionJournalPrices = new List<InstitutionJournal>();
            this.JournalPrices = new List<BaseJournalPrice>();
            this.BaseScoreCards = new List<BaseScoreCard>();
            this.ValuationScoreCards = new List<ValuationScoreCard>();
        }

        [Required]
        [StringLength(1000)]
        public string UserName { get; set; }

        [Required]
        [StringLength(1000)]
        public string DisplayName { get; set; }

        [EmailAddress]
        [StringLength(1000)]
        public string Email { get; set; }

        public DateTime DateRegistered { get; set; }

        public DateTime? DateLastLogin { get; set; }

        public string OrcId { get; set; }

        public int NumberOfBaseScoreCards { get; set; }
        public int NumberOfValuationScoreCards { get; set; }
        public int NumberOfScoreCards { get; set; }
        
        public int InstitutionId { get; set; }
        public virtual Institution Institution { get; set; }

        public virtual ICollection<InstitutionJournal> InstitutionJournalPrices { get; set; }
        public virtual ICollection<BaseJournalPrice> JournalPrices { get; set; }
        public virtual ICollection<BaseScoreCard> BaseScoreCards { get; set; }
        public virtual ICollection<ValuationScoreCard> ValuationScoreCards { get; set; }
        public virtual ICollection<Corner>  Corners { get; set; }
    }
}