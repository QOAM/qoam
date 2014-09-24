namespace QOAM.Core
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Institution : Entity
    {
        public Institution()
        {
            this.InstitutionJournalPrices = new List<InstitutionJournal>();
            this.UserProfiles = new List<UserProfile>();
        }

        [Required]
        [StringLength(1000)]
        public string Name { get; set; }

        [Required]
        [StringLength(1000)]
        public string ShortName { get; set; }

        public int NumberOfBaseScoreCards { get; set; }
        public int NumberOfValuationScoreCards { get; set; }
        public int NumberOfScoreCards { get; set; }

        public virtual ICollection<InstitutionJournal> InstitutionJournalPrices { get; set; }
        public virtual ICollection<UserProfile> UserProfiles { get; set; }
    }
}