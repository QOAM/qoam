namespace RU.Uci.OAMarket.Domain
{
    public class QuestionScore : Entity
    {
        public Score Score { get; set; }
        public int ScoreCardId { get; set; }
        public int QuestionId { get; set; }
        public virtual ScoreCard ScoreCard { get; set; }
        public virtual Question Question { get; set; }
    }
}