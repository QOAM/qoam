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
        public int NumberOfBaseReviewers { get; set; }
        public int NumberOfValuationReviewers { get; set; }
        public int JournalId { get; set; }
        public string Swot
        {
            get
            {
                if (this.OverallScore != null && this.ValuationScore != null)
                {
                    if (this.OverallScore.AverageScore >= 3 && this.ValuationScore.AverageScore >= 3)
                        return SwotVerdict.StrongJournal;
                    if (this.OverallScore.AverageScore < 3 && this.ValuationScore.AverageScore < 3)
                        return SwotVerdict.WeakerJournal;
                    if (this.OverallScore.AverageScore >= 3 && this.ValuationScore.AverageScore < 3)
                        return SwotVerdict.ThreatToAuthor;
                    if (this.OverallScore.AverageScore < 3 && this.ValuationScore.AverageScore >= 3)
                        return SwotVerdict.OpportunityToPublisher;

                    return string.Empty;
                }
                
                return string.Empty;
            }
        }

        public virtual Journal Journal { get; set; }
        public virtual ICollection<Journal> Journals { get; set; }
    }
}