namespace QOAM.Core
{
    using System;

    public class ValuationJournalPrice : Entity
    {
        public ValuationJournalPrice()
        {
            this.Price = new Price();
        }

        public DateTime DateAdded { get; set; }
        public Price Price { get; set; }
        public int JournalId { get; set; }
        public int ValuationScoreCardId { get; set; }
        public int UserProfileId { get; set; }
        public virtual ValuationScoreCard ValuationScoreCard { get; set; }
        public virtual Journal Journal { get; set; }
        public virtual UserProfile UserProfile { get; set; }
    }
}