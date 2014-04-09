namespace QOAM.Core
{
    using System;
    using System.Collections.Generic;

    public class BaseScoreCard : Entity
    {
        public BaseScoreCard()
        {
            this.Score = new BaseScoreCardScore();
            this.QuestionScores = new List<BaseQuestionScore>();
        }

        public DateTime DateStarted { get; set; }
        public DateTime? DateExpiration { get; set; }
        public DateTime? DatePublished { get; set; }
        public string Remarks { get; set; }
        public int UserProfileId { get; set; }
        public int JournalId { get; set; }
        public int VersionId { get; set; }
        public BaseScoreCardScore Score { get; set; }
        public ScoreCardState State { get; set; }
        public bool Submitted { get; set; }
        public bool Editor { get; set; }
        public virtual UserProfile UserProfile { get; set; }
        public virtual Journal Journal { get; set; }
        public virtual ScoreCardVersion Version { get; set; }
        public virtual IList<BaseQuestionScore> QuestionScores { get; set; }
    }
}