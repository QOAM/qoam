namespace RU.Uci.OAMarket.Website.ViewModels.Score
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using RU.Uci.OAMarket.Domain;

    public class ScoreViewModel
    {
        public int Id { get; set; }
        public string Remarks { get; set; }
        public bool Submitted { get; set; }
        public IList<QuestionScoreViewModel> QuestionScores { get; set; }
        public JournalViewModel Journal { get; set; }
        public JournalPriceViewModel Price { get; set; }
        public IEnumerable<KeyValuePair<Currency, string>> Currencies { get; set; }

        public void UpdateScoreCard(ScoreCard scoreCard, TimeSpan scoreCardLifeTime)
        {
            foreach (var questionScore in scoreCard.QuestionScores)
            {
                questionScore.Score = this.QuestionScores.First(q => q.Id == questionScore.Id).Score;
            }

            scoreCard.Score = new ScoreCardScore(scoreCard.QuestionScores);
            scoreCard.Remarks = this.Remarks;
            scoreCard.Submitted = this.Submitted;
            scoreCard.State = ScoreCardState.Published;
            scoreCard.DatePublished = DateTime.Now;
            scoreCard.DateExpiration = DateTime.Now + scoreCardLifeTime;
        }
    }
}