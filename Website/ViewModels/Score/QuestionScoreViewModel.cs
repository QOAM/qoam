namespace RU.Uci.OAMarket.Website.ViewModels.Score
{
    using RU.Uci.OAMarket.Domain;

    public class QuestionScoreViewModel
    {
        public int Id { get; set; }
        public int ScoreCardId { get; set; }
        public int QuestionId { get; set; }
        public QuestionKey QuestionKey { get; set; }
        public QuestionCategory QuestionCategory { get; set; }
        public Score Score { get; set; }
    }
}