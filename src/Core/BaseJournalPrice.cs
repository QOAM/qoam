namespace QOAM.Core
{
    using System;

    public class BaseJournalPrice : Entity
    {
        public BaseJournalPrice()
        {
            this.Price = new Price();
        }
        
        public DateTime DateAdded { get; set; }
        public Price Price { get; set; }
        public int JournalId { get; set; }
        public int BaseScoreCardId { get; set; }
        public int UserProfileId { get; set; }
        public virtual BaseScoreCard BaseScoreCard { get; set; }
        public virtual Journal Journal { get; set; }
        public virtual UserProfile UserProfile { get; set; }
    }
}