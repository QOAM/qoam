namespace QOAM.Core
{
    using System;
    using System.Collections.Generic;

    public class JournalPrice : Entity
    {
        public JournalPrice()
        {
            this.Price = new Price();
            this.Journals = new List<Journal>();
        }
        
        public DateTime DateAdded { get; set; }
        public Price Price { get; set; }
        public int JournalId { get; set; }
        public int ScoreCardId { get; set; }
        public int UserProfileId { get; set; }
        public virtual ScoreCard ScoreCard { get; set; }
        public virtual Journal Journal { get; set; }
        public virtual UserProfile UserProfile { get; set; }
        public virtual ICollection<Journal> Journals { get; set; }
    }
}