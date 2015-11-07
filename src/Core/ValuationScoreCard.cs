namespace QOAM.Core
{
    using System;
    using System.Collections.Generic;

    public class ValuationScoreCard : Entity
    {
        public ValuationScoreCard()
        {
            this.Score = new ValuationScoreCardScore();
            this.QuestionScores = new List<ValuationQuestionScore>();
        }

        public DateTime DateStarted { get; set; }
        public DateTime? DateExpiration { get; set; }
        public DateTime? DatePublished { get; set; }
        public string Remarks { get; set; }
        public string PriceRemarks { get; set; }
        public int UserProfileId { get; set; }
        public int JournalId { get; set; }
        public int VersionId { get; set; }
        public ValuationScoreCardScore Score { get; set; }
        public ScoreCardState State { get; set; }
        public bool Submitted { get; set; }
        public bool Editor { get; set; }
        public virtual UserProfile UserProfile { get; set; }
        public virtual Journal Journal { get; set; }
        public virtual ScoreCardVersion Version { get; set; }
        public virtual IList<ValuationQuestionScore> QuestionScores { get; set; }
        public int BaseScoreCardId { get; set; }
    }
}