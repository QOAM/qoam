namespace RU.Uci.OAMarket.Domain
{
    public class ScoreCardStats
    {
        public int NumberOfUnpublishedScoreCards { get; set; }
        public int NumberOfPublishedScoreCards { get; set; }
        public int NumberOfExpiredScoreCards { get; set; }
    }
}