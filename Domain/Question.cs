namespace RU.Uci.OAMarket.Domain
{
    using System.Collections.Generic;

    public class Question : Entity
    {
        public Question()
        {
            this.QuestionScores = new List<QuestionScore>();
        }

        public QuestionKey Key { get; set; }
        public QuestionCategory Category { get; set; }

        public virtual ICollection<QuestionScore> QuestionScores { get; set; }
    }
}