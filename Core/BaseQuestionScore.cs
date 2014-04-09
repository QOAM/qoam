namespace QOAM.Core
{
    public class BaseQuestionScore : Entity
    {
        public Score Score { get; set; }
        public int BaseScoreCardId { get; set; }
        public int QuestionId { get; set; }
        public virtual BaseScoreCard BaseScoreCard { get; set; }
        public virtual Question Question { get; set; }
    }
}
