namespace RU.Uci.OAMarket.Domain
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Subject : Entity
    {
        public Subject()
        {
            this.Journals = new List<Journal>();
        }

        [Required]
        [StringLength(1024)]
        public string Name { get; set; }

        public virtual ICollection<Journal> Journals { get; set; }
    }
}