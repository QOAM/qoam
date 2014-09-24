namespace QOAM.Core
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Country : Entity
    {
        public Country()
        {
            this.Journals = new List<Journal>();
        }

        [Required]
        [StringLength(1000)]
        public string Name { get; set; }

        public virtual ICollection<Journal> Journals { get; set; }
    }
}