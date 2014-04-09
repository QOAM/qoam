namespace QOAM.Core
{
    using System.Collections.Generic;

    public class ScoreCardVersion : Entity
    {
        public ScoreCardVersion()
        {
            this.BaseScoreCards = new List<BaseScoreCard>();
            this.ValuationScoreCards = new List<ValuationScoreCard>();
        }

        public int Number { get; set; }
        public int OverallNumberOfQuestions { get; set; }
        public int EditorialInformationNumberOfQuestions { get; set; }
        public int PeerReviewNumberOfQuestions { get; set; }
        public int GovernanceNumberOfQuestions { get; set; }
        public int ProcessNumberOfQuestions { get; set; }
        public int ValuationNumberOfQuestions { get; set; }

        public virtual ICollection<BaseScoreCard> BaseScoreCards { get; set; }
        public virtual ICollection<ValuationScoreCard> ValuationScoreCards { get; set; }
    }
}