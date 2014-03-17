namespace QOAM.Core
{
    using System.Collections.Generic;

    public class JournalScore : Entity
    {
        public JournalScore()
        {
            this.OverallScore = new JournalCategoryScore();
            this.EditorialInformationScore = new JournalCategoryScore();
            this.PeerReviewScore = new JournalCategoryScore();
            this.GovernanceScore = new JournalCategoryScore();
            this.ProcessScore = new JournalCategoryScore();
            this.ValuationScore = new JournalCategoryScore();
            this.Journals = new List<Journal>();
        }
        
        public JournalCategoryScore OverallScore { get; set; }
        public JournalCategoryScore EditorialInformationScore { get; set; }
        public JournalCategoryScore PeerReviewScore { get; set; }
        public JournalCategoryScore GovernanceScore { get; set; }
        public JournalCategoryScore ProcessScore { get; set; }
        public JournalCategoryScore ValuationScore { get; set; }
        public int NumberOfReviewers { get; set; }
        public int JournalId { get; set; }
        public virtual Journal Journal { get; set; }
        public virtual ICollection<Journal> Journals { get; set; }
    }
}