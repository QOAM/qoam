namespace QOAM.Core
{
    public class ValuationQuestionScore : Entity
    {
        public Score Score { get; set; }
        public int ValuationScoreCardId { get; set; }
        public int QuestionId { get; set; }
        public virtual ValuationScoreCard ValuationScoreCard { get; set; }
        public virtual Question Question { get; set; }
    }
}