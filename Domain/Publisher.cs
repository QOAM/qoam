namespace RU.Uci.OAMarket.Domain
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Publisher : Entity
    {
        public Publisher()
        {
            this.Journals = new List<Journal>();
        }

        [Required]
        [StringLength(900)]
        public string Name { get; set; }

        public virtual ICollection<Journal> Journals { get; set; }
    }
}