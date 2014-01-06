namespace RU.Uci.OAMarket.Domain
{
    using System;
    using System.Collections.Generic;

    public class ScoreCard : Entity
    {
        public ScoreCard()
        {
            this.Score = new ScoreCardScore();
            this.QuestionScores = new List<QuestionScore>();
        }

        public DateTime DateStarted { get; set; }
        public DateTime? DateExpiration { get; set; }
        public DateTime? DatePublished { get; set; }
        public string Remarks { get; set; }
        public int UserProfileId { get; set; }
        public int JournalId { get; set; }
        public int VersionId { get; set; }
        public ScoreCardScore Score { get; set; }
        public ScoreCardState State { get; set; }
        public bool Submitted { get; set; }
        public bool Editor { get; set; }
        public virtual UserProfile UserProfile { get; set; }
        public virtual Journal Journal { get; set; }
        public virtual ScoreCardVersion Version { get; set; }
        public virtual ICollection<QuestionScore> QuestionScores { get; set; }
    }
}