using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

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

        public string CorrespondingInstitutions { get; set; }

        [NotMapped]
        public List<int> CorrespondingInstitutionIds
        {
            get => CorrespondingInstitutions?.Split(',').Select(x =>
            {
                var success = int.TryParse(x, out var value);
                return new { result = success, value };
            })
            .Where(pair => pair.result)
            .Select(pair=> pair.value)
            .ToList() ?? new List<int>();

            set => CorrespondingInstitutions = value.Select(x => x.ToString()).Aggregate((a, b) => $"{a},{b}");
        }

        public virtual ICollection<InstitutionJournal> InstitutionJournalPrices { get; set; }
        public virtual ICollection<UserProfile> UserProfiles { get; set; }
    }
}