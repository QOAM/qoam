namespace QOAM.Core
{
    using System.Collections.Generic;

    public class Question : Entity
    {
        public Question()
        {
            this.QuestionScores = new List<BaseQuestionScore>();
        }

        public QuestionKey Key { get; set; }
        public QuestionCategory Category { get; set; }

        public virtual ICollection<BaseQuestionScore> QuestionScores { get; set; }
    }
}